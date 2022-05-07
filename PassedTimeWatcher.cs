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
        public static event EventHandler<bool>? OnRunningChanged;
        
        public static TimeSpan MaxTime;
        private static readonly int UpdateFrequencyMillis = (int)TimeSpan.FromSeconds(1).TotalMilliseconds;
        private static readonly int IdleThresholdMillis = (int)TimeSpan.FromMinutes(7).TotalMilliseconds;
        private static readonly Timer Timer = new Timer(OnTimerTick, null, Timeout.Infinite, UpdateFrequencyMillis);

        private static bool Idle;
        public static int PassedMillis;
        private static bool _running;
        public static bool Running
        {
            get => _running;
            set
            {
                if (value == Running)
                    return;
                
                OnRunningChanged?.Invoke(null, value);
                _running = value;
                if (value)
                    Timer.Change(0, UpdateFrequencyMillis);
                else
                    Timer.Change(-1, -1);
            }
        }

        private struct Lastinputinfo { public uint cbSize, dwTime; }

        private static void OnTimerTick(object state)
        {
            uint idleTimeMillis = GetIdleTimeMillis();

            if (idleTimeMillis >= IdleThresholdMillis)
            {
                Debug.WriteLine("Is idle.");
                if (Idle)
                    return;
                
                Debug.WriteLine("Has just became idle.");
                Idle = true;
                PassedMillis -= (int)idleTimeMillis;
                OnUpdate?.Invoke(null, (PassedMillis, (int)MaxTime.TotalMilliseconds - PassedMillis));
                return;
            }
            
            Idle = false;
            PassedMillis += UpdateFrequencyMillis;
            OnUpdate?.Invoke(null, (PassedMillis, (int)MaxTime.TotalMilliseconds - PassedMillis));

            if (PassedMillis >= MaxTime.TotalMilliseconds)
            {
                Running = false;
                OnMaxTimeReached?.Invoke(null, EventArgs.Empty);
            }
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