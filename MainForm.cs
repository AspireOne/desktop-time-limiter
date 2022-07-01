using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Media;
using System.Windows.Forms;

namespace Wellbeing
{
    public partial class MainForm : Form
    {
        private const byte defaultResetHour = 3;
        private const byte DefaultMaxTimeMins = 240;
        private const byte DefaultIdleThresholdMins = 6;
        private const string DateTimeFormatter = "G";
        private const string DefaultPassword = "17861177";
        
        private readonly ResetChecker ResetChecker;
        private readonly UpdateWatcher UpdateWatcher;
        private readonly PcLocker PcLocker;
        private int LastShownMins = int.MaxValue;
        private string? Password;
        
        private static readonly List<(int timePointMins, Action action)> TimeEvents = new()
        {
            (30, () =>
            {
                PlayNotification(30);
                //MessageBox.Show("Zbývá 30 minut", "Oznámení o zbývajícím čase", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }),
            (10, () =>
            {
                PlayNotification(10);
                //MessageBox.Show("Zbývá 10 minut", "Oznámení o zbývajícím čase", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            })
        };
        
        public MainForm()
        {
            InitializeComponent();
            SetButtonListeners();
            versionLbl.Text = Program.Version;

            DateTime lastOpen = Config.GetDateTime(Config.Property.LastOpenDateTime, DateTimeFormatter) ?? DateTime.MinValue;
            int resetHour = Config.GetIntOrNull(Config.Property.ResetHour) ?? defaultResetHour;
            ResetChecker = new(resetHour, lastOpen);
            
            Password = Config.GetValueOrNull(Config.Property.Password) ?? DefaultPassword;
            UpdateWatcher = new();
            PcLocker = new(this);

            ResetChecker.ShouldResetHandler += (_, _) => Reset();
            UpdateWatcher.OnUpdateAvailable += (_, _) =>
            {
                Updater.DownloadLatestUpdateAsync(update =>
                {
                    PassedTimeWatcher.SaveToConfig();
                    update();
                });
            };
            
            PassedTimeWatcher.OnRunningChanged += (_, running) =>
                Invoke(new EventHandler((_, _) => StatusLbl.Text = running ? "Zapnuto" : "Pozastaveno"));
            PassedTimeWatcher.OnUpdate += (_, time) =>
                Invoke(new EventHandler((_, _) => HandleTick(time.passedMillis, time.remainingMillis)));
            PassedTimeWatcher.OnMaxTimeReached += (_, _) =>
                Invoke(new EventHandler((_, _) => HandleMaxTimeReached()));

            int passedSecsToday = Config.GetIntOrNull(Config.Property.PassedTodaySecs) ?? 0;
            PassedTimeWatcher.PassedMillis = (int)TimeSpan.FromSeconds(passedSecsToday).TotalMilliseconds;
            PassedTimeWatcher.MaxTime = TimeSpan.FromMinutes(Config.GetIntOrNull(Config.Property.MaxTimeMins) ?? DefaultMaxTimeMins);
            PassedTimeWatcher.IdleThreshold = TimeSpan.FromMinutes(Config.GetIntOrNull(Config.Property.IdleThresholdMins) ?? DefaultIdleThresholdMins);
            
            Config.SetValue(Config.Property.LastOpenDateTime, DateTime.Now.ToString(DateTimeFormatter));
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            if (ResetChecker.ShouldResetPassedTime())
                Reset();

            /*if (PassedTimeWatcher.PassedMillis >= PassedTimeWatcher.MaxTime.TotalMilliseconds)
            {
                MessageBox.Show("Dnes už nezbývá žádný čas", "Oznámení o zbývajícím čase", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }*/
            
            ResetChecker.Start();
            UpdateWatcher.Start();
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
            Logger.Log("Resetting passed time.");
            PassedTimeWatcher.PassedMillis = 0;
            Resume();
            Config.SetValue(Config.Property.PassedTodaySecs, "0");
        }

        private void Resume()
        {
            PcLocker.Unlock();
            PassedTimeWatcher.Running = true;
        }

        private void HandleMaxTimeReached()
        {
            PlayNotification(0);
            Opacity = 1;
            PcLocker.Lock();
        }

        private void HandleTick(int passedMillis, int remainingMillis)
        {
            TimeSpan passedTime = TimeSpan.FromMilliseconds(passedMillis);
            TimeSpan remainingTime = TimeSpan.FromMilliseconds(remainingMillis);
            string formatted = "Čas: " + Format(passedTime) + " / " + Format(PassedTimeWatcher.MaxTime);
            TimeLbl.Text = formatted;

            int remainingTimeMins = (int)remainingTime.TotalMinutes;
            if (LastShownMins < remainingTimeMins)
                LastShownMins = int.MaxValue;
            
            foreach ((int timePointMins, Action action) in TimeEvents)
            {
                if (LastShownMins <= timePointMins || (int)remainingTime.TotalMinutes != timePointMins)
                    continue;

                LastShownMins = timePointMins;
                action.Invoke();
            }
        }

        // Make sure the window doesn't go out of screen's bounds.
        protected override void OnResizeEnd(EventArgs e)
        {
            base.OnResizeEnd(e);
            int rightSideXPos = Location.X + Width;
            int bottomSideYPos = Location.Y + Height;
            
            int screenMaxY = Screen.PrimaryScreen.Bounds.Height;
            int screenMaxX = Screen.PrimaryScreen.Bounds.Width;

            if (rightSideXPos > screenMaxX)
                Location = Location with { X = screenMaxX - Width };
            if (bottomSideYPos > screenMaxY)
                Location = Location with { Y = screenMaxY - Height };
            if (Location.X < 0)
                Location = Location with { X = 0 };
            if (Location.Y < 0)
                Location = Location with { Y = 0 };
        }

        private void SetButtonListeners()
        {
            CloseButt.Click += (_, _) =>
            {
                if (RequestPassword())
                    Application.Exit();
            };

            ChangeIdleTimeButt.Click += (_, _) =>
            {
                TimeSpan currTime = PassedTimeWatcher.IdleThreshold;
                TimeSpan? time = ObtainTimeOrNull(currTime);
                if (time is not null && RequestPassword())
                {
                    PassedTimeWatcher.IdleThreshold = time.Value;
                    Config.SetValue(Config.Property.IdleThresholdMins, (int)time.Value.TotalMinutes);
                }
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
                if (PassedTimeWatcher.PassedMillis < PassedTimeWatcher.MaxTime.TotalMilliseconds)
                    Resume();
            };

            ChangeMaxButt.Click += (_, _) =>
            {
                TimeSpan? time = ObtainTimeOrNull(PassedTimeWatcher.MaxTime);
                
                if (!time.HasValue || !RequestPassword())
                    return;
                
                Config.SetValue(Config.Property.MaxTimeMins, (int)time.Value.TotalMinutes);
                PassedTimeWatcher.MaxTime = time.Value;
                
                if (PassedTimeWatcher.PassedMillis < PassedTimeWatcher.MaxTime.TotalMilliseconds)
                    Resume();
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

            return null;
        }

        private static void PlayNotification(int remaining = -1)
        {
            using SoundPlayer audio = new();
            audio.Stream = remaining switch
            {
                30 => Properties.Resources._30_minutes,
                10 => Properties.Resources._10_minutes,
                0 => Properties.Resources.time_reached,
                _ => Properties.Resources.generic_notification
            };
            audio.Play();
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
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                if (!PcLocker.Locked)
                    Opacity = 0;
            }
            else
            {
                PassedTimeWatcher.SaveToConfig();
                Logger.Log($"Closing program. Reason: {e.CloseReason}");
            }
            base.OnFormClosing(e);
        }

        private void label1_Click(object sender, EventArgs e)
        {
            
        }
    }
}