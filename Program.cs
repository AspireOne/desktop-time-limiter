using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Wellbeing.Properties;

namespace Wellbeing
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
            for (int i = 0; i < 3; ++i)
            {
                var instances = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(Application.ExecutablePath));
                if (instances.Length <= 1)
                    break;
                if (instances.Length > 1 && i == 2)
                    Environment.Exit(0);

                Thread.Sleep(1000);
            }

            var mainForm = new MainForm();
            mainForm.Opacity = 0;
            PinNotifyIcon.Click += (_, _) =>
            {
                mainForm.Opacity = mainForm.Opacity == 0 ? 1 : 0;
                if ((int)mainForm.Opacity != 1)
                    return;
                
                mainForm.WindowState = FormWindowState.Minimized;
                mainForm.WindowState = FormWindowState.Normal;
            };
            
            StartupLauncher.SetStartup();
            /*if (StartupLauncher.ExecutablePath != Application.ExecutablePath)
            {
                StartupLauncher.ExcludeFromDefender();
                Process.Start(StartupLauncher.ExecutablePath);
                Environment.Exit(0);
            }*/
            Application.Run(mainForm);
        }
    }
}