using System;
using System.Windows.Forms;
using CustomsForgeManager.CustomsForgeManagerLib;
using CustomsForgeManager.CustomsForgeManagerLib.Objects;
using RocksmithToolkitLib.DLCPackage;
using System.Collections.Generic;
using System.Linq;
using RocksmithToolkitLib.DLCPackage.Manifest2014;
using System.IO;

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

            InitializeComponent();

            // start marquee Pbar on frmMain 
            Cursor.Current = Cursors.WaitCursor;
            using (var psarc = new PsarcPackage())
                info = psarc.ReadPackage(songPath);
            
            info.Showlights = true;

            Cursor.Current = Cursors.Default;
            filePath = songPath;

            // stop marquee Pbar on frmMain

            LoadSongInfo();
        }

        private bool Dirty
        {
            get
            {
                return FEditorControls.Where(x => x.Dirty).Count() > 0;
            }
        }

        List<Tuple<Attributes2014, string>> GetAttributeFiles()
        {
            var files = Directory.GetFiles(System.IO.Path.GetDirectoryName(info.AlbumArtPath), "*.json");
            var result = new List<Tuple<Attributes2014, string>>();
            foreach (var file in files)
            {
                Attributes2014 attributes = Manifest2014<Attributes2014>.LoadFromFile(file).Entries.ToArray<KeyValuePair<string, Dictionary<string, Attributes2014>>>()[0].Value.ToArray<KeyValuePair<string, Attributes2014>>()[0].Value;
                if (attributes.Tones != null && attributes.Tones.Count > 0)
                    result.Add(new Tuple<Attributes2014, string>(attributes, file));
            }
            return result;
        }


        private void Save(string outputPath)
        {
            if (!Dirty)
                return;

            Cursor.Current = Cursors.WaitCursor;
            try
            {

                var attrEditors = FEditorControls.Where(x => x is IAttributesEditor && x.Dirty).Select(x => x as IAttributesEditor).ToList();
                if (attrEditors.Count() > 0)
                {
                    var jsonSettings = new Newtonsoft.Json.JsonSerializerSettings()
                    {
                        NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore,
                        Formatting = Newtonsoft.Json.Formatting.Indented
                    };

                    var al = GetAttributeFiles();
                    foreach (var item in al)
                    {
                        var attr = item.Item1;
                        attrEditors.ForEach(ae => ae.EditSongAttributes(attr));

                        Manifest2014<Attributes2014> p = new Manifest2014<Attributes2014>();
                        var dic = new Dictionary<string, Attributes2014>();
                        dic.Add("Attributes", attr);
                        p.Entries.Add(attr.PersistentID.ToUpper(), dic);
                        var data = Newtonsoft.Json.JsonConvert.SerializeObject(p, jsonSettings);
                        File.WriteAllText(item.Item2, data);
                    }
                }

                FEditorControls.ForEach(ec => ec.Save());

                using (var psarc = new PsarcPackage(true))
                    psarc.WritePackage(outputPath, info);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
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
                var type = typeof(T);
                var cc = type.GetConstructor(new Type[] { });
                if (cc != null)
                {
                    T obj = (T)cc.Invoke(new object[] { });
                    page.Controls.Clear();
                    page.Controls.Add(obj);
                    obj.Dock = DockStyle.Fill;
                    obj.FilePath = filePath;
                    obj.SongData = info;
                    obj.DoInit();
                    FEditorControls.Add(obj);
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
            }
     

         }

        private void frmSongEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Dirty)
            {
                if (MessageBox.Show(String.Format(Properties.Resources.SongDataModifiedConfirmation,
                    Environment.NewLine), "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
                    e.Cancel = true;
            }
        }
    }
}
