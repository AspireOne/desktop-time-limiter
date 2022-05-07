using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Digital_wellbeing
{
    internal static class Program
    {
        private static readonly NotifyIcon PinNotifyIcon = new()
        {
            Text = "Klikni pro otevření/zavření",
            Icon = Resources.program_icon,
            Visible = true
        };
        
        /// <summary>The main entry point for the application.</summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            
            var instances = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(Application.ExecutablePath));
            if (instances.Length > 1)
                Environment.Exit(0);

            var mainForm = new MainForm();
            mainForm.Opacity = 0;
            PinNotifyIcon.Click += (_, _) =>
            {
                mainForm.Opacity = mainForm.Opacity == 0 ? 1 : 0;
                if (mainForm.Opacity != 1)
                    return;
                
                mainForm.WindowState = FormWindowState.Minimized;
                mainForm.WindowState = FormWindowState.Normal;
            };
            
            StartupLauncher.SetStartup();
            Application.Run(mainForm);
        }
    }
}