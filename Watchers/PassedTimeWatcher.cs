using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace Wellbeing
{
    public static class PassedTimeWatcher
    {
        public static event EventHandler<(int passedMillis, int remainingMillis)>? OnUpdate;
        public static event EventHandler? OnMaxTimeReached;
        public static event EventHandler<bool>? OnRunningChanged;
        
        private static readonly int AutosaveFrequencyMillis = (int)TimeSpan.FromMinutes(5).TotalMilliseconds;
        private const int UpdateFrequencyMillis = 1000;
        private static readonly Timer Timer = new(OnTimerTick, null, Timeout.Infinite, UpdateFrequencyMillis);
        
        public static TimeSpan MaxTime;
        public static TimeSpan IdleThreshold;
        private static uint AutosaveCounterMillis;
        public static uint LastIdleTimeMillis { get; private set; }
        public static uint IdleMillisDuringSleep { get; private set; }
        public static bool Idle { get; private set; }
        public static int PassedMillis;
        private static bool _Running;
        public static bool Running
        {
            get => _Running;
            set
            {
                if (value == Running)
                    return;

                IdleTimeWatcher.Run();
                LastIdleTimeMillis = IdleTimeWatcher.IdleTimeMillis;
                _Running = value;
                if (value)
                    Timer.Change(0, UpdateFrequencyMillis);
                else
                    Timer.Change(-1, -1);
                
                OnRunningChanged?.Invoke(null, value);
            }
        }

        private static void OnTimerTick(object state)
        {
            uint idleTimeMillis = IdleTimeWatcher.Update(UpdateFrequencyMillis);
            
            AutosaveCounterMillis += UpdateFrequencyMillis;
            if (AutosaveCounterMillis >= AutosaveFrequencyMillis)
            {
                SaveToConfig();
                AutosaveCounterMillis = 0;
            }

            bool isIdleAfterSleep = idleTimeMillis > LastIdleTimeMillis + UpdateFrequencyMillis * 3;
            
            // If is idle after sleep and after_sleep_idle_time_offset has not been set yet.
            if (isIdleAfterSleep && IdleMillisDuringSleep == 0)
            {
                IdleMillisDuringSleep = idleTimeMillis - LastIdleTimeMillis;
                Logger.Log($"Woke up from sleep." +
                           $"Idle during sleep: {Utils.FormatTime(IdleMillisDuringSleep)} | " +
                           $"Idle before sleep: {Utils.FormatTime(idleTimeMillis - IdleMillisDuringSleep)} | " +
                           $"Idle total: {Utils.FormatTime(idleTimeMillis)}");
            }
            else if (idleTimeMillis <= UpdateFrequencyMillis * 3)
                IdleMillisDuringSleep = 0;

            idleTimeMillis -= IdleMillisDuringSleep;

            if (idleTimeMillis >= IdleThreshold.TotalMilliseconds)
                HandleIdleTick(idleTimeMillis);
            else
                HandleTick(idleTimeMillis);
            
            OnUpdate?.Invoke(null, (PassedMillis, (int)MaxTime.TotalMilliseconds - PassedMillis));
            LastIdleTimeMillis = idleTimeMillis;
        }

        private static void HandleIdleTick(uint idleTimeMillis)
        {
            if (Idle)
                return;
            Idle = true;
            Logger.Log($"Has just became idle", false);
            if (idleTimeMillis > PassedMillis)
                PassedMillis = 0;
            else
                PassedMillis -= (int)idleTimeMillis;
        }

        private static void HandleTick(uint idleTimeMillis)
        {
            if (Idle)
            {
                Logger.Log($"Has just stopped being idle. Idle time (minutes): {LastIdleTimeMillis/1000/60}");
                Idle = false;
            }
            PassedMillis += UpdateFrequencyMillis;

            // If max time is not reached yet.
            if (PassedMillis - idleTimeMillis < MaxTime.TotalMilliseconds)
                return;
            
            // When the timer runs out.
            Running = false;
            OnMaxTimeReached?.Invoke(null, EventArgs.Empty);
        }

        public static void SaveToConfig() => Config.SetValue(Config.Property.PassedTodaySecs, (int)TimeSpan.FromMilliseconds(PassedMillis).TotalSeconds);
    }
}