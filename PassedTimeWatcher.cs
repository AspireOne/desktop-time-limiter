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
        
        public static TimeSpan MaxTime;
        public static TimeSpan IdleThreshold;
        private static readonly int UpdateFrequencyMillis = (int)TimeSpan.FromSeconds(1).TotalMilliseconds;
        private static readonly Timer Timer = new(OnTimerTick, null, Timeout.Infinite, UpdateFrequencyMillis);

        private static uint LastIdleTimeMillis = 0;
        private static uint IdleMillisDuringSleep = 0;
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
            uint idleTimeMillis = GetIdleTimeMillis();
            
            bool isIdleAfterSleep = idleTimeMillis > LastIdleTimeMillis + UpdateFrequencyMillis * 2;
            
            // If is idle after sleep and after_sleep_idle_time_offset has not been set yet.
            if (isIdleAfterSleep && IdleMillisDuringSleep == 0)
                IdleMillisDuringSleep = idleTimeMillis - LastIdleTimeMillis;
            else if (idleTimeMillis <= UpdateFrequencyMillis * 2)
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
            Debug.WriteLine($"Has just became idle (time: {PassedMillis}).");
            /*bool wokeUpFromSleep = idleTimeMillis > LastIdleTimeMillis + UpdateFrequencyMillis * 2;
            Debug.WriteLine("Woke up from sleep: " + wokeUpFromSleep);*/
            PassedMillis -= (int)idleTimeMillis;
        }

        private static void HandleTick(uint idleTimeMillis)
        {
            Idle = false;
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
    }
}