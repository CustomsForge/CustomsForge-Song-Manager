using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomsForgeManager_Winforms.Forms
{
    public partial class frmReleaseNotes : Form
    {
        public frmReleaseNotes()
        {
            InitializeComponent();
            try
            {
                tbNotes.Text = File.ReadAllText("ReleaseNotes.txt");
            }
            catch (Exception)
            {
                tbNotes.Text = "Could not find release notes...";
            }
        }
    }
}
