using System;
using System.Deployment.Application;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Reflection;
using CustomsForgeManager.CustomsForgeManagerLib.Objects;
using DLogNet;


namespace CustomsForgeManager.Forms
{
    public partial class frmMain : Form
    {
        private static Point UCLocation = new Point(5, 10);
        private static Size UCSize = new Size(990, 490);
        private static NotifyIcon notifier;
        private static ToolStripLabel tsCancel;
        private static ToolStripLabel tsDisabledCounter;
        private static ToolStripLabel tsMainMsg;
        private static ToolStripProgressBar tsProgressBar;
        private static ToolStripLabel tsStatusMsg;
        public Control currentControl = null;

        public frmMain(DLogger myLog)
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);
            this.FormClosing += frmMain_FormClosing;
            this.FormClosed += frmMain_FormClosed; // moved here for better access
            // gets rid of notifier icon on closing
            this.FormClosed += delegate
            {
                notifyIcon_Main.Visible = false;
                notifyIcon_Main.Dispose();
                notifyIcon_Main = null;
            };

            this.Text = String.Format("{0} (v{1})", Constants.ApplicationName, Constants.CustomVersion());
            // bring CFM to the front on startup
            this.WindowState = FormWindowState.Minimized;


            Globals.MyLog = myLog;
            Globals.Notifier = this.notifyIcon_Main;
            Globals.TsProgressBar_Main = this.tsProgressBar_Main;
            Globals.TsLabel_MainMsg = this.tsLabel_MainMsg;
            Globals.TsLabel_StatusMsg = this.tsLabel_StatusMsg;
            Globals.TsLabel_DisabledCounter = this.tsLabel_DisabledCounter;
            Globals.TsLabel_Cancel = this.tsLabel_Cancel;
            Globals.ResetToolStripGlobals();
            Globals.MyLog.AddTargetTextBox(tbLog);

            Globals.OnScanEvent += (s, e) =>
            {
                tcMain.InvokeIfRequired(a =>
                    {
                        tcMain.Enabled = !e.IsScanning;
                    }
                );
            };

            // create application directory structure if it does not exist
            if (!Directory.Exists(Constants.WorkDirectory))
            {
                Directory.CreateDirectory(Constants.WorkDirectory);
                Globals.Log(String.Format("Created working directory: {0}", Constants.WorkDirectory));
            }

            // get server version of application
            if (ApplicationDeployment.IsNetworkDeployed)
                Globals.Log(String.Format("Application loaded, using version: {0}", ApplicationDeployment.CurrentDeployment.CurrentVersion));

            // initialize all global variables
            Globals.Log(String.Format("CFSongManager Version: {0}", Constants.CustomVersion()));
            Globals.Log(GetRSTKLibVersion());

            // load settings
            Globals.Settings.LoadSettingsFromFile();

            if (AppSettings.Instance.ShowLogWindow)
            {
                tsLabel_ShowHideLog.Text = "Hide Log ";
                scMain.Panel2Collapsed = false;
            }

            AppSettings.Instance.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == "ShowLogWindow")
                {
                    scMain.Panel2Collapsed = !AppSettings.Instance.ShowLogWindow;
                    tsLabel_ShowHideLog.Text = scMain.Panel2Collapsed ? "Show Log" : "Hide Log ";
                }
            };

            this.Show();
            this.WindowState = AppSettings.Instance.FullScreen ? FormWindowState.Maximized : FormWindowState.Normal;


            if (AppSettings.Instance.EnabledLogBaloon)
                Globals.MyLog.AddTargetNotifyIcon(Globals.Notifier);
            else
                Globals.MyLog.RemoveTargetNotifyIcon(Globals.Notifier);

            // load Song Manager Tab
            LoadSongManager();
            //CustomsForgeManagerLib.Extensions.Benchmark(LoadSongManager, 1);
        }

        private frmMain()
        {
            throw new Exception("Improper constructor used");
        }

        private string GetRSTKLibVersion()
        {
            Assembly assembly = Assembly.LoadFrom("RocksmithToolkitLib.dll");
            Version ver = assembly.GetName().Version;
            return String.Format("RocksmithToolkitLib Version: {0}", ver);
        }

        private void LoadSongManager()
        {
            if (!tpSongManager.Controls.Contains(Globals.SongManager))
            {
                this.tpSongManager.Controls.Clear();
                this.tpSongManager.Controls.Add(Globals.SongManager);
                Globals.SongManager.Dock = DockStyle.Fill;
                Globals.SongManager.Location = UCLocation;
                Globals.SongManager.Size = UCSize;
            }
        }

        private void ShowHelp()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Stream stream = assembly.GetManifestResourceStream("CustomsForgeManager.Resources.HelpSongMgr.txt");
            using (StreamReader reader = new StreamReader(stream))
            {
                var helpSongManager = reader.ReadToEnd();

                using (var noteViewer = new frmNoteViewer())
                {
                    noteViewer.Text = String.Format("{0} . . . {1}", noteViewer.Text, "Song Manager Help");
                    noteViewer.PopulateText(helpSongManager);
                    noteViewer.ShowDialog();
                }
            }
        }

        private void ShowHideLog()
        {
            AppSettings.Instance.ShowLogWindow = !AppSettings.Instance.ShowLogWindow;
        }

        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            AppSettings.Instance.FullScreen = WindowState == FormWindowState.Maximized;

            // get rid of leftover notifications
            Globals.MyLog.RemoveTargetNotifyIcon(Globals.Notifier);
            notifyIcon_Main.Visible = false;
            notifyIcon_Main.Dispose();
            notifyIcon_Main.Icon = null;
            Dispose();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            Globals.Log("Application is Closing");

            if (Globals.Settings == null || Globals.SongManager == null)
            {
                Globals.Log("<ERROR>: Save on close failed ...");
                return;
            }

            Globals.SongManager.LeaveSongManager();
            Globals.Settings.SaveSettingsToFile();
            Globals.SongManager.SaveSongCollectionToFile();
        }

        private void frmMain_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F1:
                    ShowHelp();
                    e.Handled = true;
                    break;
                case Keys.F3:
                    ShowHideLog();
                    e.Handled = true;
                    break;
                case Keys.F12:
                    tstripContainer.TopToolStripPanelVisible = !tstripContainer.TopToolStripPanelVisible;
                    e.Handled = true;
                    break;
            }
        }

        private void tcMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Docking.Fill causes screen flicker so only use if needed
            // reset toolstrip labels
            Globals.ResetToolStripGlobals();

            if (currentControl != null)
                if (currentControl is CustomsForgeManagerLib.INotifyTabChanged)
                    (currentControl as CustomsForgeManagerLib.INotifyTabChanged).TabLeave();


            // get first four charters from tab control text
            switch (tcMain.SelectedTab.Text.Substring(0, 4).ToUpper())
            {
                // passing variables(objects) by value to UControl
                case "SONG":
                    LoadSongManager();
                    Globals.SongManager.UpdateToolStrip();
                    currentControl = Globals.SongManager;
                    break;
                case "DUPL":
                    this.tpDuplicates.Controls.Clear();
                    this.tpDuplicates.Controls.Add(Globals.Duplicates);
                    Globals.Duplicates.Dock = DockStyle.Fill;
                    Globals.Duplicates.UpdateToolStrip();
                    Globals.Duplicates.Location = UCLocation;
                    Globals.Duplicates.Size = UCSize;
                    currentControl = Globals.Duplicates;
                    break;
                case "RENA":
                    this.tpRenamer.Controls.Clear();
                    this.tpRenamer.Controls.Add(Globals.Renamer);
                    Globals.Renamer.UpdateToolStrip();
                    Globals.Renamer.Location = UCLocation;
                    Globals.Renamer.Size = UCSize;
                    currentControl = Globals.Renamer;
                    break;
                case "SETL":
                    this.tpSetlistManager.Controls.Clear();
                    this.tpSetlistManager.Controls.Add(Globals.SetlistManager);
                    Globals.SetlistManager.Dock = DockStyle.Fill;
                    Globals.SetlistManager.UpdateToolStrip();
                    Globals.SetlistManager.Location = UCLocation;
                    Globals.SetlistManager.Size = UCSize;
                    currentControl = Globals.SetlistManager;
                    break;
#if TAGGER
                case "TAGG":
                    this.tpTagger.Controls.Clear();
                    this.tpTagger.Controls.Add(Globals.Tagger);
                    Globals.Tagger.Dock = DockStyle.Fill;
                    Globals.Tagger.PopulateTagger();
                    Globals.Tagger.UpdateToolStrip();
                    Globals.Tagger.Location = UCLocation;
                    Globals.Tagger.Size = UCSize;
                    currentControl = Globals.Tagger;
                    break;
#endif
                case "SETT":
                    // using LeaveSongManager instead of EH SongMangager_Leave
                    Globals.SongManager.LeaveSongManager();
                    this.tpSettings.Controls.Clear();
                    this.tpSettings.Controls.Add(Globals.Settings);
                    Globals.Settings.Dock = DockStyle.Fill;
                    // TODO: auto detect column width and visibility changes and use conditional check    
                    // done everytime in case user changes column width or visibility
                    Globals.Settings.PopulateSettings();
                    Globals.Settings.Location = UCLocation;
                    Globals.Settings.Size = UCSize;
                    currentControl = Globals.Settings;
                    break;
                case "ABOU":
                    this.tpAbout.Controls.Add(Globals.About);
                    Globals.About.Location = UCLocation;
                    Globals.About.Size = UCSize;
                    currentControl = Globals.About;
                    break;
            }

            if (currentControl != null)
                if (currentControl is CustomsForgeManagerLib.INotifyTabChanged)
                    (currentControl as CustomsForgeManagerLib.INotifyTabChanged).TabEnter();
        }

        private void tsBtnBackup_Click(object sender, EventArgs e)
        {
            Globals.TsProgressBar_Main.Value = 50;
            CustomsForgeManagerLib.Extensions.BackupRocksmithProfile();
            Globals.TsProgressBar_Main.Value = 100;
        }

        private void tsBtnHelp_Click(object sender, EventArgs e)
        {
            ShowHelp();
        }

        private void tsBtnLaunchRS_Click(object sender, EventArgs e)
        {
            CustomsForgeManagerLib.Extensions.LaunchRocksmith2014();
        }

        private void tsBtnRequest_Click(object sender, EventArgs e)
        {
            CustomsForgeManagerLib.Extensions.RequestSongOnCustomsForge();
        }

        private void tsBtnUpload_Click(object sender, EventArgs e)
        {
            CustomsForgeManagerLib.Extensions.UploadToCustomsForge();
        }

        private void tsLabelCancel_Click(object sender, EventArgs e)
        {
            Globals.TsLabel_Cancel.Text = tsLabel_Cancel.Text == "Cancel" ? "Canceling" : "Cancel";
            tsLabel_Cancel.Enabled = false;
        }

        private void tsLabelClearLog_Click(object sender, EventArgs e)
        {
            tbLog.Clear();
            Globals.TsProgressBar_Main.Value = 0;
        }

        private void tsLabelShowHideLog_Click(object sender, EventArgs e)
        {
            ShowHideLog();
        }

        public static NotifyIcon Notifier
        {
            get { return notifier; }
            set { notifier = value; }
        }

        public static ToolStripLabel TsCancel
        {
            get { return tsCancel; }
            set { tsCancel = value; }
        }

        public static ToolStripLabel TsDisabledCounter
        {
            get { return tsDisabledCounter; }
            set { tsDisabledCounter = value; }
        }

        public static ToolStripLabel TsMainMsg
        {
            get { return tsMainMsg; }
            set { tsMainMsg = value; }
        }

        public static ToolStripProgressBar TsProgressBar
        {
            get { return tsProgressBar; }
            set { tsProgressBar = value; }
        }

        public static ToolStripLabel TsStatusMsg
        {
            get { return tsStatusMsg; }
            set { tsStatusMsg = value; }
        }
    }
}