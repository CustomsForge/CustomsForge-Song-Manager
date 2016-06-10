using System;
using System.Drawing;
using System.Windows.Forms;
using CFSM.ImageTools;
using CustomsForgeSongManager.DataObjects;
using DF.WinForms.ThemeLib;
using DataGridViewTools;

namespace CustomsForgeSongManager.UITheme
{
    public partial class ThemePreview : ThemedForm
    {
        private Color _Enabled = Color.Lime;
        private Color _Disabled = Color.Red;

        protected ThemePreview() : base()
        {
            InitializeComponent();
            this.Icon = Properties.Resources.cfsm_48x48;
            tscbTaggerThemes.SelectedIndex = 0;
            tspbAudioPosition.Value = 50;
        }

        public ThemePreview(Theme theme) : base(theme)
        {
            InitializeComponent();
            this.Icon = Properties.Resources.cfsm_48x48;
            tscbTaggerThemes.SelectedIndex = 0;
            tspbAudioPosition.Value = 50;
            dgvSongsMaster.DataSource = new BindingSource {DataSource = new FilteredBindingList<SongData>(Globals.SongCollection)};
        }

        public override void ApplyTheme(Theme sender)
        {
            base.ApplyTheme(sender);

            ThemeColumnSettings cs = sender.GetThemeSetting<ThemeColumnSettings>();
            if (cs != null)
            {
                colBass.Image = cs.ColBass.Image.ScaleImage(16);
                colRhythm.Image = cs.ColRhythm.Image.ScaleImage(16);
                colLead.Image = cs.ColLead.Image.ScaleImage(16);
                colVocals.Image = cs.ColVocal.Image.ScaleImage(16);
                _Enabled = cs.ColAvaliable;
                _Disabled = cs.ColNotAvaliable;
                //  DataGridViewAutoFilterColumnHeaderCell.SetThemed(cs.FilterOn.Image, cs.FilterOff.Image, null, null);
            }

            ThemeToolbarImages tbi = sender.GetThemeSetting<ThemeToolbarImages>();
            if (tbi != null)
            {
                if (tbi.Play.Image != null)
                    tsbPlay.Image = tbi.Play.Image.ScaleImage(16);
                if (tbi.Stop.Image != null)
                    tsbStop.Image = tbi.Stop.Image.ScaleImage(16);
                if (tbi.Export.Image != null)
                    tsddExport.Image = tbi.Export.Image.ScaleImage(16);
                if (tbi.Launch.Image != null)
                    tsBtnLaunchRS.Image = tbi.Launch.Image.ScaleImage(16);
                if (tbi.Backup.Image != null)
                    tsBtnBackup.Image = tbi.Backup.Image.ScaleImage(16);
                if (tbi.Upload.Image != null)
                    tsBtnUpload.Image = tbi.Upload.Image.ScaleImage(16);
                if (tbi.Help.Image != null)
                    tsBtnHelp.Image = tbi.Help.Image.ScaleImage(16);
                if (tbi.Request.Image != null)
                    tsBtnRequest.Image = tbi.Request.Image.ScaleImage(16);
            }
        }

        private void dgvSongsMaster_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == colBass.Index || e.ColumnIndex == colVocals.Index || e.ColumnIndex == colLead.Index || e.ColumnIndex == colRhythm.Index)
            {
                var x = (SongData) dgvSongsMaster.Rows[e.RowIndex].DataBoundItem;
                if (x != null)
                {
                    string arrInit = x.Arrangements.ToUpper();

                    if (e.ColumnIndex == colBass.Index)
                        e.CellStyle.BackColor = arrInit.Contains("BASS") ? _Enabled : _Disabled;
                    else if (e.ColumnIndex == colVocals.Index)
                        e.CellStyle.BackColor = arrInit.Contains("VOCAL") ? _Enabled : _Disabled;
                    else if (e.ColumnIndex == colLead.Index)
                    {
                        if (arrInit.Contains("COMBO"))
                            e.CellStyle.BackColor = arrInit.Contains("COMBO") ? _Enabled : _Disabled;
                        else
                            e.CellStyle.BackColor = arrInit.Contains("LEAD") ? _Enabled : _Disabled;
                    }
                    else if (e.ColumnIndex == colRhythm.Index)
                    {
                        if (arrInit.Contains("COMBO"))
                            e.CellStyle.BackColor = arrInit.Contains("COMBO") ? _Enabled : _Disabled;
                        else
                            e.CellStyle.BackColor = arrInit.Contains("RHYTHM") ? _Enabled : _Disabled;
                    }
                }
            }
        }

        private void tspbAudioPosition_MouseDown(object sender, MouseEventArgs e)
        {
            tspbAudioPosition.Value = Convert.ToInt32(((float) e.Location.X/(float) tspbAudioPosition.Width)*100);
        }

        private void tsButtonTagSelected_Click(object sender, EventArgs e)
        {
            if (Theme != null)
                Theme.Enabled = !Theme.Enabled;
        }
    }
}