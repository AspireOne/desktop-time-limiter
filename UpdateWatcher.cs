using System;
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

    public void Start() => Timer.Enabled = true;

    private async void HandleTick(object obj, ElapsedEventArgs e)
    {
        if (!await Updater.IsUpdateAvailable())
            return;
        
        OnUpdateAvailable?.Invoke(this, EventArgs.Empty);
        Timer.Enabled = false;
    }
}