using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using CustomControls;
using CustomsForgeSongManager.DataObjects;
using CustomsForgeSongManager.Forms;
using CustomsForgeSongManager.LocalTools;
using CustomsForgeSongManager.UITheme;
using DataGridViewTools;
using System.ComponentModel;
using CustomsForgeSongManager.Properties;
using GenTools;
using CustomsForgeSongManager.SongEditor;


// TODO: try loading Globals.MasterCollection to dgvDuplicates and then filter data
// to find/show duplicates, then delete, then unfilter - thus preserving binding

namespace CustomsForgeSongManager.UControls
{
    public partial class Duplicates : UserControl, IDataGridViewHolder, INotifyTabChanged
    {
        private DataGridViewCellStyle ErrorStyle;
        private Color ErrorStyleBackColor = Color.DarkGray;
        private Color ErrorStyleForeColor = Color.White;
        private bool allSelected = false;
        private bool bindingCompleted;
        private bool dgvPainted;
        private List<string> distinctPIDS = new List<string>();
        private bool dupPidSelected;
        private List<SongData> duplicateList = new List<SongData>(); // prevents filtering from being inherited
        private bool keepOpen;
        private bool keyDisabled;
        private bool keyEnabled;
        private string lastSelectedSongPath = string.Empty;
        private string olderVersionType = String.Empty;
        private DgvStatus statusDuplicates = new DgvStatus();

        public Duplicates()
        {
            // this only gets called once
            InitializeComponent();
            Globals.TsLabel_StatusMsg.Click += lnkShowAll_Click;
            ErrorStyle = new DataGridViewCellStyle() { Font = new Font("Arial", 8, FontStyle.Italic), ForeColor = ErrorStyleForeColor, BackColor = ErrorStyleBackColor };

            // NOTE: for safety subfolders are always included when checking for duplicates
            chkIncludeSubfolders.Visible = false;
            // dev testing new menu item
            //if (!Constants.DebugMode)
            //    tsmiDuplicateType.Visible = false;

            // test for duplicate DLCKey
            dupPidSelected = false;
            PopulateDuplicates(dupPidSelected);

            // test for duplicate PID
            if (!duplicateList.Any())
            {
                dupPidSelected = true;
                PopulateDuplicates(dupPidSelected);

                // switch back
                if (!duplicateList.Any())
                    dupPidSelected = false;
            }
        }

        // TODO: make method generic to find any Type of Duplicate
        public void PopulateDuplicates(bool findDupPIDs = false)
        {
            // remove all previous selections
            foreach (var song in Globals.MasterCollection)
                song.Selected = false;

            // NOTE: do not add SongData.Arrangements to the datagridview
            Globals.Log("Populating Duplicates GUI ...");
            dgvDuplicates.Visible = false;
            dgvDuplicates.AllowUserToAddRows = false; // ensures row count = 0    

            if (Globals.WorkerFinished == Globals.Tristate.Cancelled)
            {
                MessageBox.Show("Duplicates needs to be rescanned.", Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            duplicateList.Clear();
            distinctPIDS.Clear();

            if (findDupPIDs) // NOTE: duplicate PID cause in-game lockup
            {
                Globals.Log("Showing CDLC with duplicate PID (GAME CRASHERS) ...");
                var pidList = new List<SongData>();

                // assuming every song has at least one arrangement
                foreach (var song in Globals.MasterCollection)
                {
                    foreach (var arrangement in song.Arrangements2D)
                    {
                        // cleaned up code using Lovro's reflection concept
                        SongData pidSong = new SongData();
                        var propInfo = song.GetType().GetProperties();

                        foreach (var item in propInfo)
                        {
                            if (item.CanWrite)
                            {
                                pidSong.GetType().GetProperty(item.Name).SetValue(pidSong, item.GetValue(song, null), null);
                            }
                        }

                        pidSong.PID = arrangement.PersistentID;
                        pidSong.PIDArrangement = arrangement.ArrangementName;
                        pidList.Add(pidSong);
                    }
                }

                // use case insensitive PID duplicate detection
                duplicateList = pidList.GroupBy(x => x.PID.ToUpperInvariant()).Where(group => group.Count() > 1).SelectMany(group => group).ToList();

                // for safety subfolders are always included when checking for duplicates
                if (chkIncludeSubfolders.Visible && !chkIncludeSubfolders.Checked)
                    duplicateList = duplicateList.Where(x => Path.GetFileName(Path.GetDirectoryName(x.FilePath)) == "dlc").GroupBy(x => x.PID).Where(group => group.Count() > 1).SelectMany(group => group).ToList();

                distinctPIDS = duplicateList.Select(x => x.PID.ToUpperInvariant()).Distinct().ToList();
            }
            else
            {
                Globals.Log("Showing CDLC with either duplicate DLCKey or duplicate ArtistTitleAlbum (case insensitive) ...");
                // use case insensitive duplicate detection
                var dupDlcKey = Globals.MasterCollection.GroupBy(x => x.DLCKey.ToUpperInvariant()).Where(g => g.Count() > 1).SelectMany(g => g);
                // move short words like 'the', expand abbreviations, and strip non-alphanumeric characters and whitespace
                var dupATA = Globals.MasterCollection.GroupBy(x => GenExtensions.CleanString(x.ArtistTitleAlbum)).Where(group => group.Count() > 1).SelectMany(group => group);
                duplicateList = dupATA.Union(dupDlcKey).ToList(); // duplicate list gets bound to MasterCollection by LINQ

                // for safety subfolders are always included when checking for duplicates
                if (chkIncludeSubfolders.Visible && !chkIncludeSubfolders.Checked)
                    duplicateList = duplicateList.Where(x => Path.GetFileName(Path.GetDirectoryName(x.FilePath)) == "dlc").ToList();
            }

            if (keyEnabled)
            {
                duplicateList = duplicateList.Where(x => !Path.GetFileName(x.FilePath).Contains("disabled")).ToList();
                Globals.Log("Showing duplicate enabled songs ...");
            }

            if (keyDisabled)
            {
                duplicateList = duplicateList.Where(x => Path.GetFileName(x.FilePath).Contains("disabled")).ToList();
                Globals.Log("Showing duplicate disabled songs ...");
            }

            duplicateList.RemoveAll(x => x.FileName.ToLower().Contains(Constants.RS1COMP));

            if (!duplicateList.Any())
                return;

            // final ordering
            duplicateList.OrderBy(x => x.ArtistTitleAlbumDate);

            // PopulateDataGridView respect processing order
            DgvExtensions.DoubleBuffered(dgvDuplicates);
            LoadFilteredBindingList(duplicateList);
            CFSMTheme.InitializeDgvAppearance(dgvDuplicates);
            // reload column order, width, visibility
            Globals.Settings.LoadSettingsFromFile(dgvDuplicates);

            if (RAExtensions.ManagerGridSettings != null)
                dgvDuplicates.ReLoadColumnOrder(RAExtensions.ManagerGridSettings.ColumnOrder);
            else
                Globals.Settings.SaveSettingsToFile(dgvDuplicates);

            if (findDupPIDs)
            {
                this.colPID.Visible = true;
                this.colPIDArrangement.Visible = true;
            }
            else
            {
                this.colPID.Visible = false;
                this.colPIDArrangement.Visible = false;
            }
        }

        public void UpdateToolStrip()
        {
            chkIncludeSubfolders.Checked = AppSettings.Instance.IncludeSubfolders;

            if (Globals.RescanDuplicates)
            {
                Globals.RescanDuplicates = false;
                Rescan();
                PopulateDuplicates(dupPidSelected);
            }
            else if (Globals.ReloadDuplicates)
            {
                Globals.ReloadDuplicates = false;
                PopulateDuplicates(dupPidSelected);
            }
            // for safety subfolders are always included when checking for duplicates
            else if (chkIncludeSubfolders.Visible && chkIncludeSubfolders.Checked)
                IncludeSubfolders();

            if (!duplicateList.Any())
            {
                // reset the grid
                dgvDuplicates.Rows.Clear();
                dgvDuplicates.DataSource = null;

                if (dupPidSelected)
                {
                    Globals.Log("Good news, no duplicate PID found ...");
                    txtNoDuplicates.Text = "\r\nGood News ...\r\nNo Duplicate PID Found";
                }
                else
                {
                    Globals.Log("Good news, no duplicate DLCKey and/or duplicate ArtistTitleAlbum found ...");
                    txtNoDuplicates.Text = "\r\nGood News ...\r\nNo Duplicate DLCKey and/or\r\n Duplicate ArtistTitleAlbum Found";
                }

                txtNoDuplicates.Visible = true;
            }
            else
                txtNoDuplicates.Visible = false;

            Globals.TsLabel_MainMsg.Text = String.Format(Properties.Resources.RocksmithSongsCountFormat, Globals.MasterCollection.Count);
            Globals.TsLabel_MainMsg.Visible = true;
            Globals.TsLabel_DisabledCounter.Alignment = ToolStripItemAlignment.Right;
            Globals.TsLabel_DisabledCounter.Text = String.Format("Duplicates Count: {0}", dgvDuplicates.Rows.Count);
            Globals.TsLabel_DisabledCounter.Visible = true;
            Globals.TsLabel_StatusMsg.Visible = false;


            tsmiDuplicateTypeDLCKeyATA.Checked = !dupPidSelected;
            tsmiDuplicateTypePID.Checked = dupPidSelected;
        }

        private void ColumnMenuItemClick(object sender, EventArgs eventArgs)
        {
            ToolStripMenuItem currentContextMenuItem = sender as ToolStripMenuItem;
            if (currentContextMenuItem != null)
            {
                if (!string.IsNullOrEmpty(currentContextMenuItem.Tag.ToString()))
                {
                    var dataGridViewColumn = dgvDuplicates.Columns[currentContextMenuItem.Tag.ToString()];
                    if (dataGridViewColumn != null)
                    {
                        var columnIndex = dataGridViewColumn.Index;
                        var columnSetting = RAExtensions.ManagerGridSettings.ColumnOrder.SingleOrDefault(x => x.ColumnIndex == columnIndex);
                        if (columnSetting != null)
                        {
                            columnSetting.Visible = !columnSetting.Visible;
                            dgvDuplicates.Columns[columnIndex].Visible = columnSetting.Visible;
                            currentContextMenuItem.Checked = columnSetting.Visible;
                            //   dgvDuplicates.Invalidate();
                        }
                    }

                    //foreach (var item in dgvDuplicates.Columns.Cast<DataGridViewColumn>())
                    //    if (item.Visible)
                    //        dgvDuplicates.InvalidateCell(item.HeaderCell);
                }
            }
        }

        private void IncludeSubfolders()
        {
            if (dgvDuplicates.Columns["colPID"].Visible)
                PopulateDuplicates(true);
            else
                PopulateDuplicates();
        }

        private void LoadFilteredBindingList(dynamic list)
        {
            bindingCompleted = false;
            dgvPainted = false;
            // sortable binding list with dropdown filtering
            dgvDuplicates.AutoGenerateColumns = false;
            FilteredBindingList<SongData> fbl = new FilteredBindingList<SongData>(list);
            BindingSource bs = new BindingSource { DataSource = fbl };
            dgvDuplicates.DataSource = bs;
        }

        private void PopulateMenuWithColumnHeaders(ContextMenuStrip contextMenuStrip)
        {
            // fix for contextual menu bug 'Object reference not set to an instance of an object.' 
            // that occur on startup when dgv settings have not yet been saved       
            if (RAExtensions.ManagerGridSettings == null)
            {
                Globals.Settings.SaveSettingsToFile(dgvDuplicates);
                Globals.Settings.LoadSettingsFromFile(dgvDuplicates);
                dgvDuplicates.ReLoadColumnOrder(RAExtensions.ManagerGridSettings.ColumnOrder);
            }

            contextMenuStrip.Items.Clear();
            foreach (ColumnOrderItem columnOrderItem in RAExtensions.ManagerGridSettings.ColumnOrder)
            {
                var cn = dgvDuplicates.Columns[columnOrderItem.ColumnIndex].Name;
                if (cn.ToLower().StartsWith("col"))
                    cn = cn.Remove(0, 3);

                ToolStripMenuItem columnsMenuItem = new ToolStripMenuItem(cn, null, ColumnMenuItemClick) { Checked = dgvDuplicates.Columns[columnOrderItem.ColumnIndex].Visible, Tag = dgvDuplicates.Columns[columnOrderItem.ColumnIndex].Name };
                contextMenuStrip.Items.Add(columnsMenuItem);
            }
        }

        private void RemoveFilter()
        {
            // save current sorting before removing filter
            statusDuplicates.SaveSorting(dgvDuplicates);
            // remove the filter
            var filterStatus = DataGridViewAutoFilterColumnHeaderCell.GetFilterStatus(dgvDuplicates);
            if (!String.IsNullOrEmpty(filterStatus))
                DataGridViewAutoFilterTextBoxColumn.RemoveFilter(dgvDuplicates);

            UpdateToolStrip();
            // reapply sort direction to reselect the filtered song
            statusDuplicates.RestoreSorting(dgvDuplicates);
        }

        private void Rescan()
        {
            // this should never happen
            if (String.IsNullOrEmpty(AppSettings.Instance.RSInstalledDir))
            {
                MessageBox.Show(Properties.Resources.ErrorRocksmith2014InstallationDirectorySet, Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // run new worker
            using (Worker worker = new Worker())
            {
                worker.BackgroundScan(this);
                while (Globals.WorkerFinished == Globals.Tristate.False)
                    Application.DoEvents();
            }

            if (Globals.WorkerFinished == Globals.Tristate.Cancelled)
            {
                Globals.Log(Resources.UserCancelledProcess);
                return;
            }
        }

        private void SelectAllNone()
        {
            TemporaryDisableDatabindEvent(() =>
            {
                foreach (DataGridViewRow row in dgvDuplicates.Rows)
                    row.Cells["colSelect"].Value = !allSelected;
            });

            allSelected = !allSelected;
            dgvDuplicates.Refresh();
        }

        private void SelectOlderVersions()
        {
            // always start fresh and deselect all 
            DgvExtensions.RowsCheckboxValue(dgvDuplicates, false);
            var sortedDups = new List<SongData>();

            if (olderVersionType == "ToolkitVersion") // using cleaned concatinated ToolkitVerion column to order by/sort on
            {
                sortedDups = duplicateList.OrderBy(x => GenExtensions.CleanString(x.ArtistTitleAlbum))
                 .ThenBy(x => new Version(GenExtensions.CleanVersion(x.ToolkitVersion))).ToList();
            }
            else // using cleaned concatinated ArtistTitleAlbumDate column to order by/sort on
                sortedDups = duplicateList.OrderBy(x => GenExtensions.CleanString(x.ArtistTitleAlbumDate)).ToList();

            LoadFilteredBindingList(sortedDups);

            if (String.IsNullOrEmpty(olderVersionType))
            {
                Globals.Log("Reset selection ...");
            }
            else // select older versions
            {
                var rowCount = dgvDuplicates.Rows.Count;
                for (int i = rowCount - 1; i >= 0; i--)
                {
                    if (i - 1 == -1)
                        break;

                    var currSong = DgvExtensions.GetObjectFromRow<SongData>(dgvDuplicates, i);
                    var nextSong = DgvExtensions.GetObjectFromRow<SongData>(dgvDuplicates, i - 1);
                    if (GenExtensions.CleanString(currSong.ArtistTitleAlbum) == GenExtensions.CleanString(nextSong.ArtistTitleAlbum))
                        dgvDuplicates.Rows[i - 1].Cells["colSelect"].Value = true;

                    if (currSong.IsODLC || nextSong.IsODLC)
                        dgvDuplicates.Rows[i - 1].Cells["colSelect"].Value = false;
                }

                var selection = DgvExtensions.GetObjectsFromRows<SongData>(dgvDuplicates);
                Globals.Log(String.Format("Selected ({0}) older duplicate songs ordered by {1} ...", selection.Count, olderVersionType));
            }

            dgvDuplicates.Refresh();
        }

        private void SelectionDeleteMove(DataGridView dgvCurrent, bool modeDelete = false)
        {
            // user must check Select to Delete/Move
            var selection = DgvExtensions.GetObjectsFromRows<SongData>(dgvCurrent);
            if (!selection.Any())
            {
                MessageBox.Show("Please select the checkbox next to song(s)." + Environment.NewLine + "First left mouse click the row to select it then" + Environment.NewLine + "right mouse click to quickly Move or Delete.  ", Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (modeDelete)
            {
                var diaMsg = "You are about to delete file(s).";
                if (selection.Where(x => x.IsODLC).Any())
                    diaMsg = "You are about to delete ODLC file(s).";

                diaMsg = diaMsg + Environment.NewLine +
                    "Deletion is permenant and can not be undone." + Environment.NewLine +
                    "Do you want to continue?";

                if (DialogResult.No == BetterDialog2.ShowDialog(diaMsg, "Delete File(s) ...", null, "Yes", "No", Bitmap.FromHicon(SystemIcons.Warning.Handle), "Warning", 0, 150))
                    return;
            }

            for (int ndx = dgvCurrent.Rows.Count - 1; ndx >= 0; ndx--)
            {
                DataGridViewRow row = dgvCurrent.Rows[ndx];

                if (Convert.ToBoolean(row.Cells["colSelect"].Value))
                    dgvCurrent.Rows.Remove(row);
            }

            if (!modeDelete)
            {
                if (FileTools.CreateBackupOfType(selection, Constants.DuplicatesFolder, Constants.EXT_DUP, false))
                    FileTools.DeleteFiles(selection, false);
            }
            else
                FileTools.DeleteFiles(selection);

            // force reload
            Globals.ReloadSongManager = true;
            Globals.ReloadRenamer = true;
            Globals.ReloadSetlistManager = true;
            Globals.ReloadProfileSongLists = true;
            Globals.RescanDuplicates = true;
            UpdateToolStrip();
        }

        // TODO: move to FileTools as generic method
        private void SelectionEnableDisable(DataGridView dgvCurrent)
        {
            // user must check Select to Enable/Disable
            var selection = DgvExtensions.GetObjectsFromRows<SongData>(dgvCurrent);
            if (!selection.Any())
            {
                MessageBox.Show("Please select the checkbox next to song(s)." + Environment.NewLine + "First left mouse click the row to select it then" + Environment.NewLine + "right mouse click to quickly Enable/Disable.  ", Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            for (int ndx = dgvCurrent.Rows.Count - 1; ndx >= 0; ndx--)
            {
                var sd = DgvExtensions.GetObjectFromRow<SongData>(dgvCurrent, ndx);
                var originalPath = sd.FilePath;
                if (!originalPath.ToLower().Contains(Constants.RS1COMP))
                {
                    DataGridViewRow row = dgvCurrent.Rows[ndx];
                    if (Convert.ToBoolean(row.Cells["colSelect"].Value))
                    {
                        try
                        {
                            if (sd.Enabled == "Yes")
                            {
                                var disabledPath = originalPath.Replace(Constants.EnabledExtension, Constants.DisabledExtension);
                                File.Move(originalPath, disabledPath);
                                sd.FilePath = disabledPath;
                                sd.Enabled = "No";
                            }
                            else
                            {
                                var enabledPath = originalPath.Replace(Constants.DisabledExtension, Constants.EnabledExtension);
                                File.Move(originalPath, enabledPath);
                                sd.FilePath = enabledPath;
                                sd.Enabled = "Yes";
                            }
                        }
                        catch (IOException ex)
                        {
                            MessageBox.Show(String.Format(Properties.Resources.UnableToEnableDisableSongX0InDlcFolderX1Er, Path.GetFileName(originalPath), Environment.NewLine, ex.Message));
                        }
                    }
                }
                else
                    Globals.Log(String.Format(Properties.Resources.ThisIsARocksmith1CompatiblitySongX0RS1Comp, Environment.NewLine));
            }

            dgvCurrent.Refresh();
        }

        private void ShowSongInfo()
        {
            if (dgvDuplicates.SelectedRows.Count > 0)
            {
                var song = DgvExtensions.GetObjectFromFirstSelectedRow<SongData>(dgvDuplicates);
                if (song != null)
                {
                    frmSongInfo infoWindow = new frmSongInfo(song);
                    infoWindow.Show();
                }
            }
            else
                MessageBox.Show(String.Format("Please select (highlight) the song that  {0}you would like information about.", Environment.NewLine), Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void TemporaryDisableDatabindEvent(Action action)
        {
            dgvDuplicates.DataBindingComplete -= dgvDuplicates_DataBindingComplete;
            try
            {
                action();
            }
            finally
            {
                dgvDuplicates.DataBindingComplete += dgvDuplicates_DataBindingComplete;
            }
        }

        private void chkIncludeSubfolders_MouseUp(object sender, MouseEventArgs e)
        {
            if (chkIncludeSubfolders.Visible)
            {
                AppSettings.Instance.IncludeSubfolders = chkIncludeSubfolders.Checked;
                Globals.Settings.SaveSettingsToFile(dgvDuplicates); // need to save here
                UpdateToolStrip();
            }
        }

        private void Duplicates_Resize(object sender, EventArgs e)
        {
            var p = new Point() { X = (Width - txtNoDuplicates.Width) / 2, Y = (Height - txtNoDuplicates.Height) / 2 };

            if (p.X < 3)
                p.X = 3;

            if (p.Y < 3)
                p.Y = 3;
            txtNoDuplicates.Location = p;
        }

        private void cmsDelete_Click(object sender, EventArgs e)
        {
            SelectionDeleteMove(dgvDuplicates, true);
        }

        private void cmsDuplicates_Closing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            if (keepOpen && e.CloseReason == ToolStripDropDownCloseReason.ItemClicked)
                e.Cancel = true;
        }

        private void cmsDuplicates_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            // keeps cms open if Actions clicked
            if (e.ClickedItem == cmsDuplicates.Items["cmsActions"])
                keepOpen = true;
            else
                keepOpen = false;
        }

        private void cmsEnableDisable_Click(object sender, EventArgs e)
        {
            SelectionEnableDisable(dgvDuplicates);
        }

        private void cmsMove_Click(object sender, EventArgs e)
        {
            SelectionDeleteMove(dgvDuplicates, false);
        }

        private void cmsOpenLocation_Click(object sender, EventArgs e)
        {
            var path = dgvDuplicates.SelectedRows[0].Cells["colFilePath"].Value.ToString();
            var directory = new FileInfo(path);
            if (directory.DirectoryName != null)
                Process.Start("explorer.exe", String.Format("/select,\"{0}\"", directory.FullName));
        }

        private void cmsShowSongInfo_Click(object sender, EventArgs e)
        {
            ShowSongInfo();
        }

        private void dgvDuplicates_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // make sure grid has been painted before proceeding
            if (!dgvPainted)
                return;

            if (dgvDuplicates.SelectedRows.Count > 0)
                lastSelectedSongPath = dgvDuplicates.SelectedRows[0].Cells["colFilePath"].Value.ToString();
        }

        private void dgvDuplicates_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // triggered by any key
            if (e.RowIndex != -1 && e.RowIndex != colSelect.Index)
                ShowSongInfo();
        }

        private void dgvDuplicates_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // speed hacks ...
            if (e.RowIndex == -1)
                return;
            if (dgvDuplicates.Columns[e.ColumnIndex] is DataGridViewCheckBoxColumn)
                return;
            if (dgvDuplicates.Rows[e.RowIndex].IsNewRow) // || !dgvDuplicates.IsCurrentRowDirty)
                return;
            if (dgvDuplicates.Rows.Count < 1) // needed in case filter was set that returns no items
                return;

            // added ODLC font highlighting
            try
            {
                SongData song = dgvDuplicates.Rows[e.RowIndex].DataBoundItem as SongData;

                if (song != null)
                {
                    if (song.IsODLC)
                    {
                        e.CellStyle.Font = Constants.OfficialDLCFont;
                        e.CellStyle.BackColor = Color.Red;
                        // prevent ODLC deletion
                        //DataGridViewCell cell = dgvDuplicates.Rows[e.RowIndex].Cells["colSelect"];
                        //DataGridViewCheckBoxCell chkCell = cell as DataGridViewCheckBoxCell;
                        //chkCell.Style.ForeColor = Color.DarkGray;
                        //chkCell.FlatStyle = FlatStyle.Flat;
                        //chkCell.Value = false;
                        //cell.ReadOnly = true;
                    }

                    if (distinctPIDS.Contains(song.PID))
                    {
                        // make select checkbox consistent with color change
                        dgvDuplicates.Rows[e.RowIndex].Cells["colSelect"].Style.BackColor = ErrorStyle.BackColor;
                        dgvDuplicates.Rows[e.RowIndex].Cells["colSelect"].Style.ForeColor = ErrorStyle.ForeColor;
                        // change color of duplicate PIDs
                        e.CellStyle.BackColor = ErrorStyle.BackColor;
                        e.CellStyle.ForeColor = ErrorStyle.ForeColor;
                        e.CellStyle.Font = ErrorStyle.Font;
                    }
                }
            }
            catch (Exception ex)
            {
                Globals.DebugLog(String.Format("{0}", ex.Message));
            }
        }

        private void dgvDuplicates_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex == -1)
                return;

            // same in all grids
            if (e.Button == MouseButtons.Left)
            {
                // select a single row by Ctrl-Click
                if (ModifierKeys == Keys.Control)
                {
                    var song = DgvExtensions.GetObjectFromRow<SongData>(dgvDuplicates, e.RowIndex);
                    song.Selected = !song.Selected;
                }
                // select multiple rows by Shift-Click on two outer rows
                else if (ModifierKeys == Keys.Shift)
                {
                    if (dgvDuplicates.SelectedRows.Count > 0)
                    {
                        var first = dgvDuplicates.SelectedRows[0];
                        var start = first.Index;
                        var end = e.RowIndex + 1;

                        if (start > end)
                        {
                            var tmp = start;
                            start = end;
                            end = tmp;
                        }

                        TemporaryDisableDatabindEvent(() =>
                        {
                            for (int i = start; i < end; i++)
                            {
                                var song = DgvExtensions.GetObjectFromRow<SongData>(dgvDuplicates, i);
                                song.Selected = !song.Selected;
                            }
                        });
                    }
                }
            }
        }

        private void dgvDuplicates_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            // has precedent over a ColumnHeader_MouseClick
            // MouseUp detection is more reliable than MouseDown
            var grid = (DataGridView)sender;

            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex != -1)
                {
                    grid.Rows[e.RowIndex].Selected = true;
                    cmsDuplicates.Show(Cursor.Position);
                }
                else
                {
                    PopulateMenuWithColumnHeaders(cmsDuplicateColumns);
                    cmsDuplicateColumns.Show(Cursor.Position);
                }
            }

            // user complained that clicking a row should not autocheck select
            // programmatic left clicking on colSelect
            if (e.Button == MouseButtons.Left && e.RowIndex != -1 && e.ColumnIndex == colSelect.Index)
            {
                try
                {

                    TemporaryDisableDatabindEvent(() => // prevents grid bounce
                    {
                        grid.Rows[e.RowIndex].Cells["colSelect"].Value = !(bool)(grid.Rows[e.RowIndex].Cells["colSelect"].Value);
                        Thread.Sleep(200); // debounce excess clicking
                    });
                }
                catch
                {
                    // debounce clicking
                    TemporaryDisableDatabindEvent(() => { grid.EndEdit(); });
                }
            }
        }

        private void dgvDuplicates_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            // HACK: catch DataBindingComplete called by other UC's
            var grid = (DataGridView)sender;
            if (grid.Name != "dgvDuplicates")
                return;

            if (!bindingCompleted)
            {
                Debug.WriteLine("DataBinding Complete ... ");
                bindingCompleted = true;
            }

            if (!dgvPainted) // speed hack
                return;

            var filterStatus = DataGridViewAutoFilterColumnHeaderCell.GetFilterStatus(dgvDuplicates);
            // filter applied
            if (!String.IsNullOrEmpty(filterStatus))
            {
                Globals.TsLabel_StatusMsg.Alignment = ToolStripItemAlignment.Right;
                Globals.TsLabel_StatusMsg.Text = "Show &All";
                Globals.TsLabel_StatusMsg.IsLink = true;
                Globals.TsLabel_StatusMsg.LinkBehavior = LinkBehavior.HoverUnderline;
                Globals.TsLabel_StatusMsg.Visible = true;
                Globals.TsLabel_DisabledCounter.Alignment = ToolStripItemAlignment.Right;
                Globals.TsLabel_DisabledCounter.Text = filterStatus;
                Globals.TsLabel_DisabledCounter.Visible = true;
            }

            // filter removed
            if (String.IsNullOrEmpty(filterStatus) && this.dgvDuplicates.CurrentCell != null)
                RemoveFilter();
        }

        private void dgvDuplicates_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            Globals.DebugLog(String.Format("<ERROR> (Row: {0}, Col: {1}), {2} ...", e.RowIndex, e.ColumnIndex, e.Exception.Message));
            e.Cancel = true;
        }

        private void dgvDuplicates_KeyDown(object sender, KeyEventArgs e)
        {
            // space bar used to select a song (w/ checkbox "Select")
            if (e.KeyCode == Keys.Space)
            {
                var song = DgvExtensions.GetObjectFromFirstSelectedRow<SongData>(dgvDuplicates);

                // TemporaryDisableDatabindEvent prevents selection from changing back to first row
                if (song != null)
                    TemporaryDisableDatabindEvent(() => { song.Selected = !song.Selected; });
            }
        }

        private void dgvDuplicates_Paint(object sender, PaintEventArgs e)
        {
            if (bindingCompleted && !dgvPainted)
            {
                dgvPainted = true;
                // Globals.DebugLog("dgvDuplicates Painted ... ");
                // it is safe to do cell formatting (coloring)
                // here
            }
        }

        private void dgvDuplicates_Sorted(object sender, EventArgs e)
        {
            return;

            if (dgvDuplicates.SortedColumn != null)
            {
                // force grid data to rebind/refresh w/o bounce and avoid exceptions when row has been deleted
                dgvDuplicates.ResetBindings();

                // Reselect last selected row after sorting
                if (!String.IsNullOrEmpty(lastSelectedSongPath) && dgvDuplicates.Rows.Count > 0)
                {
                    int newRowIndex = dgvDuplicates.Rows.Cast<DataGridViewRow>().FirstOrDefault(r => r.Cells["colFilePath"].Value.ToString() == lastSelectedSongPath).Index;
                    dgvDuplicates.Rows[newRowIndex].Selected = true;
                    dgvDuplicates.FirstDisplayedScrollingRowIndex = newRowIndex;
                }
                else
                    lastSelectedSongPath = String.Empty;
            }
        }

        private void exploreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var filePath = duplicateList[dgvDuplicates.SelectedRows[0].Index].FilePath;

            if (File.Exists(filePath))
                Process.Start("explorer.exe", String.Format("/select,\"{0}\"", filePath));
        }

        private void lnkLblSelectAll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SelectAllNone();
        }

        // TODO: depricate lnkPersistentId and use new tsmiDuplicateType to make selection
        private void lnkPersistentId_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (dupPidSelected)
            {
                PopulateDuplicates(false);
                dupPidSelected = false;
            }
            else
            {
                PopulateDuplicates(true);
                dupPidSelected = true;
            }

            UpdateToolStrip();
        }

        private void lnkSelectOlderVersions_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var lnkLabel = sender as LinkLabel;

            if (olderVersionType == "ToolkitVersion" && lnkLabel.Text.EndsWith("ToolkitVersion") ||
                 olderVersionType == "ArtistTitleAlbumDate" && lnkLabel.Text.EndsWith("ArtistTitleAlbumDate"))
            {
                olderVersionType = String.Empty;
            }
            else if (lnkLabel.Text.EndsWith("ToolkitVersion"))
            {
                olderVersionType = "ToolkitVersion";
                this.colToolkitVersion.Visible = true;
            }
            else if (lnkLabel.Text.EndsWith("ArtistTitleAlbumDate"))
            {
                olderVersionType = "ArtistTitleAlbumDate";
                this.colArtistTitleAlbumDate.Visible = true;
            }

            SelectOlderVersions();
            UpdateToolStrip();
        }

        private void lnkShowAll_Click(object sender, EventArgs e)
        {
            RemoveFilter();
        }

        private void lnklblToggle_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            TemporaryDisableDatabindEvent(() =>
                {
                    foreach (DataGridViewRow row in dgvDuplicates.Rows)
                        row.Cells["colSelect"].Value = !Convert.ToBoolean(row.Cells["colSelect"].Value);
                });

            dgvDuplicates.Refresh();
        }

        private void tsmiHelp_Click(object sender, EventArgs e)
        {
            frmNoteViewer.ViewResourcesFile("CustomsForgeSongManager.Resources.HelpDuplicates.rtf", "Duplicates Help");
        }

        private void tsmiRescanAll_Click(object sender, EventArgs e)
        {
            bindingCompleted = false;
            dgvPainted = false;
            keyDisabled = false;
            keyEnabled = false;
            Globals.RescanDuplicates = true;
            olderVersionType = String.Empty;
            UpdateToolStrip();
        }

        public DataGridView GetGrid()
        {
            return dgvDuplicates;
        }

        public void TabEnter()
        {
            Globals.DgvCurrent = dgvDuplicates;
            GetGrid().ResetBindings(); // force grid data to rebind/refresh
            // workaround for disabled column VS bug
            dgvDuplicates.Columns["colSelect"].ReadOnly = false;
            dgvDuplicates.Columns["colPackageVersion"].ReadOnly = false;
            statusDuplicates.RestoreSorting(Globals.DgvCurrent);
            Globals.Log("Duplicate GUI Activated...");
        }

        public void TabLeave()
        {
            statusDuplicates.SaveSorting(Globals.DgvCurrent);
            GetGrid().ResetBindings(); // force grid data to rebind/refresh

            // remove all selections
            foreach (var song in Globals.MasterCollection)
                song.Selected = false;

            txtNoDuplicates.Visible = false;
            Globals.Settings.SaveSettingsToFile(dgvDuplicates);
            Globals.Log("Duplicates GUI Deactivated ...");
        }

        private void tsmiDuplicateTypeDLCKeyATA_Click(object sender, EventArgs e)
        {
            PopulateDuplicates(false);
            dupPidSelected = false;
            UpdateToolStrip();
        }

        private void tsmiDuplicateTypePID_Click(object sender, EventArgs e)
        {
            PopulateDuplicates(true);
            dupPidSelected = true;
            UpdateToolStrip();
        }

        private void tsmiShowEnabledDisabled_Click(object sender, EventArgs e)
        {
            bindingCompleted = false;
            dgvPainted = false;

            if (!keyDisabled && !keyEnabled)
                keyEnabled = true;
            else if (keyEnabled)
            {
                keyDisabled = true;
                keyEnabled = false;
            }
            else
            {
                keyDisabled = false;
                keyEnabled = true;
            }

            Globals.RescanDuplicates = true;
            UpdateToolStrip();

            // continue to show dropdown menu
            //tsmiRescan.ShowDropDown();
            //tsmiShowEnabledDisabled.ShowDropDown();
            //menuStrip.Focus();
        }

        private void cmsEdit_Click(object sender, EventArgs e)
        {
            var sd = DgvExtensions.GetObjectFromRow<SongData>(dgvDuplicates.SelectedRows[0]);
            if (sd.IsODLC || sd.IsRsCompPack || sd.IsSongsPsarc)
                return;

            // DO NOT edit/modify/repair disabled CDLC
            if (sd.Enabled != "Yes")
            {
                var diaMsg = "Disabled CDLC may not be edited ..." + Environment.NewLine +
                             "Please enable the CLDC and then edit it." + Environment.NewLine;
                BetterDialog2.ShowDialog(diaMsg, "Disabled CDLC ...", null, null, "Ok", Bitmap.FromHicon(SystemIcons.Warning.Handle), "ReadMe", 0, 150);
                return;
            }

            // DO NOT edit/modify/repair tagged CDLC - artifact data will be lost
            if (sd.Tagged == SongTaggerStatus.True)
            {
                var diaMsg = "Tagged CDLC may not be edited ..." + Environment.NewLine +
                             "Please untag the CLDC and then edit it." + Environment.NewLine;
                BetterDialog2.ShowDialog(diaMsg, "Tagged CDLC ...", null, null, "Ok", Bitmap.FromHicon(SystemIcons.Warning.Handle), "ReadMe", 0, 150);
                return;
            }

            var filePath = sd.FilePath;
            using (var songEditor = new frmSongEditor(filePath))
            {
                songEditor.Text = String.Format("{0}{1}", "Song Editor ... Loaded: ", Path.GetFileName(filePath));
                songEditor.ShowDialog();
            }

            if (Globals.ReloadDuplicates)
                UpdateToolStrip();
        }


    }
}