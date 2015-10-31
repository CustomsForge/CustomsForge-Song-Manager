using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using CustomsForgeManager.CustomsForgeManagerLib.Objects;
using CustomsForgeManager.Forms;
using System.Drawing;

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
            Process.Start(Constants.EOFURL);
        }

        private void btnRSTKSite_Click(object sender, EventArgs e)
        {
            Process.Start("http://www.rscustom.net/");
        }

        private void btnCFSMSite_Click(object sender, EventArgs e)
        {
            Process.Start("http://cfmanager.com");
        }


        private void lnkDonations_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(Constants.CustomsForgeURL + "donate/");
        }

        private void lnkFAQ_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(Constants.CustomsForgeURL + "faq/");
        }

        private void lnkForum_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(Constants.CustomsForgeURL + "/forum/81-customsforge-song-manager/");
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
            Process.Start(Constants.CustomsForgeURL);
        }

        private void lnkIgnition_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(Constants.IgnitionURL);
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
            Process.Start(Constants.RequestURL + "/?b");
        }


        private void lnkVideos_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(Constants.CustomsForgeURL + "videos/");
        }


        private void picCF_Click(object sender, EventArgs e)
        {
            Process.Start(Constants.IgnitionURL);
        }

        private void btnCFSMSupport_Click(object sender, EventArgs e)
        {
            Process.Start(Constants.CustomsForgeURL + "/forum/81-customsforge-song-manager/");
        }

    }

    public sealed class LinkLabelStatic : LinkLabel
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


    public sealed class ProfileLinkLabel : LinkLabel
    {
        Bitmap img;
        Boolean? hasImage;
        public string URL { get; set; }
        public string ResourceImg { get; set; }

        public About GetAboutOwner()
        {
            var parent = Parent;
            while (parent != null)
            {
                if (parent is About)
                    return (About)parent;
                parent = parent.Parent;
            }
            return null;
        }

        protected override void OnLinkClicked(LinkLabelLinkClickedEventArgs e)
        {
            if (!String.IsNullOrEmpty(URL))
                Process.Start(String.Format(Constants.CustomsForgeUserURL_Format, URL));
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            if (GetAboutOwner() == null)
                return;

            if (img == null && !hasImage.HasValue)
            {
                img = (Bitmap)Properties.Resources.ResourceManager.GetObject(String.IsNullOrEmpty(ResourceImg) ? 
                    Text : ResourceImg);
       
                hasImage = img != null;
            }
            if (hasImage.Value)
            {
                var pbProfile = GetAboutOwner().pbProfile;
                pbProfile.Image = img;
                var p = new Point(Left + Width + 10, Top + 20);
                pbProfile.Location = p;
                pbProfile.Visible = true;
                pbProfile.BringToFront();
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            if (GetAboutOwner() == null)
                return;
            GetAboutOwner().pbProfile.Visible = false;
        }
    }

}
