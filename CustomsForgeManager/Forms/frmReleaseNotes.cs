using System;
using System.IO;
using System.Windows.Forms;

namespace CustomsForgeManager.Forms
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
            finally
            {
                tbNotes.Select(0,0);
            }
        }
    }
}
