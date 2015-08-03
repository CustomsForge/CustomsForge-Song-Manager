using System;
using System.Drawing;
using System.Windows.Forms;
using CustomsForgeManager.CustomsForgeManagerLib.Objects;

namespace CustomsForgeManager.Forms
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
            string[] splits = arrangements.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (DataGridViewColumn column in dgv_Arrangements.Columns)
            {
                column.Visible = false;
            }

            foreach (string split in splits)
            {
                if (split.ToLower().Contains("combo"))
                {
                    dgv_Arrangements.Columns["cLead"].DefaultCellStyle.BackColor = Color.Yellow;
                    dgv_Arrangements.Columns["cRhythm"].DefaultCellStyle.BackColor = Color.Yellow;
                    dgv_Arrangements.Columns["cLead"].Visible = true;
                    dgv_Arrangements.Columns["cRhythm"].Visible = true;
                }
                else
                {
                    string column_name = "c" + split;
                    var dataGridViewColumn = dgv_Arrangements.Columns[column_name];
                    if (dataGridViewColumn != null)
                    {
                        dataGridViewColumn.DefaultCellStyle.BackColor = Color.Lime;
                        dataGridViewColumn.Visible = true;
                    }
                }
            }

            dgv_Arrangements.Rows.Add(new[] { Properties.Resources.Letter_L, Properties.Resources.Letter_R, Properties.Resources.Letter_B, Properties.Resources.Letter_V });
            dgv_Arrangements.ClearSelection();
        }

    }
}
