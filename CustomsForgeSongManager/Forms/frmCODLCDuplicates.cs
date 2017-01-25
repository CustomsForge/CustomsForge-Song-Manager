using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CustomsForgeSongManager.DataObjects;
using System.Diagnostics;
using DataGridViewTools;
using System.IO;
using GenTools;
using Newtonsoft.Json;

namespace CustomsForgeSongManager.Forms
{
    public partial class frmCODLCDuplicates : Form
    {
        private List<string> links;
        private List<SongData> songDataList;
        private bool allSelectedOlder = false, allSelectedCurrent = false;

        public frmCODLCDuplicates()
        {
            InitializeComponent();

            links = new List<string>();
            songDataList = new List<SongData>();

            if (Globals.OfficialDLCSongList.Count == 0)
                PopulateODLCList();
        }

        private void PopulateODLCList()
        {
            string oDLCJson = Properties.Resources.OfficialSongs;
            Globals.OfficialDLCSongList = JsonConvert.DeserializeObject<List<OfficialDLCSong>>(oDLCJson);
        }

        private Tuple<List<OfficialDLCSong>, List<SongData>> GetDuplicateODLCSongs(bool clean = true)
        {
            List<OfficialDLCSong> duplicateList = new List<OfficialDLCSong>();
            List<SongData> songDataList = new List<SongData>();

            if (Globals.OfficialDLCSongList.Count == 0)
                PopulateODLCList();

            // commented out for testing
            //if (Globals.OfficialDLCSongList.Count == 0 || Globals.SongCollection.Count == 0 || (DateTime.Today - AppSettings.Instance.LastODLCCheckDate).TotalDays < 7)
            //    return;

            foreach (SongData song in Globals.SongCollection)
            {
                if (song.OfficialDLC)
                    continue;

                foreach (OfficialDLCSong officialSong in Globals.OfficialDLCSongList)
                {
                    if (GenExtensions.CleanName(song.Artist) == GenExtensions.CleanName(officialSong.Artist) && GenExtensions.CleanName(song.Title) == GenExtensions.CleanName(officialSong.Title))
                    {
                        duplicateList.Add(officialSong);
                        songDataList.Add(song);
                    }
                }
            }

            if (clean)
                duplicateList = duplicateList.GroupBy(x => new { x.Title, x.Artist }).Select(y => y.First()).ToList();

            return Tuple.Create(duplicateList, songDataList);

            // For later:
            // AppSettings.Instance.LastODLCCheckDate = DateTime.Now; 
        }

        public bool PopulateLists()
        {
            Tuple<List<OfficialDLCSong>, List<SongData>> lists = GetDuplicateODLCSongs();
            List<OfficialDLCSong> duplicateList = lists.Item1;

            List<OfficialDLCSong> currentODLCList = new List<OfficialDLCSong>();
            List<OfficialDLCSong> olderODLCList = new List<OfficialDLCSong>();

            if(duplicateList.Count == 0)
            {
                MessageBox.Show(Properties.Resources.NoODLCDuplicatesDetected, "Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                // Application.Exit(); // WTF?
                return false;
            }

            foreach (OfficialDLCSong song in duplicateList)
            {
                if ((DateTime.Today - song.ReleaseDate).TotalDays < 7)
                    currentODLCList.Add(song);
                else
                    olderODLCList.Add(song);
            }

            foreach (OfficialDLCSong duplicate in currentODLCList)
                dgvCurrentODLC.Rows.Add(false, duplicate.Title, duplicate.Artist, duplicate.Pack, duplicate.ReleaseDate.ToShortDateString(), duplicate.Link);

            foreach (OfficialDLCSong duplicate in olderODLCList)
                dgvOlderODLC.Rows.Add(false, duplicate.Title, duplicate.Artist, duplicate.Pack, duplicate.ReleaseDate.ToShortDateString(), duplicate.Link);

            songDataList = lists.Item2;
            return true;
        }

        private void OpenInBrowser(DataGridView dgv)
        {
            links.Clear();

            var colNdxSelect = DgvExtensions.GetDataPropertyColumnIndex(dgv, "Select");

            for (int ndx = dgv.Rows.Count - 1; ndx >= 0; ndx--)
            {
                DataGridViewRow row = dgv.Rows[ndx];

                if (row.Selected || Convert.ToBoolean(row.Cells[colNdxSelect].Value))
                {
                    links.Add(row.Cells.Cast<DataGridViewCell>().First(c => c.OwningColumn.HeaderText == "Link").Value.ToString());
                }
            }

            links = links.Distinct().ToList();

            foreach (string link in links)
                Process.Start(link);
        }

        private void DeleteSelectedSongs(DataGridView dgv)
        {
            bool safe2Delete = false;
            var colNdxTitle = DgvExtensions.GetDataPropertyColumnIndex(dgv, "Title");
            var colNdxArtist = DgvExtensions.GetDataPropertyColumnIndex(dgv, "Artist");
            var colNdxSelect = DgvExtensions.GetDataPropertyColumnIndex(dgv, "Select");

            for (int ndx = dgv.Rows.Count - 1; ndx >= 0; ndx--)
            {
                DataGridViewRow row = dgv.Rows[ndx];
                var     sdList = songDataList.Where(s => GenExtensions.CleanName(s.Artist) == GenExtensions.CleanName(row.Cells[colNdxArtist].Value.ToString())
                          && GenExtensions.CleanName(s.Title) == GenExtensions.CleanName(row.Cells[colNdxTitle].Value.ToString())).ToList();

                if (row.Selected || Convert.ToBoolean(row.Cells[colNdxSelect].Value))
                {
                    string songPath = "";

                    if (!safe2Delete)
                    {
                        // DANGER ZONE
                        if (MessageBox.Show(string.Format(Properties.Resources.YouAreAboutToPermanentlyDeleteAllSelectedS, Environment.NewLine), Constants.ApplicationName + " ... Warning ... Warning", MessageBoxButtons.YesNo) == DialogResult.No)
                            return;

                        safe2Delete = true;
                    }

                    if (safe2Delete)
                    {
                        try
                        {
                            for (int i = sdList.Count() - 1; i >= 0; i--)
                            {
                                songPath = sdList[i].FilePath;
                                sdList[i].Delete();
                                dgv.Rows.Remove(row);
                            }
                        }
                        catch (IOException ex)
                        {
                            MessageBox.Show(string.Format(Properties.Resources.UnableToDeleteSongX0X1ErrorX2, songPath, Environment.NewLine, ex.Message), Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }

            if (safe2Delete)
            {
                Globals.RescanSongManager = true;
                Globals.RescanSetlistManager = true;
                Globals.RescanDuplicates = true;
                Globals.RescanRenamer = true;
            }
        }

        private void btnDeleteCurrentSongs_Click(object sender, EventArgs e)
        {
            DeleteSelectedSongs(dgvCurrentODLC);
        }

        private void btnDeleteOlderSongs_Click(object sender, EventArgs e)
        {
            DeleteSelectedSongs(dgvOlderODLC);
        }

        private void btnOpenCurrentDLCRRPage_Click(object sender, EventArgs e)
        {
            OpenInBrowser(dgvCurrentODLC);
        }

        private void btnOpenOlderDLCRRPage_Click(object sender, EventArgs e)
        {
            OpenInBrowser(dgvOlderODLC);
        }

        private void linkLblSelectAllCurrent_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            foreach (DataGridViewRow row in dgvCurrentODLC.Rows)
                row.Cells["colSelect"].Value = !allSelectedCurrent;

            allSelectedCurrent = !allSelectedCurrent;
        }

        private void linkLblSelectAllOlder_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            foreach (DataGridViewRow row in dgvOlderODLC.Rows)
                row.Cells["colSelect2"].Value = !allSelectedOlder;

            allSelectedOlder = !allSelectedOlder;
        }

        private void dgvCurrentODLC_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && e.RowIndex != -1 && e.ColumnIndex == colSelect.Index)
            {
                dgvCurrentODLC.Rows[e.ColumnIndex].Cells["colSelect"].Value = !(bool)(dgvCurrentODLC.Rows[e.ColumnIndex].Cells["colSelect"].Value);
            }
        }

        private void dgvOlderODLC_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && e.RowIndex != -1 && e.ColumnIndex == colSelect.Index)
            {
                dgvOlderODLC.Rows[e.ColumnIndex].Cells["colSelect2"].Value = !(bool)(dgvOlderODLC.Rows[e.ColumnIndex].Cells["colSelect2"].Value);
            }
        }
    }
}
