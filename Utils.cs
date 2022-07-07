using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Wellbeing
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
        public static bool AreFileContentsEqual(string path1, string path2)
        {
            if (!File.Exists(path1) || !File.Exists(path2))
                return false;
            
            return File.ReadAllBytes(path1).SequenceEqual(File.ReadAllBytes(path2));
        }

        public static void ShowInactiveTopmost(Form frm)
        {
            try
            {
                ShowWindow(frm.Handle, SW_SHOWNOACTIVATE);
                SetWindowPos(frm.Handle.ToInt32(), HWND_TOPMOST, 0, 0, Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, SWP_NOACTIVATE);
            }
            catch (Exception ex)
            {
                Logger.Log("Could not set overlay topmost.");
            }
        }

        public static string FormatTime(TimeSpan time)
        {
            /*int hours = (int)time.TotalHours;
            int minutes = time.Minutes;
            int seconds = time.Seconds;
            return (hours == 0 ? "" : hours + "h")
                   + (hours != 0 && minutes != 0 ? " " : "")
                   + (hours != 0 && minutes == 0 ? "" : minutes + "min")
                   + (seconds == 0 ? "" : $" {seconds} secs");*/
            return time.ToString("h'h 'm'm 's's'");
        }

        public static void StartWithParameters(string filePath, string parameters)
        {
            ProcessStartInfo startInfo = new()
            {
                FileName = filePath,
                Arguments = parameters
            };
            Process.Start(startInfo);
        }

        public static string FormatTime(long millis) => FormatTime(TimeSpan.FromMilliseconds(millis));
    }
}