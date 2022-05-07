using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace Digital_wellbeing
{
    public static class PassedTimeWatcher
    {
        public static event EventHandler<(int passedMillis, int remainingMillis)>? OnUpdate;
        public static event EventHandler? OnMaxTimeReached;
        
        public static TimeSpan MaxTime;
        private static readonly int UpdateFrequencyMillis = (int)TimeSpan.FromSeconds(1).TotalMilliseconds;
        private static readonly int IdleThresholdMillis = (int)TimeSpan.FromMinutes(7).TotalMilliseconds;
        private static readonly Timer Timer = new Timer(OnTimerTick, null, Timeout.Infinite, UpdateFrequencyMillis);

        private static bool Idle;
        public static int PassedMillis;
        public static bool Running { get; private set; }
        private struct Lastinputinfo { public uint cbSize, dwTime; }

        public static void Run()
        {
            if (Running)
                return;
            
            Running = true;
            Timer.Change(0, UpdateFrequencyMillis);
        }

        public static void Pause()
        {
            if (!Running)
                return;
            
            Running = false;
            Timer.Change(-1, -1);
        }

        private static void OnTimerTick(object state)
        {
            uint idleTimeMillis = GetIdleTimeMillis();

            if (idleTimeMillis >= IdleThresholdMillis)
            {
                if (Idle)
                    return;
                
                Idle = true;
                PassedMillis -= (int)idleTimeMillis;
                OnUpdate?.Invoke(null, (PassedMillis, (int)MaxTime.TotalMilliseconds - PassedMillis));
                return;
            }
            Idle = false;

            if (PassedMillis >= MaxTime.TotalMilliseconds)
            {
                Pause();
                OnMaxTimeReached?.Invoke(null, EventArgs.Empty);
            }
            
            PassedMillis += UpdateFrequencyMillis;
            OnUpdate?.Invoke(null, (PassedMillis, (int)MaxTime.TotalMilliseconds - PassedMillis));
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