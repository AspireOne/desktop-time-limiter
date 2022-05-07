using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace Digital_wellbeing
{
    public partial class MainForm : Form
    {
        private static string? Password = Config.GetValueOrNull(Config.Property.Password) ?? "17861177";
        private const byte HourToResetAt = 3; // 0 - 23
        private int LastShown = int.MaxValue;

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
            PassedTimeWatcher.OnUpdate += (_, time) =>
            {
                TimeSpan passedTime = TimeSpan.FromMilliseconds(time.passedMillis);
                TimeSpan remainingTime = TimeSpan.FromMilliseconds(time.remainingMillis);
                string formatted = "Čas: " + Format(passedTime) + " / " + Format(PassedTimeWatcher.MaxTime);
                Invoke(new EventHandler((_, _) => TimeLbl.Text = formatted));
              
                foreach ((int timePoint, Action action) in TimeEvents)
                {
                    if (LastShown <= timePoint || (int)remainingTime.TotalMinutes != timePoint)
                        continue;

                    LastShown = timePoint;
                    action.Invoke();
                }
            };

            CloseButt.Click += (_, _) =>
            {
                if (RequestPassword())
                    Application.Exit();
            };

            ToggleButt.Click += (_, _) =>
            {
                if (!RequestPassword())
                    return;

                if (PassedTimeWatcher.Running)
                    PassedTimeWatcher.Pause();
                else
                    PassedTimeWatcher.Run();

                StatusLbl.Text = PassedTimeWatcher.Running ? "Zapnuto" : "Pozastaveno";
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
                
                Config.SetValue(Config.Property.MaxTimeMins, (int)time.Value.TotalMinutes + "");
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
            
            bool maxTimeParsed = int.TryParse(Config.GetValueOrNull(Config.Property.MaxTimeMins), out int maxTime);
            PassedTimeWatcher.MaxTime = maxTimeParsed ? TimeSpan.FromMinutes(maxTime) : TimeSpan.FromHours(4);

            string? passedMinsTodayStr = Config.GetValueOrNull(Config.Property.PassedTodayMins);
            int passedMinsToday = int.Parse(passedMinsTodayStr ?? "0");

            if (ShouldReset())
            {
                passedMinsToday = 0;
                Config.SetValue(Config.Property.PassedTodayMins, "0");
            }

            PassedTimeWatcher.PassedMillis = (int)TimeSpan.FromMinutes(passedMinsToday).TotalMilliseconds;
            Config.SetValue(Config.Property.LastOpenUnixSecs, DateTimeOffset.Now.ToUnixTimeSeconds() + "");
            PassedTimeWatcher.Run();
        }

        private static bool ShouldReset()
        {
            string? lastOpenedUnixSecsStr = Config.GetValueOrNull(Config.Property.LastOpenUnixSecs);
            int lastOpenedUnixSecs = int.Parse(lastOpenedUnixSecsStr ?? "0");

            DateTimeOffset lastOpenDatetime = DateTimeOffset.FromUnixTimeSeconds(lastOpenedUnixSecs);
            DateTimeOffset currentDatetime = DateTimeOffset.Now;
            bool isEligibleForReset =
                lastOpenDatetime.Day != currentDatetime.Day
                || lastOpenDatetime.Month != currentDatetime.Month
                || lastOpenDatetime.Year != currentDatetime.Year
                || lastOpenDatetime.Hour < HourToResetAt;

            return isEligibleForReset && currentDatetime.Hour >= HourToResetAt;
        }

        private static bool RequestPassword() => ObtainTextOrNull("Heslo", true) == Password;
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
            if (e.CloseReason is CloseReason.WindowsShutDown or CloseReason.ApplicationExitCall/* || e.CloseReason == CloseReason.TaskManagerClosing*/)
                return;
            
            e.Cancel = true;
            Visible = false;
        }
    }
}