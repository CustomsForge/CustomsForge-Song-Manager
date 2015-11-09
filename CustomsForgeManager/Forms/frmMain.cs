using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Reflection;
using CustomsForgeManager.CustomsForgeManagerLib.Objects;
using System.Linq;
using System.Text;
using CustomsForgeManager.CustomsForgeManagerLib;
using System.Collections.Generic;
using CFMAudioTools;


namespace CustomsForgeManager.Forms
{
    public partial class frmMain : Form, IMainForm, IThemeListener
    {
        private static Point UCLocation = new Point(5, 10);
        private static Size UCSize = new Size(990, 490);
        private Control currentControl = null;

        public frmMain(DLogNet.DLogger myLog)
        {
            InitializeComponent();

            // prevent toolstrip from growing/changing at runtime
            // toolstrip may appear changed in design mode (this is a known VS bug)
            TopToolStripPanel.MaximumSize = new Size(0, 28); // force height and makes tsLable_Tagger positioning work
            tsUtilities.AutoSize = false; // a key to preventing movement
            tsTagger.AutoSize = false; // a key to preventing movement
            tsUtilities.Location = new Point(0, 0); // force location
            tsTagger.Location = new Point(tsUtilities.Width + 10, 0); // force location

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);

            this.FormClosed += frmMain_FormClosed; // moved here for better access
            // gets rid of notifier icon on closing
            this.FormClosed += delegate
                {
                    notifyIcon_Main.Visible = false;
                    notifyIcon_Main.Dispose();
                    notifyIcon_Main = null;
                };
#if BETA
#if DEBUG
            var stringVersion = String.Format("{0} (v{1} - DEBUG)", Constants.ApplicationName, Constants.CustomVersion());
#else
            var stringVersion = String.Format("{0} (v{1} - BETA VERSION)", Constants.ApplicationName, Constants.CustomVersion());            
#endif
#else
            var stringVersion = String.Format("{0} (v{1})", Constants.ApplicationName, Constants.CustomVersion());
#endif
            this.Text = stringVersion;
            // bring CFM to the front on startup
            this.WindowState = FormWindowState.Minimized;


            Globals.MyLog = myLog;
            Globals.Notifier = this.notifyIcon_Main;
            Globals.TsProgressBar_Main = this.tsProgressBar_Main;
            Globals.TsLabel_MainMsg = this.tsLabel_MainMsg;
            Globals.TsLabel_StatusMsg = this.tsLabel_StatusMsg;
            Globals.TsLabel_DisabledCounter = this.tsLabel_DisabledCounter;
            Globals.TsLabel_Cancel = this.tsLabel_Cancel;
            Globals.TsComboBox_TaggerThemes = this.tscbTaggerThemes;
            Globals.ResetToolStripGlobals();
            Globals.MyLog.AddTargetTextBox(tbLog);
        //    Globals.CFMTheme.AddListener(this);

            Globals.OnScanEvent += (s, e) =>
                {
                    tcMain.InvokeIfRequired(a =>
                        { tcMain.Enabled = !e.IsScanning; });
                };

            // create application directory structure if it does not exist
            if (!Directory.Exists(Constants.WorkDirectory))
            {
                Directory.CreateDirectory(Constants.WorkDirectory);
                Globals.Log(String.Format("Created working directory: {0}", Constants.WorkDirectory));
            }

            // initialize all global variables
            Globals.Log(stringVersion);
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

#if !TAGGER
            toolstripTagger.Visible = false;
#else
            tsTagger.Visible = true;
            tsButtonTagSelected.Click += tsButtonTagSelected_Click;
            tsButtonUntagSelection.Click += tsButtonUntagSelection_Click;

            tscbTaggerThemes.Items.AddRange(Globals.Tagger.Themes.ToArray());
            tscbTaggerThemes.SelectedIndex = 0;

            if (!String.IsNullOrEmpty(Globals.Tagger.ThemeName))
                tscbTaggerThemes.SelectedItem = Globals.Tagger.ThemeName;
            tscbTaggerThemes.SelectedIndexChanged += (s, e) =>
                {
                    if (tscbTaggerThemes.SelectedItem != null)
                        Globals.Tagger.ThemeName = tscbTaggerThemes.SelectedItem.ToString();
                };
#endif

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
            currentControl = Globals.SongManager;
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
            // get rid of leftover notifications
            Globals.MyLog.RemoveTargetNotifyIcon(Globals.Notifier);
            notifyIcon_Main.Visible = false;
            notifyIcon_Main.Dispose();
            notifyIcon_Main.Icon = null;
            Dispose();
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
                    tstripContainer.BottomToolStripPanelVisible = tstripContainer.TopToolStripPanelVisible;
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
                if (currentControl is INotifyTabChanged)
                    (currentControl as INotifyTabChanged).TabLeave();


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

        private void tsBtnBackup_Click(object sender, EventArgs e)
        {
            Globals.TsProgressBar_Main.Value = 50;
            Extensions.BackupRocksmithProfile();
            Globals.TsProgressBar_Main.Value = 100;
        }

        private void tsBtnHelp_Click(object sender, EventArgs e)
        {
            ShowHelp();
        }

        private void tsBtnLaunchRS_Click(object sender, EventArgs e)
        {
            Extensions.LaunchRocksmith2014();
        }

        private void tsBtnRequest_Click(object sender, EventArgs e)
        {
            Extensions.RequestSongOnCustomsForge();
        }

        private void tsBtnUpload_Click(object sender, EventArgs e)
        {
            Extensions.UploadToCustomsForge();
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

#if TAGGER
        private void TaggerProgress(object sender, TaggerProgress e)
        {
            tsProgressBar_Main.Value = e.Progress;
            Application.DoEvents();
        }

        private void tsButtonTagSelected_Click(object sender, EventArgs e)
        {
            // uncommented code so this would work 
            var selection = Globals.SongManager.GetSelectedSongs(); //.Where(sd => sd.Tagged == false);

            if (selection.Count > 0)
            {
                Globals.Tagger.OnProgress += TaggerProgress;
                try
                {
                    Globals.Tagger.TagSongs(selection.ToArray());
                }
                finally
                {
                    Globals.Tagger.OnProgress -= TaggerProgress;
                    // force dgvSongsMaster data to refresh after Tagging
                    Globals.SongManager.GetGrid().Invalidate();
                    Globals.SongManager.GetGrid().Refresh();
                }
            }
        }

        private void tsButtonUntagSelection_Click(object sender, EventArgs e)
        {
            var selection = Globals.SongManager.GetSelectedSongs(); //.Where(sd => sd.Tagged);

            if (selection.Count > 0)
            {
                Globals.Tagger.OnProgress += TaggerProgress;
                try
                {
                    Globals.Tagger.UntagSongs(selection.ToArray());
                }
                finally
                {
                    Globals.Tagger.OnProgress -= TaggerProgress;
                    // force dgvSongsMaster data to refresh after Untagging
                    Globals.SongManager.GetGrid().Invalidate();
                    Globals.SongManager.GetGrid().Refresh();
                }
            }
        }
#endif

        private void frmMain_Load(object sender, EventArgs e)
        {
#if AUTOUPDATE
            Autoupdater.OnInfoRecieved += (S, E) =>
                {
                    if (Autoupdater.NeedsUpdate())
                    {
                        using (frmNoteViewer f = new frmNoteViewer())
                        {
                            f.btnCopyToClipboard.Text = "Update";

                            if (!Constants.DebugMode)
                            {
                                f.RemoveButtonHandler();
                                f.btnCopyToClipboard.Click += (sen, evt) =>
                                    {
                                        //run setup file, since updating is done in the installer just use the installer to handle updates
                                        //the install will force the user to close the program before installing.
                                        if (File.Exists("CFSMSetup.exe"))
                                        {
                                            System.Diagnostics.Process.Start("CFSMSetup.exe", "-appupdate");
                                        }
                                        else
                                            MessageBox.Show("CFSMSetup not found, please download the program again.");
                                        f.Close();
                                    };
                            }
                            f.PopulateText(Autoupdater.ReleaseNotes);
                            f.Text = String.Format("New version detected {0}", Autoupdater.LatestVersion.ToString());
                            f.ShowDialog();
                        }
                    }
                };
#endif
        }

        private delegate void DoSomethingWithGridSelectionAction(DataGridView dg, IEnumerable<DataGridViewRow> selected, DataGridViewColumn colSel, List<int> IgnoreColums);

        private void DoSomethingWithGrid(DoSomethingWithGridSelectionAction action)
        {
            if (action != null && currentControl != null && currentControl is IDataGridViewHolder)
            {
                var dataGrid = (currentControl as IDataGridViewHolder).GetGrid();
                if (dataGrid != null)
                {
                    var selected = dataGrid.Rows.Cast<DataGridViewRow>();
                    DataGridViewColumn colSel = null;
                    int colSelIdx = -1;
                    if (dataGrid.Columns.Contains("colSelect"))
                    {
                        colSel = dataGrid.Columns["colSelect"];
                        colSelIdx = colSel.Index;
                        var xselected = selected.Where(r => r.Cells["colSelect"].Value != null).Where(r => Convert.ToBoolean(r.Cells["colSelect"].Value)).ToList();
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
                    string columns = "";
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
            var path = Path.Combine(Constants.WorkDirectory, "SongListCSV.csv");
            using (var sfdSongListToCSV = new SaveFileDialog() { Filter = "csv files(*.csv)|*.csv|All files (*.*)|*.*", FileName = path })

                if (sfdSongListToCSV.ShowDialog() == DialogResult.OK)
                {
                    path = sfdSongListToCSV.FileName;

                    DoSomethingWithGrid((dataGrid, selection, colSel, ignoreColumns) =>
                        {
                            var sbCSV = new StringBuilder();


                            const char csvSep = ';';
                            sbCSV.AppendLine(String.Format(@"sep={0}", csvSep)); // used by Excel to recognize seperator if Encoding.Unicode is used
                            string columns = "";
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
                                Globals.Log("<Error>:" + ex.Message);
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

        private void bBCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SongListToBBCode();
        }

        private void cSVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SongListToCSV();
        }

        private void hTMLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SongListToHTML();
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

        private void tsbPlay_Click(object sender, EventArgs e)
        {
            if (Globals.AudioEngine.IsPaused() || Globals.AudioEngine.IsPlaying())
                Globals.AudioEngine.Pause();
            else
                Globals.SongManager.PlaySelectedSong();
            timerAudioProgress.Enabled = (Globals.AudioEngine.IsPlaying());
        }

        private void timerAudioProgress_Tick(object sender, EventArgs e)
        {
            tspbAudioPosition.Value = Globals.AudioEngine.GetSongCompletedPercentage();
        }

        private void tsbStop_Click(object sender, EventArgs e)
        {
            Globals.AudioEngine.Stop();
            timerAudioProgress.Enabled = (Globals.AudioEngine.IsPlaying());
        }

        private void tspbAudioPosition_MouseDown(object sender, MouseEventArgs e)
        {
            if (Globals.AudioEngine.IsLoaded())
            {
                var pos = (float)e.Location.X / (float)tspbAudioPosition.Width;
                Globals.AudioEngine.Seek(pos * Globals.AudioEngine.GetSongLength());
            }
        }



        public void ApplyTheme(Theme sender)
        {
            //var gs = sender.SpecificThemeSetting<DataGridThemeSetting>();
        }
    }
}