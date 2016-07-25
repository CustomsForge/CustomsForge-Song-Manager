using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using CustomsForgeSongManager.DataObjects;
using CustomsForgeSongManager.Forms;
using CustomsForgeSongManager.LocalTools;
using CustomsForgeSongManager.UITheme;
using DataGridViewTools;

namespace CustomsForgeSongManager.UControls
{
    public partial class Duplicates : UserControl, IDataGridViewHolder, INotifyTabChanged
    {
        private DataGridViewCellStyle ErrorStyle;
        private Color ErrorStyleBackColor = Color.DarkGray;
        private Color ErrorStyleForeColor = Color.White;
        private bool bindingCompleted = false;
        private bool dgvPainted = false;
        private List<string> distinctPIDS = new List<string>();
        private List<SongData> duplicates = new List<SongData>();
        private bool keyDisabled = false;
        private bool keyEnabled = false;

        public Duplicates()
        {
            InitializeComponent();
            Globals.TsLabel_StatusMsg.Click += lnkShowAll_Click;
            btnMove.Click += DeleteMoveSelected;
            btnDeleteSong.Click += DeleteMoveSelected;

            ErrorStyle = new DataGridViewCellStyle() { Font = new Font("Arial", 8, FontStyle.Italic), ForeColor = ErrorStyleForeColor, BackColor = ErrorStyleBackColor };

            txtNoDuplicates.Visible = false;
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
                MessageBox.Show(Properties.Resources.DuplicatesNeedToBeRescanned, Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

                // reset easter egg keys
                keyEnabled = false;
                keyDisabled = false;

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

            Globals.ReloadDuplicates = false;
        }

        public void UpdateToolStrip()
        {
            if (Globals.RescanDuplicates)
            {
                Globals.RescanDuplicates = false;
                Rescan();
                PopulateDuplicates();
            }

            if (Globals.ReloadDuplicates)
            {
                Globals.ReloadDuplicates = false;
                PopulateDuplicates();
            }

            if (dgvDuplicates.Rows.Count == 0)
            {
                //  dgvDuplicates.ColumnHeadersVisible = false;
                txtNoDuplicates.Visible = true;
            }
            else
            {
                // dgvDuplicates.ColumnHeadersVisible = true;
                txtNoDuplicates.Visible = false;
            }

            Globals.TsLabel_MainMsg.Text = string.Format(Properties.Resources.RocksmithSongsCountFormat, Globals.SongCollection.Count);
            Globals.TsLabel_MainMsg.Visible = true;
            Globals.TsLabel_DisabledCounter.Alignment = ToolStripItemAlignment.Right;
            Globals.TsLabel_DisabledCounter.Text = String.Format(Properties.Resources.DuplicatesCountFormat, dgvDuplicates.Rows.Count);
            Globals.TsLabel_DisabledCounter.Visible = true;
            Globals.TsLabel_StatusMsg.Visible = false;
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

        private void ShowSongInfo()
        {
            if (dgvDuplicates.SelectedRows.Count > 0)
            {
                //var selectedRow = dgvDuplicates.SelectedRows[0];
                //var title = selectedRow.Cells["colTitle"].Value.ToString();
                //var artist = selectedRow.Cells["colArtist"].Value.ToString();
                //var album = selectedRow.Cells["colAlbum"].Value.ToString();
                //var path = selectedRow.Cells["colFilePath"].Value.ToString();

                //var song = duplicates.FirstOrDefault(x => x.Title == title && x.Album == album && x.Artist == artist && x.Path == path);

                SongData song = (SongData)dgvDuplicates.SelectedRows[0].DataBoundItem;
                if (song != null)
                {
                    frmSongInfo infoWindow = new frmSongInfo(song);
                    infoWindow.Show();
                }
            }
            else
                MessageBox.Show(Properties.Resources.PleaseSelectHighlightTheSongThatNYouWould, Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void DeleteMoveSelected(object sender, System.EventArgs e)
        {
            if (dgvDuplicates.Rows.Count == 0)
                return;

            var selectedCount = dgvDuplicates.Rows.Cast<DataGridViewRow>().Count(row => Convert.ToBoolean(row.Cells["colSelect"].Value));
            if (selectedCount == 0)
            {
                MessageBox.Show(Properties.Resources.PleaseSelectTheCheckboxNextToSongsNthatYou, Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var buttonName = (Button)sender;

            if (buttonName.Text.ToLower().Contains("delete"))
                if (MessageBox.Show(Properties.Resources.DeleteTheSelectedDuplicates, Constants.ApplicationName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
                    return;

            var dupDir = Path.Combine(AppSettings.Instance.RSInstalledDir, "duplicates");

            if (!Directory.Exists(dupDir))
                Directory.CreateDirectory(dupDir);

            for (int ndx = dgvDuplicates.Rows.Count - 1; ndx >= 0; ndx--)
            {
                DataGridViewRow row = dgvDuplicates.Rows[ndx];

                if (Convert.ToBoolean(row.Cells["colSelect"].Value))
                {
                    // check if user manually (re)moved the song and has not rescanned 
                    if (File.Exists(duplicates[ndx].FilePath))
                    {
                        if (buttonName.Text.ToLower().Contains("move"))
                        {
                            var filePath = duplicates[ndx].FilePath;
                            var dupFileName = String.Format("{0}{1}", Path.GetFileName(filePath), ".dup");
                            var dupFilePath = Path.Combine(dupDir, dupFileName);

                            if (File.Exists(dupFilePath))
                                File.Delete(dupFilePath);

                            File.Move(duplicates[ndx].FilePath, dupFilePath);
                            Globals.Log(Properties.Resources.DuplicateFile + dupFileName);
                            Globals.Log(Properties.Resources.MovedTo + dupDir);
                        }
                        else
                            File.Delete(duplicates[ndx].FilePath);
                    }

                    dgvDuplicates.Rows.Remove(row);
                }
            }

            if (dgvDuplicates.Rows.Count == 1)
                dgvDuplicates.Rows.Clear();

            Globals.RescanDuplicates = true;
            UpdateToolStrip();
            // rescan on tabpage change to remove from Globals.SongCollection
            Globals.RescanSongManager = true;
            Globals.RescanSetlistManager = true;
            Globals.RescanRenamer = true;
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

        private void btnEnableDisable_Click(object sender, EventArgs e)
        {
            bool updateSongCollection = false;

            try
            {
                foreach (DataGridViewRow row in dgvDuplicates.Rows)
                {
                    var cell = (DataGridViewCheckBoxCell)row.Cells["colSelect"];

                    if (cell.Value == null)
                        cell.Value = false;

                    if (Convert.ToBoolean(cell.Value))
                    {
                        var originalPath = row.Cells["colFilePath"].Value.ToString();
                        if (!originalPath.ToLower().Contains(String.Format("{0}{1}", Constants.RS1COMP, "disc")))
                        {
                            var ColEnabled = row.Cells["colEnabled"];
                            // confirmed CDLC is disabled in game when using this file naming method
                            if (ColEnabled.Value.ToString() == "Yes")
                            {
                                var disabledDLCPath = originalPath.Replace("_p.psarc", "_p.disabled.psarc");
                                File.Move(originalPath, disabledDLCPath);
                                row.Cells["colFilePath"].Value = disabledDLCPath;
                                ColEnabled.Value = "No";
                            }
                            else
                            {
                                var enabledDLCPath = originalPath.Replace("_p.disabled.psarc", "_p.psarc");
                                File.Move(originalPath, enabledDLCPath);
                                row.Cells["colFilePath"].Value = enabledDLCPath;
                                ColEnabled.Value = "Yes";
                            }

                            cell.Value = "false";
                            updateSongCollection = true;
                            dgvDuplicates.Refresh();
                        }
                        else
                            Globals.Log(Properties.Resources.ThisIsARocksmith1SongItCanTBeDisabled);
                    }
                }
            }
            catch (IOException)
            {
                Globals.Log("<ERROR>: Unable to disable the song(s)!");
            }

            if (updateSongCollection)
            {
                UpdateToolStrip();
                // rescan on tabpage change
                Globals.RescanSongManager = true;
                Globals.RescanDuplicates = true;
                Globals.RescanSetlistManager = true;
                Globals.RescanRenamer = true;
            }
        }

        private void btnRescan_Click(object sender, EventArgs e)
        {
            Rescan();
            PopulateDuplicates();
            Globals.RescanDuplicates = false;
            Globals.ReloadDuplicates = false;
            UpdateToolStrip();
        }

        private void btnRescan_KeyDown(object sender, KeyEventArgs e)
        {
            keyDisabled = false;
            keyEnabled = false;

            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.D)
                keyDisabled = true;

            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.E)
                keyEnabled = true;
        }

        private void chkSubFolders_MouseUp(object sender, MouseEventArgs e)
        {
            if (dgvDuplicates.Columns["colPID"].Visible)
                PopulateDuplicates(true);
            else
                PopulateDuplicates();
        }

        private void dgvDuplicates_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // triggered by any key
            if (e.RowIndex != -1 && e.RowIndex != colSelect.Index)
                ShowSongInfo();
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
                    var s = DgvExtensions.GetObjectFromRow<SongData>(dgvDuplicates, e.RowIndex);
                    s.Selected = !s.Selected;
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
                                var s = DgvExtensions.GetObjectFromRow<SongData>(dgvDuplicates, i);
                                s.Selected = !s.Selected;
                            }
                        });
                    }
                }
            }
        }

        // use to manipulate data with causing error
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
                    // TODO: impliment cmsDuplicates action consistent with other grids
                    //  cmsDuplicates.Show(Cursor.Position);
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

        private void dgvDuplicates_KeyDown(object sender, KeyEventArgs e)
        {
            // space bar used to select a song (w/ checkbox "Select")
            if (e.KeyCode == Keys.Space)
                foreach (DataGridViewRow row in dgvDuplicates.Rows)
                    if (row.Selected)
                    {
                        if (row.Cells["colSelect"].Value == null)
                            row.Cells["colSelect"].Value = false;

                        if (Convert.ToBoolean(row.Cells["colSelect"].Value))
                            row.Cells["colSelect"].Value = false;
                        else
                            row.Cells["colSelect"].Value = true;
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

        private void dgvDups_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
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

            // Note to Lovro ... 
            // this small changes adds ability to show CDLC that have been replaced by ODLC i.e. duplicate CDLC and ODLC
            // added ODLC font highlighting
            try
            {
                SongData song = dgvDuplicates.Rows[e.RowIndex].DataBoundItem as SongData;

                if (song != null)
                {
                    if (song.OfficialDLC)
                    {
                        e.CellStyle.Font = Constants.OfficialDLCFont;
                        // prevent checking (selecting) ODCL all together ... evil genious code
                        DataGridViewCell cell = dgvDuplicates.Rows[e.RowIndex].Cells["colSelect"];
                        DataGridViewCheckBoxCell chkCell = cell as DataGridViewCheckBoxCell;
                        chkCell.Value = false;
                        chkCell.FlatStyle = FlatStyle.Flat;
                        chkCell.Style.ForeColor = Color.DarkGray;
                        // cell.ReadOnly = true;
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

        private void dgvDups_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            Globals.DebugLog(String.Format("{0}, row:{1},col:{2}", e.Exception.Message, e.RowIndex, e.ColumnIndex));
            e.Cancel = true;
        }

        private void exploreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var filePath = duplicates[dgvDuplicates.SelectedRows[0].Index].FilePath;

            if (File.Exists(filePath))
                Process.Start("explorer.exe", string.Format("/select,\"{0}\"", filePath));
        }

        private void lnkPersistentId_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            PopulateDuplicates(!dgvDuplicates.Columns["colPID"].Visible);
            UpdateToolStrip();
        }

        private void lnkShowAll_Click(object sender, EventArgs e)
        {
            RemoveFilter();
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

        /* Region of copy-pasta from SongManager */
        private void PopulateMenuWithColumnHeaders(ContextMenuStrip contextMenuStrip)
        {

            if (RAExtensions.ManagerGridSettings == null)
            {
                if (Globals.DgvCurrent == null)
                    Globals.DgvCurrent = dgvDuplicates;

                Globals.Settings.SaveSettingsToFile(dgvDuplicates);
            }

            contextMenuStrip.Items.Clear();
            var gridSettings = RAExtensions.ManagerGridSettings;

            foreach (ColumnOrderItem columnOrderItem in gridSettings.ColumnOrder)
            {
                var cn = dgvDuplicates.Columns[columnOrderItem.ColumnIndex].Name;
                if (cn.ToLower().StartsWith("col"))
                    cn = cn.Remove(0, 3);
                ToolStripMenuItem columnsMenuItem = new ToolStripMenuItem(cn, null, ColumnMenuItemClick) { Checked = dgvDuplicates.Columns[columnOrderItem.ColumnIndex].Visible, Tag = dgvDuplicates.Columns[columnOrderItem.ColumnIndex].Name };
                contextMenuStrip.Items.Add(columnsMenuItem);
            }
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
                            //   dgvSongsMaster.Invalidate();
                        }
                    }

                    //foreach (var item in dgvSongsMaster.Columns.Cast<DataGridViewColumn>())
                    //    if (item.Visible)
                    //        dgvSongsMaster.InvalidateCell(item.HeaderCell);
                }
            }
        }

        /*End of copy-pasta region*/
    }
}