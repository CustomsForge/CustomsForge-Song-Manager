using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CustomsForgeSongManager.DataObjects;
using CustomsForgeSongManager.Forms;
using CustomsForgeSongManager.LocalTools;
using GenTools;
using RocksmithToolkitLib.XmlRepository;

namespace CustomsForgeSongManager.UControls
{
    public partial class About : UserControl
    {
        public About()
        {
            InitializeComponent();
            PopulateAbout(); // only done one time

            // display Christmas logo
            //if (DateTimeExtensions.TisTheSeason())
                //this.picCF.Image = CustomsForgeSongManager.Properties.Resources.christmas;
        }

        public void PopulateAbout()
        {
            Globals.Log("Populating About GUI ...");
        }

        private void ToggleUIControls(bool enable)
        {
            GenExtensions.InvokeIfRequired(lnkDeployRSTK, delegate { lnkDeployRSTK.Enabled = enable; });
            GenExtensions.InvokeIfRequired(lnkDeployEOF, delegate { lnkDeployEOF.Enabled = enable; });
            GenExtensions.InvokeIfRequired(lnkDeployCGT, delegate { lnkDeployCGT.Enabled = enable; });
        }

        private void btnCFSMSite_Click(object sender, EventArgs e)
        {
            Process.Start("https://cfmanager.com");
        }

        private void btnCFSMSupport_Click(object sender, EventArgs e)
        {
            Process.Start(Constants.CustomsForgeURL + "index.php?/81-customsforge-song-manager/");
        }

        private void btnEOFSite_Click(object sender, EventArgs e)
        {
            Process.Start(Constants.EOFURL);
        }

        private void btnRSTKSite_Click(object sender, EventArgs e)
        {
            Process.Start(Constants.RSToolkitURL); 
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
                GenExtensions.InvokeIfRequired(Globals.TsProgressBar_Main.GetCurrentParent(), delegate { Globals.TsProgressBar_Main.Value = 0; });
                ZipUtilities.DeleteDirectory(downloadDir);
                Directory.CreateDirectory(downloadDir);
                Globals.Log("Downloading WebApp: " + appArchive + " ...");

                // TODO: generate a programatic click to update google stats 
                Process.Start(new ProcessStartInfo(versInfoUrl)
                {
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden
                });

                if (AutoUpdater.DownloadWebApp(downloadUrl, appArchive, downloadDir))
                {
                    Globals.Log(appExe + " download ... SUCCESSFUL");
                    GenExtensions.InvokeIfRequired(Globals.TsProgressBar_Main.GetCurrentParent(), delegate { Globals.TsProgressBar_Main.Value = 30; });

                    if (ZipUtilities.UnrarDir(Path.Combine(downloadDir, appArchive), downloadDir))
                    {
                        File.Delete(Path.Combine(downloadDir, appArchive));
                        Globals.Log(appAcro + " Archive Unpacked ... SUCCESSFUL");
                        GenExtensions.InvokeIfRequired(Globals.TsProgressBar_Main.GetCurrentParent(), delegate { Globals.TsProgressBar_Main.Value = 60; });

                        var ret = GenExtensions.RunExtExe(Path.Combine(downloadDir, appSetup), false, arguments: @"/SP /VERYSILENT /SUPPRESSMSGBOXES ");
                        //if (AutoUpdater.UpdateWithInno(Path.Combine(downloadDir, appSetup)))                  
                        if (String.IsNullOrEmpty(ret))
                        {
                            // CGT user prefs are auto preserved by CGT (not overwritten)
                            Globals.Log(appAcro + " shortcut added to Start Menu ... SUCCESSFUL");
                            Globals.Log(appAcro + " Update ... SUCCESSFUL");
                            GenExtensions.InvokeIfRequired(Globals.TsProgressBar_Main.GetCurrentParent(), delegate { Globals.TsProgressBar_Main.Value = 100; });
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
                GenExtensions.InvokeIfRequired(Globals.TsProgressBar_Main.GetCurrentParent(), delegate { Globals.TsProgressBar_Main.Value = 0; });
                // save user prefs
                Globals.Log(appExe + " [" + versInstalled + "] needs updating ...");

                if (File.Exists(Path.Combine(downloadDir, "appCfg")))
                    File.Copy(Path.Combine(downloadDir, appCfg), Path.Combine(Path.GetTempPath(), appCfg));

                ZipUtilities.DeleteDirectory(downloadDir);
                Directory.CreateDirectory(downloadDir);

                if (AutoUpdater.DownloadWebApp(updateUrl, appArchive, downloadDir))
                {
                    Globals.Log(appArchive + " download ... SUCCESSFUL");
                    GenExtensions.InvokeIfRequired(Globals.TsProgressBar_Main.GetCurrentParent(), delegate { Globals.TsProgressBar_Main.Value = 30; });

                    if (ZipUtilities.UnzipDir(Path.Combine(downloadDir, appArchive), downloadDir))
                    {
                        File.Delete(Path.Combine(downloadDir, appArchive));
                        Globals.Log(appArchive + " archive unpacked ... SUCCESSFUL");
                        GenExtensions.InvokeIfRequired(Globals.TsProgressBar_Main.GetCurrentParent(), delegate { Globals.TsProgressBar_Main.Value = 50; });

                        if (File.Exists(Path.Combine(Constants.WorkFolder, appCfg)))
                        {
                            File.Copy(Path.Combine(Path.GetTempPath(), appCfg), Path.Combine(downloadDir, appCfg));
                            Globals.Log(appAcro + " configuration restored ...");
                            GenExtensions.InvokeIfRequired(Globals.TsProgressBar_Main.GetCurrentParent(), delegate { Globals.TsProgressBar_Main.Value = 70; });
                        }

                        var dirInfo = new DirectoryInfo(downloadDir);
                        DirectoryInfo[] subDirs = dirInfo.GetDirectories();
                        var exePath = Path.Combine(downloadDir, subDirs[0].Name, appExe);
                        var iconPath = Path.Combine(downloadDir, subDirs[0].Name, "EOF20.ico");

                        GenExtensions.AddShortcut(Environment.SpecialFolder.Programs, exeShortcutLink: "EOF.lnk", exePath: exePath, exeIconPath: iconPath, shortcutDescription: "Editor on Fire", destSubDirectory: "Editor on Fire");

                        Globals.Log(appAcro + " shortcut added to Start Menu, Programs ... SUCCESSFUL");
                        GenExtensions.InvokeIfRequired(Globals.TsProgressBar_Main.GetCurrentParent(), delegate { Globals.TsProgressBar_Main.Value = 100; });
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

        private void lnkDeployRSTK_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {           
            ToggleUIControls(false);
            const string updateUrl = Constants.RSToolkitURL;
            const string versInfoUrl = updateUrl + "/builds/latest_test";
            const string appExe = "RocksmithToolkitGUI.exe";
            const string appAcro = "RSTK";
            const string dlSubDir = appAcro + "_BIN";
            var downloadDir = Path.Combine(Constants.WorkFolder, dlSubDir);
            var appExePath = Path.Combine(downloadDir, "RocksmithToolkit", appExe);

            if (AutoUpdater.NeedsUpdate(appExePath, versInfoUrl))
            {
                // Fire up a progress bar
                GenExtensions.InvokeIfRequired(Globals.TsProgressBar_Main.GetCurrentParent(), delegate { Globals.TsProgressBar_Main.Value = 0; });

                // save user prefs
                Globals.Log(appAcro + " needs updating ...");
                if (File.Exists(Path.Combine(downloadDir, "RocksmithToolkitLib.TuningDefinition.xml")))
                {
                    File.Copy(Path.Combine(downloadDir, "RocksmithToolkitLib.Config.xml"), Path.Combine(Path.GetTempPath(), "RocksmithToolkitLib.Config.xml"));
                    File.Copy(Path.Combine(downloadDir, "RocksmithToolkitLib.SongAppId.xml"), Path.Combine(Path.GetTempPath(), "RocksmithToolkitLib.SongAppId.xml"));
                    File.Copy(Path.Combine(downloadDir, "RocksmithToolkitLib.TuningDefinition.xml"), Path.Combine(Path.GetTempPath(), "RocksmithToolkitLib.TuningDefinition.xml"));
                }

                GenExtensions.InvokeIfRequired(Globals.TsProgressBar_Main.GetCurrentParent(), delegate { Globals.TsProgressBar_Main.Value = 10; });
                ZipUtilities.DeleteDirectory(downloadDir);
                Directory.CreateDirectory(downloadDir);

                Globals.Log("Extracting " + appAcro + " Download ...");
                var urlLinks = new List<string>();
                // urlLinks = AutoUpdater.ExtractUrlData(updateUrl);
                // using latest_test.zip (much faster than ExctractUrlData method)
                urlLinks.Add(Path.Combine(Constants.RSToolkitURL, "builds", "latest_test.zip"));
                GenExtensions.InvokeIfRequired(Globals.TsProgressBar_Main.GetCurrentParent(), delegate { Globals.TsProgressBar_Main.Value = 30; });

                if (urlLinks.Any())
                {
                    // TODO: manually update beta version number here
                    // var downloadLink = urlLinks.FirstOrDefault(url => url.ToLower().Contains("rstoolkit-2.9.2.0-") && url.ToLower().Contains("-win.zip"));
                    var downloadLink = urlLinks.FirstOrDefault(url => url.ToLower().EndsWith("latest_test.zip"));

                    if (downloadLink == null)
                    {
                        Globals.Log(appAcro + "  Download ... NOT FOUND");
                        ToggleUIControls(true);
                        return;
                    }

                    var appArchive = Path.GetFileName(downloadLink);

                    if (AutoUpdater.DownloadWebApp(downloadLink, appArchive, downloadDir))
                    {
                        Globals.Log(appAcro + " Download ... SUCCESSFUL");
                        GenExtensions.InvokeIfRequired(Globals.TsProgressBar_Main.GetCurrentParent(), delegate { Globals.TsProgressBar_Main.Value = 50; });

                        if (ZipUtilities.UnzipDir(Path.Combine(downloadDir, appArchive), downloadDir))
                        {
                            File.Delete(Path.Combine(downloadDir, appArchive));
                            Globals.Log(appAcro + " Archive unpacked ... SUCCESSFUL");
                            GenExtensions.InvokeIfRequired(Globals.TsProgressBar_Main.GetCurrentParent(), delegate { Globals.TsProgressBar_Main.Value = 70; });

                            // use the toolkit new directory structure
                            downloadDir = Path.Combine(downloadDir, "RocksmithToolkit");

                            if (File.Exists(Path.Combine(Constants.WorkFolder, "RocksmithToolkitLib.TuningDefinition.xml")))
                            {
                                XmlRepository<TuningDefinition> tuning = new TuningDefinitionRepository();
                                XmlRepository<Config> config = new ConfigRepository();
                                XmlRepository<SongAppId> appid = new SongAppIdRepository();
                                config.Merge(Path.Combine(Path.GetTempPath(), "RocksmithToolkitLib.Config.xml"), Path.Combine(downloadDir, "RocksmithToolkitLib.Config.xml"));
                                appid.Merge(Path.Combine(Path.GetTempPath(), "RocksmithToolkitLib.SongAppId.xml"), Path.Combine(downloadDir, "RocksmithToolkitLib.SongAppId.xml"));
                                tuning.Merge(Path.Combine(Path.GetTempPath(), "RocksmithToolkitLib.TuningDefinition.xml"), Path.Combine(downloadDir, "RocksmithToolkitLib.TuningDefinition.xml"));
                                Globals.Log(appAcro + " Configurations Restored ...");
                                GenExtensions.InvokeIfRequired(Globals.TsProgressBar_Main.GetCurrentParent(), delegate { Globals.TsProgressBar_Main.Value = 80; });
                            }

                            var exePath = Path.Combine(downloadDir, appExe);
                            var iconPath = Path.Combine(downloadDir, "songcreator.ico");
                            GenExtensions.AddShortcut(Environment.SpecialFolder.Programs, exeShortcutLink: "RocksmithToolkit.lnk", exePath: exePath, exeIconPath: iconPath, shortcutDescription: "Rocksmith Custom Song Toolkit", destSubDirectory: "RocksmithToolkit");
                            Globals.Log(appAcro + " Shortcut added to Start Menu, Programs ... SUCCESSFUL");
                            GenExtensions.InvokeIfRequired(Globals.TsProgressBar_Main.GetCurrentParent(), delegate { Globals.TsProgressBar_Main.Value = 100; });
                        }
                        else
                            Globals.Log(appAcro + " Archive unpacked ... FAILED");
                    }
                    else
                        Globals.Log(appAcro + " Download ... FAILED");
                }
                else
                    Globals.Log("Link Extraction ... FAILED");
            }

            ToggleUIControls(true);
        }

        private void lnkDonations_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(Constants.CustomsForgeURL + "index.php?/donate/");
        }

        private void lnkFAQ_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(Constants.CustomsForgeURL + "index.php?/faq/");
        }

        private void lnkForum_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(Constants.CustomsForgeURL + "index.php?/forum/122-issues/");
        }

        private void lnkHelp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmNoteViewer.ViewResourcesFile("CustomsForgeSongManager.Resources.HelpGeneral.rtf", "General Help");
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
            frmNoteViewer.ViewExternalFile("ReleaseNotes.txt", "Release Notes");
        }

        private void lnkRequests_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(Constants.RequestURL);
        }

        private void lnkUserProfile_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var linkLabel = sender as CustomControls.LinkLabelPopup;
            var linkUrl = linkLabel.UrlLink;

            if (!String.IsNullOrEmpty(linkUrl))
                Process.Start(String.Format(Constants.CustomsForgeUserURL_Format, linkUrl));
        }

        private void lnkVideos_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(Constants.CustomsForgeURL + "index.php?/videos/");
        }

        private void picCF_Click(object sender, EventArgs e)
        {
            Process.Start(Constants.IgnitionURL);
        }

        private void lnkOfficialGuide_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(Constants.CustomsForgeURL + "index.php?/topic/51771-customsforge-song-manager-official-guide-2019/");
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void link_Developer1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void link_CFOwner_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }


    }
}



