using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;
using CFSM.GenTools;
using CustomsForgeSongManager.DataObjects;
using CustomsForgeSongManager.Forms;
using CustomsForgeSongManager.LocalTools;

namespace CustomsForgeSongManager.UControls
{
    public partial class About : UserControl
    {
        private bool downloadComplete;
        private bool downloadError;

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
            Process.Start(Constants.RSToolkitURL);
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
            Stream stream = assembly.GetManifestResourceStream("CustomsForgeSongManager.Resources.HelpGeneral.txt");
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

        private void lnkDeployRSTK_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
             // check system config, RSTK website http://www.rscustom.net/ uses TSL 1.2 encryption
            var errMsg = String.Empty;
            var ieVers = SysExtensions.GetBrowserVersion(SysExtensions.GetInternetExplorerVersion());
            if (ieVers < 8.0)
                errMsg = "Internet Explorer 8 or greater is required";

            var sysVers = SysExtensions.MajorVersion + (double)SysExtensions.MinorVersion / 10;
            if (sysVers < 6.1)
                errMsg = !String.IsNullOrEmpty(errMsg) ?
                    errMsg + Environment.NewLine + "and OS Windows 7 or greater is required" :
                    "OS Windows 7 or greater is required";

            if (!String.IsNullOrEmpty(errMsg))
            {
                errMsg = errMsg + Environment.NewLine + "to download Rocksmith Custom Songs Toolkit.";
                BetterDialog.ShowDialog(errMsg, "Incompatible System Configuration", null, null, "Ok", Bitmap.FromHicon(SystemIcons.Warning.Handle), "Warning", 150, 150);
                return;
            }

            ToggleUIControls(false);
            var dirRSTK = Path.Combine(Constants.WorkDirectory, "RSTK");

            if (!Directory.Exists(dirRSTK))
                Directory.CreateDirectory(dirRSTK);

            // TODO: save RSTK preferences
            Globals.Log("Extracting RSTK Beta Download Link ...");
            var urlLinksRSTK = ExtractUrlLinks(Constants.RSToolkitURL);

            // latest_test.zip data for testing
            // urlLinksRSTK.Add(Path.Combine(Constants.RSToolkitURL, "builds", "latest_test.zip"));

            if (urlLinksRSTK.Any())
            {
                // update beta version number here
                var downloadLink = urlLinksRSTK.FirstOrDefault(url =>
                    url.ToLower().Contains("rstoolkit-2.7.1.0-") &&
                    url.ToLower().Contains("-win.zip"));

                if (downloadLink == null)
                {
                    Globals.Log("RSTK Beta Download Link ... NOT FOUND");
                    ToggleUIControls(true);
                    return;
                }

                var zipFileName = Path.GetFileName(downloadLink);

                if (DownloadWebApp(downloadLink, zipFileName, dirRSTK))
                {
                    Globals.Log("RSTK Download ... SUCESSFUL");

                    if (ZipUtilities.UnzipDir(Path.Combine(dirRSTK, zipFileName), dirRSTK))
                    {
                        File.Delete(Path.Combine(dirRSTK, zipFileName));
                        Globals.Log("RSTK Archive Unpacked ... SUCESSFUL");

                        var exePath = Path.Combine(dirRSTK, "RocksmithToolkitGUI.exe");
                        var iconPath = Path.Combine(dirRSTK, "songcreator.ico");

                        GenExtensions.AddShortcut(Environment.SpecialFolder.Programs, exeShortcutLink: "RSTK.lnk",
                        exePath: exePath, exeIconPath: iconPath, shortcutDescription: "Rocksmith Custom Song Toolkit",
                        destSubDirectory: "Rocksmith Custom Song Toolkit");

                        Globals.Log("RSTK shortcut added to Start Menu, Programs ... SUCESSFUL");
                    }
                    else
                        Globals.Log("RSTK Archive Unpacked ... FAILED");
                }
                else
                    Globals.Log("RSTK Download ... FAILED");
            }
            else
                Globals.Log("Link Extraction ... FAILED");

            ToggleUIControls(true);
        }

        private List<string> ExtractUrlLinks(string webUrl, int attempts = 4)
        {
            var urlLinks = new List<string>();
            var webClient = new WebClient();

            for (int i = 0; i < attempts; i++)
            {
                try
                {
                    byte[] buffer = webClient.DownloadData(webUrl);
                    string html = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                    List<string> links = LinkExtractor.Extract(html);

                    foreach (var link in links)
                    {
                        urlLinks.Add(link);
                    }

                    return urlLinks;
                }
                catch (WebException ex)
                {
                    Globals.Log("Web Exception: " + ex.Message + " ...");
                }
                catch (NotSupportedException ex)
                {
                    Globals.Log("Not Supported Exception: " + ex.Message + " ...");
                }
                Thread.Sleep(200);
            }

            Globals.Log("No internet connection detected ...");
            return urlLinks;

            //    for (int i = 0; i < attempts; i++)
            //    {
            //        var htmlDoc = new HtmlAgilityPack.HtmlDocument();
            //        htmlDoc.Load(webUrl);

            //        if (htmlDoc.ParseErrors != null && htmlDoc.ParseErrors.Any())
            //            Globals.Log("Parsing Errors: " + i + " ...");
            //        else
            //        {
            //            var links = htmlDoc.DocumentNode.SelectNodes("//a[@href]");

            //            foreach (HtmlNode link in links)
            //            {
            //                urlLinks.Add(link.InnerText);
            //            }
            //        }
            //    }

            //    return urlLinks;
        }

        private bool DownloadWebApp(string webUrl, string appFileName, string downloadDir, int attempts = 4)
        {
            // requires Net 4.5, or Win7 and IE8
            // use TLS 1.2 protocol if available
            // ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;

            for (int i = 0; i < attempts; i++)
            {
                try
                {
                    using (var webClient = new WebClient())
                    {
                        // async download with progress
                        downloadComplete = false;
                        downloadError = false;
                        webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(wcCompleted);
                        webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(wcProgressChanged);
                        webClient.DownloadFileAsync(new Uri(webUrl), Path.Combine(downloadDir, appFileName));

                        while (!downloadComplete && !downloadError)
                        {
                            Application.DoEvents();
                            Thread.Sleep(100);
                        }

                        if (downloadError)
                            throw new WebException("The remote name could not be resolved: " + webUrl);

                        return true;

                        // direct download - no feedback
                        //webClient.DownloadFile(webUrl, Path.Combine(downloadDir, appFileName));

                        // pooling data prevents writing an empty file - no feedback
                        //byte[] downloadedBytes = webClient.DownloadData(webUrl);

                        //if (downloadedBytes.Length != 0)
                        //{
                        //    Stream file = File.Open(Path.Combine(downloadDir, appFileName), FileMode.Create);
                        //    file.Write(downloadedBytes, 0, downloadedBytes.Length);
                        //    file.Close();
                        //    return true;
                        //}
                    }
                }
                catch (WebException ex)
                {
                    Globals.Log("Web Exception: " + ex.Message + " ...");
                }
                catch (NotSupportedException ex)
                {
                    Globals.Log("Not Supported Exception: " + ex.Message + " ...");
                }
                Thread.Sleep(200);
            }

            Globals.Log("No internet connection detected ...");
            return false;
        }

        private void lnkDeployEOF_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ToggleUIControls(false);
            var dirEOF = Path.Combine(Constants.WorkDirectory, "EOF");

            if (!Directory.Exists(dirEOF))
                Directory.CreateDirectory(dirEOF);

            // TODO: save EOF preferences
            Globals.Log("Downloading EOF ...");

            if (DownloadWebApp(Constants.EOFURL + @"/download/3", "EOF.zip", dirEOF))
            {
                Globals.Log("EOF Download ... SUCESSFUL");

                if (ZipUtilities.UnzipDir(Path.Combine(dirEOF, "EOF.zip"), dirEOF))
                {
                    File.Delete(Path.Combine(dirEOF, "EOF.zip"));
                    Globals.Log("EOF Archive Unpacked ... SUCESSFUL");

                    var dirInfo = new DirectoryInfo(dirEOF);
                    DirectoryInfo[] subDirs = dirInfo.GetDirectories();
                    var exePath = Path.Combine(dirEOF, subDirs[0].Name, "eof.exe");
                    var iconPath = Path.Combine(dirEOF, subDirs[0].Name, "EOF20.ico");

                    GenExtensions.AddShortcut(Environment.SpecialFolder.Programs,
                    exeShortcutLink: "EOF.lnk", exePath: exePath, exeIconPath: iconPath,
                    shortcutDescription: "Editor on Fire", destSubDirectory: "Editor on Fire");

                    Globals.Log("EOF shortcut added to Start Menu, Programs ... SUCESSFUL");
                }
                else
                    Globals.Log("EOF Archive Unpacked ... FAILED");
            }
            else
                Globals.Log("EOF Download ... FAILED");

            ToggleUIControls(true);
        }

        private void lnkDeployCGT_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // CGT application deployment test site
            ToggleUIControls(false);
            var dirCGT = Path.Combine(Constants.WorkDirectory, "CGT");

            if (!Directory.Exists(dirCGT))
                Directory.CreateDirectory(dirCGT);

            // TODO: save CGT preferences
            Globals.Log("Downloading CGT ...");

            if (DownloadWebApp(@"https://goo.gl/aZ1gXR", "CustomGameToolkitSetup.rar", dirCGT))
            {
                Globals.Log("CGT Download ... SUCESSFUL");

                if (ZipUtilities.UnzipDir(Path.Combine(dirCGT, "CustomGameToolkitSetup.rar"), dirCGT))
                {
                    File.Delete(Path.Combine(dirCGT, "CustomGameToolkitSetup.rar"));
                    Globals.Log("CGT Archive Unpacked ... SUCESSFUL");

                    GenExtensions.RunExtExe(Path.Combine(dirCGT, "CustomGameToolkitSetup.exe"), false, arguments: @"/SP /VERYSILENT /SUPPRESSMSGBOXES");

                    Globals.Log("CGT shortcut added to Start Menu ... SUCESSFUL");
                }
                else
                    Globals.Log("CGT Archive Unpacked ... FAILED");
            }
            else
                Globals.Log("CGT Download ... FAILED");
            
            ToggleUIControls(true);
        }

        private void wcProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            GenExtensions.InvokeIfRequired(Globals.TsProgressBar_Main.GetCurrentParent(), delegate
            {
                Globals.TsProgressBar_Main.Value = e.ProgressPercentage;
            });
        }

        private void wcCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                GenExtensions.InvokeIfRequired(Globals.TsProgressBar_Main.GetCurrentParent(), delegate
                    {
                        Globals.TsProgressBar_Main.Value = 100;
                    });

                downloadComplete = true;
                Globals.Log("Download Completed ...");
            }
            else
                downloadError = true;
        }

        private void btnCGTSite_Click(object sender, EventArgs e)
        {
            Process.Start("https://goo.gl/hJVyLB");
        }

        private void ToggleUIControls(bool enable)
        {

            GenExtensions.InvokeIfRequired(lnkDeployRSTK, delegate { lnkDeployRSTK.Enabled = enable; });
            GenExtensions.InvokeIfRequired(lnkDeployEOF, delegate { lnkDeployEOF.Enabled = enable; });
            GenExtensions.InvokeIfRequired(lnkDeployCGT, delegate { lnkDeployCGT.Enabled = enable; });
         }


    }

    public class LinkExtractor
    {
        /// <summary>
        /// Extracts all src and href links from a HTML string.
        /// </summary>
        /// <param name="html">The html source</param>
        /// <returns>A list of links - these will be all links including javascript ones.</returns>
        public static List<string> Extract(string html)
        {
            List<string> list = new List<string>();
            Regex regex = new Regex("(?:href|src)=[\"|']?(.*?)[\"|'|>]+", RegexOptions.Singleline | RegexOptions.CultureInvariant);

            if (regex.IsMatch(html))
            {
                foreach (Match match in regex.Matches(html))
                {
                    list.Add(match.Groups[1].Value);
                }
            }

            return list;
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
        private Bitmap img;
        private Boolean? hasImage;
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
                img = (Bitmap)Properties.Resources.ResourceManager.GetObject(String.IsNullOrEmpty(ResourceImg) ? Text : ResourceImg);

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

// used with AsyncDownload
//webClient.DownloadFileCompleted -= new AsyncCompletedEventHandler(wcCompleted);
//webClient.DownloadProgressChanged -= new DownloadProgressChangedEventHandler(wcProgressChanged);
//webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(wcCompleted);
//webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(wcProgressChanged);
//webClient.DownloadFileAsync(new Uri(webUrl), Path.Combine(downloadDir, appFileName));

//Task task = Task.Factory.StartNew(() => webClient.DownloadFileAsync(new Uri(webUrl), Path.Combine(downloadDir, appFileName)));
//// this method may not be desirable but at least the GUI stays responsive during task
//while (!task.IsCompleted)
//{
//    Application.DoEvents();
//    Thread.Sleep(100);
//}

//if (task.IsCanceled || !task.IsFaulted)
//    Globals.Log("Download ... FAILED");
//else
//{
//    Globals.Log("Download ... SUCESSFUL");
//    return true;
//} 

