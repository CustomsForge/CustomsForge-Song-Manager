using System;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using CustomsForgeSongManager.DataObjects;
using RocksmithToolkitLib;
using RocksmithToolkitLib.DLCPackage;
using RocksmithToolkitLib.DLCPackage.Manifest2014.Tone;
using RocksmithToolkitLib.Extensions;
using RocksmithToolkitLib.Sng;
using RocksmithToolkitLib.XML;
using Arrangement = RocksmithToolkitLib.DLCPackage.Arrangement;
using RocksmithToolkitLib.PSARC;
using CustomsForgeSongManager.LocalTools;

namespace CustomsForgeSongManager.SongEditor
{
    public partial class frmSongEditor : Form
    {
        private DLCPackageData packageData;
        private string filePath;

        private List<DLCPackageEditorControlBase> editorControls = new List<DLCPackageEditorControlBase>();

        public frmSongEditor()
        {
            throw new Exception("Improper Usage");
        }

        public frmSongEditor(string songPath)
        {
            try
            {
                if (String.IsNullOrEmpty(songPath))
                    return;

                Globals.Log("Loading song information from: " + Path.GetFileName(songPath));
                Cursor.Current = Cursors.WaitCursor;
                Globals.TsProgressBar_Main.Value = 10;
                Globals.TsProgressBar_Main.Value = 20;

                InitializeComponent();
                this.Icon = Properties.Resources.cfsm_48x48;
                tslExit.Alignment = ToolStripItemAlignment.Right;

                var psarc = new PsarcPackager();
                packageData = psarc.ReadPackage(songPath);

                filePath = songPath;
                Globals.TsProgressBar_Main.Value = 80;
                LoadSongInfo();
                Globals.TsProgressBar_Main.Value = 100;
                Cursor.Current = Cursors.Default;
                Globals.Log("Song information loaded ... ");

            }
            catch (InvalidDataException ex)
            {
                Globals.Log("Unable to edit: " + Path.GetFileName(songPath) + " !");
                Globals.Log("Error: " + ex.Message.ToString().Replace(Environment.NewLine, " - "));

                Load += (s, e) => Close();

                return;
            }
        }

        private bool Dirty
        {
            get { return editorControls.Where(x => x.Dirty).Count() > 0; }
        }

        private void Save(string destPath, string srcPath)
        {
            // force validation of user controls
            this.ValidateChildren();
            // halt on any error
            if (editorControls.Where(ec => ec.HaltOnError).Any())
                return;

            Globals.Log("Saving song information to: " + Path.GetFileName(destPath));
            Cursor.Current = Cursors.WaitCursor;
            tsProgressBar.Value = 30;
            tsMsg.Text = "Working ...";
            statusStripMain.Refresh();

            try
            {
                // update SongData
                editorControls.ForEach(ec => { if (ec.Dirty) ec.Save(); });

                //Generate metronome arrangemnts here
                var mArr = new List<Arrangement>();
                foreach (var arr in packageData.Arrangements)
                    if (arr.Metronome == Metronome.Generate)
                        mArr.Add(GenMetronomeArr(arr));

                packageData.Arrangements.AddRange(mArr);
                packageData.DefaultShowlights = true;

                var msg = "The song information has been changed." + Environment.NewLine +
                    "Do you want to update the 'Persistent ID'?" + Environment.NewLine +
                    "Answering 'Yes' will reduce the risk of CDLC" + Environment.NewLine +
                    "in game hanging and song stats will be reset.  ";
                //only ask if it's a new filename.
                bool updateArrangementID = (destPath != filePath) ? MessageBox.Show(msg, "Song Editor ...", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes : false;
                this.Refresh();

                // Update Xml arrangements song info
                foreach (var arr in packageData.Arrangements)
                {
                    // preserve existing xml comments
                    if (arr.ArrangementType == ArrangementType.Guitar || arr.ArrangementType == ArrangementType.Bass)
                        arr.XmlComments = Song2014.ReadXmlComments(arr.SongXml.File);

                    UpdateXml(arr, packageData, updateArrangementID);

                    if (arr.ArrangementType == ArrangementType.Guitar || arr.ArrangementType == ArrangementType.Bass)
                    {
                        var isCommented = false;
                        var cfsmComment = String.Format("Modified with CFSM v{0} SongEditor", Constants.CustomVersion());
                        var commentNodes = arr.XmlComments as List<XComment> ?? arr.XmlComments.ToList();

                        foreach (var commentNode in commentNodes)
                        {
                            if (commentNode.ToString().Contains(cfsmComment))
                                isCommented = true;
                        }

                        if (!isCommented)
                            Song2014.WriteXmlComments(arr.SongXml.File, commentNodes, customComment: cfsmComment);
                    }
                }

                tsProgressBar.Value = 60;

                using (var psarc = new PsarcPackager(true))
                    psarc.WritePackage(destPath, packageData, srcPath);

                // hack for replacing images
                var x = editorControls.Where(e => e.NeedsAfterSave());
                if (x.Count() > 0)
                {
                    if (!destPath.ToLower().EndsWith(Constants.EnabledExtension))
                        destPath += Constants.EnabledExtension;

                    tsProgressBar.Value = 80;
                    var p = new PSARC();
                    using (var fs = File.OpenRead(destPath))
                        p.Read(fs);
                    bool needsSave = false;
                    foreach (var ec in x)
                    {
                        if (ec.AfterSave(p))
                            needsSave = true;
                    }
                    if (needsSave)
                    {
                        using (var fs = File.Create(destPath))
                            p.Write(fs, true);
                    }
                }

                Globals.Log("Song information saved ... ");
                tsProgressBar.Value = 100;
                tsMsg.Text = "Done ...";
            }
            finally
            {
                statusStripMain.Refresh();
                Cursor.Current = Cursors.Default;
                 // force reload of CDLC
                Globals.ReloadSongManager = true;
                Globals.ReloadArrangements = true;
                Globals.ReloadDuplicates = true;
                Globals.ReloadRenamer = true;
                Globals.ReloadSetlistManager = true;
                this.Close(); // need to prevent re-editing error
            }
        }

        private void tslSave_Click(object sender, EventArgs e)
        {
            Save(filePath, filePath);
        }

        private void tslSaveAs_Click(object sender, EventArgs e)
        {
            // force validation of user controls
            this.ValidateChildren();

            // update packageData needed for creating fileName
            editorControls.ForEach(ec => { if (ec.Dirty) ec.Save(); });

            using (var sfd = new SaveFileDialog())
            {
                var fileNameprefix = StringExtensions.GetValidShortFileName(packageData.SongInfo.ArtistSort, packageData.SongInfo.SongDisplayNameSort, "v" + packageData.ToolkitInfo.PackageVersion.Replace(".", "_"), false);
                var fileName = String.Format("{0}{1}", fileNameprefix, Constants.EnabledExtension);

                sfd.FileName = fileName;
                sfd.InitialDirectory = Path.GetDirectoryName(filePath);

                if (sfd.ShowDialog() == DialogResult.OK)
                    Save(sfd.FileName, filePath);
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
                    obj.SongData = packageData;
                    editorControls.Add(obj);
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

        public T GetEditorControl<T>() where T : DLCPackageEditorControlBase
        {
            return (T)editorControls.FirstOrDefault(x => x.GetType() == typeof(T));
        }

        private void LoadSongInfo()
        {
            if (GetEditorControl<ucSongInfo>() == null)
            {
                LoadEditorControl<ucSongInfo>(this.tpSongInfo).Dock = DockStyle.None;
                tpSongInfo_Resize(tpSongInfo, EventArgs.Empty);
            }
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
            if (GetEditorControl<ucArrangements>() == null)
                LoadEditorControl<ucArrangements>(this.tpArrangements);
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

        public void UpdateXml(Arrangement arr, DLCPackageData info, bool updateArrangementID = false)
        {
            // Updates the xml with user modified DLCPackageData info
            // generate new Arrangement IDs
            if (updateArrangementID)
            {
                arr.Id = IdGenerator.Guid();
                arr.MasterId = RandomGenerator.NextInt();
            }

            if (arr.ArrangementType == ArrangementType.Vocal)
                return;
            if (arr.ArrangementType == ArrangementType.ShowLight)
                return;

            // D:\Documents and Settings\Administrator\Local Settings\Temp\Three Days Grace_One X_vNA_BLRV_RS2014_Pc\EOF\threedaysgrace_bass.xml
            var songXml = Song2014.LoadFromFile(arr.SongXml.File);
            arr.ClearCache();
            songXml.AlbumName = info.SongInfo.Album;
            songXml.AlbumYear = info.SongInfo.SongYear.ToString();
            songXml.ArtistName = info.SongInfo.Artist;
            songXml.ArtistNameSort = info.SongInfo.ArtistSort;
            songXml.AverageTempo = info.SongInfo.AverageTempo;
            songXml.Title = info.SongInfo.SongDisplayName;
            songXml.Tuning = arr.TuningStrings;
            if (!String.IsNullOrEmpty(arr.ToneBase)) songXml.ToneBase = arr.ToneBase;
            if (!String.IsNullOrEmpty(arr.ToneA)) songXml.ToneA = arr.ToneA;
            if (!String.IsNullOrEmpty(arr.ToneB)) songXml.ToneB = arr.ToneB;
            if (!String.IsNullOrEmpty(arr.ToneC)) songXml.ToneC = arr.ToneC;
            if (!String.IsNullOrEmpty(arr.ToneD)) songXml.ToneD = arr.ToneD;

            if (GetEditorControl<ucSongInfo>().ApplyBassFix() && arr.ArrangementType == ArrangementType.Bass)
            {
                if (arr.TuningStrings == null)
                {
                    // need to load tuning here from the xml arrangement
                    arr.TuningStrings = new TuningStrings();
                    arr.TuningStrings = songXml.Tuning;
                }

                if (!TuningFrequency.ApplyBassFix(arr))
                {
                    MessageBox.Show(new Form { TopMost = true }, "This bass arrangement is already at 220Hz pitch.  ", Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Refresh(); // required to refresh screen
                }
                else
                {
                    var commentsList = arr.XmlComments.ToList();
                    commentsList.Add(new XComment("Low Bass Tuning Fixed"));
                    arr.XmlComments = commentsList;
                }

                songXml.Tuning = arr.TuningStrings;
            }

            using (var stream = File.Open(arr.SongXml.File, FileMode.Create))
                songXml.Serialize(stream);
        }

        public Arrangement GenMetronomeArr(Arrangement arr)
        {
            var mArr = GeneralExtension.Copy<Arrangement>(arr);
            var songXml = Song2014.LoadFromFile(mArr.SongXml.File);
            var newXml = Path.GetTempFileName();
            mArr.SongXml = new RocksmithToolkitLib.DLCPackage.AggregateGraph.SongXML { File = newXml };
            mArr.SongFile = new RocksmithToolkitLib.DLCPackage.AggregateGraph.SongFile { File = "" };
            mArr.ClearCache();
            mArr.BonusArr = true;
            mArr.Id = IdGenerator.Guid();
            mArr.MasterId = RandomGenerator.NextInt();
            mArr.Metronome = Metronome.Itself;
            songXml.ArrangementProperties.Metronome = (int)Metronome.Itself;

            var ebeats = songXml.Ebeats;
            var songEvents = new RocksmithToolkitLib.XML.SongEvent[ebeats.Length];
            for (var i = 0; i < ebeats.Length; i++)
            {
                songEvents[i] = new RocksmithToolkitLib.XML.SongEvent { Code = ebeats[i].Measure == -1 ? "B1" : "B0", Time = ebeats[i].Time };
            }
            songXml.Events = songXml.Events.Union(songEvents, new EqSEvent()).OrderBy(x => x.Time).ToArray();
            using (var stream = File.OpenWrite(mArr.SongXml.File))
            {
                songXml.Serialize(stream);
            }
            return mArr;
        }

        private class EqSEvent : IEqualityComparer<RocksmithToolkitLib.XML.SongEvent>
        {
            public bool Equals(RocksmithToolkitLib.XML.SongEvent x, RocksmithToolkitLib.XML.SongEvent y)
            {
                if (x == null)
                    return y == null;

                return x.Code == y.Code && x.Time.Equals(y.Time);
            }

            public int GetHashCode(RocksmithToolkitLib.XML.SongEvent obj)
            {
                if (ReferenceEquals(obj, null))
                    return 0;
                return obj.Code.GetHashCode() | obj.Time.GetHashCode();
            }
        }

        private void tpSongInfo_Resize(object sender, EventArgs e)
        {
            var uSongInfo = GetEditorControl<ucSongInfo>();
            if (uSongInfo != null)
            {
                var p = new System.Drawing.Point() { X = (tpSongInfo.Width - uSongInfo.Width) / 2, Y = 3 /* (tpSongInfo.Height - uSongInfo.Height) / 2;*/};

                if (p.X < 3)
                    p.X = 3;

                //if (p.Y < 3)
                //    p.Y = 3;
                uSongInfo.Location = p;
            }
        }
    }
}