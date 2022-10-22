using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Timers;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

//using Timer = System.Timers.Timer;

namespace Wellbeing;

/// <summary>
/// Exposes API for fading Forms in and out.
/// </summary>
public class FadeAnimation
{
    public enum Fades {In, Out}

    public int Interval
    {
        get => (int)Timer.Interval;
        set => Timer.Interval = value;
    }
    public double MaxOpacity { get; set; }
    public Form TargetForm { get; init; }

    private const double Step = 0.01;
    
    private Timer Timer = new() { /*AutoReset = true, Enabled = false, */Interval = 10 };
    private readonly Queue<(Fades fade, Action? startedCallback)> Queued = new();
    private Fades? RunningFade;
    
    /// <param name="form">The form to fade.</param>
    public FadeAnimation(Form form)
    {
        TargetForm = form;
        Timer.Tick += HandleTimerTick;
    }
    
    /// <summary>
    /// MUST BE CALLED FROM AN UI THREAD. <br/>
    /// If this form is already in the process of fading, this fade will be put into queue and executed as soon
    /// as the first fade completes. Otherwise starts immediately.
    /// </summary>
    /// <param name="fade">The fade type.</param>
    /// <param name="startedCallback">A callback that will be called when the fade starts.</param>
    public void StartFade(Fades fade, Action? startedCallback = null)
    {
        if (RunningFade != null)
        {
            Queued.Enqueue((fade, startedCallback));
            return;
        }

        RunningFade = fade;
        Timer.Start();
        startedCallback?.Invoke();
    }

    private void HandleTimerTick(object sender, EventArgs e)
    {
        if (RunningFade == Fades.In) HandleFadeInTick();
        else if (RunningFade == Fades.Out) HandleFadeOutTick();
    }

    private void HandleFadeOutTick()
    {
        if (TargetForm.Opacity <= 0) EndFade();
        else TargetForm.Opacity -= Step;
    }

    private void HandleFadeInTick()
    {
        if (TargetForm.Opacity >= MaxOpacity) EndFade();
        else TargetForm.Opacity += Step;
    }
    
    private void EndFade()
    {
        Timer.Stop();
        RunningFade = null;
        if (Queued.Count == 0) return;
        var nextFade = Queued.Dequeue();
        StartFade(nextFade.fade, nextFade.startedCallback);
    }
}