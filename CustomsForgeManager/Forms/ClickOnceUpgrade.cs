using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace CustomsForgeManager.Forms
{
    public partial class ClickOnceUpgrade : Form
    {
#if RELEASE
        const string UpdateURL = "http://app.customsforge.com/release";
#else
        const string UpdateURL = "http://appdev.cfmanager.com/beta";
#endif
        const string SetupURL = UpdateURL + "/CFSMSetup.exe";

        public ClickOnceUpgrade()
        {
            InitializeComponent();
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
    }
}
