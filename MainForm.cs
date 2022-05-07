﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace Digital_wellbeing
{
    public partial class MainForm : Form
    {
        private const byte DefaultMaxTimeMins = 240;
        private readonly ResetChecker ResetChecker;
        private readonly PcLocker PcLocker;
        private int LastShown = int.MaxValue;
        private string? Password;


        private static readonly List<(int timePoint, Action action)> TimeEvents = new()
        {
            (30, () =>
            {
                PlayNotificationSound();
                MessageBox.Show("Zbývá 30 minut", "Oznámení o zbývajícím čase", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }),
            (10, () =>
            {
                PlayNotificationSound();
                MessageBox.Show("Zbývá 10 minut", "Oznámení o zbývajícím čase", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            })
        };
        
        public MainForm()
        {
            InitializeComponent();
            SetButtonListeners();

            Password = Config.GetValueOrNull(Config.Property.Password) ?? "17861177";
            ResetChecker = new(Config.GetIntOrNull(Config.Property.ResetHour) ?? 3);
            PcLocker = new(this);

            ResetChecker.ShouldReset += (_, _) => Reset();
            PassedTimeWatcher.OnRunningChanged += (_, running) =>
                Invoke(new EventHandler((_, _) => StatusLbl.Text = running ? "Zapnuto" : "Pozastaveno"));
            PassedTimeWatcher.OnUpdate += (_, time) =>
                Invoke(new EventHandler((_, _) => HandleTick(time.passedMillis, time.remainingMillis)));
            PassedTimeWatcher.OnMaxTimeReached += (_, _) =>
                Invoke(new EventHandler((_, _) => HandleMaxTimeReached()));

            int passedSecsToday = Config.GetIntOrNull(Config.Property.PassedTodaySecs) ?? 0;
            PassedTimeWatcher.PassedMillis = (int)TimeSpan.FromSeconds(passedSecsToday).TotalMilliseconds;
            PassedTimeWatcher.MaxTime = TimeSpan.FromMinutes(Config.GetIntOrNull(Config.Property.MaxTimeMins) ?? DefaultMaxTimeMins);

            Config.SetValue(Config.Property.LastOpenUnixSecs, DateTimeOffset.Now.ToUnixTimeSeconds());
            
            if (ResetChecker.ShouldResetPassedTime())
                Reset();
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            ResetChecker.Start();
            PassedTimeWatcher.Running = true;
            base.OnHandleCreated(e);
        }

        // To hide the window from task manager. It is still visible in processes.
        protected override CreateParams CreateParams {
            get {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x80;  // Turn on WS_EX_TOOLWINDOW
                return cp;
            }
        }

        private void Reset()
        {
            Debug.WriteLine("Resetting passed time.");
            int passedMillisBefore = PassedTimeWatcher.PassedMillis;
            
            Config.SetValue(Config.Property.PassedTodaySecs, "0");
            PassedTimeWatcher.PassedMillis = 0;
            
            if (passedMillisBefore >= PassedTimeWatcher.MaxTime.TotalMilliseconds)
            {
                PcLocker.Unlock();
                PassedTimeWatcher.Running = true;
            }
        }

        private void HandleMaxTimeReached()
        {
            PlayNotificationSound();
            Opacity = 1;
            PcLocker.Lock();
        }

        private void HandleTick(int passedMillis, int remainingMillis)
        {
            TimeSpan passedTime = TimeSpan.FromMilliseconds(passedMillis);
            TimeSpan remainingTime = TimeSpan.FromMilliseconds(remainingMillis);
            string formatted = "Čas: " + Format(passedTime) + " / " + Format(PassedTimeWatcher.MaxTime);
            TimeLbl.Text = formatted;

            foreach ((int timePoint, Action action) in TimeEvents)
            {
                if (LastShown <= timePoint || (int)remainingTime.TotalMinutes != timePoint)
                    continue;

                LastShown = timePoint;
                action.Invoke();
            }   
        }

        private void SetButtonListeners()
        {
            CloseButt.Click += (_, _) =>
            {
                if (RequestPassword())
                    Application.Exit();
            };

            ChangeResetHourButt.Click += (_, _) =>
            {
                TimeSpan currHour = TimeSpan.FromHours(ResetChecker.ResetHour);
                TimeSpan? hour = ObtainTimeOrNull(currHour);
                if (!hour.HasValue || hour == currHour || !RequestPassword())
                    return;
                
                ResetChecker.ResetHour = (byte)hour.Value.Hours;
                Config.SetValue(Config.Property.ResetHour, hour.Value.Hours);
            };

            ToggleButt.Click += (_, _) =>
            {
                if (!RequestPassword())
                    return;

                PassedTimeWatcher.Running = !PassedTimeWatcher.Running;
            };

            ChangePassedButt.Click += (_, _) =>
            {
                TimeSpan? time = ObtainTimeOrNull(TimeSpan.FromMilliseconds(PassedTimeWatcher.PassedMillis));
                if (!time.HasValue || !RequestPassword())
                    return;
                
                PassedTimeWatcher.PassedMillis = (int)time.Value.TotalMilliseconds;
            };

            ChangeMaxButt.Click += (_, _) =>
            {
                TimeSpan? time = ObtainTimeOrNull(PassedTimeWatcher.MaxTime);
                
                if (!time.HasValue || !RequestPassword())
                    return;
                
                Config.SetValue(Config.Property.MaxTimeMins, (int)time.Value.TotalMinutes);
                PassedTimeWatcher.MaxTime = time.Value;
            };

            ChangePasswordButt.Click += (_, _) =>
            {
                string? newPassword = ObtainTextOrNull("Nové heslo", true);
                
                if (newPassword is null || !RequestPassword())
                    return;
                
                Config.SetValue(Config.Property.Password, newPassword);
                Password = newPassword;
            };
        }

        private bool RequestPassword() => ObtainTextOrNull("Heslo", true) == Password;
        private static string? ObtainTextOrNull(string title, bool password)
        {
            var dialog = new TextDialog();
            dialog.Text = title;
            if (password)
                dialog.TextBox.UseSystemPasswordChar = true;
            
            return dialog.ShowDialog() == DialogResult.OK ? dialog.TextBox.Text : null;
        }
        
        private static TimeSpan? ObtainTimeOrNull(TimeSpan? initialTime)
        {
            using var dialog = new TimeDialog();
            if (initialTime != null)
            {
                dialog.HoursBox.Value = (int)initialTime.Value.TotalHours;
                dialog.MinutesBox.Value = initialTime.Value.Minutes;   
            }

            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK)
                return TimeSpan.FromMinutes((int)dialog.HoursBox.Value * 60 + (int)dialog.MinutesBox.Value);
            
            Debug.WriteLine(result);
            
            return null;
        }

        private static void PlayNotificationSound()
        {
            new Thread(() =>
            {
                for (int i = 0; i < 3; ++i)
                {
                    System.Media.SystemSounds.Exclamation.Play();
                    Thread.Sleep(300);
                }
            }).Start();
        }

        private static string Format(TimeSpan time)
        {
            int hours = (int)time.TotalHours;
            int minutes = time.Minutes;
            return (hours == 0 ? "" : hours + "h")
                   + (hours != 0 && minutes != 0 ? " " : "")
                   + (minutes == 0 && hours != 0 ? "" : minutes + "min");
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            Config.SetValue(Config.Property.PassedTodaySecs, (int)TimeSpan.FromMilliseconds(PassedTimeWatcher.PassedMillis).TotalSeconds);
            if (e.CloseReason is CloseReason.WindowsShutDown or CloseReason.ApplicationExitCall or CloseReason.TaskManagerClosing)
                return;
            
            e.Cancel = true;
            if (!PcLocker.Locked && e.CloseReason == CloseReason.UserClosing)
                Opacity = 0;
        }
    }
}