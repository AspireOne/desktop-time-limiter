using System;
using System.Diagnostics;
using System.Timers;

namespace Wellbeing;

public class UpdateWatcher
{
    public event EventHandler? OnUpdateAvailable;
    private readonly Timer Timer;
    
    public UpdateWatcher()
    {
        Timer = new()
        {
            Interval = TimeSpan.FromMinutes(60).TotalMilliseconds,
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
        Logger.Log("Checking if update available");
        if (!await Updater.IsUpdateAvailable())
            return;
        
        Logger.Log("Update available, proceeding.");
        OnUpdateAvailable?.Invoke(this, EventArgs.Empty);
        Timer.Enabled = false;
    }
}