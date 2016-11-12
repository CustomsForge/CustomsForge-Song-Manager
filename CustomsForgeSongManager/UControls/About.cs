using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Drawing;
using CFSM.GenTools;
using CustomsForgeSongManager.DataObjects;
using CustomsForgeSongManager.Forms;
using CustomsForgeSongManager.LocalTools;
using RocksmithToolkitLib;
using RocksmithToolkitLib.XmlRepository;

namespace CustomsForgeSongManager.UControls
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
            ToggleUIControls(false);
            const string updateUrl = Constants.RSToolkitURL;
            const string versInfoUrl = updateUrl + "/builds/latest_test";
            const string appExe = "RocksmithToolkitGUI.exe";
            const string appAcro = "RSTK";
            const string dlSubDir = appAcro + "_BIN";
            var downloadDir = Path.Combine(Constants.WorkFolder, dlSubDir);
            var appExePath = Path.Combine(downloadDir, appExe);

            if (AutoUpdater.NeedsUpdate(appExePath, versInfoUrl))
            {
                // save user prefs
                Globals.Log(appAcro + " needs updating ...");
                if (File.Exists(Path.Combine(downloadDir, "RocksmithToolkitLib.TuningDefinition.xml")))
                {
                    File.Copy(Path.Combine(downloadDir, "RocksmithToolkitLib.Config.xml"), Path.Combine(Path.GetTempPath(), "RocksmithToolkitLib.Config.xml"));
                    File.Copy(Path.Combine(downloadDir, "RocksmithToolkitLib.SongAppId.xml"), Path.Combine(Path.GetTempPath(), "RocksmithToolkitLib.SongAppId.xml"));
                    File.Copy(Path.Combine(downloadDir, "RocksmithToolkitLib.TuningDefinition.xml"), Path.Combine(Path.GetTempPath(), "RocksmithToolkitLib.TuningDefinition.xml"));
                }

                ZipUtilities.DeleteDirectory(downloadDir);
                Directory.CreateDirectory(downloadDir);

                Globals.Log("Extracting " + appAcro + " Beta download link ...");
                var urlLinks = AutoUpdater.ExtractUrlData(updateUrl);
                // latest_test.zip data for testing
                // urlLinks.Add(Path.Combine(Constants.RSToolkitURL, "builds", "latest_test.zip"));

                if (urlLinks.Any())
                {
                    // update beta version number here
                    var downloadLink = urlLinks.FirstOrDefault(url => url.ToLower()
                        .Contains("rstoolkit-2.7.1.0-") && url.ToLower()
                        .Contains("-win.zip"));

                    if (downloadLink == null)
                    {
                        Globals.Log(appAcro + "  Beta download link ... NOT FOUND");
                        ToggleUIControls(true);
                        return;
                    }

                    var appArchive = Path.GetFileName(downloadLink);

                    if (AutoUpdater.DownloadWebApp(downloadLink, appArchive, downloadDir))
                    {
                        Globals.Log(appAcro + " download ... SUCCESSFUL");

                        if (ZipUtilities.UnzipDir(Path.Combine(downloadDir, appArchive), downloadDir))
                        {
                            File.Delete(Path.Combine(downloadDir, appArchive));
                            Globals.Log(appAcro + " archive unpacked ... SUCCESSFUL");

                            if (File.Exists(Path.Combine(Constants.WorkFolder, "RocksmithToolkitLib.TuningDefinition.xml")))
                            {
                                XmlRepository<TuningDefinition> tuning = new TuningDefinitionRepository();
                                XmlRepository<Config> config = new ConfigRepository();
                                XmlRepository<SongAppId> appid = new SongAppIdRepository();
                                config.Merge(Path.Combine(Path.GetTempPath(), "RocksmithToolkitLib.Config.xml"), Path.Combine(downloadDir, "RocksmithToolkitLib.Config.xml"));
                                appid.Merge(Path.Combine(Path.GetTempPath(), "RocksmithToolkitLib.SongAppId.xml"), Path.Combine(downloadDir, "RocksmithToolkitLib.SongAppId.xml"));
                                tuning.Merge(Path.Combine(Path.GetTempPath(), "RocksmithToolkitLib.TuningDefinition.xml"), Path.Combine(downloadDir, "RocksmithToolkitLib.TuningDefinition.xml"));
                                Globals.Log(appAcro + " configurations restored ...");
                            }

                            var exePath = Path.Combine(downloadDir, appExe);
                            var iconPath = Path.Combine(downloadDir, "songcreator.ico");

                            GenExtensions.AddShortcut(Environment.SpecialFolder.Programs, exeShortcutLink: "RSTK.lnk", exePath: exePath, exeIconPath: iconPath, shortcutDescription: "Rocksmith Custom Song Toolkit", destSubDirectory: "Rocksmith Custom Song Toolkit");

                            Globals.Log(appAcro + " shortcut added to Start Menu, Programs ... SUCCESSFUL");
                        }
                        else
                            Globals.Log(appAcro + " archive unpacked ... FAILED");
                    }
                    else
                        Globals.Log(appAcro + " download ... FAILED");
                }
                else
                    Globals.Log("Link Extraction ... FAILED");
            }

            ToggleUIControls(true);
        }

        private void lnkDeployEOF_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ToggleUIControls(false);
            const string appExe = "eof.exe";
            const string appArchive = "eof.zip";
            const string appAcro = "EOF";
            const string appCfg = "eof.cfg";
            const string dlSubDir = appAcro + "_BIN";
            const string updateUrl = Constants.EOFURL + @"/download/3";
            var downloadDir = Path.Combine(Constants.WorkFolder, dlSubDir);

            // EOF stores VersionInfo in the html div class description
            // NeedsUpdate can not be used for non .NET apps
            var divTags = AutoUpdater.ExtractUrlData(Constants.EOFURL, "div");
            if (!divTags.Any()) // no internet connection
            {
                ToggleUIControls(true);
                return;
            }

            string versInstalled = "";
            var versOnline = divTags.FirstOrDefault(url => url.StartsWith("1.8 RC10"));
            if (versOnline == null)
            {
                Globals.Log(appAcro + " online version ... NOT FOUND");
                ToggleUIControls(true);
                return;
            }

            bool needsUpdate = false;
            if (!Directory.Exists(downloadDir))
                needsUpdate = true;
            else
            {
                var dirInfo = new DirectoryInfo(downloadDir);
                DirectoryInfo[] subDirs = dirInfo.GetDirectories();
                if (!subDirs.Any())
                    needsUpdate = true;
                else
                {
                    versInstalled = subDirs[0].Name.Trim();
                    versOnline = versOnline.Replace(" ", "").Trim();
                    if (!versInstalled.Contains(versOnline))
                        needsUpdate = true;
                }
            }

            if (needsUpdate)
            {
                // save user prefs
                Globals.Log(appExe + " [" + versInstalled + "] needs updating ...");

                if (File.Exists(Path.Combine(downloadDir, "appCfg")))
                    File.Copy(Path.Combine(downloadDir, appCfg), Path.Combine(Path.GetTempPath(), appCfg));

                ZipUtilities.DeleteDirectory(downloadDir);
                Directory.CreateDirectory(downloadDir);

                if (AutoUpdater.DownloadWebApp(updateUrl, appArchive, downloadDir))
                {
                    Globals.Log(appArchive + " download ... SUCCESSFUL");

                    if (ZipUtilities.UnzipDir(Path.Combine(downloadDir, appArchive), downloadDir))
                    {
                        File.Delete(Path.Combine(downloadDir, appArchive));
                        Globals.Log(appArchive + " archive unpacked ... SUCCESSFUL");

                        if (File.Exists(Path.Combine(Constants.WorkFolder, appCfg)))
                        {
                            File.Copy(Path.Combine(Path.GetTempPath(), appCfg), Path.Combine(downloadDir, appCfg));
                            Globals.Log(appAcro + " configuration restored ...");
                        }

                        var dirInfo = new DirectoryInfo(downloadDir);
                        DirectoryInfo[] subDirs = dirInfo.GetDirectories();
                        var exePath = Path.Combine(downloadDir, subDirs[0].Name, appExe);
                        var iconPath = Path.Combine(downloadDir, subDirs[0].Name, "EOF20.ico");

                        GenExtensions.AddShortcut(Environment.SpecialFolder.Programs, exeShortcutLink: "EOF.lnk", exePath: exePath, exeIconPath: iconPath, shortcutDescription: "Editor on Fire", destSubDirectory: "Editor on Fire");

                        Globals.Log(appAcro + " shortcut added to Start Menu, Programs ... SUCCESSFUL");
                    }
                    else
                        Globals.Log(appAcro + " archive unpacked ... FAILED");
                }
                else
                    Globals.Log(appExe + " download ... FAILED");
            }
            else
                Globals.Log(appExe + " [" + versInstalled + "] does not need updating ...");


            ToggleUIControls(true);
        }

        private void lnkDeployCGT_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // CGT application deployment test site
            ToggleUIControls(false);
            const string appExe = "CustomGameToolkit.exe";
            const string appArchive = "CustomGameToolkitSetup.rar";
            const string appSetup = "CustomGameToolkitSetup.exe";
            const string appAcro = "CGT";
            const string dlSubDir = appAcro + "_BIN";
            const string versInfoUrl = "https://goo.gl/K4y73H";
            const string downloadUrl = "https://goo.gl/qRBPFI";
            var downloadDir = Path.Combine(Constants.WorkFolder, dlSubDir);
            var appSetupPath = Path.Combine(downloadDir, appSetup);

            if (AutoUpdater.NeedsUpdate(appSetupPath, versInfoUrl))
            {
                ZipUtilities.DeleteDirectory(downloadDir);
                Directory.CreateDirectory(downloadDir);

                // TODO: generate a click for google stats 
                if (AutoUpdater.DownloadWebApp(downloadUrl, appArchive, downloadDir))
                {
                    Globals.Log(appExe + " download ... SUCCESSFUL");

                    if (ZipUtilities.UnzipDir(Path.Combine(downloadDir, appArchive), downloadDir))
                    {
                        File.Delete(Path.Combine(downloadDir, appArchive));
                        Globals.Log(appAcro + " Archive Unpacked ... SUCCESSFUL");

                        var ret = GenExtensions.RunExtExe(Path.Combine(downloadDir, appSetup), false, arguments: @"/SP /VERYSILENT /SUPPRESSMSGBOXES ");
                        //if (AutoUpdater.UpdateWithInno(Path.Combine(downloadDir, appSetup)))                  
                        if (String.IsNullOrEmpty(ret))
                        {
                            // CGT user prefs are auto preserved by CGT (not overwritten)
                            Globals.Log(appAcro + " shortcut added to Start Menu ... SUCCESSFUL");
                            Globals.Log(appAcro + " Update ... SUCCESSFUL");
                        }
                        else
                            Globals.Log(appAcro + " Update ... FAILED");
                    }
                    else
                        Globals.Log(appAcro + " archive unpacked ... FAILED");
                }
                else
                    Globals.Log(appExe + " download ... FAILED");
            }

            ToggleUIControls(true);
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
//    Globals.Log("Download ... SUCCESSFUL");
//    return true;
//} 

