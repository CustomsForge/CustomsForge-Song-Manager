using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using CustomsForgeSongManager.DataObjects;
using RocksmithToolkitLib;
using RocksmithToolkitLib.Extensions;

namespace CustomsForgeSongManager.SongEditor
{
    public partial class ucSongInfo : DLCPackageEditorControlBase
    {
        public ucSongInfo()
        {
            InitializeComponent();
        }

        public override void DoInit()
        {
            // this will throw exception if SongFilePath not found
            var song = Globals.SongCollection.First(x => x.Path == FilePath);

            cbLowBass.Visible = song.ArrangementInitials.Contains('B');

            txtKey.Text = SongData.Name;
            txtArtist.Text = SongData.SongInfo.Artist;
            txtArtistSort.Text = SongData.SongInfo.ArtistSort;
            txtTitle.Text = SongData.SongInfo.SongDisplayName;
            txtTitleSort.Text = SongData.SongInfo.SongDisplayNameSort;
            txtAlbum.Text = SongData.SongInfo.Album;
            txtAlbumSort.Text = SongData.SongInfo.AlbumSort;
            txtAppId.Text = SongData.AppId;
            txtVersion.Text = SongData.PackageVersion;
            txtYear.Text = SongData.SongInfo.SongYear.ToString();
            txtAvgTempo.Text = SongData.SongInfo.AverageTempo.ToString();
            cmbSongVolume.Value = Convert.ToDecimal(SongData.Volume);
            cmbPreviewVolume.Value = Convert.ToDecimal(SongData.PreviewVolume);
            txtCharter.Text = song.CharterName; // not editable

            txtKey.Validating += ValidateKey;
            txtArtist.Validating += ValidateName;
            txtArtistSort.Validating += ValidateSort;
            txtTitle.Validating += ValidateName;
            txtTitleSort.Validating += ValidateSort;
            txtAlbum.Validating += ValidateName;
            txtAlbumSort.Validating += ValidateSort;
            txtAppId.Validating += ValidateVersion;
            txtVersion.Validating += ValidateVersion;
            txtYear.Validating += ValidateVersion;
            txtAvgTempo.Validating += ValidateTempo;
            cmbSongVolume.Validating += ValidateText;
            cmbPreviewVolume.Validating += ValidateText;

            cbLowBass.Validating += ValidateText;
            //set up events
            PopulateAppIdCombo(SongData.AppId, GameVersion.RS2014);
        }

        private void ValidateKey(object sender, CancelEventArgs e)
        {
            var tb = sender as TextBox;
            if (tb != null)
            {
                tb.Text = tb.Text.Trim().GetValidDlcKey(txtTitle.Text);
                this.Dirty = true;
            }
        }

        private void ValidateTempo(object sender, CancelEventArgs e)
        {
            var tb = sender as TextBox;
            if (tb != null)
            {
                float tempo = 0;
                float.TryParse(tb.Text.Trim(), out tempo);
                tb.Text = Math.Round(tempo).ToString();
                this.Dirty = true;
            }
        }

        private void ValidateSort(object sender, CancelEventArgs e)
        {
            var tb = sender as TextBox;
            if (tb != null)
            {
                tb.Text = tb.Text.Trim().GetValidSortName();
                this.Dirty = true;
            }
        }

        private void ValidateName(object sender, CancelEventArgs e)
        {
            var tb = sender as TextBox;
            if (tb != null)
            {
                tb.Text = tb.Text.Trim().GetValidName(true, true);
                this.Dirty = true;

                //if (tb.Name == "txtTitle")
                //    txtKey.Text = String.Format("{0} {1}", txtArtist.Text.Acronym(), txtTitle.Text.GetValidSortName());
            }
        }

        private void ValidateVersion(object sender, CancelEventArgs e)
        {
            var tb = sender as TextBox;
            if (tb != null)
            {
                tb.Text = tb.Text.Trim().GetValidVersion();
                this.Dirty = true;
            }
        }

        private void ValidateText(object sender, CancelEventArgs e)
        {
            this.Dirty = true;
        }

        public bool ApplyBassFix()
        {
            return cbLowBass.Checked;
        }

        public override void Save()
        {
            txtKey.Focus();
            SongData.Name = txtKey.Text;
            SongData.SongInfo.Artist = txtArtist.Text;
            SongData.SongInfo.ArtistSort = txtArtistSort.Text;
            SongData.SongInfo.SongDisplayName = txtTitle.Text;
            SongData.SongInfo.SongDisplayNameSort = txtTitleSort.Text;
            SongData.SongInfo.Album = txtAlbum.Text;
            SongData.SongInfo.AlbumSort = txtAlbumSort.Text;
            SongData.AppId = txtAppId.Text;
            SongData.PackageVersion = txtVersion.Text;
            SongData.SongInfo.SongYear = Convert.ToInt32(txtYear.Text);
            SongData.SongInfo.AverageTempo = Convert.ToInt32(txtAvgTempo.Text);
            SongData.Volume = Convert.ToSingle(cmbSongVolume.Value);
            SongData.PreviewVolume = Convert.ToSingle(cmbPreviewVolume.Value);
            base.Save();
        }

        private void PopulateAppIdCombo(string appId, GameVersion gameVersion)
        {
            cmbAppId.Items.Clear();
            try
            {
                foreach (var song in SongAppIdRepository.Instance().Select(gameVersion))
                    cmbAppId.Items.Add(song);
            }
            catch (Exception)
            {
                var msg = "Can not find file: RocksmithToolkitLib.SongAppId.xml" + Environment.NewLine + "Try reinstalling the complete CFM Applicton.";
                MessageBox.Show(msg, Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                var songAppId = SongAppIdRepository.Instance().Select(appId, gameVersion);
                cmbAppId.SelectedItem = songAppId; // triggers SelectedIndexChanged
            }
            catch (Exception)
            {
                var msg = "Please select a new AppId from available selection." + Environment.NewLine + "Can not find: " + appId;
                MessageBox.Show(msg, Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbAppId_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbAppId.SelectedItem != null)
            {
                txtAppId.Text = cmbAppId.SelectedItem.ToString().Split(new string[] { " - " }, StringSplitOptions.None)[2];
            }
        }
    }
}