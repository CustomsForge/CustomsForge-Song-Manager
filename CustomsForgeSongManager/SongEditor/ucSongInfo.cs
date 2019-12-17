using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using CustomsForgeSongManager.DataObjects;
using RocksmithToolkitLib;
using RocksmithToolkitLib.Extensions;
using RocksmithToolkitLib.Sng;
using RocksmithToolkitLib.XmlRepository;
using System.Text.RegularExpressions;

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
            // validate on-load to address some old CDLC issues
            txtKey.Text = SongData.Name.GetValidKey();
            txtArtist.Text = SongData.SongInfo.Artist.GetValidAtaSpaceName();
            txtArtistSort.Text = SongData.SongInfo.ArtistSort.GetValidSortableName();
            txtTitle.Text = SongData.SongInfo.SongDisplayName.GetValidAtaSpaceName();
            txtTitleSort.Text = SongData.SongInfo.SongDisplayNameSort.GetValidSortableName();
            txtAlbum.Text = SongData.SongInfo.Album.GetValidAtaSpaceName();
            txtAlbumSort.Text = SongData.SongInfo.AlbumSort.GetValidSortableName();
            txtAppId.Text = SongData.AppId.GetValidAppIdSixDigits();
            txtVersion.Text = SongData.ToolkitInfo.PackageVersion.GetValidVersion();
            txtYear.Text = SongData.SongInfo.SongYear.ToString().GetValidYear();
            txtAvgTempo.Text = SongData.SongInfo.AverageTempo.ToString().GetValidTempo();
            cmbSongVolume.Value = Convert.ToDecimal(SongData.Volume);
            cmbPreviewVolume.Value = Convert.ToDecimal(SongData.PreviewVolume);
            txtCharter.Text = SongData.ToolkitInfo.PackageAuthor;
            txtNote.Text = ExtractUserNote(SongData.ToolkitInfo.PackageComment);
            cbLowBass.Visible = SongData.Arrangements.Any(x => x.ArrangementType == ArrangementType.Bass);

            //set up events
            PopulateAppIdCombo(SongData.AppId, GameVersion.RS2014);

            // create validation event handlers
            txtKey.Validating += ValidateKey;
            txtArtist.Validating += ValidateName;
            txtArtistSort.Validating += ValidateSort;
            txtTitle.Validating += ValidateName;
            txtTitleSort.Validating += ValidateSort;
            txtAlbum.Validating += ValidateName;
            txtAlbumSort.Validating += ValidateSort;
            txtAppId.Validating += ValidateAppId;
            txtVersion.Validating += ValidateVersion;
            txtYear.Validating += ValidateYear;
            txtAvgTempo.Validating += ValidateTempo;
            cmbSongVolume.Validating += ValidateText;
            cmbPreviewVolume.Validating += ValidateText;
            txtCharter.Validating += ValidateName;
            txtNote.Validating += ValidateText;
            cbLowBass.Validating += ValidateText;
        }

        private void ValidateKey(object sender, CancelEventArgs e)
        {
            var tb = sender as TextBox;
            if (tb != null)
            {
                tb.Text = tb.Text.Trim().GetValidKey(txtTitle.Text);
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
                tb.Text = tb.Text.Trim().GetValidSortableName();
                this.Dirty = true;
            }
        }

        private void ValidateName(object sender, CancelEventArgs e)
        {
            var tb = sender as TextBox;
            if (tb != null)
            {
                tb.Text = tb.Text.Trim().GetValidAtaSpaceName();
                this.Dirty = true;

                //if (tb.Name == "txtTitle")
                //    txtKey.Text = String.Format("{0} {1}", txtArtist.Text.Acronym(), txtTitle.Text.GetValidSortableName());
            }
        }

        private void ValidateAppId(object sender, CancelEventArgs e)
        {
            var tb = sender as TextBox;
            if (tb != null)
            {
                tb.Text = tb.Text.Trim().GetValidAppIdSixDigits();
                this.Dirty = true;
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

        private void ValidateYear(object sender, CancelEventArgs e)
        {
            var tb = sender as TextBox;
            if (tb != null)
            {
                tb.Text = tb.Text.Trim().GetValidYear();
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
            if (!Dirty)
                return;

            SongData.Name = txtKey.Text;
            SongData.AppId = txtAppId.Text;
            SongData.Volume = Convert.ToSingle(cmbSongVolume.Value);
            SongData.PreviewVolume = Convert.ToSingle(cmbPreviewVolume.Value);
            //
            SongData.SongInfo.Artist = txtArtist.Text;
            SongData.SongInfo.ArtistSort = txtArtistSort.Text;
            SongData.SongInfo.SongDisplayName = txtTitle.Text;
            SongData.SongInfo.SongDisplayNameSort = txtTitleSort.Text;
            SongData.SongInfo.Album = txtAlbum.Text;
            SongData.SongInfo.AlbumSort = txtAlbumSort.Text;
            SongData.SongInfo.SongYear = Convert.ToInt32(txtYear.Text);
            SongData.SongInfo.AverageTempo = Convert.ToInt32(txtAvgTempo.Text);
            // many CDLC authors do not enter their charter name so permit user to edit the name
            SongData.ToolkitInfo.PackageVersion = txtVersion.Text;
            SongData.ToolkitInfo.PackageAuthor = txtCharter.Text;
            SongData.ToolkitInfo.PackageComment = CreatePackageComment(SongData.ToolkitInfo.PackageComment, txtNote.Text);

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

        /// <summary>
        /// given a full toolkit PackageComment string with user's Note
        /// <para>return just the user note portion [xxxx]</para>
        /// </summary>
        /// <param name="tkPackageComment"></param>
        private string ExtractUserNote(string tkPackageComment)
        {
            var packageNote = String.Empty;
            if (String.IsNullOrEmpty(tkPackageComment))
                return packageNote;

            var bonIndex = tkPackageComment.IndexOf("["); // BeginingOfNote
            var eonIndex = tkPackageComment.IndexOf("]"); // EndOfNote
            if (bonIndex < 0 || eonIndex < 0)
                return packageNote;

            packageNote = tkPackageComment.Substring(bonIndex + 1, eonIndex - 1);
            return packageNote;
        }

        /// <summary>
        /// given a full toolkit PackageComment string including any notes [xxxx]
        /// <para>return only the original comment portion</para>
        /// </summary>
        /// <param name="tkPackageComment"></param>
        private string ExtractCommentOnly(string tkPackageComment)
        {
            if (String.IsNullOrEmpty(ExtractUserNote(tkPackageComment)))
                return tkPackageComment;

            var packageNote = String.Format("[{0}]", ExtractUserNote(tkPackageComment));
            var packageComment = tkPackageComment.Replace(packageNote, "").Trim();
            return packageComment;
        }

        /// <summary>
        /// given an existing toolkit PackageComment string
        /// <para>inject the new user's note [xxxx]</para>
        /// <para>overwrites any existing note</para>
        /// </summary>
        /// <param name="srcText"></param>
        private string CreatePackageComment(string tkPackageComment, string newCustomNote)
        {
            if (String.IsNullOrEmpty(newCustomNote))
                return ExtractCommentOnly(tkPackageComment);

            // remove toolkit reserved characters ':[]' from the user's note that sneaked through
            const string pattern = @"[\:\]\[]";
            newCustomNote = Regex.Replace(newCustomNote, pattern, "", RegexOptions.IgnoreCase);
            var commentOnly = ExtractCommentOnly(tkPackageComment);
            var commentNote = String.Format("[{0}] {1}", newCustomNote, commentOnly);

            return commentNote;
        }

        private void txtNote_TextChanged(object sender, EventArgs e)
        {
            // get current note and cursor postion
            var text = txtNote.Text;
            var position = txtNote.SelectionStart;
            // remove reserved characters ':[]' from text
            const string pattern = @"[\:\]\[]";
            text = Regex.Replace(text, pattern, "", RegexOptions.IgnoreCase);
            if (text != txtNote.Text)
            {
                txtNote.Text = text;
                txtNote.SelectionStart = position - 1;
                Globals.Log(" - <WARNING> System reserved charaters ':[]' may not be used in user note ...");
            }
        }

    }
}