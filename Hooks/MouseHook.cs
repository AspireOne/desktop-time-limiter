using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Wellbeing.Hook
{
    /// <summary>
    /// Class for intercepting low level Windows mouse hooks.
    /// </summary>
    class MouseHook
    {
        /// <summary>
        /// Internal callback processing function
        /// </summary>
        private delegate IntPtr MouseHookHandler(int nCode, IntPtr wParam, IntPtr lParam);
        private MouseHookHandler HookHandler;

        /// <summary>
        /// Function to be called when defined even occurs
        /// </summary>
        /// <param name="mouseStruct">MSLLHOOKSTRUCT mouse structure</param>
        public delegate void MouseHookCallback(Msllhookstruct mouseStruct);

        #region Events
        public event MouseHookCallback LeftButtonDown;
        public event MouseHookCallback LeftButtonUp;
        public event MouseHookCallback RightButtonDown;
        public event MouseHookCallback RightButtonUp;
        public event MouseHookCallback MouseMove;
        public event MouseHookCallback MouseWheel;
        public event MouseHookCallback DoubleClick;
        public event MouseHookCallback MiddleButtonDown;
        public event MouseHookCallback MiddleButtonUp;
        #endregion

        /// <summary>
        /// Low level mouse hook's ID
        /// </summary>
        private IntPtr HookId = IntPtr.Zero;

        /// <summary>
        /// Install low level mouse hook
        /// </summary>
        /// <param name="mouseHookCallbackFunc">Callback function</param>
        public void Install()
        {
            HookHandler = HookFunc;
            HookId = SetHook(HookHandler);
        }

        /// <summary>
        /// Remove low level mouse hook
        /// </summary>
        public void Uninstall()
        {
            if (HookId == IntPtr.Zero)
                return;

            UnhookWindowsHookEx(HookId);
            HookId = IntPtr.Zero;
        }

        /// <summary>
        /// Destructor. Unhook current hook
        /// </summary>
        ~MouseHook()
        {
            Uninstall();
        }

        /// <summary>
        /// Sets hook and assigns its ID for tracking
        /// </summary>
        /// <param name="proc">Internal callback function</param>
        /// <returns>Hook ID</returns>
        private IntPtr SetHook(MouseHookHandler proc)
        {
            using (ProcessModule module = Process.GetCurrentProcess().MainModule)
                return SetWindowsHookEx(WhMouseLl, proc, GetModuleHandle(module.ModuleName), 0);
        }

        /// <summary>
        /// Callback function
        /// </summary>
        private IntPtr HookFunc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            // parse system messages
            if (nCode >= 0)
            {
                if (MouseMessages.WmLbuttondown == (MouseMessages)wParam)
                    if (LeftButtonDown != null)
                        LeftButtonDown((Msllhookstruct)Marshal.PtrToStructure(lParam, typeof(Msllhookstruct)));
                if (MouseMessages.WmLbuttonup == (MouseMessages)wParam)
                    if (LeftButtonUp != null)
                        LeftButtonUp((Msllhookstruct)Marshal.PtrToStructure(lParam, typeof(Msllhookstruct)));
                if (MouseMessages.WmRbuttondown == (MouseMessages)wParam)
                    if (RightButtonDown != null)
                        RightButtonDown((Msllhookstruct)Marshal.PtrToStructure(lParam, typeof(Msllhookstruct)));
                if (MouseMessages.WmRbuttonup == (MouseMessages)wParam)
                    if (RightButtonUp != null)
                        RightButtonUp((Msllhookstruct)Marshal.PtrToStructure(lParam, typeof(Msllhookstruct)));
                if (MouseMessages.WmMousemove == (MouseMessages)wParam)
                    if (MouseMove != null)
                        MouseMove((Msllhookstruct)Marshal.PtrToStructure(lParam, typeof(Msllhookstruct)));
                if (MouseMessages.WmMousewheel == (MouseMessages)wParam)
                    if (MouseWheel != null)
                        MouseWheel((Msllhookstruct)Marshal.PtrToStructure(lParam, typeof(Msllhookstruct)));
                if (MouseMessages.WmLbuttondblclk == (MouseMessages)wParam)
                    if (DoubleClick != null)
                        DoubleClick((Msllhookstruct)Marshal.PtrToStructure(lParam, typeof(Msllhookstruct)));
                if (MouseMessages.WmMbuttondown == (MouseMessages)wParam)
                    if (MiddleButtonDown != null)
                        MiddleButtonDown((Msllhookstruct)Marshal.PtrToStructure(lParam, typeof(Msllhookstruct)));
                if (MouseMessages.WmMbuttonup == (MouseMessages)wParam)
                    if (MiddleButtonUp != null)
                        MiddleButtonUp((Msllhookstruct)Marshal.PtrToStructure(lParam, typeof(Msllhookstruct)));
            }
            return CallNextHookEx(HookId, nCode, wParam, lParam);
        }

        #region WinAPI
        private const int WhMouseLl = 14;

        private enum MouseMessages
        {
            WmLbuttondown = 0x0201,
            WmLbuttonup = 0x0202,
            WmMousemove = 0x0200,
            WmMousewheel = 0x020A,
            WmRbuttondown = 0x0204,
            WmRbuttonup = 0x0205,
            WmLbuttondblclk = 0x0203,
            WmMbuttondown = 0x0207,
            WmMbuttonup = 0x0208
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Point
        {
            public int x;
            public int y;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Msllhookstruct
        {
            public Point pt;
            public uint mouseData;
            public uint flags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook,
            MouseHookHandler lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
        #endregion
    }
}
