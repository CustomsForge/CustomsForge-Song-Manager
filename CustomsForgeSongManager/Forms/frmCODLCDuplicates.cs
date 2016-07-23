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
using CFSM.GenTools;

namespace CustomsForgeSongManager.Forms
{
    public partial class frmCODLCDuplicates : Form
    {
        private List<string> links;
        private List<SongData> songDataList;

        public frmCODLCDuplicates()
        {
            InitializeComponent();

            links = new List<string>();
            songDataList = new List<SongData>();
        }

        public void PopulateText(List<OfficialDLCSong> currentDuplicates, List<OfficialDLCSong> olderDuplicates, List<SongData> allDuplicates)
        {
            foreach (OfficialDLCSong duplicate in currentDuplicates)
                dgvCurrentODLC.Rows.Add(false, duplicate.Title, duplicate.Artist, duplicate.Pack, duplicate.ReleaseDate.ToShortDateString(), duplicate.Link);

            foreach (OfficialDLCSong duplicate in olderDuplicates)
                dgvOlderODLC.Rows.Add(false, duplicate.Title, duplicate.Artist, duplicate.Pack, duplicate.ReleaseDate.ToShortDateString(), duplicate.Link);

            songDataList = allDuplicates;
        }

        public void OpenInBrowser(DataGridView dgv)
        {
            links.Clear();

            var colNdxSelect = DgvExtensions.GetDataPropertyColumnIndex(dgv, "Select");

            for (int ndx = dgv.Rows.Count - 1; ndx >= 0; ndx--)
            {
                DataGridViewRow row = dgv.Rows[ndx];

                if (row.Selected || Convert.ToBoolean(row.Cells[colNdxSelect].Value)) //TODO: make the Boolean check work for both DataGridViews
                {
                    links.Add(row.Cells.Cast<DataGridViewCell>().First(c => c.OwningColumn.HeaderText == "Link").Value.ToString());
                }
            }

            links = links.Distinct().ToList();

            foreach (string link in links)
                Process.Start(link);
        }

        public void DeleteSelectedSongs(DataGridView dgv)
        {
            bool safe2Delete = false;
            var colNdxTitle = DgvExtensions.GetDataPropertyColumnIndex(dgv, "Title");
            var colNdxArtist = DgvExtensions.GetDataPropertyColumnIndex(dgv, "Artist");
            var colNdxSelect = DgvExtensions.GetDataPropertyColumnIndex(dgv, "Select");

            for (int ndx = dgv.Rows.Count - 1; ndx >= 0; ndx--)
            {
                DataGridViewRow row = dgv.Rows[ndx];
                var sdList = songDataList.Where(s => GenExtensions.CleanName(s.Artist) == GenExtensions.CleanName(row.Cells[colNdxArtist].Value.ToString())
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
                            for (int i = sdList.Count() - 1;  i >= 0; i--)
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
    }
}
