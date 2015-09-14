using System;
using System.Deployment.Application;
using System.Drawing;
using System.IO;
using System.Threading;
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

        public frmMain(DLogger myLog, AppSettings mySettings)
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);

            this.FormClosed += frmMain_FormClosed; // moved here for better access
            // this.FormClosed += delegate { notifyIcon_Main = null; }; // gets rid of notifyer icon on closing
            this.Text = String.Format("{0} (v{1})", Constants.ApplicationName, Constants.CustomVersion());

            if (Constants.DebugMode)
            {
                this.TopMost = false;
                tsLabel_ShowHideLog.Text = "Hide Log ";
                scMain.Panel2Collapsed = false;
            }

            Globals.MyLog = myLog;
            Globals.MySettings = mySettings;
            Globals.Notifier = this.notifyIcon_Main;
            Globals.TsProgressBar_Main = this.tsProgressBar_Main;
            Globals.TsLabel_MainMsg = this.tsLabel_MainMsg;
            Globals.TsLabel_StatusMsg = this.tsLabel_StatusMsg;
            Globals.TsLabel_DisabledCounter = this.tsLabel_DisabledCounter;
            Globals.TsLabel_Cancel = this.tsLabel_Cancel;
            Globals.ResetToolStripGlobals();
            Globals.MyLog.AddTargetTextBox(tbLog);

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

            // eye candy while songs get parsed for first time
            var noteViewer = new frmNoteViewer();
            if (Program.FirstRun())
            {
                noteViewer.rtbNotes.Clear();
                noteViewer.rtbNotes.SelectionAlignment = HorizontalAlignment.Center;
                noteViewer.rtbNotes.Font = new Font("Arial", 14, FontStyle.Bold);
                noteViewer.rtbNotes.AppendText(Environment.NewLine + Environment.NewLine + Environment.NewLine);
                noteViewer.rtbNotes.AppendText("Starting up CFM for the first time.");
                noteViewer.rtbNotes.AppendText(Environment.NewLine + Environment.NewLine);
                noteViewer.rtbNotes.AppendText("Please be patient while your songs are being parsed.");
                noteViewer.rtbNotes.AppendText(Environment.NewLine + Environment.NewLine + Environment.NewLine);
                noteViewer.rtbNotes.SelectionBackColor = Color.Lime;
                noteViewer.rtbNotes.SelectedText = "Get Ready . . . Get Set . . .";
                noteViewer.rtbNotes.AppendText(Environment.NewLine + Environment.NewLine);
                noteViewer.Show();
            }

            // load settings
            Globals.Settings.LoadSettingsFromFile();

            if (Globals.MySettings.EnabledLogBaloon)
                Globals.MyLog.AddTargetNotifyIcon(Globals.Notifier);
            else
                Globals.MyLog.RemoveTargetNotifyIcon(Globals.Notifier);

            // load Song Manager Tab
            LoadSongManager();

            noteViewer.rtbNotes.SelectionBackColor = Color.Lime;
            noteViewer.rtbNotes.SelectedText = "Go!";
            noteViewer.rtbNotes.Refresh();
            Thread.Sleep(800);
            noteViewer.Close();
            noteViewer.Dispose();
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
            this.tpSongManager.Controls.Clear();
            this.tpSongManager.Controls.Add(Globals.SongManager);
            Globals.SongManager.Dock = DockStyle.Fill;
            Globals.SongManager.Location = UCLocation;
            Globals.SongManager.Size = UCSize;
        }

        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
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
            // get rid of leftover notifications
            Globals.MyLog.RemoveTargetNotifyIcon(Globals.Notifier);
            notifyIcon_Main.Icon = null;
            Dispose();
        }

        private void tcMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Docking.Fill causes screen flicker so only use if needed
            // reset toolstrip labels
            Globals.ResetToolStripGlobals();

            // get first four charters from tab control text
            switch (tcMain.SelectedTab.Text.Substring(0, 4).ToUpper())
            {
                // passing variables(objects) by value to UControl
                case "SONG":
                    LoadSongManager();
                    Globals.SongManager.UpdateToolStrip();
                    break;
                case "DUPL":
                    this.tpDuplicates.Controls.Clear();
                    this.tpDuplicates.Controls.Add(Globals.Duplicates);
                    Globals.Duplicates.Dock = DockStyle.Fill;
                    Globals.Duplicates.UpdateToolStrip();
                    Globals.Duplicates.Location = UCLocation;
                    Globals.Duplicates.Size = UCSize;
                    break;
                case "RENA":
                    this.tpRenamer.Controls.Clear();
                    this.tpRenamer.Controls.Add(Globals.Renamer);
                    Globals.Renamer.UpdateToolStrip();
                    Globals.Renamer.Location = UCLocation;
                    Globals.Renamer.Size = UCSize;
                    break;
                case "SETL":
                    this.tpSetlistManager.Controls.Clear();
                    this.tpSetlistManager.Controls.Add(Globals.SetlistManager);
                    Globals.SetlistManager.Dock = DockStyle.Fill;
                    Globals.SetlistManager.UpdateToolStrip();
                    Globals.SetlistManager.Location = UCLocation;
                    Globals.SetlistManager.Size = UCSize;
                    break;
                case "UTIL":
                    this.tpUtilities.Controls.Clear();
                    this.tpUtilities.Controls.Add(Globals.Utilities);
                    Globals.Utilities.Location = UCLocation;
                    Globals.Utilities.Size = UCSize;
                    break;
                case "SETT":
                    // using LeaveSongManager instead of EH SongMangager_Leave
                    Globals.SongManager.LeaveSongManager();
                    this.tpSettings.Controls.Clear();
                    this.tpSettings.Controls.Add(Globals.Settings);
                    Globals.Settings.Dock = DockStyle.Fill;
                    // TODO: auto detect column width and visibility changes and use conditional check    
                    // done everytime in case user changes column width or visiblity
                    Globals.Settings.PopulateSettings();
                    Globals.Settings.Location = UCLocation;
                    Globals.Settings.Size = UCSize;
                    break;
                case "ABOU":
                    this.tpAbout.Controls.Add(Globals.About);
                    Globals.About.Location = UCLocation;
                    Globals.About.Size = UCSize;
                    break;
            }
        }

        private void tsLabelCancel_Click(object sender, EventArgs e)
        {
            Globals.TsLabel_Cancel.Text = tsLabel_Cancel.Text == "Cancel" ? "Cancelling" : "Cancel";
            tsLabel_Cancel.Enabled = false;
        }

        private void tsLabelClearLog_Click(object sender, EventArgs e)
        {
            tbLog.Clear();
            Globals.TsProgressBar_Main.Value = 0;
        }

        private void tsLabelShowHideLog_Click(object sender, EventArgs e)
        {
            scMain.Panel2Collapsed = !scMain.Panel2Collapsed;
            tsLabel_ShowHideLog.Text = scMain.Panel2Collapsed ? "Show Log" : "Hide Log ";
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