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
using CustomsForgeManager.Forms;
using DataGridViewTools;

namespace CustomsForgeManager.UControls
{
    public partial class Duplicates : UserControl, IDataGridViewHolder
    {
        private bool bindingCompleted = false;
        private bool dgvPainted = false;
        private List<SongData> duplicates = new List<SongData>();
        private List<SongData> dupPIDS = new List<SongData>();
        private Color ErrorStyleBackColor = Color.Red;
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
            Globals.Log("Populating Duplicates GUI ...");
            dgvDups.Visible = false;

            if (Globals.WorkerFinished == Globals.Tristate.Cancelled)
            {
                MessageBox.Show(CustomsForgeManager.Properties.Resources.DuplicatesNeedToBeRescanned,
                    Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            duplicates.Clear();
            if (findDupPIDs)
            {
                colPID.Visible = true;
                colArrangementPID.Visible = true;
                var pidList = new List<SongData>();

                // assuming every song has at least one arrangement
                foreach (var song in Globals.SongCollection)
                {
                    foreach (var arrangement in song.Arrangements2D)
                    {
                        var pidSongData = new SongData();
                        pidSongData.Enabled = song.Enabled;
                        pidSongData.Artist = song.Artist;
                        pidSongData.Title = song.Title;
                        pidSongData.Album = song.Album;
                        pidSongData.LastConversionDateTime = song.LastConversionDateTime;
                        pidSongData.Path = song.Path;
                        pidSongData.PID = arrangement.PersistentID;
                        pidSongData.ArrangementPID = arrangement.Name;
                        pidList.Add(pidSongData);
                    }
                }

                duplicates = pidList.GroupBy(x => x.PID).Where(group => group.Count() > 1).SelectMany(group => group).ToList();
            }
            else
            {
                colPID.Visible = false;
                colArrangementPID.Visible = false;
                duplicates = Globals.SongCollection.GroupBy(x => x.ArtistTitleAlbum).Where(group => group.Count() > 1).SelectMany(group => group).ToList();
            }

            duplicates.RemoveAll(x => x.FileName.ToLower().Contains(Constants.RS1COMP));

            //probably a better way to do this.
            dupPIDS.Clear();
            if (!findDupPIDs)
            {
                var dl = (from z in duplicates
                          from x in z.Arrangements2D
                          select x).GroupBy(x => x.PersistentID).Where(x => x.Count() > 1).ToList();

                dl.ForEach(d =>
                {
                    d.Where(sd => sd.Parent != null).ToList().ForEach
                        (x =>
                            {
                                if (!dupPIDS.Contains(x.Parent))
                                    dupPIDS.Add(x.Parent);
                            });
                });
            }

            LoadFilteredBindingList(duplicates);
            DgvDupsAppearance();

            // set custom selection (highlighting) color
            dgvDups.DefaultCellStyle.SelectionBackColor = Color.Gold; // dgvSongs.DefaultCellStyle.BackColor;
            dgvDups.DefaultCellStyle.SelectionForeColor = dgvDups.DefaultCellStyle.ForeColor;
            dgvDups.ClearSelection();


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

            if (dgvDups.Rows.Count == 0)
            {
                dgvDups.ColumnHeadersVisible = false;
                txtNoDuplicates.Visible = true;
            }
            else
            {
                dgvDups.ColumnHeadersVisible = true;
                txtNoDuplicates.Visible = false;
            }

            Globals.TsLabel_MainMsg.Text = string.Format(CustomsForgeManager.Properties.Resources.RocksmithSongsCountFormat, Globals.SongCollection.Count);
            Globals.TsLabel_MainMsg.Visible = true;
            Globals.TsLabel_DisabledCounter.Alignment = ToolStripItemAlignment.Right;
            Globals.TsLabel_DisabledCounter.Text = String.Format(CustomsForgeManager.Properties.Resources.DuplicatesCountFormat, dgvDups.Rows.Count);
            Globals.TsLabel_DisabledCounter.Visible = true;
            Globals.TsLabel_StatusMsg.Visible = false;
        }

        private void DgvDupsAppearance() // overrides Duplicates.Desinger.cs
        {
            // set all columns to read only except colSelect
            foreach (DataGridViewColumn col in dgvDups.Columns)
                col.ReadOnly = true;

            dgvDups.Visible = true; // needs to come now so settings apply correctly

            // see SongManager.Designer for custom appearance settings
            dgvDups.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle() { BackColor = Color.LightSteelBlue };
            dgvDups.AllowUserToAddRows = false; // removes empty row at bottom
            dgvDups.AllowUserToDeleteRows = false;
            dgvDups.AllowUserToOrderColumns = true;
            dgvDups.AllowUserToResizeColumns = true;
            dgvDups.AllowUserToResizeRows = false;
            dgvDups.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
            dgvDups.BackgroundColor = SystemColors.AppWorkspace;
            dgvDups.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            // required when using FilteredBindingList
            dgvDups.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;
            dgvDups.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            // set custom selection (highlighting) color
            dgvDups.DefaultCellStyle.SelectionBackColor = Color.Gold; // dgvDups.DefaultCellStyle.BackColor; // or removes selection highlight
            dgvDups.DefaultCellStyle.SelectionForeColor = dgvDups.DefaultCellStyle.ForeColor;
            // this overrides any user ability to make changes 
            //dgvDups.EditMode = DataGridViewEditMode.EditProgrammatically;
            dgvDups.EditMode = DataGridViewEditMode.EditOnEnter;
            dgvDups.EnableHeadersVisualStyles = true;
            dgvDups.Font = new Font("Arial", 8);
            dgvDups.GridColor = SystemColors.ActiveCaption;
            dgvDups.MultiSelect = false;
            dgvDups.Name = "dgvDups";
            dgvDups.RowHeadersVisible = false; // remove row arrow
            dgvDups.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // always visible and first
            // always visible on restart in case user changed
            colSelect.ReadOnly = false; // is overridden by EditProgrammatically
            colSelect.Visible = true;
            colSelect.Width = 50;
            colEnabled.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colEnabled.Width = 54;
            //dgvDups.Columns["colArtist"].Visible = true;
            //dgvDups.Columns["colArtist"].Width = 140;
            //dgvDups.Columns["colTitle"].Visible = true;
            //dgvDups.Columns["colTitle"].Width = 140;
            //dgvDups.Columns["colAlbum"].Visible = true;
            //dgvDups.Columns["colAlbum"].Width = 200;
            //dgvDups.Columns["colUpdated"].Visible = true;
            //dgvDups.Columns["colUpdated"].Width = 100;
            //dgvDups.Columns["colPath"].Visible = true;
            //dgvDups.Columns["colPath"].Width = 305;

            dgvDups.Refresh();
        }

        private void LoadFilteredBindingList(IList<SongData> list)
        {
            bindingCompleted = false;
            dgvPainted = false;
            // sortable binding list with drop down filtering
            dgvDups.AutoGenerateColumns = false;
            FilteredBindingList<SongData> fbl = new FilteredBindingList<SongData>(list);
            BindingSource bs = new BindingSource { DataSource = fbl };
            dgvDups.DataSource = bs;
        }

        private void RemoveFilter()
        {
            DataGridViewAutoFilterTextBoxColumn.RemoveFilter(dgvDups);
            LoadFilteredBindingList(duplicates);

            // reset alternating row color
            foreach (DataGridViewRow row in dgvDups.Rows)
                row.DefaultCellStyle.BackColor = Color.Empty;

            dgvDups.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle() { BackColor = Color.LightSteelBlue };
            UpdateToolStrip();
        }

        private void Rescan()
        {
            // save settings (column widths) in case user has modified
            Globals.Settings.SaveSettingsToFile();

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
            Globals.RescanSetlistManager = false;
            Globals.RescanDuplicates = false;
            Globals.RescanSongManager = false;
            Globals.RescanRenamer = false;
            Globals.ReloadDuplicates = false;
            Globals.ReloadSongManager = true;
            Globals.ReloadRenamer = true;
            Globals.ReloadSetlistManager = true;
        }

        private void ShowSongInfo()
        {
            if (dgvDups.SelectedRows.Count > 0)
            {

                //var selectedRow = dgvDups.SelectedRows[0];
                //var title = selectedRow.Cells["colTitle"].Value.ToString();
                //var artist = selectedRow.Cells["colArtist"].Value.ToString();
                //var album = selectedRow.Cells["colAlbum"].Value.ToString();
                //var path = selectedRow.Cells["colPath"].Value.ToString();

                //var song = duplicates.FirstOrDefault(x => x.Title == title && x.Album == album && x.Artist == artist && x.Path == path);

                SongData song = (SongData)dgvDups.SelectedRows[0].DataBoundItem;
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
            if (dgvDups.Rows.Count == 0)
                return;

            var selectedCount = dgvDups.Rows.Cast<DataGridViewRow>().Count(row => Convert.ToBoolean(row.Cells["colSelect"].Value));
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

            for (int ndx = dgvDups.Rows.Count - 1; ndx >= 0; ndx--)
            {
                DataGridViewRow row = dgvDups.Rows[ndx];

                if (Convert.ToBoolean(row.Cells["colSelect"].Value))
                {
                    // check if user manually (re)moved the song and has not rescanned 
                    if (File.Exists(duplicates[ndx].Path))
                    {
                        if (buttonName.Text.ToLower().Contains("move"))
                        {
                            var filePath = duplicates[ndx].Path;
                            var dupFileName = String.Format("{0}{1}", Path.GetFileName(filePath), ".duplicate");
                            var dupFilePath = Path.Combine(dupDir, dupFileName);
                            File.Move(duplicates[ndx].Path, dupFilePath);
                            Globals.Log(CustomsForgeManager.Properties.Resources.DuplicateFile + dupFileName);
                            Globals.Log(CustomsForgeManager.Properties.Resources.MovedTo + dupDir);
                        }
                        else
                            File.Delete(duplicates[ndx].Path);
                    }

                    dgvDups.Rows.Remove(row);
                }
            }

            if (dgvDups.Rows.Count == 1)
                dgvDups.Rows.Clear();

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
                foreach (DataGridViewRow row in dgvDups.Rows)
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

        private void dgvDups_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // triggered by any key
            if (e.RowIndex != -1 && e.RowIndex != colSelect.Index)
                ShowSongInfo();
        }


        private void dgvDups_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            // has precedent over a ColumnHeader_MouseClick
            // MouseUp detection is more reliable than MouseDown

            var rowIndex = e.RowIndex;

            if (e.ColumnIndex == colSelect.Index && rowIndex != -1)
            {
                dgvDups.DataBindingComplete -= dgvDups_DataBindingComplete;
                try
                {
                    dgvDups.EndEdit();
                }
                finally
                {
                    dgvDups.DataBindingComplete += dgvDups_DataBindingComplete;
                }
            }
            else

                if (e.Button == MouseButtons.Right)
                {
                    // fancy way to decide when context menu pops up
                    dgvDups.ContextMenuStrip.Opening += (s, i) =>
                        {
                            if (rowIndex != -1)
                            {
                                dgvDups.Rows[rowIndex].Selected = true;
                                i.Cancel = false; // resets e.RowIndex
                                cmsDuplicate.Show(Cursor.Position);
                            }
                            else
                                i.Cancel = true;
                        };
                }
        }

        private void dgvDups_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            // workaround to catch DataBindingComplete called by other UC's
            var grid = (DataGridView)sender;
            if (grid.Name != "dgvDups")
                return;

            if (!bindingCompleted)
            {
                Debug.WriteLine("DataBinding Complete ... ");
                bindingCompleted = true;
            }

            var filterStatus = DataGridViewAutoFilterColumnHeaderCell.GetFilterStatus(dgvDups);
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
            if (String.IsNullOrEmpty(filterStatus) && dgvPainted && this.dgvDups.CurrentCell != null)
                RemoveFilter();
        }

        private void dgvDups_KeyDown(object sender, KeyEventArgs e)
        {
            // space bar used to select a song (w/ checkbox "Select")
            if (e.KeyCode == Keys.Space)
                foreach (DataGridViewRow row in dgvDups.Rows)
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

        private void dgvDups_Paint(object sender, PaintEventArgs e)
        {
            if (bindingCompleted && !dgvPainted)
            {
                dgvPainted = true;
                Globals.DebugLog("dgvDups Painted ... ");
                // it is safe to do cell formatting (coloring)
                // here
            }
        }

        private void exploreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var filePath = duplicates[dgvDups.SelectedRows[0].Index].Path;

            if (File.Exists(filePath))
                Process.Start("explorer.exe", string.Format("/select,\"{0}\"", filePath));
        }

        private void lnkPersistentId_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            PopulateDuplicates(!colPID.Visible);

            UpdateToolStrip();
        }

        private void lnkShowAll_Click(object sender, EventArgs e)
        {
            RemoveFilter();
        }


        public DataGridView GetGrid()
        {
            return dgvDups;
        }

        private void dgvDups_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex == -1) return;

            try
            {
                var x = (SongData)dgvDups.Rows[e.RowIndex].DataBoundItem;
                if (x != null)
                {
                    if (dupPIDS.Contains(x))
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

    }
}
