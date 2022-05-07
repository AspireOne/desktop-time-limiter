using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Interop;

namespace Digital_wellbeing
{
    // http://pinvoke.net/default.aspx/wtsapi32.WTSRegisterSessionNotification
    public class SessionChangeHandler : Control
    {
        [DllImport("user32")]
        public static extern void LockWorkStation();
        [DllImport("WtsApi32.dll")]
        private static extern bool WTSRegisterSessionNotification(IntPtr hWnd, [MarshalAs(UnmanagedType.U4)] int dwFlags);

        [DllImport("WtsApi32.dll")]
        private static extern bool WTSUnRegisterSessionNotification(IntPtr hWnd);

        private const int NOTIFY_FOR_THIS_SESSION = 0;
        private const int WM_WTSSESSION_CHANGE = 0x2b1;
        private const int WTS_SESSION_LOCK = 0x7;
        private const int WTS_SESSION_UNLOCK = 0x8;

        public event EventHandler? MachineLocked;
        public event EventHandler? MachineUnlocked;

        public void RegisterHandler()
        {
            if (!WTSRegisterSessionNotification(Handle, NOTIFY_FOR_THIS_SESSION))
                Marshal.ThrowExceptionForHR(Marshal.GetLastWin32Error());
        }

        public void UnregisterHandler() => WTSUnRegisterSessionNotification(Handle);

        protected override void OnHandleDestroyed(EventArgs e)
        {
            // Unregister the handle before it gets destroyed.
            UnregisterHandler();
            base.OnHandleDestroyed(e);
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_WTSSESSION_CHANGE)
            {
                int value = m.WParam.ToInt32();
                if (value == WTS_SESSION_LOCK) MachineLocked?.Invoke(this, EventArgs.Empty);
                else if (value == WTS_SESSION_UNLOCK) MachineUnlocked?.Invoke(this, EventArgs.Empty);
            }

            base.WndProc(ref m);
        }
    }
}