using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows.Forms;

// ClickOnce installer/upgrade has been depricated

namespace CustomsForgeSongManager.Forms
{
    public partial class ClickOnceUpgrade : Form
    {
#if RELEASE

        const string UpdateURL = "http://appdev.cfmanager.com/release";
        const string SetupURL = UpdateURL + "/CFSMSetup.exe";

        public ClickOnceUpgrade()
        {
            InitializeComponent();
            this.button1.Click += new System.EventHandler(this.button1_Click);
        }
        

        private void button1_Click(object sender, EventArgs e)
        {
            lblInfo.Text += " ---UPDATING---";
            this.Cursor = Cursors.WaitCursor;
            this.Enabled = false;
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(SetupURL);
            webRequest.Method = "GET";
            webRequest.Timeout = 10000;
            HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();
            string s = Path.Combine(Path.GetTempPath(), "CFSMSetup.exe");
            using (var FS = File.Create(s))
            {
                response.GetResponseStream().CopyTo(FS);
            }
            Process.Start(s);

            Environment.Exit(0);
        }

       
#else
        public ClickOnceUpgrade()
        {
            InitializeComponent();
        }
#endif
    }
}