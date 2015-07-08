using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CustomsForgeManager_Winforms.Models;

namespace CustomsForgeManager_Winforms.Forms
{
    public partial class frmSongInfo : Form
    {
        private readonly SongData song;

        public frmSongInfo(SongData data)
        {
            this.song = data;
            InitializeComponent();
        }

        private void frmSongInfo_Load(object sender, EventArgs e)
        {
            lbl_PanelSongTitle.Text = song.Song;
            lbl_PanelSongAlbum.Text = song.Album;
            lbl_PanelSongArtist.Text = song.Artist;
            lbl_PanelSongYear.Text = song.SongYear;
            lbl_PanelSongTuning.Text = song.Tuning;
            lbl_PanelSongArrangements.Text = song.Arrangements;
            lbl_PanelSongDD.Text = song.DD == "0" ? "No" : "Yes";
            lbl_PanelSongAuthor.Text = song.Author;

            FillGridWithArrangements(song.Arrangements);

        }

        private void FillGridWithArrangements(string arrangements)
        {
            string[] splits = arrangements.Split(new char[]{','}, StringSplitOptions.RemoveEmptyEntries);

            //string grid_columns = "";
            foreach (DataGridViewColumn column in dgv_Arrangements.Columns)
            {
                column.Visible = false;
            }
            //MessageBox.Show(grid_columns);

            foreach (string split in splits)
            {
                string column_name = "c" + split;
                var dataGridViewColumn = dgv_Arrangements.Columns[column_name];
                if (dataGridViewColumn != null) dataGridViewColumn.Visible = true;
            }
            dgv_Arrangements.Rows.Add(new []{Properties.Resources.Letter_L, Properties.Resources.Letter_R, Properties.Resources.Letter_B, Properties.Resources.Letter_V});
            dgv_Arrangements.ClearSelection();
        }
    }
}
