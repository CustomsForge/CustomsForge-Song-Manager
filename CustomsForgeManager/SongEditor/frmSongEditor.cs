using System;
using System.ComponentModel;
using System.Windows.Forms;
using CustomsForgeManager.CustomsForgeManagerLib;
using CustomsForgeManager.CustomsForgeManagerLib.Objects;
using RocksmithToolkitLib.DLCPackage;
using System.Collections.Generic;
using System.Linq;

namespace CustomsForgeManager.SongEditor
{
    public partial class frmSongEditor : Form
    {
        private DLCPackageData info;
        private string filePath;

        private List<DLCPackageEditorControlBase> FEditorControls = new List<DLCPackageEditorControlBase>();

        // TODO: consider revamp frmMain code to be like this
        public frmSongEditor(string songPath)
        {
            if (String.IsNullOrEmpty(songPath))
                return;

            Cursor.Current = Cursors.WaitCursor;
            Globals.TsProgressBar_Main.Value = 10;
            InitializeComponent();
            Globals.TsProgressBar_Main.Value = 20;
            var psarc = new PsarcPackage();
            info = psarc.ReadPackage(songPath);
            filePath = songPath;
            Globals.TsProgressBar_Main.Value = 60;
            LoadSongInfo();
            Globals.TsProgressBar_Main.Value = 100;
            Cursor.Current = Cursors.Default;
        }

        private bool Dirty
        {
            get { return FEditorControls.Where(x => x.Dirty).Count() > 0; }
        }


        private void Save(string outputPath)
        {
            if (!Dirty)
                return;

            Cursor.Current = Cursors.WaitCursor;
            tsProgressBar.Value = 30;

            try
            {
                FEditorControls.ForEach(ec => { if (ec.Dirty) ec.Save(); });

                // required but may not have been included in SongInfo
                //info.Showlights = true;

                using (var psarc = new PsarcPackage(true))
                    psarc.WritePackage(outputPath, info);
            }
            finally
            {
                tsProgressBar.Value = 100;
                Cursor.Current = Cursors.Default;
                Globals.RescanSongManager = true;
            }

        }

        private void tslSave_Click(object sender, EventArgs e)
        {
            Save(filePath);
        }

        private void tslSaveAs_Click(object sender, EventArgs e)
        {
            using (var sd = new SaveFileDialog())
            {
                sd.InitialDirectory = System.IO.Path.GetDirectoryName(filePath);
                if (sd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    Save(sd.FileName);
            }
        }

        private void tslExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private T LoadEditorControl<T>(TabPage page) where T : DLCPackageEditorControlBase
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                T obj = null;
                var type = typeof(T);
                var cc = type.GetConstructor(new Type[] { typeof(frmSongEditor) });
                if (cc != null)
                    obj = (T)cc.Invoke(new object[] { this });
                else
                {
                    cc = type.GetConstructor(new Type[] { });
                    if (cc != null)
                        obj = (T)cc.Invoke(new object[] { });
                }

                if (obj != null)
                {
                    page.Controls.Clear();
                    page.Controls.Add(obj);
                    page.Tag = obj;
                    obj.Dock = DockStyle.Fill;
                    obj.FilePath = filePath;
                    obj.SongData = info;
                    FEditorControls.Add(obj);
                    obj.DoInit();
                    return obj;
                }
                return null;
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }


        private T GetEditorControl<T>() where T : DLCPackageEditorControlBase
        {
            return (T)FEditorControls.Where(x => x.GetType() == typeof(T)).FirstOrDefault();
        }

        private void LoadSongInfo()
        {
            if (GetEditorControl<ucSongInfo>() == null)
                LoadEditorControl<ucSongInfo>(this.tpSongInfo);
        }

        private void LoadTones()
        {
            if (GetEditorControl<ucTones>() == null)
                LoadEditorControl<ucTones>(this.tpTones);
        }

        private void LoadAlbumArt()
        {
            if (GetEditorControl<ucAlbumArt>() == null)
                LoadEditorControl<ucAlbumArt>(this.tpAlbumArt);
        }

        private void LoadArrangements()
        {
            if (GetEditorControl<ucArrangments>() == null)
                LoadEditorControl<ucArrangments>(this.tpArrangements);
        }

        private void tcMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Docking.Fill causes screen flicker so only use if needed
            // reset toolstrip labels
            Globals.ResetToolStripGlobals();

            // get first four charters from tab control text
            switch (tcMain.SelectedTab.Text.Substring(0, 4).ToUpper())
            {
                // passing variables(objects) by value to UControl
                case "SONG":
                    LoadSongInfo();
                    break;
                case "TONE":
                    LoadTones();
                    break;
                case "ALBU":
                    LoadAlbumArt();
                    break;
                case "ARRA":
                    LoadArrangements();
                    break;

            }
        }

        private void frmSongEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Dirty)
            {
                if (MessageBox.Show(String.Format(Properties.Resources.SongDataModifiedConfirmation, Environment.NewLine), "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
                    e.Cancel = true;
            }
        }
    }


    public class NumericUpDownFixed : NumericUpDown
    {
        protected override void OnValidating(CancelEventArgs e)
        {
            // Prevent bug where typing a value bypasses min/max validation
            var fixValidation = Value;
            base.OnValidating(e);
        }
    }


}

