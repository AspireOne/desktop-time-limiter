using System;
using System.Diagnostics;
using System.Timers;

namespace Digital_wellbeing
{
    public class ResetChecker
    {
        public event EventHandler? ShouldReset;
        private readonly Timer Timer;
        private readonly DateTimeOffset LastOpen;
        public int ResetHour;

        public ResetChecker(int resetHour)
        {
            ResetHour = resetHour;
            LastOpen = DateTimeOffset.FromUnixTimeSeconds(Config.GetIntOrNull(Config.Property.LastOpenUnixSecs) ?? 0);
            Timer = new()
            {
                Interval = TimeSpan.FromMinutes(5).TotalMilliseconds,
                Enabled = false,
                AutoReset = true
            };
            Timer.Elapsed += HandleTick;
        }

        public void Start() => Timer.Enabled = true;
        public void Stop() => Timer.Enabled = false;

        private void HandleTick(object obj, ElapsedEventArgs e)
        {
            Debug.WriteLine("Checking if should reset passed time...");
            if (!ShouldResetPassedTime())
                return;

            ShouldReset?.Invoke(this, EventArgs.Empty);
        }

        public bool ShouldResetPassedTime()
        {
            DateTimeOffset currentDatetime = DateTimeOffset.Now;
            bool lastOpenBeforeResetTime =
                LastOpen.Day != currentDatetime.Day
                || LastOpen.Month != currentDatetime.Month
                || LastOpen.Year != currentDatetime.Year
                || LastOpen.Hour <= ResetHour;
            
            bool isAfterResetTime = currentDatetime.Hour >= ResetHour;
            return lastOpenBeforeResetTime && isAfterResetTime;
        }
    }
}