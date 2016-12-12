using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Reflection;
using DataGridViewTools;
using CustomsForgeSongManager.Forms;
using CustomsForgeSongManager.UITheme;
using CustomsForgeSongManager.LocalTools;
using CFSM.GenTools;
using RocksmithToolkitLib.Extensions;
using RocksmithToolkitLib.DLCPackage;
using RocksmithToolkitLib.PsarcLoader;
using RocksmithToolkitLib.Sng;
using RocksmithToolkitLib.DLCPackage.Manifest.Functions;
using RocksmithToolkitLib.Xml;
using RocksmithToolkitLib;
using CustomsForgeSongManager.DataObjects;

namespace CustomsForgeSongManager.UControls
{
    public partial class RepairManager : UserControl, IDataGridViewHolder, INotifyTabChanged
    {
        private Bitmap MinusBitmap = new Bitmap(Properties.Resources.minus);
        private Bitmap PlusBitmap = new Bitmap(Properties.Resources.plus);

        private const string corExt = ".cor";
        private const string maxExt = ".max"; // backup
        private const string orgExt = ".org"; // backup

        private Stopwatch counterStopwatch = new Stopwatch();
        private List<string> bakFilePaths = new List<string>();
        private List<string> dlcFilePaths = new List<string>();
        private byte checkByte; // tracks repair checkbox condition
        private bool bindingCompleted = false;
        private bool allSelected = false;
        private bool dgvPainted = false;

        private StringBuilder sbErrors;
        private bool addedDD;
        private bool ddError;

        private int rFailed;
        private int rProcessed;
        private int rProgress;
        private int rSkipped;
        private int rTotal;

        private bool RepairMastery = true, ReapplyDD = false, IgnoreLimit = false, SkipRepaired = true, RepairOrg = false, PreserveStats = true, AddDD = true, IgnoreMultitoneEx = true, RepairMaxFive = true;
        private bool ROFormClosedByUser = false;
        private byte CheckByte;
        private string SelectedArrs = string.Empty;
        private SettingsDDC SettingsDD = new SettingsDDC();

        public RepairManager()
        {
            InitializeComponent();

            PopulateRepairsManager();
        }

        #region main DGV init
        private void LoadFilteredBindingList(dynamic list)
        {
            bindingCompleted = false;
            dgvPainted = false;
            // sortable binding list with drop down filtering
            dgvRepairManager.AutoGenerateColumns = false;
            FilteredBindingList<SongData> fbl = new FilteredBindingList<SongData>(list);
            BindingSource bs = new BindingSource { DataSource = fbl };
            dgvRepairManager.DataSource = bs;
        }

        private void PopulateRepairsManager()
        {
            Globals.Log("Populating RepairManager GUI ...");

            // respect processing order
            DgvExtensions.DoubleBuffered(dgvRepairManager);
            LoadFilteredBindingList(Globals.SongCollection);
            CFSMTheme.InitializeDgvAppearance(dgvRepairManager);
            // reload column order, width, visibility
            //Globals.Settings.LoadSettingsFromFile(dgvSongsMaster);

            if (RAExtensions.ManagerGridSettings != null)
                dgvRepairManager.ReLoadColumnOrder(RAExtensions.ManagerGridSettings.ColumnOrder);

            // lock OfficialDLC from being selected
            foreach (DataGridViewRow row in dgvRepairManager.Rows)
            {
                var sng = DgvExtensions.GetObjectFromRow<SongData>(row);
                if (sng.OfficialDLC)
                {
                    row.Cells["colSelect"].Value = false;
                    row.Cells["colSelect"].ReadOnly = sng.OfficialDLC;
                    sng.Selected = false;
                }
            }

            dgvRepairManager.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
            dgvRepairManager.Refresh();

            //Worker actually does the sorting after parsing, this is just to tell the grid that it is sorted.
            if (!String.IsNullOrEmpty(AppSettings.Instance.SortColumn))
            {
                var colX = dgvRepairManager.Columns.Cast<DataGridViewColumn>().Where(col => col.DataPropertyName == AppSettings.Instance.SortColumn).FirstOrDefault();
                if (colX != null)
                    dgvRepairManager.Sort(colX, AppSettings.Instance.SortAscending ? ListSortDirection.Ascending : ListSortDirection.Descending);
            }

            dgvPainted = true;
        }
        #endregion

        #region Others
        private void TemporaryDisableDatabindEvent(Action action)
        {
            dgvRepairManager.DataBindingComplete -= dgvRepairManager_DataBindingComplete;
            try
            {
                action();
            }
            finally
            {
                dgvRepairManager.DataBindingComplete += dgvRepairManager_DataBindingComplete;
            }
        }

        public void UpdateToolStrip()
        {
            Globals.TsLabel_MainMsg.Text = string.Format("Rocksmith Song Count: {0}", Globals.SongCollection.Count);
            Globals.TsLabel_MainMsg.Visible = true;
            Globals.TsLabel_DisabledCounter.Alignment = ToolStripItemAlignment.Right;
            Globals.TsLabel_DisabledCounter.Visible = true;
            Globals.TsLabel_StatusMsg.Visible = false;
        }

        private void ResetDetail()
        {
            //reset plus/minus in case user did not
            for (int ndx = dgvRepairManager.Rows.Count - 1; ndx >= 0; ndx--)
                if (!String.IsNullOrEmpty(dgvRepairManager.Rows[ndx].Cells["colShowDetail"].Tag as String))
                {
                    dgvRepairManager.Rows[ndx].Cells["colShowDetail"].Value = PlusBitmap;
                    dgvRepairManager.Rows[ndx].Cells["colShowDetail"].Tag = null;
                    dgvSongsDetail.Visible = false;
                    break;
                }
        }

        private void btnRepairOptions_Click(object sender, EventArgs e)
        {
            cmsRepairOptons.Show(btnRepairOptions, 0, btnRepairOptions.Height + 2);
        }

        private void dgvRepairManager_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            // HACK: catch DataBindingComplete called by other UC's
            var grid = (DataGridView)sender;
            if (grid.Name != "dgvRepairManager")
                return;

            if (e.ListChangedType != ListChangedType.Reset)
                return;

            // need to wait for DataBinding and DataGridView Paint to complete before  
            // changing BLRV column color (cell formating) on initial loading

            if (!bindingCompleted)
            {
                // Globals.Log("DataBinding Complete ... ");
                bindingCompleted = true;
            }

            var filterStatus = DataGridViewAutoFilterColumnHeaderCell.GetFilterStatus(dgvRepairManager);
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
                chkMyCDLC.Checked = false;
            }

            // filter removed
            if (String.IsNullOrEmpty(filterStatus) && dgvPainted && this.dgvRepairManager.CurrentCell != null)
                RemoveFilter();
        }

        private void RemoveFilter()
        {
            Globals.Settings.SaveSettingsToFile(dgvRepairManager);
            chkMyCDLC.Checked = false;
            DataGridViewAutoFilterTextBoxColumn.RemoveFilter(dgvRepairManager);
            ResetDetail();

            if (!chkSubFolders.Checked)
            {
                var results = Globals.SongCollection.Where(x => Path.GetFileName(Path.GetDirectoryName(x.FilePath)) == "dlc").ToList();
                LoadFilteredBindingList(results);
            }
            else
                LoadFilteredBindingList(Globals.SongCollection);

            // reset alternating row color
            foreach (DataGridViewRow row in dgvRepairManager.Rows)
                row.DefaultCellStyle.BackColor = Color.Empty;

            dgvRepairManager.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle() { BackColor = Color.LightSteelBlue };
            UpdateToolStrip();
        }

        private void SearchCDLC(string criteria)
        {
            var lowerCriteria = criteria.ToLower();
            var results = Globals.SongCollection.Where(x => x.ArtistTitleAlbum.ToLower().Contains(lowerCriteria) || x.Tuning.ToLower().Contains(lowerCriteria) || x.Arrangements.ToLower().Contains(lowerCriteria) || x.CharterName.ToLower().Contains(lowerCriteria) || (x.IgnitionAuthor != null && x.IgnitionAuthor.ToLower().Contains(lowerCriteria) || (x.IgnitionID != null && x.IgnitionID.ToLower().Contains(lowerCriteria)) || x.SongYear.ToString().Contains(criteria) || x.FilePath.ToLower().Contains(lowerCriteria))).ToList();

            if (!chkSubFolders.Checked)
                results = results.Where(x => Path.GetFileName(Path.GetDirectoryName(x.FilePath)) == "dlc").ToList();

            LoadFilteredBindingList(results);
        }

        private void ColumnMenuItemClick(object sender, EventArgs eventArgs)
        {
            ToolStripMenuItem currentContextMenuItem = sender as ToolStripMenuItem;
            if (currentContextMenuItem != null)
            {
                if (!string.IsNullOrEmpty(currentContextMenuItem.Tag.ToString()))
                {
                    var dataGridViewColumn = dgvRepairManager.Columns[currentContextMenuItem.Tag.ToString()];
                    if (dataGridViewColumn != null)
                    {
                        var columnIndex = dataGridViewColumn.Index;
                        var columnSetting = RAExtensions.ManagerGridSettings.ColumnOrder.SingleOrDefault(x => x.ColumnIndex == columnIndex);
                        if (columnSetting != null)
                        {
                            columnSetting.Visible = !columnSetting.Visible;
                            dgvRepairManager.Columns[columnIndex].Visible = columnSetting.Visible;
                            currentContextMenuItem.Checked = columnSetting.Visible;
                        }
                    }

                }
            }
        }

        private void PopulateMenuWithColumnHeaders(ContextMenuStrip contextMenuStrip)
        {

            if (RAExtensions.ManagerGridSettings == null)
            {
                if (Globals.DgvCurrent == null)
                    Globals.DgvCurrent = dgvRepairManager;

                Globals.Settings.SaveSettingsToFile(dgvRepairManager);
            }

            contextMenuStrip.Items.Clear();
            var gridSettings = RAExtensions.ManagerGridSettings;

            foreach (ColumnOrderItem columnOrderItem in gridSettings.ColumnOrder)
            {
                var cn = dgvRepairManager.Columns[columnOrderItem.ColumnIndex].Name;
                if (cn.ToLower().StartsWith("col"))
                    cn = cn.Remove(0, 3);
                ToolStripMenuItem columnsMenuItem = new ToolStripMenuItem(cn, null, ColumnMenuItemClick) { Checked = dgvRepairManager.Columns[columnOrderItem.ColumnIndex].Visible, Tag = dgvRepairManager.Columns[columnOrderItem.ColumnIndex].Name };
                contextMenuStrip.Items.Add(columnsMenuItem);
            }
        }

        private void ToggleUIControls(bool enabled)
        {

        }
        #endregion

        #region LinkLabel events
        private void lnklblToggle_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            TemporaryDisableDatabindEvent(() =>
            {
                foreach (DataGridViewRow row in dgvRepairManager.Rows)
                    row.Cells["colSelect"].Value = !Convert.ToBoolean(row.Cells["colSelect"].Value);
            });
        }

        private void lnkLblSelectAll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            TemporaryDisableDatabindEvent(() =>
            {
                foreach (DataGridViewRow row in dgvRepairManager.Rows)
                    row.Cells["colSelect"].Value = !allSelected;
            });

            allSelected = !allSelected;
        }

        private void lnkClearSearch_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            cueSearch.Text = String.Empty;
            RemoveFilter();
        }
        #endregion

        #region Other events
        private void cueSearch_KeyUp(object sender, KeyEventArgs e)
        {
            ResetDetail();

            if (cueSearch.Text.Length > 0) // && e.KeyCode == Keys.Enter)
                SearchCDLC(cueSearch.Text);
            else
                LoadFilteredBindingList(Globals.SongCollection);
        }
        #endregion

        #region DGV events
        private void dgvSongsMaster_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // make sure grid has been painted before proceeding
            if (!dgvPainted)
                return;

            // get detail from master
            if (e.RowIndex >= 0 && e.ColumnIndex == colShowDetail.Index)
            {
                if (dgvRepairManager.Rows[e.RowIndex].Cells["colKey"].Value == null)
                    return;

                var songKey = dgvRepairManager.Rows[e.RowIndex].Cells["colKey"].Value.ToString();

                if (String.IsNullOrEmpty(songKey))
                    return;

                if (dgvSongsDetail.Visible)
                    if (dgvSongsDetail.Rows[0].Cells["colDetailKey"].Value.ToString() != songKey)
                        ResetDetail();

                if (String.IsNullOrEmpty(dgvRepairManager.Rows[e.RowIndex].Cells["colShowDetail"].Tag as String))
                {
                    var songDetails = Globals.SongCollection.Where(master => (master.DLCKey == songKey)).ToList();
                    if (!songDetails.Any())
                        MessageBox.Show("No Song Details Found");
                    else // TODO: change positioning if near bottom of dgvSongsMaster
                    {
                        // apply some formatting
                        dgvRepairManager.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dgvSongsDetail.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;
                        dgvSongsDetail.Columns["colDetailPID"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dgvSongsDetail.Columns["colDetailSections"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dgvSongsDetail.Columns["colDetailDMax"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                        var rowHeight = dgvRepairManager.Rows[e.RowIndex].Height + 0; // height tweak
                        var colWidth = dgvRepairManager.Columns[e.ColumnIndex].Width - 16; // width tweak
                        dgvRepairManager.Rows[e.RowIndex].Cells["colShowDetail"].Tag = "TRUE";
                        dgvRepairManager.Rows[e.RowIndex].Cells["colShowDetail"].Value = MinusBitmap;
                        Rectangle dgvRectangle = dgvRepairManager.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
                        dgvSongsDetail.Location = new Point(dgvRectangle.Right + colWidth, dgvRectangle.Bottom + rowHeight - 2);

                        // CRITICAL CODE AREA - CAREFUL - No Filtering
                        dgvSongsDetail.AutoGenerateColumns = false;
                        dgvSongsDetail.DataSource = songDetails;
                        dgvSongsDetail.DataMember = "Arrangements2D";

                        // calculate the height and width of dgvSongsDetail
                        dgvSongsDetail.Columns["colDetailKey"].Width = dgvRepairManager.Columns["colKey"].Width;
                        var colHeaderHeight = dgvSongsDetail.Columns[e.ColumnIndex].HeaderCell.Size.Height;
                        dgvSongsDetail.Height = dgvSongsDetail.Rows.Cast<DataGridViewRow>().Sum(row => row.Height) + colHeaderHeight - 3;
                        dgvSongsDetail.Width = dgvSongsDetail.Columns.Cast<DataGridViewColumn>().Sum(col => col.Width) + colWidth;
                        if (dgvSongsDetail.Rows.Count < 3) // need extra tweak 
                            dgvSongsDetail.Height = dgvSongsDetail.Height + 4;

                        dgvSongsDetail.Refresh();
                        //dgvSongsDetail.Invalidate();
                        dgvSongsDetail.Visible = true;
                        dgvRepairManager.ScrollBars = ScrollBars.Horizontal;
                    }
                }
                else
                {
                    dgvRepairManager.Rows[e.RowIndex].Cells["colShowDetail"].Value = PlusBitmap;
                    dgvRepairManager.Rows[e.RowIndex].Cells["colShowDetail"].Tag = null;
                    dgvSongsDetail.Visible = false;
                    dgvRepairManager.ScrollBars = ScrollBars.Both;
                }
            }

        }
        private void dgvSongsMaster_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.A)
            {
                TemporaryDisableDatabindEvent(() =>
                {
                    for (int i = 0; i < dgvRepairManager.Rows.Count; i++)
                    {
                        DgvExtensions.GetObjectFromRow<SongData>(dgvRepairManager, i).Selected = allSelected;
                    }
                });
                allSelected = !allSelected;
            }
        }

        private void dgvSongsMaster_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt && (e.KeyCode == Keys.Down || e.KeyCode == Keys.Up))
            {
                DataGridViewAutoFilterColumnHeaderCell filterCell = this.dgvRepairManager.CurrentCell.OwningColumn.HeaderCell as DataGridViewAutoFilterColumnHeaderCell;

                if (filterCell != null)
                {
                    filterCell.ShowDropDownList();
                    e.Handled = true;
                }
            }

            // space bar used to select a song (w/ checkbox "Select")
            if (e.KeyCode == Keys.Space)
            {
                for (int i = 0; i < dgvRepairManager.Rows.Count; i++)
                {
                    if (dgvRepairManager.Rows[i].Selected)
                    {
                        var sng = DgvExtensions.GetObjectFromRow<SongData>(dgvRepairManager, i);
                        // beyound current scope of CFM
                        if (sng.IsRsCompPack)
                            Globals.Log(Properties.Resources.CanNotSelectIndividualRS1CompatiblityDLC);
                        else
                        {
                            sng.Selected = !sng.Selected;
                        }
                    }
                }
            }
        }

        private void dgvSongsMaster_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dgvRepairManager.DataSource == null)
                return;

            ResetDetail();
            // Ctrl Key w/ left mouse click to quickly turn off column visiblity
            if (ModifierKeys == Keys.Control)
            {
                dgvRepairManager.Columns[e.ColumnIndex].Visible = false;
                return;
            }
        }

        private void dgvSongsMaster_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex == -1)
                return;

            // same in all grids
            if (e.Button == MouseButtons.Left)
            {
                // select a single row by Ctrl-Click
                if (ModifierKeys == Keys.Control)
                {
                    var s = DgvExtensions.GetObjectFromRow<SongData>(dgvRepairManager, e.RowIndex);
                    s.Selected = !s.Selected;
                }
                // select multiple rows by Shift-Click two outer rows
                else if (ModifierKeys == Keys.Shift)
                {
                    if (dgvRepairManager.SelectedRows.Count > 0)
                    {
                        var first = dgvRepairManager.SelectedRows[0];
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
                                var s = DgvExtensions.GetObjectFromRow<SongData>(dgvRepairManager, i);
                                s.Selected = !s.Selected;
                            }
                        });
                    }
                }
            }
        }

        private void dgvSongsMaster_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            // TODO: make consistent with other grids
            // has precedent over a ColumnHeader_MouseClick
            // MouseUp detection is more reliable than MouseDown
            var grid = (DataGridView)sender;
            var rowIndex = e.RowIndex;

            if (e.Button == MouseButtons.Right)
            {
                if (rowIndex != -1)
                {
                    // left this for future use?
                    grid.Rows[e.RowIndex].Selected = true;
                    var sng = DgvExtensions.GetObjectFromRow<SongData>(dgvRepairManager, e.RowIndex);
                }
                else
                {
                    PopulateMenuWithColumnHeaders(cmsRepairManagerColumns);
                    cmsRepairManagerColumns.Show(Cursor.Position);
                }
            }

            // programmatic left clicking on colSelect
            if (e.Button == MouseButtons.Left && e.RowIndex != -1 && e.ColumnIndex == colSelect.Index)
            {
                // beyound current scope of CFM
                if (grid.Rows[e.RowIndex].Cells["colSelect"].Value.ToString().ToLower().Contains(Constants.RS1COMP))
                    Globals.Log(Properties.Resources.CanNotSelectIndividualRS1CompatiblityDLC);
                else // required to force selected row change
                {
                    TemporaryDisableDatabindEvent(() => { dgvRepairManager.EndEdit(); });
                }
            }

            Thread.Sleep(50); // debounce multiple clicks
            dgvRepairManager.Refresh();
        }

        private void dgvSongsMaster_Sorted(object sender, EventArgs e)
        {
            if (dgvRepairManager.SortedColumn != null)
            {
                AppSettings.Instance.SortColumn = dgvRepairManager.SortedColumn.DataPropertyName;
                AppSettings.Instance.SortAscending = dgvRepairManager.SortOrder == SortOrder.Ascending ? true : false;
            }
        }

        #endregion

        #region Repair tools
        private void GetRepairOptions(bool ShowDDTab = false)
        {
            using (frmRepairOptions frmRO = new frmRepairOptions())
            {
                frmRO.ShowDDTab = ShowDDTab;
                frmRO.ShowDialog();
                
                ReapplyDD = frmRO.ReapplyDD;
                RepairMaxFive = frmRO.RepairMax5Arr;
                SkipRepaired = frmRO.SkipRepaired;
                IgnoreLimit = frmRO.IgnoreStopLimit;
                IgnoreMultitoneEx = frmRO.IgnoreMultiToneExceptions;
                PreserveStats = frmRO.PreserveStats;
                CheckByte = frmRO.CheckByte;
                SettingsDD = frmRO.SettingsDD;
                PreserveStats = frmRO.PreserveStats;
                ROFormClosedByUser = frmRO.FormClosedByUser;
                SelectedArrs = frmRO.SelectedArrangements;
            }
        }

        private void ReportProgress(int processed, int total, int skipped, int failed)
        {
            int progress;
            if (total > 0)
                progress = processed * 100 / total;
            else
                progress = 100;

            // load to private memory so that when user leaves tab they have some data to come back to
            rProgress = progress;
            rProcessed = processed;
            rTotal = total;
            rSkipped = skipped;
            rFailed = failed;

            if (Globals.TsProgressBar_Main != null && progress <= 100)
                GenExtensions.InvokeIfRequired(Globals.TsProgressBar_Main.GetCurrentParent(), delegate { Globals.TsProgressBar_Main.Value = progress; });

            GenExtensions.InvokeIfRequired(Globals.TsLabel_MainMsg.GetCurrentParent(), delegate { Globals.TsLabel_MainMsg.Text = String.Format("Files Processed: {0} of {1}", processed, total); });
            GenExtensions.InvokeIfRequired(Globals.TsLabel_StatusMsg.GetCurrentParent(), delegate { Globals.TsLabel_StatusMsg.Text = String.Format("Skipped: {0}  Failed: {1}", skipped, failed); });
            GenExtensions.InvokeIfRequired(this, delegate { this.Refresh(); });
        }

        public void ArchiveCorruptCDLC()
        {
            Globals.Log("Archiving corrupt CDLC files ...");

            var corFilePaths = Directory.EnumerateFiles(Constants.RemasteredCorFolder, "*" + corExt + "*").ToList();
            if (!corFilePaths.Any())
            {
                Globals.Log("No corrupt CDLC to archive: " + Constants.RemasteredCorFolder);
                return;
            }

            var fileName = String.Format("{0}_{1}", "Corrupt_CDLC", DateTime.Now.ToString("yyyyMMdd_hhmm")).GetValidFileName();
            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter = ".zip files (*.zip)|*.zip";
                sfd.FilterIndex = 0;
                sfd.InitialDirectory = Constants.RemasteredFolder;
                sfd.FileName = fileName;

                if (sfd.ShowDialog() != DialogResult.OK)
                    return;

                fileName = sfd.FileName;
            }

            // save zip file to 'remastered' folder so that it is not accidently deleted
            try
            {
                if (ZipUtilities.ZipDirectory(Constants.RemasteredCorFolder, Path.Combine(Constants.RemasteredFolder, fileName)))
                    Globals.Log("Archive saved to: " + Path.Combine(Constants.RemasteredFolder, fileName));
                else
                    Globals.Log("Archiving failed ...");
            }
            catch (IOException ex)
            {
                Globals.Log("Archiving failed ...");
                Globals.Log(ex.Message);
            }
        }

        public void RepairSongs()
        {
            RepairSongs(Globals.SongCollection.ToList());
        }

        public void RepairSong(SongData song)
        {
            RepairSongs(new List<SongData>() { song });
        }

        public void RepairSongs(List<SongData> songs)
        {
            if (ROFormClosedByUser)
                return;

            sbErrors = new StringBuilder();
            List<string> srcFilePaths = new List<string>();

            // make sure 'dlc' folder is clean
            CleanDlcFolder();
            Globals.Log("Applying selected repairs to CDLC ...");

            songs = songs.Where(s => !s.FilePath.ToLower().Contains(Constants.RS1COMP) && !s.FilePath.ToLower().Contains(Constants.SONGPACK) && !s.FilePath.ToLower().Contains(Constants.ABVSONGPACK)).ToList();
            songs.ForEach(s =>
            {
                srcFilePaths.Add(s.FilePath);
            });

            // ignore the inlay(s) folder
            srcFilePaths = srcFilePaths.Where(x => !x.ToLower().Contains("inlay")).ToList();

            if (RepairOrg)
                srcFilePaths = Directory.EnumerateFiles(Constants.RemasteredOrgFolder, "*" + orgExt + "*").ToList();

            var total = srcFilePaths.Count;
            var processed = 0;
            var failed = 0;
            var skipped = 0;
            ReportProgress(processed, total, skipped, failed);

            foreach (var srcFilePath in srcFilePaths)
            {
                var isSkipped = false;
                processed++;

                var officialOrRepaired = OfficialOrRepaired(srcFilePath);
                if (!String.IsNullOrEmpty(officialOrRepaired))
                {
                    if (officialOrRepaired.Contains("Official"))
                    {
                        // GenExtensions.InvokeIfRequired(this, delegate { dgvRepair.Rows.Add(Path.GetFileName(srcFilePath), "Skipped ODLC File"); });

                        Globals.Log("Skipped ODLC File - " + srcFilePath);
                        skipped++;
                        isSkipped = true;
                    }

                    if (officialOrRepaired.Contains("Remastered") && SkipRepaired)
                    {
                        // GenExtensions.InvokeIfRequired(this, delegate { dgvRepair.Rows.Add(Path.GetFileName(srcFilePath), "Skipped Remastered File"); });

                        Globals.Log("Skipped Remastered File - " + srcFilePath);
                        skipped++;
                        isSkipped = true;
                    }
                }

                // remaster the CDLC file
                if (!isSkipped)
                {
                    var rSucess = RemasterSong(srcFilePath, ref sbErrors, CheckByte, PreserveStats, RepairOrg, IgnoreMultitoneEx, AddDD, RepairMaxFive, IgnoreLimit, SettingsDD, ReapplyDD);

                    if (rSucess)
                    {
                        var message = String.Format("Repair Sucessful ... {0}", PreserveStats ? "Preserved Song Stats" : "Reset Song Stats");
                        if (RepairOrg)
                            message += " ... Used (" + orgExt + ") File";
                        if (addedDD)
                            message += " ... Added Dynamic Difficulty";
                        if (ddError)
                            message += " ... Error Adding Dynamic Difficulty";

                        //GenExtensions.InvokeIfRequired(this, delegate { dgvRepair.Rows.Add(Path.GetFileName(srcFilePath).Replace(orgExt, ""), message); });

                        Globals.Log(srcFilePath + " - " + message);

                        if (Constants.DebugMode)
                        {
                            // cleanup every nth record
                            if (processed % 50 == 0)
                                GenExtensions.CleanLocalTemp();
                        }
                    }
                    else
                    {
                        var lines = sbErrors.ToString().Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
                        if (lines.Last().ToLower().Contains("maximum"))
                            Globals.Log(string.Format("File {0} exceeds playable arrangements limit ... Moved File ... Added to Error Log", Path.GetFileName(srcFilePath)));
                        else
                            Globals.Log(string.Format("File {0} is a corrupt CDLC ... Moved File ... Added To Error Log", Path.GetFileName(srcFilePath)));

                        //if (lines.Last().ToLower().Contains("maximum"))
                        //    GenExtensions.InvokeIfRequired(this, delegate { dgvRepair.Rows.Add(Path.GetFileName(srcFilePath), "Exceeds Playable Arrangements Limit ... Moved File ... Added To Error Log"); });
                        //else
                        //    GenExtensions.InvokeIfRequired(this, delegate { dgvRepair.Rows.Add(Path.GetFileName(srcFilePath), "Corrupt CDLC ... Moved File ... Added To Error Log"); });

                        failed++;
                    }
                }

                //GenExtensions.InvokeIfRequired(this, delegate
                //{
                //    // dgvRepair.Rows[dgvRepair.Rows.Count - 1].Selected = true;
                //    dgvRepair.ClearSelection();
                //    dgvRepair.FirstDisplayedScrollingRowIndex = dgvRepair.Rows.Count - 1;
                //    dgvRepair.Refresh();
                //});

                ReportProgress(processed, total, skipped, failed);
            }

            if (!String.IsNullOrEmpty(sbErrors.ToString())) //failed > 0)
            {
                // error log can be turned into CSV file
                sbErrors.Insert(0, "File Path, Error Message" + Environment.NewLine);
                sbErrors.Insert(0, DateTime.Now.ToString("MM-dd-yy HH:mm") + Environment.NewLine);
                using (TextWriter tw = new StreamWriter(Constants.RemasteredErrorLogPath, true))
                {
                    tw.WriteLine(sbErrors + Environment.NewLine);
                    tw.Close();
                }

                Globals.Log("Saved error log to: " + Constants.RemasteredErrorLogPath + " ...");
            }

            if (processed > 0)
            {
                if (processed > 1)
                    Globals.Log("CDLC repair completed ...");
                //Globals.RescanSongManager = true;

                if (Constants.DebugMode)
                    GenExtensions.CleanLocalTemp();
            }
            else
                Globals.Log("No CDLC were repaired ...");
        }

        public void RestoreBackups(string backupExt, string backupFolder)
        {
            ValidateBackupFolders();
            Globals.Log("Restoring (" + backupExt + ") CDLC ...");
            dlcFilePaths = Directory.EnumerateFiles(Constants.Rs2DlcFolder, "*.psarc", SearchOption.AllDirectories).Where(fi => !fi.ToLower().Contains(Constants.RS1COMP) && !fi.ToLower().Contains(Constants.SONGPACK) && !fi.ToLower().Contains(Constants.ABVSONGPACK)).ToList();
            // ignore the inlay(s) folder
            dlcFilePaths = dlcFilePaths.Where(x => !x.ToLower().Contains("inlay")).ToList();
            bakFilePaths = Directory.EnumerateFiles(backupFolder, "*" + backupExt + "*").ToList();

            var dlcFilePath = String.Empty;
            var total = bakFilePaths.Count;
            int processed = 0, failed = 0, skipped = 0;
            ReportProgress(processed, total, skipped, failed);

            foreach (var bakFilePath in bakFilePaths)
            {
                processed++;
                try
                {
                    var dlcFileName = Path.GetFileName(bakFilePath).Replace(backupExt, "");
                    dlcFilePath = Path.Combine(Constants.Rs2DlcFolder, dlcFileName);

                    // make sure bakExt file gets put back into the correct 'dlc' subfolder
                    // if CDLC is not found then bakExt file is put into default 'dlc' folder
                    var remasteredFilePath = dlcFilePaths.FirstOrDefault(x => x.Contains(dlcFileName));
                    if (remasteredFilePath != null)
                        dlcFilePath = Path.Combine(Path.GetDirectoryName(remasteredFilePath), dlcFileName);

                    // copy but don't delete bakExt
                    File.SetAttributes(bakFilePath, FileAttributes.Normal);
                    File.Copy(bakFilePath, dlcFilePath, true);

                    Globals.Log("Sucessfuly restored (" + backupExt + ") Backup of " + Path.GetFileName(dlcFilePath));

                    //GenExtensions.InvokeIfRequired(this, delegate { dgvRepair.Rows.Add(Path.GetFileName(dlcFilePath), "Sucessfully Restored (" + backupExt + ") Backup"); });
                }
                catch (IOException ex)
                {
                    Globals.Log(ex.Message);
                    Globals.Log("Could Not Restore (" + backupExt + ") Backup of " + Path.GetFileName(dlcFilePath));

                    failed++;
                    //GenExtensions.InvokeIfRequired(this, delegate { dgvRepair.Rows.Add(Path.GetFileName(dlcFilePath), "Could Not Restore (" + backupExt + ") Backup"); });
                }

                //GenExtensions.InvokeIfRequired(this, delegate
                //{
                //    // dgvRepair.Rows[dgvRepair.Rows.Count - 1].Selected = true;
                //    dgvRepair.ClearSelection();
                //    dgvRepair.FirstDisplayedScrollingRowIndex = dgvRepair.Rows.Count - 1;
                //    dgvRepair.Refresh();
                //});

                ReportProgress(processed, total, skipped, failed);
            }

            if (processed > 0)
            {
                Globals.Log("CDLC (" + backupExt + ") backups restored to original location in 'dlc' folder ...");
                Globals.RescanSongManager = true;
            }
            else
                Globals.Log("No (" + backupExt + ") backup CDLC restored from: " + backupFolder);
        }

        private void CleanDlcFolder()
        {
            ValidateBackupFolders();

            // remove any (.org, (.max) and (.cor) files from dlc folder and subfolders
            Globals.Log("Cleaning 'dlc' folder and subfolders ...");
            string[] extensions = { orgExt, maxExt, corExt };
            var extFilePaths = Directory.EnumerateFiles(Constants.Rs2DlcFolder, "*.*", SearchOption.AllDirectories).Where(fi => extensions.Any(fi.ToLower().Contains)).ToList();

            var total = extFilePaths.Count;
            var processed = 0;
            var failed = 0;
            var skipped = 0;
            ReportProgress(processed, total, skipped, failed);

            foreach (var extFilePath in extFilePaths)
            {
                processed++;
                var destFilePath = extFilePath;
                if (extFilePath.Contains(orgExt))
                    destFilePath = Path.Combine(Constants.RemasteredOrgFolder, Path.GetFileName(extFilePath));
                if (extFilePath.Contains(maxExt))
                    destFilePath = Path.Combine(Constants.RemasteredMaxFolder, Path.GetFileName(extFilePath));
                if (extFilePath.Contains(corExt))
                    destFilePath = Path.Combine(Constants.RemasteredCorFolder, Path.GetFileName(extFilePath));

                try
                {
                    File.SetAttributes(extFilePath, FileAttributes.Normal);
                    if (!File.Exists(destFilePath))
                    {
                        File.Copy(extFilePath, destFilePath, true);
                        Globals.Log("Moved file to: " + Path.GetFileName(destFilePath));

                        //GenExtensions.InvokeIfRequired(this, delegate { dgvRepair.Rows.Add(Path.GetFileName(destFilePath), "Moved File To: " + Path.GetDirectoryName(destFilePath)); });
                    }
                    else
                    {
                        Globals.Log("Deleted duplicate file: " + Path.GetFileName(extFilePath));
                        skipped++;

                        //GenExtensions.InvokeIfRequired(this, delegate { dgvRepair.Rows.Add(Path.GetFileName(extFilePath), "Deleted Duplicate File"); });
                    }

                    // this could throw an error if file is "Read-Only" or does not exist
                    File.Delete(extFilePath);
                }
                catch (IOException ex)
                {
                    Globals.Log("Moving of the file failed");
                    Globals.Log(ex.Message);
                    failed++;

                    //GenExtensions.InvokeIfRequired(this, delegate { dgvRepair.Rows.Add(Path.GetFileName(extFilePath), "Move File Failed"); });
                }

                ReportProgress(processed, total, skipped, failed);
            }

            // Commented out ... so devs don't hear, "I deleted all my cdlc files" 
            // Remove originals from Remastered_backup/orignals folder
            //DirectoryInfo backupDir = new DirectoryInfo(Constants.RemasteredCLI_OrgCDLCFolder);
            //backupDir.CleanDir();

            if (processed > 0)
            {
                Globals.RescanSongManager = true;
                Globals.Log("Finished cleaning 'dlc' folder and subfolders ...");
            }
            else
                Globals.Log("The 'dlc' folder and subfolders didn't need cleaning ...");
        }

        private bool CreateBackup(string srcFilePath)
        {
            Globals.Log(" - Making a backup copy (" + orgExt + ") ...");
            try
            {
                var properExt = Path.GetExtension(srcFilePath);
                var orgFilePath = String.Format(@"{0}{1}{2}", Path.Combine(Constants.RemasteredOrgFolder, Path.GetFileNameWithoutExtension(srcFilePath)), orgExt, properExt).Trim();

                if (!File.Exists(orgFilePath))
                {
                    File.SetAttributes(srcFilePath, FileAttributes.Normal);
                    File.Copy(srcFilePath, orgFilePath, false);
                    Globals.Log(" - Sucessfully created backup ..."); // a good thing
                }
                else
                    Globals.Log(" - Backup already exists ..."); // also a good thing
            }
            catch (Exception ex)
            {
                // it is critical that backup of originals was successful before proceeding
                Globals.Log(" - Backup failed ..."); // a bad thing
                Globals.Log(ex.Message);
                sbErrors.AppendLine(String.Format("{0}, Backup Failed", srcFilePath));
                return false;
            }

            return true;
        }

        private void ValidateBackupFolders()
        {
            if (!Directory.Exists(Constants.RemasteredFolder))
                Directory.CreateDirectory(Constants.RemasteredFolder);

            if (!Directory.Exists(Constants.RemasteredOrgFolder))
                Directory.CreateDirectory(Constants.RemasteredOrgFolder);

            if (!Directory.Exists(Constants.RemasteredMaxFolder))
                Directory.CreateDirectory(Constants.RemasteredMaxFolder);

            if (!Directory.Exists(Constants.RemasteredCorFolder))
                Directory.CreateDirectory(Constants.RemasteredCorFolder);
        }

        private void DeleteCorruptFiles()
        {
            Globals.Log("Deleting corrupt CDLC files ...");
            // very fast but little oppertunity for feedback
            //DirectoryInfo backupDir = new DirectoryInfo(Constants.Remastered_CorruptCDLCFolder);
            //if (backupDir.GetFiles().Any())
            //{
            //    backupDir.CleanDir();                
            var corFilePaths = Directory.EnumerateFiles(Constants.RemasteredCorFolder, "*" + corExt + "*").ToList();
            var total = corFilePaths.Count;
            int processed = 0, failed = 0, skipped = 0;
            ReportProgress(processed, total, skipped, failed);

            foreach (var corFilePath in corFilePaths)
            {
                processed++;
                try
                {
                    File.SetAttributes(corFilePath, FileAttributes.Normal);
                    File.Delete(corFilePath);
                    Globals.Log("Deleted Corrupt CDLC - " + Path.GetFileName(corFilePath));

                    //GenExtensions.InvokeIfRequired(this, delegate { dgvRepair.Rows.Add(Path.GetFileName(corFilePath), "Deleted Corrupt CDLC"); });
                }
                catch (IOException ex)
                {
                    Globals.Log("Could Not Delete Corrupt CDLC - " + Path.GetFileName(corFilePath));
                    Globals.Log(ex.Message);
                    failed++;

                    //GenExtensions.InvokeIfRequired(this, delegate { dgvRepair.Rows.Add(Path.GetFileName(corFilePath), "Could Not Delete Corrupt CDLC"); });
                }

                ReportProgress(processed, total, skipped, failed);
            }

            if (processed > 0)
                Globals.Log("Corrupt CDLC deletion finished ...");
            else
                Globals.Log("No corrupt CDLC to delete: " + Constants.RemasteredCorFolder);
        }

        private string GetOriginal(string srcFilePath)
        {
            // make sure (.org) file gets put back into the correct 'dlc' subfolder
            // if CDLC is not found then (.org) file is put into default 'dlc' folder
            var dlcFileName = Path.GetFileName(srcFilePath).Replace(orgExt, "");
            var dlcFilePath = Path.Combine(Constants.Rs2DlcFolder, dlcFileName);

            try
            {
                var remasteredFilePath = Globals.SongCollection.FirstOrDefault(s => s.FilePath.Contains(dlcFileName)).FilePath;
                if (remasteredFilePath != null)
                    dlcFilePath = Path.Combine(Path.GetDirectoryName(remasteredFilePath), dlcFileName);

                // copy but don't delete (.org)
                File.SetAttributes(srcFilePath, FileAttributes.Normal);
                File.Copy(srcFilePath, dlcFilePath, true);
                Globals.Log(" - Sucessfully restored backup ...");
                return dlcFilePath;
            }
            catch (Exception ex)
            {
                // this should never happen but just in case
                Globals.Log(" - Restore (" + orgExt + ") failed ...");
                Globals.Log(ex.Message);
                sbErrors.AppendLine(String.Format("{0}, Restore Failed", srcFilePath));
                return String.Empty;
            }
        }

        private DLCPackageData MaxFiveArrangements(DLCPackageData packageData)
        {
            const int playableArrLimit = 5; // one based limit
            var playableArrCount = packageData.Arrangements.Count(arr => arr.ArrangementType == ArrangementType.Guitar || arr.ArrangementType == ArrangementType.Bass);
            if (!IgnoreLimit && playableArrCount <= playableArrLimit)
                return packageData;

            var removalNdx = playableArrCount - playableArrLimit; // zero based index
            var packageDataKept = new DLCPackageData();
            packageDataKept.Arrangements = new List<RocksmithToolkitLib.DLCPackage.Arrangement>();

            foreach (var arr in packageData.Arrangements)
            {
                // skip vocal and showlight arrangements
                if (arr.ArrangementType == ArrangementType.Vocal || arr.ArrangementType == ArrangementType.ShowLight)
                    continue;

                var isKept = true;

                //var isKept = true;
                //var isNDD = false;
                //var isBass = false;
                //var isGuitar = false;
                //var isBonus = false;
                //var isMetronome = false;

                //var songXml = Song2014.LoadFromFile(arr.SongXml.File);
                //var mf = new ManifestFunctions(GameVersion.RS2014);
                //if (mf.GetMaxDifficulty(songXml) == 0) isNDD = true;
                //if (arr.ArrangementType == ArrangementType.Bass) isBass = true;
                //if (arr.ArrangementType == ArrangementType.Guitar) isGuitar = true;
                //if (arr.BonusArr) isBonus = true;
                //if (arr.Metronome == Metronome.Generate) isMetronome = true;

                //switch (checkByte)
                //{
                //    case 0x00: // repair max arrangement
                //        break;
                //    case 0x01: // remove - NDD
                //        if (isNDD) isKept = false;
                //        break;
                //    case 0x02: // remove - Bass
                //        if (isBass) isKept = false;
                //        break;
                //    case 0x03: // remove - NDD, Bass
                //        if (isNDD || isBass) isKept = false;
                //        break;
                //    case 0x04: // remove - Guitar
                //        if (isGuitar) isKept = false;
                //        break;
                //    case 0x05: // remove - NDD, Guitar
                //        if (isNDD || isGuitar) isKept = false;
                //        break;
                //    case 0x06: // remove - Bass, Guitar
                //        if (isBass || isGuitar) isKept = false;
                //        break;
                //    case 0x07: // remove - NDD, Bass, Guitar
                //        if (isNDD || isBass || isGuitar) isKept = false;
                //        break;
                //    //
                //    case 0x08: // remove - Bonus
                //        if (isBonus) isKept = false;
                //        break;
                //    case 0x09: // remove - Bounus, NDD
                //        if (isBonus || isNDD) isKept = false;
                //        break;
                //    case 0x10: // remove - Bonus, Bass
                //        if (isBonus || isBass) isKept = false;
                //        break;
                //    case 0x11: // remove - Bounus, NDD, Bass
                //        if (isBonus || isNDD || isBass) isKept = false;
                //        break;
                //    case 0x12: // remove - Bonus, Guitar
                //        if (isBonus || isGuitar) isKept = false;
                //        break;
                //    case 0x13: // remove - Bonus, NDD, Guitar
                //        if (isBonus || isNDD || isGuitar) isKept = false;
                //        break;
                //    case 0x14: // remove - Bonus, Bass, Guitar
                //        if (isBonus || isBass || isGuitar) isKept = false;
                //        break;
                //    case 0x15: // remove - Bonus, NDD, Bass, Guitar
                //        if (isBonus || isNDD || isBass || isGuitar) isKept = false;
                //        break;
                //    //
                //    case 0x16: // remove - Metronome
                //        if (isMetronome) isKept = false;
                //        break;
                //    case 0x17: // remove - Metronome, NDD
                //        if (isMetronome || isNDD) isKept = false;
                //        break;
                //    case 0x18: // remove - Metronome, Bass
                //        if (isMetronome || isBass) isKept = false;
                //        break;
                //    case 0x19: // remove - Metronome, NDD, Bass
                //        if (isMetronome || isNDD || isBass) isKept = false;
                //        break;
                //    case 0x20: // remove - Metronome, Guitar
                //        if (isMetronome || isGuitar) isKept = false;
                //        break;
                //    case 0x21: // remove - Metronome, NDD, Guitar
                //        if (isMetronome || isNDD || isGuitar) isKept = false;
                //        break;
                //    case 0x22: // remove - Metronome, Bass, Guitar
                //        if (isMetronome || isBass || isBass) isKept = false;
                //        break;
                //    case 0x23: // remove - Metronome, NDD, Bass, Guitar
                //        if (isMetronome || isNDD || isBass || isGuitar) isKept = false;
                //        break;
                //    //
                //    case 0x24: // remove - Metronome, Bonus
                //        if (isMetronome || isBonus) isKept = false;
                //        break;
                //    case 0x25: // remove - Metronome, Bounus, NDD
                //        if (isMetronome || isBonus || isNDD) isKept = false;
                //        break;
                //    case 0x26: // remove - Metronome, Bonus, Bass
                //        if (isMetronome || isBonus || isBass) isKept = false;
                //        break;
                //    case 0x27: // remove - Metronome, Bounus, NDD, Bass
                //        if (isMetronome || isBonus || isNDD || isBass) isKept = false;
                //        break;
                //    case 0x28: // remove - Metronome, Bonus, Guitar
                //        if (isMetronome || isBonus || isGuitar) isKept = false;
                //        break;
                //    case 0x29: // remove - Metronome, Bonus, NDD, Guitar
                //        if (isMetronome || isBonus || isNDD || isGuitar) isKept = false;
                //        break;
                //    case 0x30: // remove - Metronome, Bonus, Bass, Guitar
                //        if (isMetronome || isBonus || isBass || isGuitar) isKept = false;
                //        break;
                //    case 0x31: // remove - Metronome, Bonus, NDD, Bass, Guitar
                //        if (isMetronome || isBonus || isNDD || isBass || isGuitar) isKept = false;
                //        break;
                //}

                string currArr = string.Empty;

                var songXml = Song2014.LoadFromFile(arr.SongXml.File);
                var mf = new ManifestFunctions(GameVersion.RS2014);
                if (arr.ArrangementType == ArrangementType.Bass) currArr = "bass";
                if (mf.GetMaxDifficulty(songXml) == 0) currArr = "ndd";
                if (arr.ArrangementType == ArrangementType.Bass) currArr = "bass";
                if (arr.ArrangementType == ArrangementType.Guitar) currArr = "guitar";
                if (arr.BonusArr) currArr = "bonus";
                if (arr.Metronome == Metronome.Generate) currArr = "metronome";

                if (SelectedArrs.Contains(currArr))
                    isKept = false;

                if (isKept || removalNdx == 0)
                {
                    Globals.Log(" - Kept: " + arr + " ...");
                    packageDataKept.Arrangements.Add(arr);

                    if (packageDataKept.Arrangements.Count == playableArrLimit)
                    {
                        Globals.Log(" - Kept first [" + playableArrLimit + "] arrangements matching the repair criteria ...");
                        break;
                    }
                }
                else
                {
                    Globals.Log(" - Removed: " + arr + " ...");
                    if (!IgnoreLimit)
                        removalNdx--;
                }
            }

            // replace original arrangements with kept arrangements
            packageData.Arrangements = packageDataKept.Arrangements;
            return packageData;
        }

        private string OfficialOrRepaired(string filePath)
        {
            ToolkitInfo entryTkInfo;
            using (var browser = new PsarcLoader(filePath, true))
                entryTkInfo = browser.ExtractToolkitInfo();

            if (entryTkInfo == null)
                return "Official";

            if (entryTkInfo != null && entryTkInfo.PackageAuthor != null)
                if (entryTkInfo.PackageAuthor.Equals("Ubisoft"))
                    return "Official";

            if (entryTkInfo != null && entryTkInfo.PackageComment != null)
                if (entryTkInfo.PackageComment.Contains("Remastered"))
                    return "Remastered";

            return null;
        }
        #endregion
       
        #region Repair methods
        private bool RemasterSong(string srcFilePath, ref StringBuilder sbErrors, byte ArrCheckByte = 0x0, bool PreserveStats = true, bool RepairOrg = false, bool IgnoreMultitoneEx = true, bool AddDD = true, bool RepairMaxFive = true, bool IgnoreLimit = false, SettingsDDC SettingsDD = null, bool ReapplyDD = false)
        {
            if (RepairOrg)
            {
                srcFilePath = GetOriginal(srcFilePath);
                if (String.IsNullOrEmpty(srcFilePath))
                    return false;
            }

            if (!CreateBackup(srcFilePath))
                return false;

            Globals.Log("Remastering: " + Path.GetFileName(srcFilePath));
            try
            {
                // SNG's needs to be regenerated
                // ArrangmentIDs are stored in multiple place and all need to be updated
                // therefore we are going to unpack, apply repair, and repack
                Globals.Log(" - Extracting CDLC artifacts ...");
                // DDC generation variables
                addedDD = false;
                ddError = false;

                SettingsDDC.Instance.LoadConfigXml();
                // phrase length should be at least 8 to fix chord density bug
                // using 12 bar blues beat for default phrase length
                var phraseLen = 12; // SettingsDDC.Instance.PhraseLen;
                // removeSus may be depricated in latest DDC but left here for comptiblity
                var removeSus = SettingsDDC.Instance.RemoveSus;
                var rampPath = SettingsDDC.Instance.RampPath;
                var cfgPath = SettingsDDC.Instance.CfgPath;

                if (SettingsDD != null)
                {
                    phraseLen = SettingsDD.PhraseLen;
                    removeSus = SettingsDD.RemoveSus;
                    rampPath = SettingsDD.RampPath;
                    cfgPath = SettingsDD.CfgPath;
                }

                DLCPackageData packageData;
                using (var psarcOld = new PsarcPackager())
                    packageData = psarcOld.ReadPackage(srcFilePath, IgnoreMultitoneEx);

                // TODO: selectively remove arrangements here before remastering
                if (RepairMaxFive)
                    packageData = MaxFiveArrangements(packageData);
                //packageData = MaxFiveArrangements(packageData, ArrCheckByte, IgnoreLimit);

                var playableArrCount = packageData.Arrangements.Count(arr => arr.ArrangementType == ArrangementType.Guitar || arr.ArrangementType == ArrangementType.Bass);
                if (playableArrCount > 5)
                    throw new CustomException("Maximum playable arrangement limit exceeded");

                // Update arrangement song info
                foreach (var arr in packageData.Arrangements)
                {
                    if (!PreserveStats)
                    {
                        // generate new AggregateGraph
                        arr.SongFile = new RocksmithToolkitLib.DLCPackage.AggregateGraph.SongFile { File = "" };

                        // generate new Arrangement IDs
                        arr.Id = IdGenerator.Guid();
                        arr.MasterId = RandomGenerator.NextInt();
                    }

                    // skip vocal and showlight arrangements
                    if (arr.ArrangementType == ArrangementType.Vocal || arr.ArrangementType == ArrangementType.ShowLight)
                        continue;

                    // validate SongInfo
                    var songXml = Song2014.LoadFromFile(arr.SongXml.File);
                    songXml.AlbumYear = packageData.SongInfo.SongYear.ToString().GetValidYear();
                    songXml.ArtistName = packageData.SongInfo.Artist.GetValidAtaSpaceName();
                    songXml.Title = packageData.SongInfo.SongDisplayName.GetValidAtaSpaceName();
                    songXml.AlbumName = packageData.SongInfo.Album.GetValidAtaSpaceName();
                    songXml.ArtistNameSort = packageData.SongInfo.ArtistSort.GetValidSortableName();
                    songXml.SongNameSort = packageData.SongInfo.SongDisplayNameSort.GetValidSortableName();
                    songXml.AlbumNameSort = packageData.SongInfo.AlbumSort.GetValidSortableName();
                    songXml.AverageTempo = Convert.ToSingle(packageData.SongInfo.AverageTempo.ToString().GetValidTempo());

                    // write updated xml arrangement
                    using (var stream = File.Open(arr.SongXml.File, FileMode.Create))
                        songXml.Serialize(stream, true);

                    // add comments back to xml arrangement   
                    Song2014.WriteXmlComments(arr.SongXml.File, arr.XmlComments);

                    // only add DD to NDD arrangements              
                    var mf = new ManifestFunctions(GameVersion.RS2014);
                    var maxDD = mf.GetMaxDifficulty(songXml);

                    if (AddDD && maxDD == 0)
                    {
                        var consoleOutput = String.Empty;
                        var result = DynamicDifficulty.ApplyDD(arr.SongXml.File, phraseLen, removeSus, rampPath, cfgPath, out consoleOutput, true);
                        if (result == -1)
                            throw new CustomException("ddc.exe is missing");

                        if (String.IsNullOrEmpty(consoleOutput))
                        {
                            Globals.Log(" - Added DD to " + arr + " ...");
                            addedDD = true;
                        }
                        else
                        {
                            Globals.Log(" - " + arr + " DDC console output: " + consoleOutput + " ...");
                            sbErrors.AppendLine(String.Format("{0}, Could not apply DD to: {1}", srcFilePath, arr));
                            ddError = true;
                        }
                    }

                    // put arrangment comments in correct order
                    Song2014.WriteXmlComments(arr.SongXml.File);
                }

                // add comment to ToolkitInfo to identify CDLC
                if (!PreserveStats)
                    packageData.AddMsgToPackageComment(Constants.TKI_ARRID);

                if (RepairMaxFive)
                    packageData.AddMsgToPackageComment(Constants.TKI_MAX5);

                // add TKI_DDC comment
                if (AddDD && addedDD)
                    packageData.AddMsgToPackageComment(Constants.TKI_DDC);

                // add comment to ToolkitInfo to identify CDLC
                packageData.AddMsgToPackageComment(Constants.TKI_REMASTER);

                // add default package version if missing
                packageData.AddDefaultPackageVersion();

                // validate packageData (important)
                packageData.Name = packageData.Name.GetValidKey(); // DLC Key                 
                Globals.Log(" - Repackaging Remastered CDLC ...");

                // regenerates the SNG with the repair and repackages               
                using (var psarcNew = new PsarcPackager(true))
                    psarcNew.WritePackage(srcFilePath, packageData);

                if (File.Exists(srcFilePath))
                {
                    TemporaryDisableDatabindEvent(() =>
                    {
                        using (var browser = new PsarcBrowser(srcFilePath))
                        {
                            var songInfo = browser.GetSongData();

                            if (songInfo != null)
                            {
                                var song = Globals.SongCollection.FirstOrDefault(s => s.FilePath == srcFilePath);
                                int index = Globals.SongCollection.IndexOf(song);

                                //TODO: fix this when RepairOrg is used

                                if (index != -1)
                                    Globals.SongCollection[index] = songInfo.First();
                                else
                                    Globals.SongCollection.Add(songInfo.First());
                            }
                        }
                    });

                    GenExtensions.InvokeIfRequired(dgvRepairManager, delegate { dgvRepairManager.Refresh(); });
                }

                if (!ddError)
                    Globals.Log(" - Repair was sucessful ...");
                else
                    Globals.Log(" - Repair was sucessful, but DD could not be applied ...");
            }
            catch (CustomException ex)
            {
                Globals.Log(" - Repair failed ... " + ex.Message);
                Globals.Log(" - See '" + Path.GetFileName(Constants.RemasteredErrorLogPath) + "' file ... ");

                if (ex.Message.Contains("Maximum"))
                {
                    //  copy (org) to maximum (max), delete backup (org), delete original
                    var properExt = Path.GetExtension(srcFilePath);
                    var orgFilePath = String.Format(@"{0}{1}{2}", Path.Combine(Constants.RemasteredOrgFolder, Path.GetFileNameWithoutExtension(srcFilePath)), orgExt, properExt).Trim();
                    var maxFilePath = String.Format(@"{0}{1}{2}", Path.Combine(Constants.RemasteredMaxFolder, Path.GetFileNameWithoutExtension(srcFilePath)), maxExt, properExt).Trim();
                    File.SetAttributes(orgFilePath, FileAttributes.Normal);
                    File.SetAttributes(srcFilePath, FileAttributes.Normal);
                    File.Copy(orgFilePath, maxFilePath, true);
                    File.Delete(orgFilePath);
                    File.Delete(srcFilePath);
                    sbErrors.AppendLine(String.Format("{0}, Maximum playable arrangement limit exceeded", maxFilePath));
                }

                return false;
            }
            catch (Exception ex)
            {
                Globals.Log(" - Repair failed ... " + ex.Message);
                Globals.Log(" - See '" + Path.GetFileName(Constants.RemasteredErrorLogPath) + "' file ... ");

                //  copy (org) to corrupt (cor), delete backup (org), delete original
                var properExt = Path.GetExtension(srcFilePath);
                var orgFilePath = String.Format(@"{0}{1}{2}", Path.Combine(Constants.RemasteredOrgFolder, Path.GetFileNameWithoutExtension(srcFilePath)), orgExt, properExt).Trim();
                var corFilePath = String.Format(@"{0}{1}{2}", Path.Combine(Constants.RemasteredCorFolder, Path.GetFileNameWithoutExtension(srcFilePath)), corExt, properExt).Trim();
                File.SetAttributes(orgFilePath, FileAttributes.Normal);
                File.SetAttributes(srcFilePath, FileAttributes.Normal);
                File.Copy(orgFilePath, corFilePath, true);
                File.Delete(orgFilePath);
                File.Delete(srcFilePath);
                sbErrors.AppendLine(String.Format("{0}, Corrupt CDLC", corFilePath));

                return false;
            }

            return true;
        }
        private bool PitchShiftSong(SongData song, bool createNewFile = true)
        {
            string pitchShiftedMessage = "Pitch Shifted by CFSM";
            string srcFilePath = song.FilePath;

            //if (song.CharterName.ToLower() == "firekorn")
            //{
            //    Globals.Log("NOP NOP NOP NOP NOP");
            //    return false;
            //}

            if (song.OfficialDLC || (song.Tuning == "E Standard" || song.Tuning == "Drop D"))
            {
                rSkipped++;
                return false;
            }

            Globals.Log(" - Adding a pitch shifting effect to: " + Path.GetFileName(srcFilePath));
            try
            {
                string ext = string.Empty, finalPath = srcFilePath;
                Globals.Log(" - Extracting CDLC artifacts ...");
                DLCPackageData packageData;

                int gitShift = 0, bassShift = 0;
                int mix = 100;
                int tone = 50;

                //Try unpacking and if it throws InvalidDataException - fix arrangement XMLs
                packageData = PackageDataTools.GetDataWithFixedTones(srcFilePath);

                using (var psarcOld = new PsarcPackager())
                    packageData = psarcOld.ReadPackage(srcFilePath);

                if (!createNewFile && packageData.ToolkitInfo.PackageComment.Contains(pitchShiftedMessage))
                {
                    Globals.Log(" - This song has already been patched! ");
                    rSkipped++;
                    return false;
                }

                //Get info (amount of steps) and set correct tunings
                packageData = PitchShiftTools.GetSetArrInfo(packageData, ref gitShift, ref bassShift, ref ext);

                packageData = PitchShiftTools.AddPitchShiftPedalToTones(packageData, gitShift, bassShift, mix, tone);

                //Add a message if no new file will be created
                if (!createNewFile)
                    packageData = PitchShiftTools.AddPitchShiftedMsg(packageData);

                //Add extension to the names and validate
                packageData = PitchShiftTools.AddExtensionToSongName(packageData, ext);
                packageData = PackageDataTools.ValidatePackageDataName(packageData);

                //Set correct names and regenerate xml
                packageData = PitchShiftTools.RegenerateXML(packageData);

                Globals.Log(" - Repackaging pitch shifted CDLC ...");

                if (createNewFile)
                    finalPath = srcFilePath.Replace("_p.psarc", ext + "_p.psarc");

                using (var psarcNew = new PsarcPackager(true))
                    psarcNew.WritePackage(finalPath, packageData, srcFilePath);

                if (File.Exists(finalPath))
                {
                    TemporaryDisableDatabindEvent(() =>
                    {
                        using (var browser = new PsarcBrowser(finalPath))
                        {
                            var songInfo = browser.GetSongData();

                            if (songInfo != null && Globals.SongCollection.Where(sng => sng.FilePath == finalPath).Count() == 0)
                                Globals.SongCollection.Add(songInfo.First());
                        }
                    });

                    GenExtensions.InvokeIfRequired(dgvRepairManager, delegate { dgvRepairManager.Refresh(); });
                }

                Globals.Log(" - Adding a pitch shifting effect to the CDLC sucessful ...");
            }
            catch (Exception ex)
            {
                Globals.Log(" - Adding a pitch shifting effect failed ... " + ex.Message);
                rFailed++;

                return false;
            }

            rProcessed++;
            ReportProgress(rProcessed, rTotal, rSkipped, rFailed);

            return true;
        }

        public void PitchShift_Single()
        {
            var song = DgvExtensions.GetObjectFromRow<SongData>(dgvRepairManager.SelectedRows[0]);

            rTotal = 1;
            rProcessed = 0;
            rSkipped = 0;
            rFailed = 0;

            PitchShiftSong(song);
        }

        public void PitchShift_Selection()
        {
            var selection = DgvExtensions.GetObjectsFromRows<SongData>(dgvRepairManager);

            if (selection.Count > 0)
            {
                rTotal = selection.Count;
                rProcessed = 0;
                rSkipped = 0;
                rFailed = 0;
                ReportProgress(rProcessed, rTotal, rSkipped, rFailed);

                foreach (var song in selection)
                    PitchShiftSong(song);
            }
        }

        private void ApplyDDToPackage(SongData song)
        {
            AddDD = true;
            RepairSong(song);
        }

        public void ApplyDD_Single()
        {
            var song = DgvExtensions.GetObjectFromRow<SongData>(dgvRepairManager.SelectedRows[0]);

            ApplyDDToPackage(song);
        }

        public void ApplyDD_Selection()
        {
            var selection = DgvExtensions.GetObjectsFromRows<SongData>(dgvRepairManager);

            if (selection.Count > 0)
            {
                foreach (var song in selection)
                    ApplyDDToPackage(song);
            }
        }

        public void RepairSong_Single()
        {
            AddDD = false;
            var song = DgvExtensions.GetObjectFromRow<SongData>(dgvRepairManager.SelectedRows[0]);

            RepairSong(song);
        }

        public void RepairSong_Selection()
        {
            AddDD = false;
            var selection = DgvExtensions.GetObjectsFromRows<SongData>(dgvRepairManager);

            if (selection.Count > 0)
                RepairSongs(selection);
        }

        public void MoveSongsFromDownloads()
        {
            string dlDirPath = AppSettings.Instance.DownloadsDir;

            if (!Directory.Exists(dlDirPath))
                return;

            var dlcFiles = Directory.EnumerateFiles(dlDirPath, "*_p.psarc", SearchOption.AllDirectories).Where(fi => !fi.ToLower().Contains(Constants.RS1COMP) && !fi.ToLower().Contains(Constants.SONGPACK) && !fi.ToLower().Contains(Constants.ABVSONGPACK)).ToArray();

            TemporaryDisableDatabindEvent(() =>
            {
                foreach (string songPath in dlcFiles)
                {
                    string officialOrRepaired = RepairTools.OfficialOrRepaired(songPath);

                    using (var browser = new PsarcBrowser(songPath))
                    {
                        var songInfo = browser.GetSongData();

                        if (songInfo != null)  //TODO: check if is duplicate
                            continue;

                        List<SongData> song = new List<SongData>() { songInfo.First() };

                        rTotal = 1;
                        rProcessed = 0;
                        rSkipped = 0;
                        rFailed = 0;

                        if (!String.IsNullOrEmpty(officialOrRepaired) && officialOrRepaired == "Remastered")
                            RepairSongs(song);

                        File.Copy(songInfo.First().FilePath, Path.Combine(AppSettings.Instance.RSInstalledDir, "dlc", Path.GetFileName(songInfo.First().FilePath)), true);

                        Globals.SongCollection.Add(songInfo.First());
                    }
                }
            });

            GenExtensions.InvokeIfRequired(dgvRepairManager, delegate { dgvRepairManager.Refresh(); });
        }
        #endregion

        #region events
        private void btnRepairSongs_Click(object sender, EventArgs e)
        {
            var curBackColor = BackColor;
            var curForeColor = ForeColor;
            this.BackColor = Color.Black;
            this.ForeColor = Color.Red;
            var diaMsg = "Are you sure you want to repair all CDLC files that are located in" + Environment.NewLine +
                         "the 'dlc' folder and subfolders using the selected repair options?" + Environment.NewLine + Environment.NewLine +
                         "Do you have a complete backup of your CDLC collection?";

            if (DialogResult.Yes != BetterDialog.ShowDialog(diaMsg, "Repair All CDLC ...", null, "Yes", "No", Bitmap.FromHicon(SystemIcons.Question.Handle), "", 0, 150))
            {
                this.BackColor = curBackColor;
                this.ForeColor = curForeColor;
                return;
            }

            this.BackColor = curBackColor;
            this.ForeColor = curForeColor;
            ToggleUIControls(false);

            // run new generic worker
            using (var gWorker = new GenericWorker())
            {
                if (RepairMastery && RepairMaxFive)
                    gWorker.WorkDescription = "repairing mastery";
                else if (!RepairMastery && RepairMaxFive)
                    gWorker.WorkDescription = "repairing maximum playable arrangements";
                else if (RepairMastery && RepairMaxFive)
                    gWorker.WorkDescription = "repairing mastery and maximum playable arrangements";
                else
                    gWorker.WorkDescription = "unknown repair";

                gWorker.BackgroundProcess(this);
                while (Globals.WorkerFinished == Globals.Tristate.False)
                    Application.DoEvents();
            }

            ToggleUIControls(true);
        }
        private void repairOnlyToolStripMenuItem_Click(object sender, EventArgs e) //Fix BW!
        {
            if (MessageBox.Show(@"Are you sure you want to repair the selected songs?",
            Constants.ApplicationName + " ... Warning", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            GetRepairOptions();
            AddDD = false;

            using (var gWorker = new GenericWorker())
            {
                gWorker.WorkDescription = "repairing the selection";
                gWorker.BackgroundProcess(this);
                while (Globals.WorkerFinished == Globals.Tristate.False)
                    Application.DoEvents();
            }
        }

        private void betaPitchShiftToolStripMenuItem_Click(object sender, EventArgs e) //Fix BW!
        {
            if (MessageBox.Show(@"Are you sure you want to add a pitch shift effect to the selected songs (shifting to E standard for songs in standard tunings or Drop D for songs in dropped tunings)?"
                  + Environment.NewLine + Environment.NewLine + "NOTE: neither the actual audio nor the tab are changed, so make sure to use headphones while playing!",
                  Constants.ApplicationName + " ... Warning", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            GetRepairOptions();
            using (var gWorker = new GenericWorker())
            {
                gWorker.WorkDescription = "applying pitch shift to the selection";
                gWorker.BackgroundProcess(this);
                while (Globals.WorkerFinished == Globals.Tristate.False)
                    Application.DoEvents();
            }
        }

        private void cmsPitchShift_Click(object sender, EventArgs e) //Fix BW!
        {
            if (MessageBox.Show(@"Are you sure you want to add a pitch shift effect to the selected song (shifting to E standard for songs in standard tunings or Drop D for songs in dropped tunings)?"
                   + Environment.NewLine + Environment.NewLine + "NOTE: neither the actual audio nor the tab are changed, so make sure to use headphones while playing!",
                   Constants.ApplicationName + " ... Warning", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            using (var gWorker = new GenericWorker())
            {
                gWorker.WorkDescription = "applying pitch shift to a single song";
                gWorker.BackgroundProcess(this);
                while (Globals.WorkerFinished == Globals.Tristate.False)
                    Application.DoEvents();
            }
        }


        private void repairAndAddDDToolStripMenuItem_Click(object sender, EventArgs e) //Fix BW!
        {
            if (MessageBox.Show(@"Are you sure you want to repair and add DD (Dynamic difficulty) to the selected songs?",
             Constants.ApplicationName + " ... Warning", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            GetRepairOptions(true);
            AddDD = true;

            using (var gWorker = new GenericWorker())
            {
                gWorker.WorkDescription = "applying DD to the selection";
                gWorker.BackgroundProcess(this);
                while (Globals.WorkerFinished == Globals.Tristate.False)
                    Application.DoEvents();
            }
        }

        private void moveCDLCFromDLToDLCFolderToolStripMenuItem_Click(object sender, EventArgs e) //Fix BW!
        {
            string dlDirPath = AppSettings.Instance.DownloadsDir;

            if (dlDirPath == null || dlDirPath == string.Empty)
            {
                MessageBox.Show("Path of your downloads folder not found, please select it in the following menu",
                   Constants.ApplicationName + " ... Warning", MessageBoxButtons.OK);

                using (var fbd = new FolderBrowserDialog())
                {
                    fbd.Description = "Select the RS2014 installation directory";

                    if (fbd.ShowDialog() != DialogResult.OK)
                        return;

                    AppSettings.Instance.DownloadsDir = fbd.SelectedPath;
                }
            }

            GetRepairOptions(true);

            using (var gWorker = new GenericWorker())
            {
                gWorker.WorkDescription = "moving songs from Downloads folder to your dlc folder";
                gWorker.BackgroundProcess(this);
                while (Globals.WorkerFinished == Globals.Tristate.False)
                    Application.DoEvents();
            }
        }

        private void cmsRepairOnly_Click(object sender, EventArgs e) //Fix BW!
        {
            if (MessageBox.Show(@"Are you sure you want to repair the selected song?",
                Constants.ApplicationName + " ... Warning", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            GetRepairOptions();
            using (var gWorker = new GenericWorker())
            {
                gWorker.WorkDescription = "repairing a single song";
                gWorker.BackgroundProcess(this);
                while (Globals.WorkerFinished == Globals.Tristate.False)
                    Application.DoEvents();
            }
        }

        private void cmsRepairAndAddDD_Click(object sender, EventArgs e) //Fix BW!
        {
            if (MessageBox.Show(@"Are you sure you want to repair and DD (Dynamic difficulty) to the selected song?",
                Constants.ApplicationName + " ... Warning", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            GetRepairOptions(true);
            using (var gWorker = new GenericWorker())
            {
                gWorker.WorkDescription = "applying DD to a single song";
                gWorker.BackgroundProcess(this);
                while (Globals.WorkerFinished == Globals.Tristate.False)
                    Application.DoEvents();
            }
        }

        private void restoreOrgBackupsToolStripMenuItem_Click(object sender, EventArgs e) //Fix BW!
        {
            if (MessageBox.Show("Are you sure you want to restore (" + orgExt + ") CDLC to the 'dlc' folder?", Constants.ApplicationName + " ... Warning", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            ToggleUIControls(false);

            // run new generic worker
            using (var gWorker = new GenericWorker())
            {
                gWorker.WorkDescription = "restoring (.org) backups";
                gWorker.BackgroundProcess(this);
                while (Globals.WorkerFinished == Globals.Tristate.False)
                    Application.DoEvents();
            }

            ToggleUIControls(true);
        }

        private void restoreCorBackupsToolStripMenuItem_Click(object sender, EventArgs e) //Fix BW!
        {
            if (MessageBox.Show("Are you sure you want to restore (" + corExt + ") CDLC to the 'dlc' folder?", Constants.ApplicationName + " ... Warning", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            ToggleUIControls(false);

            // run new generic worker
            using (var gWorker = new GenericWorker())
            {
                gWorker.WorkDescription = "restoring (.cor) backups";
                gWorker.BackgroundProcess(this);
                while (Globals.WorkerFinished == Globals.Tristate.False)
                    Application.DoEvents();
            }

            ToggleUIControls(true);

        }

        private void restoreMaxBackupsToolStripMenuItem_Click(object sender, EventArgs e) //Fix BW!
        {
            if (MessageBox.Show("Are you sure you want to restore (" + maxExt + ") CDLC to the 'dlc' folder?", Constants.ApplicationName + " ... Warning", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            ToggleUIControls(false);

            // run new generic worker
            using (var gWorker = new GenericWorker())
            {
                gWorker.WorkDescription = "restoring (.max) backups";
                gWorker.BackgroundProcess(this);
                while (Globals.WorkerFinished == Globals.Tristate.False)
                    Application.DoEvents();
            }

            ToggleUIControls(true);
        }

        private void archiveCorruptCDLCToolStripMenuItem_Click(object sender, EventArgs e) //Fix BW!
        {
            ToggleUIControls(false);

            // run new generic worker
            using (var gWorker = new GenericWorker())
            {
                gWorker.WorkDescription = "archiving corrupt songs";
                gWorker.BackgroundProcess(this);
                while (Globals.WorkerFinished == Globals.Tristate.False)
                    Application.DoEvents();
            }

            ToggleUIControls(true);
        }

        //TODO: reconnect tsmi
        private void deleteCorruptCDLCToolStripMenuItem_Click(object sender, EventArgs e) //Fix BW!
        {
            if (MessageBox.Show("Are you sure you want to delete all corrupt CDLC files?", Constants.ApplicationName + " ... Warning", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            ToggleUIControls(false);
            DeleteCorruptFiles();
            ToggleUIControls(true);
        }

        private void cleanupDlcFolderToolStripMenuItem_Click(object sender, EventArgs e) //Fix BW!
        {
            ToggleUIControls(false);
            CleanDlcFolder();
            ToggleUIControls(true);
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Stream stream = assembly.GetManifestResourceStream("CustomsForgeSongManager.Resources.HelpRepairs.txt");
            using (StreamReader reader = new StreamReader(stream))
            {
                var helpGeneral = reader.ReadToEnd();

                using (var noteViewer = new frmNoteViewer())
                {
                    noteViewer.Text = String.Format("{0} . . . {1}", noteViewer.Text, "Repairs Help");
                    noteViewer.PopulateText(helpGeneral);
                    noteViewer.ShowDialog();
                }
            }
        }
        #endregion

        private void btnRestoreOptions_Click(object sender, EventArgs e)
        {
            cmsRestoreOptions.Show(btnRestoreOptions, 0, btnRestoreOptions.Height + 2);
        }

        public void TabEnter()
        {
            Globals.Log("RepairManager GUI TabEnter ...");
            Globals.DgvCurrent = dgvRepairManager;
        }

        public void TabLeave()
        {
            Globals.Log("RepairManager GUI TabLeave ...");
            Globals.Settings.SaveSettingsToFile(dgvRepairManager);
        }

        public DataGridView GetGrid()
        {
            return dgvRepairManager;
        }

        private void btnViewErrorLog_Click(object sender, EventArgs e)
        {
            string stringLog;

            if (!File.Exists(Constants.RemasteredErrorLogPath))
                stringLog = Path.GetFileName(Constants.RemasteredErrorLogPath) + " is empty ...";
            else
            {
                stringLog = Constants.RemasteredErrorLogPath + Environment.NewLine;
                stringLog = stringLog + File.ReadAllText(Constants.RemasteredErrorLogPath);
                stringLog = stringLog + Environment.NewLine + AppSettings.Instance.LogFilePath + Environment.NewLine;
                stringLog = stringLog + File.ReadAllText(AppSettings.Instance.LogFilePath);
            }


            using (var noteViewer = new frmNoteViewer())
            {
                noteViewer.Width = 700;
                noteViewer.PopulateText(stringLog);
                noteViewer.ShowDialog();
            }
        }

        private void btnTweakOptions_Click(object sender, EventArgs e)
        {

        }
    }
}