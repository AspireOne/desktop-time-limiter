using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Wellbeing;

public static class Logger
{
    public static readonly string LogPath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "log.txt");
    private const int MaxLines = 400;
    private static readonly object FileLock = new();

    static Logger()
    {
        File.AppendAllText(LogPath, $"\n\n———————————————————\n{DateTime.Now:F}\n");
    }
    public static void Log(object msg, bool toFile = true)
    {
        string timeMark = $"[{DateTime.Now:HH:mm}] ";
        string message = $"{timeMark} {msg}";
#if DEBUG
        Debug.WriteLine(message);
#else
        Console.WriteLine(message);
#endif
        if (!toFile)
            return;
        
        lock (FileLock)
            File.AppendAllText(LogPath, message + "\n");
    }
}