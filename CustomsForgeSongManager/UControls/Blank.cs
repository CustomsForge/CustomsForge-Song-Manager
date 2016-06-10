using System.Windows.Forms;
using CustomsForgeSongManager.DataObjects;

//
// Docking.Fill causes screen flicker so only use if needed
//

namespace CustomsForgeSongManager.UControls
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