using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms.VisualStyles;

namespace CustomControls
{
    /// <summary>
    /// Custom LinkLabel control that displays static text
    ///
    [RefreshProperties(RefreshProperties.Repaint)]
    [Browsable(true)]
    [EditorBrowsable(EditorBrowsableState.Always)]
    public class LinkLabelStatic : LinkLabel
    {
        protected override void OnMouseDown(MouseEventArgs e)
        {
            //do nothing...
        }

        protected override void WndProc(ref Message msg)
        {
            if (msg.Msg == 0x20)
            {
                // don't change the cursor
            }
            else
            {
                base.WndProc(ref msg);
            }
        }
    }

}

