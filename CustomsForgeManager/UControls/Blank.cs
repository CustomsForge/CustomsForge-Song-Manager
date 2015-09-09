using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CustomsForgeManager.CustomsForgeManagerLib.Objects;

//
// Docking.Fill causes screen flicker so only use if needed
//

namespace CustomsForgeManager.UControls
{
    public partial class Blank : UserControl
    {
        public Blank()
        {
            InitializeComponent();
            PopulateBlank(); // only done one time
        }

        public void PopulateBlank()
        {
            Globals.Log("Populating (insert tab name here) GUI ...");
        }

    }
}
