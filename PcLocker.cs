using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Wellbeing
{
    public class PcLocker
    {
        public bool Locked { get; private set; }
        private readonly SessionChangeHandler LockHandler = new();
        private readonly BlackBackground Overlay = new();

        public PcLocker(Form owner)
        {
            owner.Owner = Overlay;
        }
        public void Lock()
        {
            if (Locked)
                return;

            Locked = true;
            Overlay.Show();
            LockHandler.RegisterHandler();
            LockHandler.MachineUnlocked += HandleMachineUnlocked;
            SessionChangeHandler.LockWorkStation();
        }
        
        private void HandleMachineUnlocked(object sender, EventArgs e)
        {
            if (!Locked)
            {
                Logger.Log("Unlock event was not removed properly in PcLocker from SessionChangeHandler.");
                return;
            }
        }

        public void Unlock()
        {
            if (!Locked)
                return;
            
            LockHandler.MachineUnlocked -= HandleMachineUnlocked;
            LockHandler.UnregisterHandler();
            Overlay.Hide();
            Locked = false;
        }
    }
}