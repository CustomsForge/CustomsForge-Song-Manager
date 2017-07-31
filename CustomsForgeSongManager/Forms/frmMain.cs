using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using GenTools;
using CustomsForgeSongManager.DataObjects;
using CustomsForgeSongManager.LocalTools;
using CustomsForgeSongManager.UITheme;
using RocksmithToolkitLib.XML;
using RocksmithToolkitLib.DLCPackage;
using RocksmithToolkitLib.PsarcLoader;
using RocksmithToolkitLib;
using System.Diagnostics;
using DGVTools = DataGridViewTools;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
<<<<<<< HEAD
=======
using CustomControls;
>>>>>>> origin/develop


// NOTE: the app is designed for default user screen resolution of 1024x768
// dev screen resolution should be set to this when designing forms and controls
// all png images are 16x16 resolution for buttons, unless higher resolution for some other use

// NOTE: any usage of 'public enum' in code that will be obfuscated must be preceeded with
// [Obfuscation(Exclude = false, Feature = "-rename")]
// so that ConfuserEx does not rename the enumerators
//
namespace CustomsForgeSongManager.Forms
{
    public partial class frmMain : Form, IMainForm //,ThemedForm
    {
        private static Point UCLocation = new Point(5, 10);
        private static Size UCSize = new Size(990, 490);
        private Control currentControl = null;

        public delegate void PlayCall();
        private event PlayCall playFunction;

        public frmMain(DLogNet.DLogger myLog)
        {
            InitializeComponent();

            // create VersionInfo.txt file
            VersionInfo.CreateVersionInfo();

            //this will initialize classes that need to be initialized right away.
            TypeExtensions.InitializeClasses(new string[] { "UTILS_INIT", "CFSM_INIT" }, new Type[] { }, new object[] { });

            // prevent toolstrip from growing/changing at runtime
            // toolstrip may appear changed in design mode (this is a known VS bug)
            TopToolStripPanel.MaximumSize = new Size(0, 28); // force height and makes tsLable_Tagger positioning work
            tsUtilities.AutoSize = false; // a key to preventing movement
            tsAudioPlayer.AutoSize = false; // a key to preventing movement
            tsUtilities.Location = new Point(0, 0); // force location
            tsAudioPlayer.Location = new Point(tsUtilities.Width + 20, 0); // force location

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);

            // gets rid of notifier icon on closing
            this.FormClosed += delegate
            {
                Globals.MyLog.RemoveTargetNotifyIcon(Globals.Notifier);
                notifyIcon_Main.Visible = false;
                notifyIcon_Main.Dispose();
                notifyIcon_Main.Icon = null;
                Dispose();
            };

            var strFormatVersion = "{0} (v{1})";
#if BETA
            strFormatVersion = "{0} (v{1} - BETA VERSION)";
#endif
#if RELEASE
            strFormatVersion = "{0} (v{1} - RELEASE VERSION)";
#endif
#if DEBUG
            strFormatVersion = "{0} (v{1} - DEBUG)";
#endif
            Constants.AppTitle = String.Format(strFormatVersion, Constants.ApplicationName, Constants.CustomVersion());
            this.Text = Constants.AppTitle;
            // bring CFSM to the front on startup
            this.WindowState = FormWindowState.Minimized;

            Globals.MyLog = myLog;
            Globals.Notifier = this.notifyIcon_Main;
            Globals.TsProgressBar_Main = this.tsProgressBar_Main;
            Globals.TsLabel_MainMsg = this.tsLabel_MainMsg;
            Globals.TsLabel_StatusMsg = this.tsLabel_StatusMsg;
            Globals.TsLabel_DisabledCounter = this.tsLabel_DisabledCounter;
            Globals.TsLabel_Cancel = this.tsLabel_Cancel;
            Globals.TbLog = this.tbLog;
            Globals.ResetToolStripGlobals();
            Globals.MyLog.AddTargetTextBox(tbLog);
            //    Globals.CFMTheme.AddListener(this);

            Globals.OnScanEvent += (s, e) => { tcMain.InvokeIfRequired(a => { tcMain.Enabled = !e.IsScanning; }); };

            // verify application directory structure
            FileTools.VerifyCfsmFolders();

            // initialize all global variables
            Globals.Log(Constants.AppTitle);
            Globals.Log(GetRSTKLibVersion());

            // load settings
            Globals.Settings.LoadSettingsFromFile(null); // null => workaround to prevent showing 'Loaded appSettings.xml file ...' 2X

            if (AppSettings.Instance.ShowLogWindow)
            {
                tsLabel_ShowHideLog.Text = Properties.Resources.HideLog;
                scMain.Panel2Collapsed = false;
            }

            AppSettings.Instance.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == "ShowLogWindow")
                {
                    scMain.Panel2Collapsed = !AppSettings.Instance.ShowLogWindow;
                    tsLabel_ShowHideLog.Text = scMain.Panel2Collapsed ? Properties.Resources.ShowLog : Properties.Resources.HideLog;
                }
            };

            this.Show();
            this.WindowState = AppSettings.Instance.FullScreen ? FormWindowState.Maximized : FormWindowState.Normal;

            if (AppSettings.Instance.EnableLogBaloon)
                Globals.MyLog.AddTargetNotifyIcon(Globals.Notifier);
            else
                Globals.MyLog.RemoveTargetNotifyIcon(Globals.Notifier);

            tsAudioPlayer.Visible = true;

            playFunction += new PlayCall(PlaySong);

            // load Song Manager Tab
            LoadSongManager();

            //CustomsForgeSongManagerLib.Extensions.Benchmark(LoadSongManager, 1);
        }

        public frmMain()
        {
            //  throw new Exception("Improper constructor used");
        }

        private string GetRSTKLibVersion()
        {
            return String.Format("RocksmithToolkitLib Version: {0}", Assembly.LoadFrom("RocksmithToolkitLib.dll").GetName().Version);
        }

        private void LoadSongManager()
        {
            if (!tpSongManager.Controls.Contains(Globals.SongManager))
            {
                this.tpSongManager.Controls.Clear();
                this.tpSongManager.Controls.Add(Globals.SongManager);
                Globals.SongManager.PlaySongFunction = playFunction;
                Globals.SongManager.Dock = DockStyle.Fill;
                Globals.SongManager.Location = UCLocation;
                Globals.SongManager.Size = UCSize;
            }
            currentControl = Globals.SongManager;
        }

        private void ShowHelp()
        {
            frmNoteViewer.ViewResourcesFile("CustomsForgeSongManager.Resources.HelpGeneral.rtf", "General Help");
        }

        private void ShowHideLog()
        {
            AppSettings.Instance.ShowLogWindow = !AppSettings.Instance.ShowLogWindow;
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            AppSettings.Instance.FullScreen = WindowState == FormWindowState.Maximized;
            AppSettings.Instance.WindowWidth = this.Width;
            AppSettings.Instance.WindowHeight = this.Height;
            AppSettings.Instance.WindowTop = this.Location.Y;
            AppSettings.Instance.WindowLeft = this.Location.X;

            Globals.Log("Application is Closing");
            Globals.CancelBackgroundScan = true;

            if (Globals.Settings == null || Globals.SongManager == null)
            {
                Globals.Log("<ERROR>: Save on close failed ...");
                return;
            }

            if (AppSettings.Instance.CleanOnClosing)
            {
                // delete xml and log files (ensure clean startup)
                var cfsmFiles = Directory.EnumerateFiles(Constants.WorkFolder, "*", SearchOption.TopDirectoryOnly).ToList();
                foreach (var cfsmFile in cfsmFiles)
                    GenExtensions.DeleteFile(cfsmFile);

                if (Directory.Exists(Constants.SongPacksFolder))
                {
                    // don't delete files in 'original' subfolder
                    var songpackFiles = Directory.EnumerateFiles(Constants.SongPacksFolder, "*", SearchOption.TopDirectoryOnly).ToList();
                    foreach (var songpackFile in songpackFiles)
                        GenExtensions.DeleteFile(songpackFile);

                    ZipUtilities.DeleteDirectory(Constants.CachePcPath);
                }

                if (Directory.Exists(Constants.AudioCacheFolder))
                    ZipUtilities.DeleteDirectory(Constants.AudioCacheFolder);

                if (Directory.Exists(Constants.TaggerWorkingFolder))
                    ZipUtilities.DeleteDirectory(Constants.TaggerWorkingFolder);

                AppSettings.Instance.CleanOnClosing = false;
            }
            else
            {
                Globals.SongManager.SetRepairOptions();
                Globals.Settings.SaveSettingsToFile(Globals.DgvCurrent);
                Globals.SongManager.SaveSongCollectionToFile();
            }
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
                    tstripContainer.BottomToolStripPanelVisible = tstripContainer.TopToolStripPanelVisible;
                    e.Handled = true;
                    break;
                case Keys.F9:
                    using (ThemeDesigner ts = new ThemeDesigner())
                        ts.ShowDialog();
                    e.Handled = true;
                    break;
            }
        }

        private void tcMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            // reset toolstrip globals
            Globals.ResetToolStripGlobals();

            // quick fix ... visible on when Song Manager is active
            // avoids playback issues when other tabs are active 
            if (tcMain.SelectedIndex != 0)
            {
                Globals.AudioEngine.Stop();
                tsAudioPlayer.Visible = false;
            }
            else
                tsAudioPlayer.Visible = true;

            if (currentControl != null)
                if (currentControl is INotifyTabChanged)
                    (currentControl as INotifyTabChanged).TabLeave();

            switch (tcMain.SelectedTab.Text)
            {
                // passing variables(objects) by value to UControl
                case "Song Manager":
                    LoadSongManager();
                    Globals.SongManager.UpdateToolStrip();
                    break;
                case "Duplicates":
                    this.tpDuplicates.Controls.Clear();
                    this.tpDuplicates.Controls.Add(Globals.Duplicates);
                    Globals.Duplicates.Dock = DockStyle.Fill;
                    Globals.Duplicates.UpdateToolStrip();
                    Globals.Duplicates.Location = UCLocation;
                    Globals.Duplicates.Size = UCSize;
                    currentControl = Globals.Duplicates;
                    break;
                case "Renamer":
                    this.tpRenamer.Controls.Clear();
                    this.tpRenamer.Controls.Add(Globals.Renamer);
                    Globals.Renamer.Dock = DockStyle.Fill;
                    Globals.Renamer.UpdateToolStrip();
                    Globals.Renamer.Location = UCLocation;
                    Globals.Renamer.Size = UCSize;
                    currentControl = Globals.Renamer;
                    break;
                case "Setlist Manager":
                    this.tpSetlistManager.Controls.Clear();
                    this.tpSetlistManager.Controls.Add(Globals.SetlistManager);
                    Globals.SetlistManager.Dock = DockStyle.Fill;
                    Globals.SetlistManager.UpdateToolStrip();
                    Globals.SetlistManager.Location = UCLocation;
                    Globals.SetlistManager.Size = UCSize;
                    currentControl = Globals.SetlistManager;
                    break;
                case "Song Packs":
                    this.tpSongPacks.Controls.Clear();
                    this.tpSongPacks.Controls.Add(Globals.SongPacks);
                    Globals.SongPacks.Dock = DockStyle.Fill;
                    Globals.SongPacks.UpdateToolStrip();
                    Globals.SongPacks.Location = UCLocation;
                    Globals.SongPacks.Size = UCSize;
                    currentControl = Globals.SongPacks;
                    break;
                case "Settings":
                    this.tpSettings.Controls.Clear();
                    this.tpSettings.Controls.Add(Globals.Settings);
                    Globals.Settings.Dock = DockStyle.Fill;
                    Globals.Settings.PopulateSettings(Globals.DgvCurrent);
                    Globals.Settings.Location = UCLocation;
                    Globals.Settings.Size = UCSize;
                    currentControl = Globals.Settings;
                    break;
                case "About":
                    if (!tpAbout.Controls.Contains(tpAbout))
                        tpAbout.Controls.Add(Globals.About);
                    Globals.About.Location = UCLocation;
                    Globals.About.Size = UCSize;
                    currentControl = Globals.About;
                    break;
            }

            if (currentControl != null)
                if (currentControl is INotifyTabChanged)
                    (currentControl as INotifyTabChanged).TabEnter();
        }

        private void tsBtnUserProfiles_MouseUp(object sender, MouseEventArgs e)
        {
            Globals.TsProgressBar_Main.Value = 50;

            var resetProfileDirPath = false;
            if (e.Button == MouseButtons.Right)
                resetProfileDirPath = true;

            RocksmithProfile.BackupRestore(resetProfileDirPath);
            Globals.TsProgressBar_Main.Value = 100;
        }

        private void tsBtnHelp_Click(object sender, EventArgs e)
        {
            ShowHelp();
        }

        private void tsBtnLaunchRS_Click(object sender, EventArgs e)
        {
            LocalExtensions.LaunchRocksmith2014();
        }

        private void tsBtnRequest_Click(object sender, EventArgs e)
        {
            Ignition.RequestSongOnCustomsForge();
        }

        private void tsBtnUpload_Click(object sender, EventArgs e)
        {
            Ignition.UploadToCustomsForge();
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

        private void frmMain_Load(object sender, EventArgs e)
        {
#if RELEASE
            const string UpdateURL = "http://ignition.customsforge.com/cfsm_uploads";
#else
            const string UpdateURL = "http://ignition.customsforge.com/cfsm_uploads/beta";
#endif

            const string versInfoUrl = UpdateURL + "/VersionInfo.txt";
            const string appExe = "CustomsForgeSongManager.exe";
            const string appSetup = "CFSMSetup.exe";
            var appExePath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), appExe);

            if (AppSettings.Instance.EnableAutoUpdate)
                if (AutoUpdater.NeedsUpdate(appExePath, versInfoUrl))
                {
                    if (File.Exists(appSetup))
                        System.Diagnostics.Process.Start(appSetup, "-appupdate");
                    else
                        MessageBox.Show(appSetup + " not found, please download the program again.");
                }
        }

        private delegate void DoSomethingWithGridSelectionAction(DataGridView dg, IEnumerable<DataGridViewRow> selected, DataGridViewColumn colSel, List<int> IgnoreColums);

        private void DoSomethingWithGrid(DoSomethingWithGridSelectionAction action)
        {
            if (action != null && currentControl != null && currentControl is IDataGridViewHolder)
            {
                var dataGrid = (currentControl as IDataGridViewHolder).GetGrid();
                if (dataGrid != null)
                {
                    // selects all rows by default
                    var selected = dataGrid.Rows.Cast<DataGridViewRow>();
                    DataGridViewColumn colSel = null;
                    int colSelIdx = -1;
                    if (dataGrid.Columns.Contains("colSelect"))
                    {
                        colSel = dataGrid.Columns["colSelect"];
                        colSelIdx = colSel.Index;
                        var xselected = selected.Where(r => r.Cells["colSelect"].Value != null).Where(r => Convert.ToBoolean(r.Cells["colSelect"].Value)).ToList();
                        // select specific rows if colSelect is selected
                        if (xselected.Count > 0)
                            selected = xselected;
                    }

                    List<int> ignoreColumns = new List<int>();
                    if (colSel != null)
                        ignoreColumns.Add(colSel.Index);

                    foreach (var c in dataGrid.Columns.Cast<DataGridViewColumn>())
                    {
                        if (c is DataGridViewImageColumn || !c.Visible)
                        {
                            ignoreColumns.Add(c.Index);
                            continue;
                        }
                    }

                    action(dataGrid, selected, colSel, ignoreColumns);
                }
            }
        }

        public void SongListToBBCode()
        {
            DoSomethingWithGrid((dataGrid, selection, colSel, ignoreColumns) =>
            {
                var sbTXT = new StringBuilder();
                string columns = String.Empty;
                foreach (var c in dataGrid.Columns.Cast<DataGridViewColumn>())
                {
                    if (!ignoreColumns.Contains(c.Index))
                        columns += c.HeaderText + ", ";
                }

                sbTXT.AppendLine(columns.Trim(new char[] { ',', ' ' }));
                sbTXT.AppendLine(String.Format("[LIST={0}]", selection.Count()));

                foreach (var row in selection)
                {
                    string s = "[*]";
                    foreach (var col in row.Cells.Cast<DataGridViewCell>().Where(c => !ignoreColumns.Contains(c.ColumnIndex)))
                    {
                        s += col.Value == null ? " , " : col.Value + ", ";
                    }
                    sbTXT.AppendLine(s.Trim(new char[] { ',', ' ' }) + "[/*]");
                }
                sbTXT.AppendLine("[/LIST]");

                using (var noteViewer = new frmNoteViewer())
                {
                    noteViewer.Text = String.Format("{0} . . . {1}", noteViewer.Text, "Song list to BBCode");

                    noteViewer.PopulateText(sbTXT.ToString(), false);
                    noteViewer.ShowDialog();
                }
            });
        }

        public void SongListToCSV()
        {
            var path = Path.Combine(Constants.WorkFolder, "SongListCSV.csv");
            using (var sfdSongListToCSV = new SaveFileDialog() { Filter = "csv files(*.csv)|*.csv|All files (*.*)|*.*", FileName = path })

                if (sfdSongListToCSV.ShowDialog() == DialogResult.OK)
                {
                    path = sfdSongListToCSV.FileName;

                    DoSomethingWithGrid((dataGrid, selection, colSel, ignoreColumns) =>
                    {
                        var sbCSV = new StringBuilder();

                        const char csvSep = ';';
                        sbCSV.AppendLine(String.Format(@"sep={0}", csvSep)); // used by Excel to recognize seperator if Encoding.Unicode is used
                        string columns = String.Empty;
                        foreach (var c in dataGrid.Columns.Cast<DataGridViewColumn>())
                        {
                            if (!ignoreColumns.Contains(c.Index))
                                columns += c.HeaderText + csvSep;
                        }

                        sbCSV.AppendLine(columns.Trim(new char[] { csvSep, ' ' }));

                        foreach (var row in selection)
                        {
                            string s = "[*]";
                            foreach (var col in row.Cells.Cast<DataGridViewCell>().Where(c => !ignoreColumns.Contains(c.ColumnIndex)))
                            {
                                s += col.Value == null ? csvSep.ToString() : col.Value.ToString() + csvSep;
                            }
                            sbCSV.AppendLine(s.Trim(new char[] { ',', ' ' }));
                        }

                        //using (var noteViewer = new frmNoteViewer())
                        //{
                        //    noteViewer.Text = String.Format("{0} . . . {1}", noteViewer.Text, "Song list to CSV");

                        //    noteViewer.PopulateText(sbCSV.ToString(), false);
                        //    noteViewer.ShowDialog();
                        //}

                        try
                        {
                            using (StreamWriter file = new StreamWriter(path, false, Encoding.Unicode)) // Excel does not recognize UTF8
                                file.Write(sbCSV.ToString());

                            Globals.Log("Song list saved to:" + path);
                        }
                        catch (IOException ex)
                        {
                            Globals.Log("<Error>: " + ex.Message);
                        }
                    });
                }
        }

        public void SongListToHTML()
        {
            DoSomethingWithGrid((dataGrid, selection, colSel, ignoreColumns) =>
            {
                //in the future maybe add some javascript to sort the html table by column
                var sbTXT = new StringBuilder();
                sbTXT.AppendLine("<html><head>");

                sbTXT.AppendLine("<title>CFSM Songs</title>");
                //add the style directly to so it can be saved correctly without external css.
                sbTXT.AppendLine("<style>");
                sbTXT.AppendLine(Properties.Resources.htmExport);
                sbTXT.AppendLine("</style>");
                // sbTXT.AppendLine("<link rel='stylesheet' type='text/css' href='htmExport.css'>");
                sbTXT.AppendLine("</head><body>");

                sbTXT.AppendLine("<table id='CFMGrid'>");
                sbTXT.AppendLine("<tr>");
                var columns = String.Empty;
                foreach (var c in dataGrid.Columns.Cast<DataGridViewColumn>())
                {
                    if (!ignoreColumns.Contains(c.Index))
                        columns += ((char)9) + String.Format("<th>{0}</th>{1}", c.HeaderText, Environment.NewLine);
                }
                sbTXT.AppendLine(columns.Trim());
                sbTXT.AppendLine("</tr>");
                bool altOn = false;
                foreach (var row in selection)
                {
                    sbTXT.AppendLine("<tr" + (altOn ? " class='alt'>" : ">"));
                    string s = string.Empty;
                    foreach (var col in row.Cells.Cast<DataGridViewCell>().Where(c => !ignoreColumns.Contains(c.ColumnIndex)))
                    {
                        s += String.Format("<td>{0}</td>", col.Value == null ? "" : col.Value);
                    }
                    sbTXT.AppendLine(s.Trim());
                    sbTXT.AppendLine("</tr>");
                    altOn = !altOn;
                }
                sbTXT.AppendLine("</table>");
                sbTXT.AppendLine("</body></html>");
                using (var noteViewer = new frmHtmlViewer())
                {
                    noteViewer.Text = String.Format("{0} . . . {1}", noteViewer.Text, "Song list to HTML");
                    noteViewer.PopulateHtml(sbTXT.ToString());
                    noteViewer.ShowDialog();
                }
            });
        }

        private void SongListToJsonOrXml()
        {
            // TODO:
        }

        private void tsmiBBCode_Click(object sender, EventArgs e)
        {
            SongListToBBCode();
        }

        private void tsmiCSV_Click(object sender, EventArgs e)
        {
            SongListToCSV();
        }

        private void tsmiHTML_Click(object sender, EventArgs e)
        {
            SongListToHTML();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void frmMain_Shown(object sender, EventArgs e)
        {
            if (!AppSettings.Instance.FullScreen)
            {
                int width = this.Width;
                int height = this.Height;
                int top = this.Location.Y;
                int left = this.Location.X;

                if (AppSettings.Instance.WindowWidth > 0)
                    width = AppSettings.Instance.WindowWidth;
                if (AppSettings.Instance.WindowHeight > 0)
                    height = AppSettings.Instance.WindowHeight;
                if (AppSettings.Instance.WindowTop > 0)
                    top = AppSettings.Instance.WindowTop;
                if (AppSettings.Instance.WindowLeft > 0)
                    left = AppSettings.Instance.WindowLeft;

                this.SetBoundsCore(left, top, width, height, BoundsSpecified.All);
            }
        }

        public void PlaySong()
        {
            if (Globals.AudioEngine.IsPaused() || Globals.AudioEngine.IsPlaying())
            {
                if (Globals.AudioEngine.IsPlaying())
                    Globals.Log("Playback Paused ...");
                else
                    Globals.Log("Playback Unpaused ...");

                Globals.AudioEngine.Pause();
            }
            else
            {
                Globals.Log("Playback Started ...");
                Globals.SongManager.PlaySelectedSong();
            }

            timerAudioProgress.Enabled = (Globals.AudioEngine.IsPlaying());
        }

        private void tsbPlay_Click(object sender, EventArgs e)
        {
            PlaySong();
        }

        private void timerAudioProgress_Tick(object sender, EventArgs e)
        {
            tslblTimer.Text = Globals.AudioEngine.GetSongPosition();
            tspbAudioPosition.Value = Globals.AudioEngine.GetSongCompletedPercentage();
            tsAudioPlayer.Refresh();
        }

        private void tsbStop_Click(object sender, EventArgs e)
        {
            tslblTimer.Text = "00:00";
            tspbAudioPosition.Value = 0;
            Globals.Log("Playback Stopped ...");
            Globals.AudioEngine.Stop();
            timerAudioProgress.Enabled = (Globals.AudioEngine.IsPlaying());
        }

        private void tspbAudioPosition_MouseDown(object sender, MouseEventArgs e)
        {
            if (!Globals.AudioEngine.IsPlaying())
            {
                Globals.SongManager.PlaySelectedSong();
                timerAudioProgress.Enabled = (Globals.AudioEngine.IsPlaying());
            }

            if (Globals.AudioEngine.IsLoaded())
            {
                Globals.Log("Playback Seeking ...");
                var pos = (float)e.Location.X / (float)tspbAudioPosition.Width;
                Globals.AudioEngine.Seek(pos * Globals.AudioEngine.GetSongLength());
            }
        }

        public Control GetControl()
        {
            return this;
        }

        private class StatPair
        {
            public Dictionary<string, string> GeneralStats { get; set; }
            public Dictionary<string, int> ChordNums { get; set; }
        }

        private class AnalyzerInfo
        {
            public Dictionary<string, List<StatPair>> SongInfo { get; set; }
        }

        private void AnalyzerExport(string format)
        {
            var path = Path.Combine(Constants.WorkFolder, "Analyzer.csv");
            var filter = "csv files(*.csv)|*.csv|All files (*.*)|*.*";

            if (format == "json")
            {
                path = path.Replace("csv", "json");
                filter = filter.Replace("csv", "json");
            }

            using (var sfdSongListToCSV = new SaveFileDialog() { Filter = filter, FileName = path })
                if (sfdSongListToCSV.ShowDialog() == DialogResult.OK)
                {
                    path = sfdSongListToCSV.FileName;

                    string outputJSON = "";
                    var sbCSV = new StringBuilder();

                    const char csvSep = ';';
                    sbCSV.AppendLine(String.Format(@"sep={0}", csvSep));

                    string[] columns = { "Artist","Song name", "Path", "Notes", "Hammer-ons", "Pulloffs", "Harmonics", "Pinch harmonics", "Frethand mutes", "Palm mutes",
                                         "Plucks", "Slaps", "Slides", "Unpitched slides", "Tremolos", "Taps", "Vibratos", "Sustains", "Bends"};

                    int maxChordNumber = 0;
                    var dgvSelection = new List<DataGridViewRow>();
                    DoSomethingWithGrid((dataGrid, selection, colSel, ignoreColumns) =>
                    {
                        dgvSelection = selection.ToList();
                    });

                    string columnsCSV = "sep=" + csvSep.ToString();

                    var allInfo = new List<AnalyzerInfo>();

                    foreach (var songRow in dgvSelection)
                    {
                        var song = DGVTools.DgvExtensions.GetObjectFromRow<SongData>(songRow);

                        string s = song.Artist + " - " + song.Title;
                        var statPairList = new List<StatPair>();

                        if (!song.ExtraMetaDataScanned && song.NoteCount == 0)
                        {
<<<<<<< HEAD
                            Globals.Log("No arrangement data found in " + s);
=======
                            Globals.Log("Parsing Analyzer Data From: " + s);
>>>>>>> origin/develop

                            int sngIndex = Globals.SongCollection.IndexOf(song);

                            using (var browser = new PsarcBrowser(song.FilePath))
                            {
                                var songInfo = browser.GetSongData(true);

<<<<<<< HEAD
            //                    // TODO: FIXME what about songs.psarc and custom song packs?
=======
                                // TODO: should songs.psarc and custom song packs be addressed too
>>>>>>> origin/develop
                                if (song.FilePath.ToLower().Contains("rs1comp"))
                                    song = songInfo.FirstOrDefault(i => i.Title == song.Title);
                                else
                                    song = songInfo.First();

                                song.ExtraMetaDataScanned = true;
                                Globals.SongCollection[sngIndex] = song;
                            }
                        }

                        var arrangements = song.Arrangements2D;

                        try
                        {
                            var arrangementsWithChords = arrangements.Where(a => a.ChordCounts != null).ToList();

                            if (arrangementsWithChords != null && arrangementsWithChords.Count != 0) //Just an extra check
                            {
                                int nrOfDifferentChords = arrangementsWithChords.Max(ar => (int?)ar.ChordCounts.Count() ?? 0);

                                if (nrOfDifferentChords > maxChordNumber)
                                    maxChordNumber = nrOfDifferentChords;
                            }
                        }
                        catch (ArgumentNullException)
                        {
<<<<<<< HEAD
                            Globals.Log("Analyzer error: problem with getting a part of data for the song " + song.Title + " by " + song.Artist);
=======
                            Globals.Log("<ERROR> Could not get Analyzer Data for: " + song.Title + " by " + song.Artist);
>>>>>>> origin/develop
                        }

                        foreach (var arr in song.Arrangements2D)
                        {
                            if (arr.Name == "Vocals")
                                continue;

                            if (format == "csv")
                            {
                                //this Replace is used in order not to have to convert every one of these to string

                                s = song.Artist + csvSep + song.Title + csvSep + arr.Name + csvSep + " " + arr.NoteCount + csvSep + arr.HammerOnCount + csvSep + arr.PullOffCount + csvSep + arr.HarmonicCount
                                    + csvSep + arr.HarmonicPinchCount + csvSep + arr.FretHandMuteCount + csvSep + arr.PalmMuteCount + csvSep + arr.PluckCount + csvSep + arr.SlapCount + csvSep + arr.SlideCount
                                    + csvSep + arr.UnpitchedSlideCount + csvSep + arr.TremoloCount + csvSep + arr.TapCount + csvSep + arr.VibratoCount + csvSep + arr.SustainCount + csvSep + arr.BendCount + csvSep;

                                if (arr.ChordNames != null && arr.ChordNames.Count() == 0)
                                    s +=
                                        "no chords";
                                else
                                    for (int i = 0; i < arr.ChordNames.Count(); i++)
                                        s += arr.ChordNames[i] + csvSep + arr.ChordCounts[i] + csvSep;

                                sbCSV.AppendLine(s.Trim(new char[] { ',', ' ' }));
                            }
                            else if (format == "json")
                            {
                                var statPair = new StatPair();
                                statPair.GeneralStats = new Dictionary<string, string>();

                                statPair.GeneralStats.Add("Arrangement", arr.Name);
                                statPair.GeneralStats.Add("Note Count", arr.NoteCount.ToString());
                                statPair.GeneralStats.Add("Hammer-ons", arr.HammerOnCount.ToString());
                                statPair.GeneralStats.Add("Pulloffs", arr.PullOffCount.ToString());
                                statPair.GeneralStats.Add("Harmonics", arr.HarmonicCount.ToString());
                                statPair.GeneralStats.Add("Pinch harmonics", arr.HarmonicPinchCount.ToString());
                                statPair.GeneralStats.Add("Frethand mutes", arr.FretHandMuteCount.ToString());
                                statPair.GeneralStats.Add("Palm mutes", arr.PalmMuteCount.ToString());
                                statPair.GeneralStats.Add("Plucks", arr.PluckCount.ToString());
                                statPair.GeneralStats.Add("Slaps", arr.SlapCount.ToString());
                                statPair.GeneralStats.Add("Slides", arr.SlideCount.ToString());
                                statPair.GeneralStats.Add("Unpitched slides", arr.UnpitchedSlideCount.ToString());
                                statPair.GeneralStats.Add("Tremolos", arr.TremoloCount.ToString());
                                statPair.GeneralStats.Add("Vibratos", arr.VibratoCount.ToString());
                                statPair.GeneralStats.Add("Sustains", arr.SustainCount.ToString());
                                statPair.GeneralStats.Add("Bends", arr.BendCount.ToString());

                                statPair.ChordNums = new Dictionary<string, int>();
                                for (int i = 0; i < arr.ChordNames.Count; i++)
                                    statPair.ChordNums.Add(arr.ChordNames[i], arr.ChordCounts[i]);

                                statPairList.Add(statPair);
                            }
                        }

                        if (format == "json")
                        {
                            var analyzerInfo = new AnalyzerInfo();
                            analyzerInfo.SongInfo = new Dictionary<string, List<StatPair>>();
                            analyzerInfo.SongInfo.Add(s, statPairList);
<<<<<<< HEAD

=======
>>>>>>> origin/develop
                            allInfo.Add(analyzerInfo);
                        }
                        else
                            sbCSV.AppendLine("");
                    }


                    if (format == "csv")
                    {
                        string chordColumns = "Chord" + csvSep + "# of chord" + csvSep;
<<<<<<< HEAD

                        columnsCSV += Environment.NewLine + String.Join(csvSep.ToString(), columns);
                        columnsCSV += csvSep + string.Concat((Enumerable.Repeat(chordColumns, maxChordNumber)));

=======
                        columnsCSV += Environment.NewLine + String.Join(csvSep.ToString(), columns);
                        columnsCSV += csvSep + string.Concat((Enumerable.Repeat(chordColumns, maxChordNumber)));
>>>>>>> origin/develop
                        sbCSV.Insert(0, columnsCSV.Trim(new char[] { ',', ' ' }));
                    }

                    try
                    {
                        using (StreamWriter file = new StreamWriter(path, false, Encoding.Unicode))
                        {
<<<<<<< HEAD

=======
>>>>>>> origin/develop
                            if (format == "json")
                            {
                                JToken serializedJson = JsonConvert.SerializeObject(allInfo, Formatting.Indented);
                                outputJSON = Regex.Unescape(serializedJson.ToString());
<<<<<<< HEAD

=======
>>>>>>> origin/develop
                                file.Write(outputJSON);
                            }
                            else if (format == "csv")
                                file.Write(sbCSV.ToString());
                        }

<<<<<<< HEAD
                        Globals.Log("Song data saved to:" + path);
=======
                        Globals.Log("Analyzer Data Saved: " + path);
>>>>>>> origin/develop
                    }
                    catch (IOException ex)
                    {
                        Globals.Log("<Error>: " + ex.Message);
                    }

                    if (File.Exists(path))
                    {
                        if (MessageBox.Show("Do you want to open the exported file?", "Open the exported file", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                            Process.Start(path);
                    }
                }
        }

        private void tsmiAnalyzerCSV_Click(object sender, EventArgs e)
        {
            AnalyzerExport("csv");
        }

        private void tsmiAnalyzerJSON_Click(object sender, EventArgs e)
        {
            AnalyzerExport("json");
        }

    }
}