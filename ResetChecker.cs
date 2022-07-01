using System;
using System.Diagnostics;
using System.Timers;

namespace Wellbeing
{
    public class ResetChecker
    {
        public event EventHandler? ShouldResetHandler;
        private readonly Timer Timer;

        private int _ResetHour;

        public int ResetHour
        {
            get => _ResetHour;
            set
            {
                _ResetHour = value;
                // Refresh timepoint.
                TimePoint = TimePoint;
            }
        }
        private DateTime NextResetTime;
        private DateTime _TimePoint;
        private DateTime TimePoint
        {
            get => _TimePoint;
            set
            {
                _TimePoint = value;
                // 00:00 (date.Date) + ResetHour (3) = 3:00 am.
                DateTime timePointDayResetHour = TimePoint.Date.AddHours(ResetHour);
                // If date is before 3:00am (00:00 - 2:59), next 3:00am is today's 3:00am.
                // Otherwise it's the next 3:00am is the next day, so we add 1 day to today's 3am.
                NextResetTime = TimePoint <= timePointDayResetHour ? timePointDayResetHour : timePointDayResetHour.AddDays(1);
                //PreviousResetHour = LastOpen <= lastOpenDayResetHour ? lastOpenDayResetHour.AddDays(-1) : lastOpenDayResetHour;
            }
        }

        public ResetChecker(int resetHour, DateTime lastOpen)
        {
            ResetHour = resetHour;
            TimePoint = lastOpen;
            Timer = new()
            {
                Interval = TimeSpan.FromMinutes(10).TotalMilliseconds,
                Enabled = false,
                AutoReset = true
            };
            Timer.Elapsed += HandleTick;
        }

        public void Start() => Timer.Enabled = true;
        public void Stop() => Timer.Enabled = false;

        private void HandleTick(object obj, ElapsedEventArgs e)
        {
            Logger.Log("Checking if should reset passed time...", false);
            if (!ShouldResetPassedTime())
                return;
            
            TimePoint = DateTime.Now.Add(TimeSpan.FromMinutes(1));
            ShouldResetHandler?.Invoke(this, EventArgs.Empty);
        }

        public bool ShouldResetPassedTime() => DateTime.Now >= NextResetTime;
    }
}