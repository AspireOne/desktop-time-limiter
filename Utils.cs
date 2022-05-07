using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Digital_wellbeing
{
    public static class Utils
    {
        private const int SW_SHOWNOACTIVATE = 4;
        private const int HWND_TOPMOST = -1;
        private const uint SWP_NOACTIVATE = 0x0010;

        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        private static extern bool SetWindowPos(
            int hWnd,             // Window handle
            int hWndInsertAfter,  // Placement-order handle
            int x,                // Horizontal position
            int y,                // Vertical position
            int cx,               // Width
            int cy,               // Height
            uint uFlags);         // Window positioning flags

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        public static bool AreFileContentsEqual(string path1, string path2) =>
            File.ReadAllBytes(path1).SequenceEqual(File.ReadAllBytes(path2));
        
        public static void ShowInactiveTopmost(Form frm)
        {
            try
            {
                ShowWindow(frm.Handle, SW_SHOWNOACTIVATE);
                SetWindowPos(frm.Handle.ToInt32(), HWND_TOPMOST, 0, 0, Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, SWP_NOACTIVATE);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Could not set overlay topmost.");
            }
        }
    }
}