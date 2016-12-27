using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;

namespace TrueResize
{
    public class WizardControl : TabControl
    {
        protected override void WndProc(ref Message m)
        {
            // Hide tabs by trapping the TCM_ADJUSTRECT message
            if (m.Msg == 0x1328 && !DesignMode) m.Result = (IntPtr)1;
            else base.WndProc(ref m);
        }

        protected override void OnKeyDown(KeyEventArgs ke)
        {
            // Block Ctrl+Tab and Ctrl+Shift+Tab hotkeys
            if (ke.Control && ke.KeyCode == Keys.Tab)
            {
                return;
            }
            else if (ke.KeyCode == Keys.Left || ke.KeyCode == Keys.Right)
            {
                ke.Handled = true;
            }
            base.OnKeyDown(ke);
        }
    }
}
