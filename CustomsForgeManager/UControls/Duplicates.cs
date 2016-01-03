using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CustomsForgeManager.CustomsForgeManagerLib;
using CustomsForgeManager.CustomsForgeManagerLib.Objects;
using CustomsForgeManager.CustomsForgeManagerLib.UITheme;
using CustomsForgeManager.Forms;
using DataGridViewTools;

namespace CustomsForgeManager.UControls
{
    public partial class Duplicates : UserControl, IDataGridViewHolder, INotifyTabChanged
    {
        private bool bindingCompleted = false;
        private bool dgvPainted = false;
        private List<SongData> duplicates = new List<SongData>();
        private List<string> distinctPIDS = new List<string>();
        private Color ErrorStyleBackColor = Color.DarkGray;
        private Color ErrorStyleForeColor = Color.White;
        // private Font ErrorStyleFont;
        private DataGridViewCellStyle ErrorStyle;

        public Duplicates()
        {
            InitializeComponent();
            Globals.TsLabel_StatusMsg.Click += lnkShowAll_Click;
            btnMove.Click += DeleteMoveSelected;
            btnDeleteSong.Click += DeleteMoveSelected;

            ErrorStyle = new DataGridViewCellStyle()
            {
                Font = new Font("Arial", 8, FontStyle.Italic),
                ForeColor = ErrorStyleForeColor,
                BackColor = ErrorStyleBackColor
            };

            txtNoDuplicates.Visible = false;
            PopulateDuplicates();
        }

        public void PopulateDuplicates(bool findDupPIDs = false)
        {
            // NOTE: do not add SongData.Arrangments to the datagridview
            Globals.Log("Populating Duplicates GUI ...");
            Globals.Settings.LoadSettingsFromFile(dgvDuplicates);
            dgvDuplicates.Visible = false;

            if (Globals.WorkerFinished == Globals.Tristate.Cancelled)
            {
                MessageBox.Show(CustomsForgeManager.Properties.Resources.DuplicatesNeedToBeRescanned, Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                        // shallow/deep copy methods will not work here because of Arrangements2D
                        var pidSongData = new SongData();

                        // must have setters to be assignable
                        pidSongData.PID = arrangement.PersistentID;
                        pidSongData.PIDArrangement = arrangement.Name;
                        pidSongData.DLCKey = song.DLCKey;
                        pidSongData.Selected = song.Selected;
                        pidSongData.Enabled = song.Enabled;
                        pidSongData.Artist = song.Artist;
                        pidSongData.Title = song.Title;
                        pidSongData.Album = song.Album;
                        pidSongData.SongYear = song.SongYear;
                        pidSongData.Arrangements = song.Arrangements;
                        pidSongData.SongLength = song.SongLength;
                        pidSongData.SongAverageTempo = song.SongAverageTempo;
                        pidSongData.AppID = song.AppID;
                        pidSongData.Path = song.Path;
                        pidSongData.Status = song.Status;
                        pidSongData.Charter = song.Charter;
                        pidSongData.LastConversionDateTime = song.LastConversionDateTime;
                        pidSongData.Version = song.Version;
                        pidSongData.ToolkitVer = song.ToolkitVer;
                        pidSongData.Tagged = song.Tagged;
                        pidSongData.IgnitionID = song.IgnitionID;
                        pidSongData.IgnitionUpdated = song.IgnitionUpdated;
                        pidSongData.IgnitionVersion = song.IgnitionVersion;
                        pidSongData.IgnitionAuthor = song.IgnitionAuthor;

                        pidList.Add(pidSongData);
                    }
                }

                duplicates = pidList.GroupBy(x => x.PID).Where(group => group.Count() > 1).SelectMany(group => group).ToList();
                distinctPIDS = duplicates.Select(x => x.PID).Distinct().ToList();

                colPID.Visible = true;
                colPIDArrangement.Visible = true;
            }
            else
            {
                colPID.Visible = false;
                colPIDArrangement.Visible = false;

                duplicates = Globals.SongCollection.GroupBy(x => x.ArtistTitleAlbum).Where(group => group.Count() > 1).SelectMany(group => group).ToList();
            }

            duplicates.RemoveAll(x => x.FileName.ToLower().Contains(Constants.RS1COMP));

            // processing order effects datagridview appearance
            LoadFilteredBindingList(duplicates);
            CFSMTheme.InitializeDgvAppearance(dgvDuplicates);
            // reload column order, width, visibility
            if (!colPID.Visible)
                if (AppSettings.Instance.ManagerGridSettings != null)
                    dgvDuplicates.ReLoadColumnOrder(AppSettings.Instance.ManagerGridSettings.ColumnOrder);

            Globals.ReloadDuplicates = false;
        }

        public void UpdateToolStrip()
        {
            if (Globals.RescanDuplicates)
            {
                Globals.RescanDuplicates = false;
                Rescan();
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

            Globals.TsLabel_MainMsg.Text = string.Format(CustomsForgeManager.Properties.Resources.RocksmithSongsCountFormat, Globals.SongCollection.Count);
            Globals.TsLabel_MainMsg.Visible = true;
            Globals.TsLabel_DisabledCounter.Alignment = ToolStripItemAlignment.Right;
            Globals.TsLabel_DisabledCounter.Text = String.Format(CustomsForgeManager.Properties.Resources.DuplicatesCountFormat, dgvDuplicates.Rows.Count);
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
                MessageBox.Show(CustomsForgeManager.Properties.Resources.ErrorRocksmith2014InstallationDirectorySet, Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            PopulateDuplicates();
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
                //var path = selectedRow.Cells["colPath"].Value.ToString();

                //var song = duplicates.FirstOrDefault(x => x.Title == title && x.Album == album && x.Artist == artist && x.Path == path);

                SongData song = (SongData)dgvDuplicates.SelectedRows[0].DataBoundItem;
                if (song != null)
                {
                    frmSongInfo infoWindow = new frmSongInfo(song);
                    infoWindow.Show();
                }
            }
            else
                MessageBox.Show(CustomsForgeManager.Properties.Resources.PleaseSelectHighlightTheSongThatNYouWould,
                    Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Information);
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

            string dupDir = Path.Combine(AppSettings.Instance.RSInstalledDir, "cdlc_duplicates");

            if (!Directory.Exists(dupDir))
                Directory.CreateDirectory(dupDir);

            for (int ndx = dgvDuplicates.Rows.Count - 1; ndx >= 0; ndx--)
            {
                DataGridViewRow row = dgvDuplicates.Rows[ndx];

                if (Convert.ToBoolean(row.Cells["colSelect"].Value))
                {
                    // check if user manually (re)moved the song and has not rescanned 
                    if (File.Exists(duplicates[ndx].Path))
                    {
                        if (buttonName.Text.ToLower().Contains("move"))
                        {
                            var filePath = duplicates[ndx].Path;
                            var dupFileName = String.Format("{0}{1}", Path.GetFileName(filePath), ".dup");
                            var dupFilePath = Path.Combine(dupDir, dupFileName);

                            if (File.Exists(dupFilePath))
                                File.Delete(dupFilePath);

                            File.Move(duplicates[ndx].Path, dupFilePath);
                            Globals.Log(CustomsForgeManager.Properties.Resources.DuplicateFile + dupFileName);
                            Globals.Log(CustomsForgeManager.Properties.Resources.MovedTo + dupDir);
                        }
                        else
                            File.Delete(duplicates[ndx].Path);
                    }

                    dgvDuplicates.Rows.Remove(row);
                }
            }

            if (dgvDuplicates.Rows.Count == 1)
                dgvDuplicates.Rows.Clear();

            UpdateToolStrip();
            // rescan on tabpage change to remove from Globals.SongCollection
            Globals.RescanSongManager = true;
            Globals.RescanDuplicates = true;
            Globals.RescanSetlistManager = true;
            Globals.RescanRenamer = true;
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
                        var originalPath = row.Cells["colPath"].Value.ToString();
                        if (!originalPath.ToLower().Contains(String.Format("{0}{1}", Constants.RS1COMP, "disc")))
                        {

                            var ColEnabled = row.Cells["colEnabled"];
                            // confirmed CDLC is disabled in game when using this file naming method
                            if (ColEnabled.Value.ToString() == "Yes")
                            {
                                var disabledDLCPath = originalPath.Replace("_p.psarc", "_p.disabled.psarc");
                                File.Move(originalPath, disabledDLCPath);
                                row.Cells["colPath"].Value = disabledDLCPath;
                                ColEnabled.Value = "No";
                                updateSongCollection = true;
                            }
                            else
                            {
                                var enabledDLCPath = originalPath.Replace("_p.disabled.psarc", "_p.psarc");
                                File.Move(originalPath, enabledDLCPath);
                                row.Cells["colPath"].Value = enabledDLCPath;
                                ColEnabled.Value = "Yes";
                                updateSongCollection = true;
                            }

                            cell.Value = "false";
                        }
                        else
                            Globals.Log(CustomsForgeManager.Properties.Resources.ThisIsARocksmith1SongItCanTBeDisabled);
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
        }

        private void dgvDuplicates_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // triggered by any key
            if (e.RowIndex != -1 && e.RowIndex != colSelect.Index)
                ShowSongInfo();
        }


        private void dgvDuplicates_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            // has precedent over a ColumnHeader_MouseClick
            // MouseUp detection is more reliable than MouseDown

            var rowIndex = e.RowIndex;

            if (e.ColumnIndex == colSelect.Index && rowIndex != -1)
            {
                dgvDuplicates.DataBindingComplete -= dgvDuplicates_DataBindingComplete;
                try
                {
                    dgvDuplicates.EndEdit();
                }
                finally
                {
                    dgvDuplicates.DataBindingComplete += dgvDuplicates_DataBindingComplete;
                }
            }
            else

                if (e.Button == MouseButtons.Right)
                {
                    // fancy way to decide when context menu pops up
                    dgvDuplicates.ContextMenuStrip.Opening += (s, i) =>
                        {
                            if (rowIndex != -1)
                            {
                                dgvDuplicates.Rows[rowIndex].Selected = true;
                                i.Cancel = false; // resets e.RowIndex
                                cmsDuplicate.Show(Cursor.Position);
                            }
                            else
                                i.Cancel = true;
                        };
                }
        }

        private void dgvDuplicates_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            // workaround to catch DataBindingComplete called by other UC's
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

        private void exploreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var filePath = duplicates[dgvDuplicates.SelectedRows[0].Index].Path;

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


        private void dgvDups_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex == -1) return;

            try
            {
                var x = (SongData)dgvDuplicates.Rows[e.RowIndex].DataBoundItem;
                if (x != null)
                {
                    if (distinctPIDS.Contains(x.PID))
                    {
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


        private void Duplicates_Resize(object sender, EventArgs e)
        {
            var p = new Point()
            {
                X = (Width - txtNoDuplicates.Width) / 2,
                Y = (Height - txtNoDuplicates.Height) / 2
            };

            if (p.X < 3)
                p.X = 3;

            if (p.Y < 3)
                p.Y = 3;
            txtNoDuplicates.Location = p;
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
            LeaveDuplicates();
        }

        public void LeaveDuplicates()
        {
            Globals.Log("Leaving Duplicates GUI ...");
            Globals.Settings.SaveSettingsToFile(dgvDuplicates);

        }


    }
}
