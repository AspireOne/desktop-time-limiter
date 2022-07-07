using System;
using System.Diagnostics;
using System.Timers;

namespace Wellbeing;

public class UpdateChecker
{
    public event EventHandler? OnUpdateAvailable;
    private readonly Timer Timer;
    
    public UpdateChecker()
    {
        Timer = new()
        {
            Interval = TimeSpan.FromMinutes(90).TotalMilliseconds,
            Enabled = false,
            AutoReset = true
        };
        Timer.Elapsed += HandleTick;
    }

    public void Start()
    {
        if (Timer.Enabled)
            return;
        
        Timer.Enabled = true;
        HandleTick(null, null);
    }

    private async void HandleTick(object obj, ElapsedEventArgs e)
    {
        if (!await Updater.IsUpdateAvailable())
            return;
        
        Logger.Log("Update available, proceeding.");
        OnUpdateAvailable?.Invoke(this, EventArgs.Empty);
        Timer.Enabled = false;
    }
}