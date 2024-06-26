﻿using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CustomsForgeSongManager.DataObjects;
using DF.WinForms.ThemeLib;
using RocksmithToolkitLib;
using RocksmithToolkitLib.Extensions;
using RocksmithToolkitLib.XmlRepository;
using RocksmithToolkitLib.PSARC;

namespace CustomsForgeSongManager.Forms
{
    public partial class frmModAppId : ThemedForm
    {
        private SongData[] DataFiles;
        private bool isCancelled;

        protected frmModAppId()
        {
            InitializeComponent();
            this.Icon = Properties.Resources.cfsm_48x48;

            cmbAppId.Items.Clear();

            foreach (var song in SongAppIdRepository.Instance().Select(GameVersion.RS2014))
                cmbAppId.Items.Add(song);

            var songAppId = SongAppIdRepository.Instance().Select(ConfigRepository.Instance()["general_defaultappid_RS2014"], GameVersion.RS2014);
            cmbAppId.SelectedItem = songAppId;
        }

        public static bool BatchEdit(SongData[] dataFiles)
        {
            if (dataFiles.Any())
            {
                using (var f = new frmModAppId())
                {
                    f.DataFiles = dataFiles;
                    // commented out to encourage use of default AppId for Cherub Rock
                    // f.cmbAppId.SelectedItem = SongAppIdRepository.Instance().Select(dataFiles[0].AppID, GameVersion.RS2014);
                    return f.ShowDialog() == DialogResult.OK;
                }
            }

            return false;
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            lblMsg.Visible = true;
            this.Refresh();
            themedProgressBar1.Maximum = DataFiles.Length;
            themedProgressBar1.Value = 0;
            var newID = txtAppId.Text.Trim().GetValidAppIdSixDigits();

            // social engineering code
            if (newID.Equals("221680"))
                throw new InvalidDataException("<WARNING> Sentinel has detected futile human activity ..." + Environment.NewLine +
                    "Buy Cherub Rock and you wont have to mess around changing AppId's.");

            foreach (var song in DataFiles)
            {
                if (song.IsODLC)
                    continue;

                NoCloseStream dataStream = null;
                using (PSARC p = new PSARC(true))
                {
                    using (var fs = File.OpenRead(song.FilePath))
                        p.Read(fs);

                    dataStream = p.ReplaceData(x => x.Name.Equals("appid.appid"), newID);

                    using (var fs = File.Create(song.FilePath))
                        p.Write(fs, true);
                }

                if (dataStream != null)
                    dataStream.CloseEx();

                song.AppID = txtAppId.Text;
                song.UpdateFileInfo();
                themedProgressBar1.Value++;
                Application.DoEvents();

                if (isCancelled)
                    break;
            }

            lblMsg.Visible = false;
            DialogResult = DialogResult.OK;

            this.Close();
        }

        private void cmbAppId_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cmbAppId.SelectedItem != null)
            {
                txtAppId.Text = ((SongAppId)cmbAppId.SelectedItem).AppId;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            isCancelled = true;
        }
    }

    public class BatchEditor
    {
        public virtual bool Edit(PSARC archive)
        {
            return false;
        }
    }
}