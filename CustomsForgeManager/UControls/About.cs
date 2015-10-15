using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using CustomsForgeManager.CustomsForgeManagerLib.Objects;
using CustomsForgeManager.Forms;

namespace CustomsForgeManager.UControls
{
    public partial class About : UserControl
    {
        public About()
        {
            InitializeComponent();
            PopulateAbout(); // only done one time
        }

        public void PopulateAbout()
        {
            Globals.Log("Populating About GUI ...");
        }

        private void btnEOFSite_Click(object sender, EventArgs e)
        {
            Process.Start("http://ignition.customsforge.com/eof");
        }

        private void btnRSTKSite_Click(object sender, EventArgs e)
        {
            Process.Start("http://www.rscustom.net/");
        }

        private void lnkDarjuszProfile_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://customsforge.com/user/5299-darjusz/");
        }

        private void lnkDonations_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://customsforge.com/donate/");
            Process.Start("http://goo.gl/jREeJF");
        }

        private void lnkFAQ_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://customsforge.com/faq/");
        }

        private void lnkForum_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://customsforge.com/forum/81-customsforge-song-manager/");
        }

        private void lnkHelp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Stream stream = assembly.GetManifestResourceStream("CustomsForgeManager.Resources.HelpGeneral.txt");
            using (StreamReader reader = new StreamReader(stream))
            {
                var helpGeneral = reader.ReadToEnd();

                using (var noteViewer = new frmNoteViewer())
                {
                    noteViewer.Text = String.Format("{0} . . . {1}", noteViewer.Text, "General Help");
                    noteViewer.PopulateText(helpGeneral);
                    noteViewer.ShowDialog();
                }
            }
        }

        private void lnkHomePage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://customsforge.com/");
        }

        private void lnkIgnition_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://ignition.customsforge.com/");
        }

        private void lnkLovromanProfile_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://customsforge.com/user/43194-lovroman/");
        }

        private void lnkOthers_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // TODO: complete this
            Process.Start("http://customsforge.com/user/345-forgeon/");
        }

        private void lnkReleaseNotes_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // ensures proper disposal of objects and variables
            using (var noteViewer = new frmNoteViewer())
            {
                noteViewer.Text = String.Format("{0} . . . {1}", noteViewer.Text, "About");
                noteViewer.PopulateText(File.ReadAllText("ReleaseNotes.txt"));
                noteViewer.ShowDialog();
            }
        }

        private void lnkRequests_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://requests.customsforge.com/?b");
        }

        private void lnkUnleashedProfile_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://customsforge.com/user/1-unleashed2k/");
        }

        private void lnkVideos_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://customsforge.com/videos/");
        }

        private void lnkZerkzProfile_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://customsforge.com/user/20759-zerkz/");
        }

        private void picCF_Click(object sender, EventArgs e)
        {
            Process.Start("http://search.customsforge.com/");
        }

        private void lnkCozy1Profile_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://customsforge.com/user/4293-cozy1/");
        }
    }
}
