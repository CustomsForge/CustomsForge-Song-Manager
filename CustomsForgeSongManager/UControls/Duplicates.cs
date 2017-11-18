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

// TODO: merge duplicates tool to Song Manager

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
        private List<SongData> duplicates = new List<SongData>();
        private bool keepOpen;
        private bool keyDisabled;
        private bool keyEnabled;
        private bool olderVersionsSelected;

        public Duplicates()
        {
            InitializeComponent();
            Globals.TsLabel_StatusMsg.Click += lnkShowAll_Click;
            ErrorStyle = new DataGridViewCellStyle() { Font = new Font("Arial", 8, FontStyle.Italic), ForeColor = ErrorStyleForeColor, BackColor = ErrorStyleBackColor };
            PopulateDuplicates();
        }

        public void PopulateDuplicates(bool findDupPIDs = false)
        {
            // NOTE: do not add SongData.Arrangments to the datagridview
            Globals.Log("Populating Duplicates GUI ...");
            DgvExtensions.DoubleBuffered(dgvDuplicates);
            Globals.Settings.LoadSettingsFromFile(dgvDuplicates);
            dgvDuplicates.Visible = false;

            if (Globals.WorkerFinished == Globals.Tristate.Cancelled)
            {
                MessageBox.Show("Duplicates needs to be rescanned.", Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            duplicates.Clear();
            distinctPIDS.Clear();

            if (findDupPIDs)
            {
                var pidList = new List<SongData>();

                // assuming every song has at least one arrangement
                foreach (var song in Globals.SongCollection)
                {
                    foreach (var arrangement in song.Arrangements2D)
                    {
                        // cleaned up code using Lovro's reflection concept ;)
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
                        pidSong.PIDArrangement = arrangement.Name;
                        pidList.Add(pidSong);
                    }
                }

                if (chkSubFolders.Checked)
                    duplicates = pidList.GroupBy(x => x.PID).Where(group => group.Count() > 1).SelectMany(group => group).ToList();
                else
                    duplicates = pidList.Where(x => Path.GetFileName(Path.GetDirectoryName(x.FilePath)) == "dlc").GroupBy(x => x.PID).Where(group => group.Count() > 1).SelectMany(group => group).ToList();

                distinctPIDS = duplicates.Select(x => x.PID).Distinct().ToList();

                colPID.Visible = true;
                colPIDArrangement.Visible = true;
                Globals.Log("Showing CDLC with duplicate PID's ... GAME CRASHERS!");
            }
            else
            {
                colPID.Visible = false;
                colPIDArrangement.Visible = false;

                if (chkSubFolders.Checked)
                    duplicates = Globals.SongCollection.GroupBy(x => x.ArtistTitleAlbum).Where(group => group.Count() > 1).SelectMany(group => group).ToList();
                else
                    duplicates = Globals.SongCollection.Where(x => Path.GetFileName(Path.GetDirectoryName(x.FilePath)) == "dlc").GroupBy(x => x.ArtistTitleAlbum).Where(group => group.Count() > 1).SelectMany(group => group).ToList();

                if (keyEnabled)
                {
                    if (chkSubFolders.Checked)
                        duplicates = Globals.SongCollection.Where(x => !Path.GetFileName(x.FilePath).Contains("disabled")).GroupBy(x => x.ArtistTitleAlbum).Where(group => group.Count() > 1).SelectMany(group => group).ToList();
                    else
                        duplicates = Globals.SongCollection.Where(x => Path.GetFileName(Path.GetDirectoryName(x.FilePath)) == "dlc" && !Path.GetFileName(x.FilePath).Contains("disabled")).GroupBy(x => x.ArtistTitleAlbum).Where(group => group.Count() > 1).SelectMany(group => group).ToList();

                    Globals.Log("Showing duplicate enabled songs ...");
                }
                else if (keyDisabled)
                {
                    if (chkSubFolders.Checked)
                        duplicates = Globals.SongCollection.Where(x => Path.GetFileName(x.FilePath).Contains("disabled")).GroupBy(x => x.ArtistTitleAlbum).Where(group => group.Count() > 1).SelectMany(group => group).ToList();
                    else
                        duplicates = Globals.SongCollection.Where(x => Path.GetFileName(Path.GetDirectoryName(x.FilePath)) == "dlc" && Path.GetFileName(x.FilePath).Contains("disabled")).GroupBy(x => x.ArtistTitleAlbum).Where(group => group.Count() > 1).SelectMany(group => group).ToList();

                    Globals.Log("Showing duplicate disabled songs ...");
                }
                else
                    Globals.Log("Showing duplicate CDLC ...");
            }

            duplicates.RemoveAll(x => x.FileName.ToLower().Contains(Constants.RS1COMP));

            // processing order effects datagridview appearance
            LoadFilteredBindingList(duplicates);
            CFSMTheme.InitializeDgvAppearance(dgvDuplicates);
            // reload column order, width, visibility
            if (!findDupPIDs)
                if (RAExtensions.ManagerGridSettings != null)
                {
                    dgvDuplicates.ReLoadColumnOrder(RAExtensions.ManagerGridSettings.ColumnOrder);
                    colPID.Visible = false;
                    colPIDArrangement.Visible = false;
                }
        }

        public void UpdateToolStrip()
        {
            if (dgvDuplicates.Rows.Count == 0)
                txtNoDuplicates.Visible = true;
            else
                txtNoDuplicates.Visible = false;

            Globals.TsLabel_MainMsg.Text = string.Format(Properties.Resources.RocksmithSongsCountFormat, Globals.SongCollection.Count);
            Globals.TsLabel_MainMsg.Visible = true;
            Globals.TsLabel_DisabledCounter.Alignment = ToolStripItemAlignment.Right;
            Globals.TsLabel_DisabledCounter.Text = String.Format(Properties.Resources.DuplicatesCountFormat, dgvDuplicates.Rows.Count);
            Globals.TsLabel_DisabledCounter.Visible = true;
            Globals.TsLabel_StatusMsg.Visible = false;
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

        private void LoadFilteredBindingList(dynamic list)
        {
            bindingCompleted = false;
            dgvPainted = false;
            // sortable binding list with drop down filtering
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
            DataGridViewAutoFilterTextBoxColumn.RemoveFilter(dgvDuplicates);
            LoadFilteredBindingList(duplicates);

            // reset alternating row color
            foreach (DataGridViewRow row in dgvDuplicates.Rows)
                row.DefaultCellStyle.BackColor = Color.Empty;

            dgvDuplicates.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle() { BackColor = Color.LightSteelBlue };
            UpdateToolStrip();
        }

        private void Rescan()
        {
            // save settings (column widths) in case user has modified
            Globals.Settings.SaveSettingsToFile(dgvDuplicates);

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
                return;

            Globals.ReloadDuplicates = true;
            Globals.ReloadSongManager = true;
            Globals.ReloadRenamer = true;
            Globals.ReloadSetlistManager = true;
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
            // using concatinated ArtistTitleAlbumDate column to order by/sort on
            var sortedDupes = duplicates.OrderBy(x => x.ArtistTitleAlbumDate.ToLower()).ToList();
            LoadFilteredBindingList(sortedDupes);

            // TODO: confirm this does what is expected
            var rowCount = dgvDuplicates.Rows.Count;
            for (int i = rowCount - 1; i >= 0; i--)
            {
                if (i - 1 == -1)
                    break;

                var currSong = DgvExtensions.GetObjectFromRow<SongData>(dgvDuplicates, i);
                var nextSong = DgvExtensions.GetObjectFromRow<SongData>(dgvDuplicates, i - 1);
                if (currSong.ArtistTitleAlbum.ToLower() == nextSong.ArtistTitleAlbum.ToLower())
                    dgvDuplicates.Rows[i - 1].Cells["colSelect"].Value = true;
            }
        }

        private void SelectionDeleteMove(DataGridView dgvCurrent, bool modeDelete = false)
        {
            // INFORMATION - deleting data from the dgv should be the same as deleting data
            // from the SongCollection because the dgv and the SongCollection are bound
            // similarly deleting data from the SongCollection should be the same as deleting data
            // from the dgv after it is refreshed because the two are bound to each other

            // user must check Select to Delete/Move
            var selection = DgvExtensions.GetObjectsFromRows<SongData>(dgvCurrent);
            if (!selection.Any()) return;

            if (modeDelete)
            {
                var diaMsg = "You are about to delete CDLC file(s)." + Environment.NewLine + "Deletion is permenant and can not be undone." + Environment.NewLine + "Do you want to continue?";
                if (DialogResult.No == BetterDialog2.ShowDialog(diaMsg, "Delete CDLC ...", null, "Yes", "No", Bitmap.FromHicon(SystemIcons.Warning.Handle), "Warning", 0, 150))
                    return;
            }

            for (int ndx = dgvCurrent.Rows.Count - 1; ndx >= 0; ndx--)
            {
                DataGridViewRow row = dgvCurrent.Rows[ndx];
                if (Convert.ToBoolean(row.Cells["colSelect"].Value))
                    TemporaryDisableDatabindEvent(() => { dgvCurrent.Rows.Remove(row); });
            }

            if (!modeDelete)
            {
                if (FileTools.CreateBackupOfType(selection, Constants.DuplicatesFolder, Constants.EXT_DUP, false))
                    FileTools.DeleteFiles(selection, false);
            }
            else
                FileTools.DeleteFiles(selection);

            // stops any unnecessary full rescan set by DeleteFiles worker
            Globals.RescanSongManager = false;

            // a hacky workaround to get rid of the remaining single datagrid row
            if (dgvCurrent.Rows.Count == 1)
                dgvCurrent.Rows.Clear();

            UpdateToolStrip();
        }


        private void SelectionEnableDisable(DataGridView dgvCurrent)
        {
            // user must check Select to Enable/Disable
            var selection = DgvExtensions.GetObjectsFromRows<SongData>(dgvCurrent);
            if (!selection.Any()) return;

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
                                var disabledPath = originalPath.Replace(Constants.PsarcExtension, Constants.DisabledPsarcExtension);
                                File.Move(originalPath, disabledPath);
                                sd.FilePath = disabledPath;
                                sd.Enabled = "No";
                            }
                            else
                            {
                                var enabledPath = originalPath.Replace(Constants.DisabledPsarcExtension, Constants.PsarcExtension);
                                File.Move(originalPath, enabledPath);
                                sd.FilePath = enabledPath;
                                sd.Enabled = "Yes";
                            }
                        }
                        catch (IOException ex)
                        {
                            MessageBox.Show(string.Format(Properties.Resources.UnableToEnableDisableSongX0InDlcFolderX1Er, Path.GetFileName(originalPath), Environment.NewLine, ex.Message));
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
                MessageBox.Show(string.Format("Please select (highlight) the song that  {0}you would like information about.", Environment.NewLine), Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void Duplicates_Resize(object sender, EventArgs e)
        {
            var p = new Point() { X = (Width - txtNoDuplicates.Width) / 2, Y = (Height - txtNoDuplicates.Height) / 2 };

            if (p.X < 3)
                p.X = 3;

            if (p.Y < 3)
                p.Y = 3;
            txtNoDuplicates.Location = p;
        }

        private void chkSubFolders_MouseUp(object sender, MouseEventArgs e)
        {
            if (dgvDuplicates.Columns["colPID"].Visible)
                PopulateDuplicates(true);
            else
                PopulateDuplicates();
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
                Process.Start("explorer.exe", string.Format("/select,\"{0}\"", directory.FullName));
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
                    if (song.OfficialDLC)
                    {
                        e.CellStyle.Font = Constants.OfficialDLCFont;
                        DataGridViewCell cell = dgvDuplicates.Rows[e.RowIndex].Cells["colSelect"];
                        DataGridViewCheckBoxCell chkCell = cell as DataGridViewCheckBoxCell;
                        chkCell.FlatStyle = FlatStyle.Flat;
                        chkCell.Style.ForeColor = Color.DarkGray;
                        // allow deletion of ODLC duplicates
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
            var rowIndex = e.RowIndex;

            if (e.Button == MouseButtons.Right)
            {
                if (rowIndex != -1)
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

            // programmatic left clicking on colSelect
            if (e.Button == MouseButtons.Left && e.RowIndex != -1 && e.ColumnIndex == colSelect.Index)
            {
                TemporaryDisableDatabindEvent(() => { dgvDuplicates.EndEdit(); });
            }

            Thread.Sleep(50); // debounce multiple clicks
            dgvDuplicates.Refresh();
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

            var filterStatus = DataGridViewAutoFilterColumnHeaderCell.GetFilterStatus(dgvDuplicates);
            // filter applied
            if (!String.IsNullOrEmpty(filterStatus) && dgvPainted)
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
            if (String.IsNullOrEmpty(filterStatus) && dgvPainted && this.dgvDuplicates.CurrentCell != null)
                RemoveFilter();
        }

        private void dgvDuplicates_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            Globals.DebugLog(String.Format("{0}, row:{1},col:{2}", e.Exception.Message, e.RowIndex, e.ColumnIndex));
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

        private void exploreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var filePath = duplicates[dgvDuplicates.SelectedRows[0].Index].FilePath;

            if (File.Exists(filePath))
                Process.Start("explorer.exe", string.Format("/select,\"{0}\"", filePath));
        }

        private void lnkLblSelectAll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SelectAllNone();
        }

        private void lnkPersistentId_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            PopulateDuplicates(!dgvDuplicates.Columns["colPID"].Visible);
            UpdateToolStrip();
        }

        private void lnkSelectOlderVersions_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (olderVersionsSelected)
            {
                // reload duplicates and deselect all
                LoadFilteredBindingList(duplicates);
                DgvExtensions.RowsCheckboxValue(dgvDuplicates, false);
                olderVersionsSelected = false;
            }

            else
            {
                SelectOlderVersions();
                olderVersionsSelected = true;
            }

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
            keyDisabled = false;
            keyEnabled = false;
            Rescan();
            PopulateDuplicates();
            UpdateToolStrip();
        }

        private void tsmiRescanEnabledDisabled_Click(object sender, EventArgs e)
        {
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

            Rescan();
            PopulateDuplicates();
            UpdateToolStrip();
        }

        public DataGridView GetGrid()
        {
            return dgvDuplicates;
        }

        public void TabEnter()
        {
            Globals.Log("Duplicate GUI Activated...");
            Globals.DgvCurrent = dgvDuplicates;
        }

        public void TabLeave()
        {
            Globals.Log("Leaving Duplicates GUI ...");
            Globals.Settings.SaveSettingsToFile(dgvDuplicates);
        }

        private void cmsShowSongInfo_Click(object sender, EventArgs e)
        {
            ShowSongInfo();
        }
    }
}