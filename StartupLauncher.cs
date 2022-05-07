using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Digital_wellbeing
{
    public static class StartupLauncher
    {
        //Startup registry key and value
        private const string StartupKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
        private const string StartupValue = "DigitalWellbeing";
        private static readonly string ExecutablePath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Digital-Wellbeing.exe");

        public static void SetStartup()
        {
            bool areDifferent = ExecutablePath != Application.ExecutablePath && !Utils.AreFileContentsEqual(ExecutablePath, Application.ExecutablePath);
            if (!File.Exists(ExecutablePath) || areDifferent)
            {
                File.Delete(ExecutablePath);
                File.Copy(Application.ExecutablePath, ExecutablePath);   
            }

            using RegistryKey key = Registry.CurrentUser.OpenSubKey(StartupKey, true)!;
        
            if ((string)key.GetValue(StartupKey) != ExecutablePath)
                key.SetValue(StartupValue, ExecutablePath);
        }
    }   
}