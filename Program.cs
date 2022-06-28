using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Wellbeing.Properties;

namespace Wellbeing
{
    internal static class Program
    {
        public enum ConsoleAction { Delete };

        public const string Version = "1.0.0";
        public static readonly Dictionary<ConsoleAction, string> ConsoleActions = new()
        {
            { ConsoleAction.Delete , "--delete" }
        };
        private static readonly NotifyIcon PinNotifyIcon = new()
        {
            Text = "Klikni pro otevření/zavření",
            Icon = Resources.program_icon,
            Visible = true
        };

        public static readonly string RootDirectory =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Wellbeing");
        public const string ExeName = "Digital-Wellbeing.exe";

        /// <summary>The main entry point for the application.</summary>
        [STAThread]
        private static void Main(string[] args)
        {
            if (!Directory.Exists(RootDirectory))
                Directory.CreateDirectory(RootDirectory);
            
            for (int i = 0; i < args.Length; ++i)
            {
                if (args[i] == ConsoleActions[ConsoleAction.Delete])
                {
                    HandleDeleteArgument(args, i);
                    continue;
                }
                
                Debug.WriteLine("Unrecognized console argument.");
            }
            
            Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            
            if (ShouldReplaceCurrent())
            {
                ReplaceCurrent();
                ProcessStartInfo startInfo = new()
                {
                    FileName = StartupLauncher.ExecutablePath,
                    Arguments = ConsoleActions[ConsoleAction.Delete] + " " + Application.ExecutablePath
                };
                Process.Start(startInfo);
                return;
            }

            if (!CheckIsSingleInstance())
                return;
                
            //StartupLauncher.ExcludeFromDefender();
            StartupLauncher.SetLaunchOnStartup();

            var mainForm = InitMainForm();
            Application.Run(mainForm);
        }

        private static void HandleDeleteArgument(IReadOnlyList<string> args, int index)
        {
            if (index+1 == args.Count || args[index+1].StartsWith("-"))
                Debug.WriteLine("Invalid console arguments.");

            string filename = args[index+1];
            if (File.Exists(filename))
                TryDeleteRecursively(filename);
            else
                Debug.WriteLine("File path doesn't exist");
        }

        /// <summary>
        /// Keeps trying to delete the file for a few seconds. Doesn't throw an exception.
        /// </summary>
        /// <param name="filename">The path of the file to delete.</param>
        /// <returns></returns>
        private static bool TryDeleteRecursively(string filename)
        {
            for (int i = 0; i < 3; ++i)
            {
                try
                {
                    File.Delete(filename);
                    return true;
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Ignoring exception while deleting.");
                    Thread.Sleep(1000);
                }   
            }

            return false;
        }

        private static MainForm InitMainForm()
        {
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
            return mainForm;
        }

        /// <summary>
        /// Checks if this process is the only instance of this program with a few secs of polling.
        /// </summary>
        /// <returns>True if this exe is the only instance of this program. False otherwise.</returns>
        private static bool CheckIsSingleInstance()
        {
            for (int i = 0; i < 3; ++i)
            {
                var instances = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(Application.ExecutablePath));
                if (instances.Length == 1)
                    return true;
                Thread.Sleep(1250);
            }

            return false;
        }

        /// <summary>
        /// Closes other instances of this program, deletes supposed exe location and copies this program
        /// to that location.
        /// </summary>
        private static void ReplaceCurrent()
        {
            CloseOtherProcess();
            File.Delete(StartupLauncher.ExecutablePath);
            File.Copy(Application.ExecutablePath, StartupLauncher.ExecutablePath);
        }

        /// <summary>
        /// Checks if current exe and the supposed exe location match and if file contents are equal.
        /// </summary>
        /// <returns>True if current exe is in another location than supposed exe and has different binary. Otherwise false.</returns>
        private static bool ShouldReplaceCurrent()
        {
            return StartupLauncher.ExecutablePath != Application.ExecutablePath 
                   && !Utils.AreFileContentsEqual(StartupLauncher.ExecutablePath, Application.ExecutablePath);
        }

        /// <summary>
        /// Closes all other instances of this program.
        /// </summary>
        public static void CloseOtherProcess()
        {
            Process[] runningProcesses = Process.GetProcesses();
            int ownProcessId = Process.GetCurrentProcess().Id;
            foreach (Process process in runningProcesses)
            {
                if (process.ProcessName == "Digital-Wellbeing" && process.Id != ownProcessId)
                    process.CloseMainWindow();
            }
        }
    }
}