using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DF.WinForms.ThemeLib
{
    public static class ThemedControl
    {
        public static void SetTheme(this Control c, Theme theme)
        {
            c.Font = theme.Font;
            c.BackColor = theme.ControlColor;
            c.ForeColor = theme.TextColor;
        }

        
    }

}
