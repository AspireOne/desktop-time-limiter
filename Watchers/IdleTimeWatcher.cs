using Wellbeing.Hook;

namespace Wellbeing;

public static class IdleTimeWatcher
{
    private static readonly MouseHook MouseHook = new();
    private static readonly KeyboardHook KeyboardHook = new();
    public static bool Running { get; private set; }
    private static volatile bool Moved;
    public static uint IdleTimeMillis;

    public static void Run()
    {
        if (Running)
            return;
        
        Running = true;

        MouseHook.MouseMove += _ => Moved = true;
        MouseHook.Install();
        
        KeyboardHook.KeyDown += _ => Moved = true;
        KeyboardHook.Install();
    }

    public static uint Update(uint periodMillis)
    {
        if (Moved)
            IdleTimeMillis = 0;
        else
            IdleTimeMillis += periodMillis;
        
        Moved = false;
        return IdleTimeMillis;
    }
}