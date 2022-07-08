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
    private const string RunSeparatorLine = "———————————————————";
    private const int RunsShown = 10;
    private static readonly object FileLock = new();

    static Logger()
    {
        File.AppendAllText(LogPath, $"\n\n{RunSeparatorLine}\n{DateTime.Now:F}\n");
        FormatLog();
    }

    private static void FormatLog()
    {
        string[] logLines = File.ReadAllLines(LogPath);
        int runs = logLines.Count(str => str == RunSeparatorLine);
        if (runs <= RunsShown)
            return;

        int runsToRemove = runs - RunsShown;
        int lastLineToRemove = 0;

        int occurences = 0;
        for (int i = 0; i < logLines.Length; ++i)
        {
            if (logLines[i] == RunSeparatorLine)
                ++occurences;

            if (occurences == runsToRemove + 1)
            {
                lastLineToRemove = i;
                break;
            }   
        }

        var formatted = logLines.Skip(lastLineToRemove);
        Log($"Formatting log. Runs in log: {runs} | runs to remove: {runsToRemove}", false);
        File.WriteAllLines(LogPath, formatted);
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