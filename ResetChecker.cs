using System;
using System.Diagnostics;
using System.Timers;

namespace Wellbeing
{
    public class ResetChecker
    {
        public event EventHandler? ShouldResetHandler;
        public int ResetHour;
        private readonly Timer Timer;
        private readonly DateTimeOffset LastOpen;

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
            Logger.Log("Checking if should reset passed time...");
            if (!ShouldResetPassedTime())
                return;

            ShouldResetHandler?.Invoke(this, EventArgs.Empty);
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