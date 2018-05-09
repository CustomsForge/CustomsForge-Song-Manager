using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using CustomControls;
using CustomsForgeSongManager.DataObjects;
using CustomsForgeSongManager.Forms;
using CustomsForgeSongManager.LocalTools;
using CustomsForgeSongManager.SongEditor;
using CustomsForgeSongManager.UITheme;
using GenTools;
using DataGridViewTools;
using Newtonsoft.Json;
using System.Xml;
using CustomsForgeSongManager.Properties;
using System.Net.Cache;



namespace CustomsForgeSongManager.UControls
{
    public partial class ArrangementManager : UserControl, IDataGridViewHolder, INotifyTabChanged
    {
        private bool allSelected = false;
        private List<ArrangementData> arrangementList = new List<ArrangementData>();
        private AbortableBackgroundWorker bWorker;
        private bool bindingCompleted = false;
        private Stopwatch counterStopwatch = new Stopwatch();
        private bool dgvPainted = false;
        private int firstIndex = 0;
        private string lastSelectedSongPath = String.Empty;

        public ArrangementManager()
        {
            InitializeComponent();
            Globals.TsLabel_StatusMsg.Click += lnkShowAll_Click;
            PopulateArrangementManager();
        }

        public void PopulateArrangementManager()
        {
            Globals.Log("Populating Arrangements GUI ...");
            // Hide main dgvArrangements until load completes
            dgvArrangements.Visible = false;
            LoadArrangements();
            PopulateDataGridView();

            // Worker actually does the sorting after parsing, this is just to tell the grid that it is sorted.
            if (!String.IsNullOrEmpty(AppSettings.Instance.SortColumn))
            {
                var colX = dgvArrangements.Columns.Cast<DataGridViewColumn>().Where(col => col.DataPropertyName == AppSettings.Instance.SortColumn).FirstOrDefault();
                if (colX != null)
                    dgvArrangements.Sort(colX, AppSettings.Instance.SortAscending ? ListSortDirection.Ascending : ListSortDirection.Descending);
            }

            // TODO: maybe reapply previous filtering and search here

            UpdateToolStrip();
        }

        public void UpdateToolStrip()
        {
            Globals.TsLabel_MainMsg.Text = string.Format("Rocksmith Arrangements Count: {0}", arrangementList.Count);
            Globals.TsLabel_MainMsg.Visible = true;
            Globals.TsLabel_DisabledCounter.Visible = false;
            Globals.TsLabel_StatusMsg.Visible = false;
        }

        private void Arrangements_Resize(object sender, EventArgs e)
        {
            if (dgvArrangements.DataSource == null || dgvArrangements.RowCount == 0)
                return;

            firstIndex = dgvArrangements.FirstDisplayedCell.RowIndex;
        }

        private void ColumnMenuItemClick(object sender, EventArgs eventArgs)
        {
            ToolStripMenuItem currentContextMenuItem = sender as ToolStripMenuItem;
            if (currentContextMenuItem != null)
            {
                if (!string.IsNullOrEmpty(currentContextMenuItem.Tag.ToString()))
                {
                    var dataGridViewColumn = dgvArrangements.Columns[currentContextMenuItem.Tag.ToString()];
                    if (dataGridViewColumn != null)
                    {
                        var columnIndex = dataGridViewColumn.Index;
                        var columnSetting = RAExtensions.ManagerGridSettings.ColumnOrder.SingleOrDefault(x => x.ColumnIndex == columnIndex);
                        if (columnSetting != null)
                        {
                            columnSetting.Visible = !columnSetting.Visible;
                            dgvArrangements.Columns[columnIndex].Visible = columnSetting.Visible;
                            currentContextMenuItem.Checked = columnSetting.Visible;
                        }
                    }
                }
            }
        }

        private void LoadArrangements()
        {
            if (Globals.MasterCollection.Count == 0)
                return;

            // flatten SongData to ArrangementData
            arrangementList = new List<ArrangementData>();

            Stopwatch sw = null;
            sw = new Stopwatch();
            sw.Restart();

            // CONFIRMED ... recursion method is 4-8X faster than reflection method
            foreach (var song in Globals.MasterCollection)
            {
                foreach (var songArr in song.Arrangements2D)
                {
                    var arr = new ArrangementData
                        {
                            // Song Attributes
                            DLCKey = song.DLCKey,
                            Artist = song.Artist,
                            ArtistSort = song.ArtistSort,
                            Title = song.Title,
                            TitleSort = song.TitleSort,
                            Album = song.Album,
                            AlbumSort = song.AlbumSort,
                            SongYear = song.SongYear,
                            SongLength = song.SongLength,
                            SongAverageTempo = song.SongAverageTempo,
                            SongVolume = song.SongVolume,
                            LastConversionDateTime = song.LastConversionDateTime,
                            AppID = song.AppID,
                            ToolkitVersion = song.ToolkitVersion,
                            PackageAuthor = song.PackageAuthor,
                            PackageVersion = song.PackageVersion,
                            PackageComment = song.PackageComment,
                            IgnitionID = song.IgnitionID,
                            IgnitionVersion = song.IgnitionVersion,
                            IgnitionAuthor = song.IgnitionAuthor,
                            IgnitionDate = song.IgnitionDate,
                            FilePath = song.FilePath,
                            FileDate = song.FileDate,
                            FileSize = song.FileSize,
                            
                            // Arrangement Attributes
                            PersistentID = songArr.PersistentID,
                            Name = songArr.Name,
                            Tuning = songArr.Tuning,
                            ToneBase = songArr.ToneBase,
                            Tones = songArr.Tones,
                            DDMax = songArr.DDMax,
                            SectionCount = songArr.SectionCount,
                            TuningPitch = songArr.TuningPitch,
                            CapoFret = songArr.CapoFret,
                            
                            // Arrangement Levels
                            ChordCount = songArr.ChordCount,
                            NoteCount = songArr.NoteCount,
                            AccentCount = songArr.AccentCount,
                            BendCount = songArr.BendCount,
                            FretHandMuteCount = songArr.FretHandMuteCount,
                            HammerOnCount = songArr.HammerOnCount,
                            HarmonicCount = songArr.HarmonicCount,
                            HarmonicPinchCount = songArr.HarmonicPinchCount,
                            HighestFretUsed = songArr.HighestFretUsed,
                            HopoCount = songArr.HopoCount,
                            IgnoreCount = songArr.IgnoreCount,
                            LinkNextCount = songArr.LinkNextCount,
                            OctaveCount = songArr.OctaveCount,
                            PalmMuteCount = songArr.PalmMuteCount,
                            PluckCount = songArr.PluckCount,
                            PullOffCount = songArr.PullOffCount,
                            SlapCount = songArr.SlapCount,
                            SlideCount = songArr.SlideCount,
                            SlideUnpitchToCount = songArr.SlideUnpitchToCount,
                            SustainCount = songArr.SustainCount,
                            TapCount = songArr.TapCount,
                            TremoloCount = songArr.TremoloCount,
                            VibratoCount = songArr.VibratoCount,

                            // calculated content taken from SongData
                            ChordNamesCounts = songArr.ChordNamesCounts,
                            Selected = song.Selected,
                            OfficialDLC = song.OfficialDLC,
                            IsRsCompPack = song.IsRsCompPack,
                            ArtistTitleAlbum = song.ArtistTitleAlbum,
                            ArtistTitleAlbumDate = song.ArtistTitleAlbumDate,
                            FileName = song.FileName,
                            Tagged = song.Tagged,
                            RepairStatus = song.RepairStatus,
                            
                            // Arrangement Property
                            BassPick = songArr.BassPick
                        };

                    if (!String.IsNullOrEmpty(songArr.Tones))
                        arr.ToneChanges = songArr.Tones.Count(c => c == ',') == 0 ? 1 : songArr.Tones.Count(c => c == ',');

                    arrangementList.Add(arr);
                }
            }

            sw.Stop();
            Globals.Log(String.Format("Arrangement recursion took: {0} (msec)", sw.ElapsedMilliseconds));

            //sw = null;
            //sw = new Stopwatch();
            //sw.Restart();

            //// CONFIRMED ... reflections method is 4-8X slower than recursion method
            //foreach (var song in Globals.MasterCollection)
            //{
            //    foreach (var arrangement in song.Arrangements2D)
            //    {
            //        var arrData = new ArrangementData();
            //        foreach (var songItem in song.GetType().GetProperties())
            //            if (arrData.GetType().GetProperty(songItem.Name) != null)
            //                if (arrData.GetType().GetProperty(songItem.Name).CanWrite)
            //                    if (songItem.PropertyType.Namespace != "System.Collections.Generic")
            //                        arrData.GetType().GetProperty(songItem.Name).SetValue(arrData, songItem.GetValue(song, null), null);

            //        foreach (var arrItem in arrangement.GetType().GetProperties())
            //            if (arrData.GetType().GetProperty(arrItem.Name) != null)
            //                if (arrData.GetType().GetProperty(arrItem.Name).CanWrite)
            //                    if (arrItem.PropertyType.Namespace != "System.Collections.Generic")
            //                        arrData.GetType().GetProperty(arrItem.Name).SetValue(arrData, arrItem.GetValue(arrangement, null), null);

            //        arrangementList.Add(arrData);
            //    }
            //}

            //sw.Stop();
            //Globals.Log(String.Format("Arrangement reflection took: {0} (msec)", sw.ElapsedMilliseconds));

            var debugMe = arrangementList;
        }

        private void LoadFilteredBindingList(dynamic list)
        {
            bindingCompleted = false;
            dgvPainted = false;
            // sortable binding list with drop down filtering
            dgvArrangements.AutoGenerateColumns = false;
            FilteredBindingList<ArrangementData> fbl = new FilteredBindingList<ArrangementData>(list);
            BindingSource bs = new BindingSource { DataSource = fbl };
            dgvArrangements.DataSource = bs;
        }

        private void PopulateDataGridView()
        {
            // respect processing order
            DgvExtensions.DoubleBuffered(dgvArrangements);
            LoadFilteredBindingList(arrangementList);
            CFSMTheme.InitializeDgvAppearance(dgvArrangements);
            // reload column order, width, visibility
            Globals.Settings.LoadSettingsFromFile(dgvArrangements, true);

            if (RAExtensions.ManagerGridSettings != null)
                dgvArrangements.ReLoadColumnOrder(RAExtensions.ManagerGridSettings.ColumnOrder);
            else
                Globals.Settings.SaveSettingsToFile(dgvArrangements);

            // lock OfficialDLC from being selected
            foreach (DataGridViewRow row in dgvArrangements.Rows)
            {
                var sd = DgvExtensions.GetObjectFromRow<ArrangementData>(row);
                if (sd.OfficialDLC)
                {
                    row.Cells["colSelect"].Value = false;
                    row.Cells["colSelect"].ReadOnly = sd.OfficialDLC;
                    sd.Selected = false;
                }
            }
        }

        private void PopulateMenuWithColumnHeaders(ContextMenuStrip contextMenuStrip)
        {
            // fix for contextual menu bug 'Object reference not set to an instance of an object.' 
            // that occur on startup when dgv settings have not yet been saved       
            if (RAExtensions.ManagerGridSettings == null)
            {
                Globals.Settings.SaveSettingsToFile(dgvArrangements);
                Globals.Settings.LoadSettingsFromFile(dgvArrangements);
                dgvArrangements.ReLoadColumnOrder(RAExtensions.ManagerGridSettings.ColumnOrder);
            }

            contextMenuStrip.Items.Clear();
            foreach (ColumnOrderItem columnOrderItem in RAExtensions.ManagerGridSettings.ColumnOrder)
            {
                var cn = dgvArrangements.Columns[columnOrderItem.ColumnIndex].Name;
                if (cn.ToLower().StartsWith("col"))
                    cn = cn.Remove(0, 3);

                ToolStripMenuItem columnsMenuItem = new ToolStripMenuItem(cn, null, ColumnMenuItemClick) { Checked = dgvArrangements.Columns[columnOrderItem.ColumnIndex].Visible, Tag = dgvArrangements.Columns[columnOrderItem.ColumnIndex].Name };
                contextMenuStrip.Items.Add(columnsMenuItem);
            }
        }

        private void RemoveFilter()
        {
            // save current sorting before removing filter
            DgvExtensions.SaveSorting(dgvArrangements);

            // remove the filter
            DataGridViewAutoFilterTextBoxColumn.RemoveFilter(dgvArrangements);
            UpdateToolStrip();

            // reapply sort direction to reselect the filtered song
            DgvExtensions.RestoreSorting(dgvArrangements);
            this.Refresh();
        }

        private void SearchCDLC(string criteria)
        {
            var lowerCriteria = criteria.ToLower();
            AppSettings.Instance.SearchString = lowerCriteria;

            var results = arrangementList
                .Where(x => x.ArtistTitleAlbum.ToLower().Contains(lowerCriteria) ||
                    x.FilePath.ToLower().Contains(lowerCriteria)).ToList();

            LoadFilteredBindingList(results);
        }

        private void SelectAllNone()
        {
            TemporaryDisableDatabindEvent(() =>
                {
                    foreach (DataGridViewRow row in dgvArrangements.Rows)
                        row.Cells["colSelect"].Value = !allSelected;
                });

            allSelected = !allSelected;
            dgvArrangements.Refresh();
        }

        // use to manipulate data without causing error
        private void TemporaryDisableDatabindEvent(Action action)
        {
            dgvArrangements.DataBindingComplete -= dgvArrangements_DataBindingComplete;
            try
            {
                action();
            }
            finally
            {
                dgvArrangements.DataBindingComplete += dgvArrangements_DataBindingComplete;
            }
        }


        private void ToggleUIControls(bool enable)
        {
            GenExtensions.InvokeIfRequired(cueSearch, delegate { cueSearch.Enabled = enable; });
            GenExtensions.InvokeIfRequired(menuStrip, delegate { menuStrip.Enabled = enable; });
            GenExtensions.InvokeIfRequired(lnkLblSelectAll, delegate { lnkLblSelectAll.Enabled = enable; });
            GenExtensions.InvokeIfRequired(lnkClearSearch, delegate { lnkClearSearch.Enabled = enable; });
        }

        private void dgvArrangements_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // make sure grid has been painted before proceeding
            if (!dgvPainted)
                return;

            if (dgvArrangements.SelectedRows.Count > 0)
                lastSelectedSongPath = dgvArrangements.SelectedRows[0].Cells["colFilePath"].Value.ToString();
        }

        private void dgvArrangements_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // triggered by any key
            if (e.RowIndex != -1) //if it's not header
            {
                // do something here
            }
        }

        // LOOK HERE ... if any unusual errors show up
        private void dgvArrangements_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex == -1)
                return;
            // speed hacks ...
            if (dgvArrangements.Columns[e.ColumnIndex] is DataGridViewCheckBoxColumn)
                return;
            if (dgvArrangements.Rows[e.RowIndex].IsNewRow) // || !dgvArrangements.IsCurrentRowDirty)
                return;
            if (dgvArrangements.Rows.Count < 1) // needed in case filter was set that returns no items
                return;

            ArrangementData arr = dgvArrangements.Rows[e.RowIndex].DataBoundItem as ArrangementData;

            if (arr != null)
            {
                if (arr.OfficialDLC)
                {
                    e.CellStyle.Font = Constants.OfficialDLCFont;
                    // prevent checking (selecting) ODCL all together ... evil genious code
                    DataGridViewCell cell = dgvArrangements.Rows[e.RowIndex].Cells["colSelect"];
                    DataGridViewCheckBoxCell chkCell = cell as DataGridViewCheckBoxCell;
                    chkCell.Value = false;
                    chkCell.FlatStyle = FlatStyle.Flat;
                    chkCell.Style.ForeColor = Color.DarkGray;
                    // cell.ReadOnly = true;
                }
            }
        }

        private void dgvArrangements_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex == -1)
                return;

            // same in all grids
            if (e.Button == MouseButtons.Left)
            {
                // select a single row by Ctrl-Click
                if (ModifierKeys == Keys.Control)
                {
                    var arr = DgvExtensions.GetObjectFromRow<ArrangementData>(dgvArrangements, e.RowIndex);
                    arr.Selected = !arr.Selected;
                }
                // select multiple rows by Shift-Click two outer rows
                else if (ModifierKeys == Keys.Shift)
                {
                    if (dgvArrangements.SelectedRows.Count > 0)
                    {
                        var first = dgvArrangements.SelectedRows[0];
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
                                    var s = DgvExtensions.GetObjectFromRow<ArrangementData>(dgvArrangements, i);
                                    s.Selected = !s.Selected;
                                }
                            });
                    }
                }
            }
        }

        private void dgvArrangements_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
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
                    grid.Rows[e.RowIndex].Selected = true;
                    var arr = DgvExtensions.GetObjectFromRow<ArrangementData>(dgvArrangements, e.RowIndex);
                    // cmsArrangements.Show(Cursor.Position);
                }
                else
                {
                    PopulateMenuWithColumnHeaders(cmsArrangementsColumns);
                    cmsArrangementsColumns.Show(Cursor.Position);
                }
            }

            // programmatic left clicking on colSelect
            if (e.Button == MouseButtons.Left && e.RowIndex != -1 && e.ColumnIndex == colSelect.Index)
            {
                // beyound current scope of CFSM
                if (grid.Rows[e.RowIndex].Cells["colSelect"].Value.ToString().ToLower().Contains(Constants.RS1COMP))
                    Globals.Log(Properties.Resources.CanNotSelectIndividualRS1CompatiblityDLC);
                else // required to force selected row change
                {
                    TemporaryDisableDatabindEvent(() => { dgvArrangements.EndEdit(); });
                }
            }

            Thread.Sleep(50); // debounce multiple clicks
            dgvArrangements.Refresh();
        }

        private void dgvArrangements_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // right mouse click anywhere popup Context Control
            // left mouse click on header sort routines with BLRV refresh           

            if (dgvArrangements.DataSource == null)
                return;

            // Ctrl Key w/ left mouse click to quickly turn off column visiblity
            if (ModifierKeys == Keys.Control)
            {
                dgvArrangements.Columns[e.ColumnIndex].Visible = false;
                return;
            }
        }

        private void dgvArrangements_KeyDown(object sender, KeyEventArgs e)
        {
            // shortcut keys to show column filter dropdown
            if (e.Alt && (e.KeyCode == Keys.Down || e.KeyCode == Keys.Up))
            {
                DataGridViewAutoFilterColumnHeaderCell filterCell = this.dgvArrangements.CurrentCell.OwningColumn.HeaderCell as DataGridViewAutoFilterColumnHeaderCell;

                if (filterCell != null)
                {
                    filterCell.ShowDropDownList();
                    e.Handled = true;
                }
            }

            // space bar used to select a song (w/ checkbox "Select")
            if (e.KeyCode == Keys.Space)
            {
                for (int i = 0; i < dgvArrangements.Rows.Count; i++)
                {
                    if (dgvArrangements.Rows[i].Selected)
                    {
                        var song = DgvExtensions.GetObjectFromRow<ArrangementData>(dgvArrangements, i);
                        // beyound current scope of CFSM
                        if (song.IsRsCompPack)
                            Globals.Log(Properties.Resources.CanNotSelectIndividualRS1CompatiblityDLC);
                        else
                            song.Selected = !song.Selected;
                    }
                }
            }
        }

        private void dgvArrangements_KeyUp(object sender, KeyEventArgs e)
        {
            // TODO: fix cludgy action of this selection method
            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.A)
            {
                TemporaryDisableDatabindEvent(() =>
                    {
                        for (int i = 0; i < dgvArrangements.Rows.Count; i++)
                        {
                            DgvExtensions.GetObjectFromRow<ArrangementData>(dgvArrangements, i).Selected = allSelected;
                        }
                    });

                allSelected = !allSelected;
            }
        }

        private void dgvArrangements_Paint(object sender, PaintEventArgs e)
        {
            // need to wait for DataBinding and DataGridView Paint to complete before  
            // changing BLRV column color (cell formating) on initial loading

            if (bindingCompleted && !dgvPainted)
            {
                dgvPainted = true;
                // Globals.Log("dgvArrangements Painted ... ");
            }
        }

        private void dgvArrangements_Scroll(object sender, ScrollEventArgs e)
        {
            if (dgvArrangements.DataSource == null || dgvArrangements.RowCount == 0)
                return;

            firstIndex = dgvArrangements.FirstDisplayedCell.RowIndex;
        }

        private void dgvArrangements_Sorted(object sender, EventArgs e)
        {
            if (dgvArrangements.SortedColumn != null)
            {
                AppSettings.Instance.SortColumn = dgvArrangements.SortedColumn.DataPropertyName;
                AppSettings.Instance.SortAscending = dgvArrangements.SortOrder == SortOrder.Ascending ? true : false;

                // refresh is necessary to avoid exceptions when row has been deleted
                dgvArrangements.Refresh();

                // Reselect last selected row after sorting
                if (lastSelectedSongPath != string.Empty)
                {
                    int newRowIndex = dgvArrangements.Rows.Cast<DataGridViewRow>().FirstOrDefault(r => r.Cells["colFilePath"].Value.ToString() == lastSelectedSongPath).Index;
                    dgvArrangements.Rows[newRowIndex].Selected = true;
                    dgvArrangements.FirstDisplayedScrollingRowIndex = newRowIndex;
                }
                else
                    lastSelectedSongPath = String.Empty;
            }

            dgvArrangements.Refresh();
        }

        private void cueSearch_KeyUp(object sender, KeyEventArgs e)
        {
            // save current sort
            DgvExtensions.SaveSorting(dgvArrangements);

            if (cueSearch.Text.Length > 0) // && e.KeyCode == Keys.Enter)
                SearchCDLC(cueSearch.Text);
            else
                LoadFilteredBindingList(arrangementList);

            // restore current sort
            DgvExtensions.RestoreSorting(dgvArrangements);
        }

        private void dgvArrangements_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            // HACK: catch DataBindingComplete called by other UC's
            var grid = (DataGridView)sender;
            if (grid.Name != "dgvArrangements")
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

            if (!dgvPainted) // speed hack
                return;

            var filterStatus = DataGridViewAutoFilterColumnHeaderCell.GetFilterStatus(dgvArrangements);
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
            if (String.IsNullOrEmpty(filterStatus) && this.dgvArrangements.CurrentCell != null && String.IsNullOrEmpty(cueSearch.Text))
                RemoveFilter();

            // save filter - future use
            AppSettings.Instance.FilterString = DataGridViewAutoFilterColumnHeaderCell.GetFilterString(dgvArrangements);
        }

        private void lnkClearSearch_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            cueSearch.Text = String.Empty;

            // save current sorting before clearing search
            DgvExtensions.SaveSorting(dgvArrangements);
            LoadFilteredBindingList(arrangementList);
            UpdateToolStrip();
            DgvExtensions.RestoreSorting(dgvArrangements);
            AppSettings.Instance.FilterString = String.Empty;
            AppSettings.Instance.SearchString = String.Empty;
        }

        private void lnkLblSelectAll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SelectAllNone();
        }

        private void lnkShowAll_Click(object sender, EventArgs e)
        {
            RemoveFilter();
        }

        private void lnklblToggle_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            TemporaryDisableDatabindEvent(() =>
                {
                    foreach (DataGridViewRow row in dgvArrangements.Rows)
                        row.Cells["colSelect"].Value = !Convert.ToBoolean(row.Cells["colSelect"].Value);
                });

            dgvArrangements.Refresh();
        }

        public DataGridView GetGrid()
        {
            return dgvArrangements;
        }

        public void TabEnter()
        {
            Globals.DgvCurrent = dgvArrangements;
            Globals.Log("Arrangements GUI Activated ...");

            if (Globals.ReloadArrangements)
            {
                PopulateArrangementManager();
                Globals.ReloadArrangements = false;
            }

        }

        public void TabLeave()
        {
            Globals.Log("Arrangements GUI Deactivated ...");
        }

    }
}

