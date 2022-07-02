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
        private static readonly int UpdateFrequencyMillis = (int)TimeSpan.FromSeconds(1).TotalMilliseconds;
        private static readonly Timer Timer = new(OnTimerTick, null, Timeout.Infinite, UpdateFrequencyMillis);
        
        public static TimeSpan MaxTime;
        public static TimeSpan IdleThreshold;
        private static uint AutosaveCounterMillis;
        private static uint LastIdleTimeMillis;
        private static uint IdleMillisDuringSleep;
        private static bool Idle;
        public static int PassedMillis;
        private static bool _Running;
        public static bool Running
        {
            get => _Running;
            set
            {
                if (value == Running)
                    return;
                
                LastIdleTimeMillis = GetIdleTimeMillis();
                _Running = value;
                if (value)
                    Timer.Change(0, UpdateFrequencyMillis);
                else
                    Timer.Change(-1, -1);
                
                OnRunningChanged?.Invoke(null, value);
            }
        }

        private struct Lastinputinfo { public uint cbSize, dwTime; }

        private static void OnTimerTick(object state)
        {
            AutosaveCounterMillis += (uint)UpdateFrequencyMillis;
            if (AutosaveCounterMillis >= AutosaveFrequencyMillis)
            {
                SaveToConfig();
                AutosaveCounterMillis = 0;
            }
            
            uint idleTimeMillis = GetIdleTimeMillis();
            
            bool isIdleAfterSleep = idleTimeMillis > LastIdleTimeMillis + UpdateFrequencyMillis * 3;
            
            // If is idle after sleep and after_sleep_idle_time_offset has not been set yet.
            if (isIdleAfterSleep && IdleMillisDuringSleep == 0)
                IdleMillisDuringSleep = idleTimeMillis - LastIdleTimeMillis;
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
            /*bool wokeUpFromSleep = idleTimeMillis > LastIdleTimeMillis + UpdateFrequencyMillis * 2;
            Logger.Log("Woke up from sleep: " + wokeUpFromSleep);*/
            if (idleTimeMillis > PassedMillis)
                PassedMillis = 0;
            else
                PassedMillis -= (int)idleTimeMillis;
        }

        private static void HandleTick(uint idleTimeMillis)
        {
            if (Idle)
            {
                Logger.Log($"Has just stopped being idle. Idle time (minutes): " + (LastIdleTimeMillis / 1000)/60);
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

        // https://www.pinvoke.net/default.aspx/user32.getlastinputinfo
        [DllImport("User32.dll")] private static extern bool GetLastInputInfo(ref Lastinputinfo plii);
        [DllImport("Kernel32.dll")] private static extern uint GetLastError();
        private static uint GetIdleTimeMillis()
        {
            var lastInput = new Lastinputinfo();
            lastInput.cbSize = (uint)Marshal.SizeOf(lastInput);

            if (!GetLastInputInfo(ref lastInput))
                throw new Exception(GetLastError().ToString());

            return (uint)Environment.TickCount - lastInput.dwTime;
        }

        public static void SaveToConfig() => Config.SetValue(Config.Property.PassedTodaySecs, (int)TimeSpan.FromMilliseconds(PassedMillis).TotalSeconds);
    }
}