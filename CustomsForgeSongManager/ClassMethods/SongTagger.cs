using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using CFSM.GenTools;
using CFSM.ImageTools;
using CFSM.NCalc;
using CustomsForgeSongManager.DataObjects;


namespace CustomsForgeSongManager.ClassMethods
{
    public class SongTagger
    {
        public static void CFSM_INIT()
        {
            Globals.Tagger = new SongTagger();
        }

        private string[] defaultTagFolders = {"frackDefault", "motive_bl_", "motive_nv_", "motive_ws_", "motive1"};

        public List<String> Themes { get; private set; }
        private List<XmlThemeStorage> XmlThemes { get; set; }

        private class XmlThemeStorage
        {
            public XmlThemeStorage(string name, string folder)
            {
                this.Name = name;
                this.Folder = folder;
            }

            public string Name;
            public string Folder;
        }

        public SongTagger()
        {
            ThemeName = defaultTagFolders[0];
            Themes = new List<string>();
            XmlThemes = new List<XmlThemeStorage>();
            Populate();
        }

        public event EventHandler<TaggerProgress> OnProgress;

        private void Populate()
        {
            if (!Directory.Exists(Constants.TaggerTemplatesFolder) || GenExtensions.IsDirectoryEmpty(Constants.TaggerTemplatesFolder) || !File.Exists(Path.Combine(Constants.TaggerTemplatesFolder, "Default.tagtheme")))
                CreateDefaultFolders();

            if (!File.Exists(Path.Combine(Constants.TaggerTemplatesFolder, "User.Default.tagtheme")))
                File.Copy(Path.Combine(Constants.TaggerTemplatesFolder, "Default.tagtheme"), Path.Combine(Constants.TaggerTemplatesFolder, "User.Default.tagtheme"));

            Themes.Clear();
            foreach (string tagPreview in
                Directory.EnumerateFiles(Constants.TaggerTemplatesFolder, "*.png").Where(file => file.ToLower().Contains("prev")))
                Themes.Add(Path.GetFileName(tagPreview).Replace(@"Tagger\", "").Replace("prev.png", ""));

            foreach (string tagPreview in
                Directory.EnumerateFiles(Constants.TaggerTemplatesFolder, "*.tagtheme", SearchOption.AllDirectories).Where(file => !file.ToLower().Contains("default") && !file.ToLower().Contains("example")))
                XmlThemes.Add(new XmlThemeStorage(Path.GetFileNameWithoutExtension(tagPreview), Path.GetDirectoryName(tagPreview)));

            Themes.AddRange(XmlThemes.Select(f => f.Name));
        }

        private void CreateDefaultFolders()
        {
            if (!Directory.Exists(Constants.TaggerWorkingFolder))
                Directory.CreateDirectory(Constants.TaggerWorkingFolder);
           
            // this creates the necessary directories
            GenExtensions.ExtractEmbeddedResources(Constants.TaggerTemplatesFolder, Assembly.GetExecutingAssembly(),"CustomsForgeSongManager.Resources.tags");

            //foreach (string resourceDir in defaultTagFolders)
            //{
            //    string folderPath = Path.Combine(Constants.TaggerTemplatesFolder, resourceDir);

            //    if (!Directory.Exists(folderPath))
            //        Directory.CreateDirectory(folderPath);

            //    UtilExtensions.ExtractEmbeddedResource(folderPath, "CustomsForgeManager.Resources.tags." + resourceDir, defaultFiles);
            //}

            //foreach (string previewFile in defaultTagFolders)
            //    UtilExtensions.ExtractEmbeddedResource(Constants.TaggerTemplatesFolder,
            //        "CustomsForgeManager.Resources.tags", new string[] { previewFile + "prev.png" });
        }

        public string ThemeName { get; set; }

        public Image GetThemePreview(string themeName)
        {
            string preview = Path.Combine(Constants.TaggerTemplatesFolder, themeName, "prev.png");
            if (File.Exists(preview))
                return Bitmap.FromFile(preview);
            return null;
        }

        public string GetThemeText(string themeName)
        {
            string info = Path.Combine(Constants.TaggerTemplatesFolder, themeName, "info.txt");
            if (File.Exists(info))
                return File.ReadAllText(info);
            return string.Empty;
        }

        public Image Preview(SongData sd, string themeName = "")
        {
            if (String.IsNullOrEmpty(themeName))
            {
                if (ThemeName == String.Empty)
                    ThemeName = defaultTagFolders[0];
                themeName = ThemeName;
            }

            string aPath = Path.Combine(Constants.TaggerTemplatesFolder, themeName);

            var xTheme = XmlThemes.Find(x => x.Name == themeName);
            if (xTheme != null)
            {
                aPath = xTheme.Folder;
            }
            try
            {
                using (var images = new BitmapHolder(aPath))
                {
                    string songPath = sd.FilePath;
                    using (CFSM.RSTKLib.PSARC.PSARC archive = new CFSM.RSTKLib.PSARC.PSARC(true))
                    using (var fs = File.OpenRead(songPath))
                    {
                        archive.Read(fs, true);

                        var imgEntry = archive.TOC.FirstOrDefault(entry => entry.Name.EndsWith("256.dds"));

                        if (imgEntry != null)
                        {
                            archive.InflateEntry(imgEntry);
                            if (imgEntry.Data == null)
                            {
                                Globals.Log("Error inflating image entry.");
                                return null;
                            }
                            imgEntry.Data.Position = 0;
                            var albumArtDDS = new DDSImage(imgEntry.Data);
                            var AlbumArt = albumArtDDS.images[0];

                            TagDrawImage(sd, images, xTheme, archive, AlbumArt, aPath);

                            return AlbumArt;
                        }
                    }
                }
            }
            catch (ArgumentException)
            {
                ShowTaggerError();
            }

            return null;
        }

        private void ShowTaggerError()
        {
            MessageBox.Show(string.Format("Make sure that you have all required files in the CFM\tags folder: \n" + "-Tagger/templates/{0}/Background.png \n" + "-Tagger/templates/{0}/Lead.png \n" + "-Tagger/templates/{0}/Lead Bonus.png \n" + "-Tagger/templates/{0}/Rhythm.png \n" + "-Tagger/templates/{0}/Rhythm Bonus.png \n" + "-Tagger/templates/{0}/Custom.png \n" + "-Tagger/templates/{0}/Vocal.png", ThemeName));
        }

        private string tagsFolderFullPath
        {
            get
            {
                var xTheme = XmlThemes.Find(x => x.Name == ThemeName);
                if (xTheme != null)
                    return xTheme.Folder;
                return Path.Combine(Constants.TaggerTemplatesFolder, ThemeName);
            }
        }

        private void TagDrawImage(SongData song, BitmapHolder images, XmlThemeStorage xTheme, CFSM.RSTKLib.PSARC.PSARC archive, Bitmap bigAlbumArt, String FolderFullPath)
        {
            bool lead = false;
            bool rhythm = false;
            bool bass = false;
            bool vocals = false;
            bool bonusLead = false;
            bool bonusRhythm = false;
            bool bonusBass = false;
            bool DD = song.DD > 0;

            var arrangements = archive.TOC.Where(entry => entry.Name.ToLower().EndsWith(".json")).Select(entry => entry.Name);

            foreach (string arrangement in arrangements)
            {
                if (arrangement.Contains("lead") && !arrangement.Contains("lead2"))
                    lead = true;
                if (arrangement.Contains("lead2"))
                    bonusLead = true;
                if (arrangement.Contains("rhythm") && !arrangement.Contains("rhythm2"))
                    rhythm = true;
                if (arrangement.Contains("rhythm2"))
                    bonusRhythm = true;
                if (arrangement.Contains("bass") && !arrangement.Contains("bass2"))
                    bass = true;
                if (arrangement.Contains("bass2"))
                    bonusBass = true;
                if (arrangement.Contains("vocals"))
                    vocals = true;
                if (arrangement.Contains("combo"))
                {
                    lead = true;
                    rhythm = true;
                }
            }

            SongTaggerTheme tt = null;
            var ttpath = tagsFolderFullPath;
            if (xTheme != null)
            {
                using (FileStream fs1 = File.OpenRead(Path.Combine(xTheme.Folder, xTheme.Name + ".tagtheme")))
                    tt = SongTaggerTheme.Create(fs1);
            }
            else if (File.Exists(Path.Combine(FolderFullPath, "Default.tagtheme")))
            {
                using (FileStream fs1 = File.OpenRead(Path.Combine(FolderFullPath, "Default.tagtheme")))
                    tt = SongTaggerTheme.Create(fs1);
            }
            else if (File.Exists(Path.Combine(Constants.TaggerTemplatesFolder, "User.Default.tagtheme")))
            {
                using (FileStream fs1 = File.OpenRead(Path.Combine(Constants.TaggerTemplatesFolder, "User.Default.tagtheme")))
                    tt = SongTaggerTheme.Create(fs1);
            }
            else if (File.Exists(Path.Combine(Constants.TaggerTemplatesFolder, "Default.tagtheme")))
            {
                using (FileStream fs1 = File.OpenRead(Path.Combine(Constants.TaggerTemplatesFolder, "Default.tagtheme")))
                    tt = SongTaggerTheme.Create(fs1);
            }
            if (tt != null)
                tt.Data = song;
            GenExtensions.TempChangeDirectory(ttpath, () =>
                {
                    //Add layers to big album art
                    using (Graphics gra = Graphics.FromImage(bigAlbumArt))
                    {
                        if (images.backgroundLayer != null)
                            gra.DrawImage(images.backgroundLayer, 0, 0.5f);
                        if (vocals && images.vocalLayer != null)
                            gra.DrawImage(images.vocalLayer, 0, 0.5f);
                        if (bass && images.bassLayer != null)
                            gra.DrawImage(images.bassLayer, 0, 0.5f);
                        if (bonusBass && images.bassBonusLayer != null)
                            gra.DrawImage(images.bassBonusLayer, 0, 0.5f);
                        if (rhythm && images.rhythmLayer != null)
                            gra.DrawImage(images.rhythmLayer, 0, 0.5f);
                        if (bonusRhythm && images.rhythmBonusLayer != null)
                            gra.DrawImage(images.rhythmBonusLayer, 0, 0.5f);
                        if (lead && images.leadLayer != null)
                            gra.DrawImage(images.leadLayer, 0, 0.5f);
                        if (bonusLead && images.leadBonusLayer != null)
                            gra.DrawImage(images.leadBonusLayer, 0, 0.5f);
                        if (images.customTagLayer != null)
                            gra.DrawImage(images.customTagLayer, 0, 0.5f);
                        if (DD && images.DDLayer != null)
                            gra.DrawImage(images.DDLayer, 0, 0.5f);
                        //Apply the xml theme
                        if (tt != null)
                        {
                            if (DD)
                                tt.DD.Draw(gra, bigAlbumArt);
                            else
                                tt.NDD.Draw(gra, bigAlbumArt);
                            tt.Custom.Draw(gra, bigAlbumArt);
                        }
                    }
                });
        }

        private bool AllowEditingOfODLC
        {
            get { return false; }
        }

        private void TagSong(SongData song, BitmapHolder images)
        {
            if (song.Tagged == SongTaggerStatus.ODLC)
                return;

            if (ThemeName == String.Empty)
                ThemeName = defaultTagFolders[0];
            if (!Directory.Exists(Constants.TaggerTemplatesFolder) || !Directory.Exists(tagsFolderFullPath))
                CreateDefaultFolders();
            var xTheme = XmlThemes.Find(x => x.Name == ThemeName);

            //   songTagged = songTagged || File.GetCreationTime(song.Path) == new DateTime(1990, 1, 1) ? true : false;

            if (File.Exists(song.FilePath)) //Just to be sure :)
            {
                string songPath = song.FilePath;
                using (CFSM.RSTKLib.PSARC.PSARC archive = new CFSM.RSTKLib.PSARC.PSARC())
                {
                    using (var fs = File.OpenRead(songPath))
                        archive.Read(fs);

                    if (song.Tagged == SongTaggerStatus.True)
                    {
                        UntagSong(song, archive);
                        Globals.Log("Retagging song: " + song.Title);
                    }
                    else
                        Globals.Log("Tagging song: " + song.Title);

                    var taggerOriginal = archive.TOC.FirstOrDefault(entry => entry.Name == "tagger.org");
                    if (taggerOriginal != null)
                    {
                        song.Tagged = SongTaggerStatus.True;
                        return;
                    }

                    var toolKitEntry = archive.TOC.FirstOrDefault(entry => entry.Name == "toolkit.version");

                    // CFSM does not tag Official DLC songs
                    if (!AllowEditingOfODLC && toolKitEntry == null)
                    {
                        Globals.Log("CFSM can not be used to tag Official DLC songs.");
                        song.Tagged = SongTaggerStatus.ODLC;
                        return;
                    }

                    var albumSmallArtEntry = archive.TOC.FirstOrDefault(entry => entry.Name.EndsWith("64.dds")); //Get album art paths
                    var albumMidArtEntry = archive.TOC.FirstOrDefault(entry => entry.Name.EndsWith("128.dds"));
                    var albumBigArtEntry = archive.TOC.FirstOrDefault(entry => entry.Name.EndsWith("256.dds"));
                    albumBigArtEntry.Data.Position = 0;

                    //var albumArtDDS = new DDSImage(albumBigArtEntry.Data);
                    var bigAlbumArt = ImageExtensions.DDStoBitmap(albumBigArtEntry.Data); // albumArtDDS.images[0];
                    var orginalArtStream = new MemoryStream();
                    albumBigArtEntry.Data.Position = 0;
                    albumBigArtEntry.Data.CopyTo(orginalArtStream);
                    orginalArtStream.Position = 0;

                    //Check which arrangements it contains
                    TagDrawImage(song, images, xTheme, archive, bigAlbumArt, tagsFolderFullPath);

                    var largeDDS = bigAlbumArt.ToDDS(256, 256);
                    var midDDS = bigAlbumArt.ToDDS(128, 128);
                    var smallDDS = bigAlbumArt.ToDDS(64, 64);

                    if (largeDDS == null || midDDS == null || smallDDS == null)
                        throw new Exception("unable to convert image steams.");

                    albumSmallArtEntry.Data.Dispose();
                    smallDDS.Position = 0;
                    albumSmallArtEntry.Data = smallDDS;

                    albumMidArtEntry.Data.Dispose();
                    midDDS.Position = 0;
                    albumMidArtEntry.Data = midDDS;

                    albumBigArtEntry.Data.Dispose();
                    largeDDS.Position = 0;
                    albumBigArtEntry.Data = largeDDS;

                    //     archive.TOC.Insert(0, new Entry() { Name = "NamesBlock.bin" });
                    archive.AddEntry("tagger.org", orginalArtStream);
                    songPath = song.FilePath;

                    using (var FS = File.Create(songPath))
                        archive.Write(FS, true);
                    song.FilePath = songPath;

                    song.FileDate = File.GetLastWriteTimeUtc(song.FilePath);

                    song.Tagged = SongTaggerStatus.True;

                    //  counter++;
                    //    gbTags.Text = string.Format("Tagged: {0}/{1}", counter, songCount);                            
                }

                Globals.Log("Finished tagging song ...");
            }
        }

        public void TagSong(SongData song)
        {
            using (var images = new BitmapHolder(tagsFolderFullPath))
                TagSong(song, images);
        }

        public void UntagSong(SongData song, CFSM.RSTKLib.PSARC.PSARC archive)
        {
            if (File.Exists(song.FilePath)) //Just to be sure :)
            {
                if (song.Tagged == SongTaggerStatus.True)
                    Globals.Log("Untagging song: " + song.Title);

                var toolKitEntry = archive.TOC.FirstOrDefault(entry => entry.Name == "toolkit.version");
                var taggerOriginal = archive.TOC.FirstOrDefault(entry => entry.Name == "tagger.org");
                if (toolKitEntry != null && taggerOriginal != null)
                {
                    DDSImage orgDDS = new DDSImage(taggerOriginal.Data);
                    var bigAlbumArt = orgDDS.images[0];

                    var albumSmallArtEntry = archive.TOC.FirstOrDefault(entry => entry.Name.EndsWith("64.dds")); //Get album art paths
                    var albumMidArtEntry = archive.TOC.FirstOrDefault(entry => entry.Name.EndsWith("128.dds"));
                    var albumBigArtEntry = archive.TOC.FirstOrDefault(entry => entry.Name.EndsWith("256.dds"));

                    albumBigArtEntry.Data.Dispose();
                    albumBigArtEntry.Data = bigAlbumArt.ToDDS(256, 256);
                    albumMidArtEntry.Data.Dispose();
                    albumMidArtEntry.Data = bigAlbumArt.ToDDS(128, 128);
                    albumSmallArtEntry.Data.Dispose();
                    albumSmallArtEntry.Data = bigAlbumArt.ToDDS(64, 64);

                    archive.TOC.Remove(taggerOriginal);
                }

                song.Tagged = SongTaggerStatus.False;
                Globals.Log("Finished untagging song ...");
            }
        }

        public void UntagSong(SongData song)
        {
            if (song.Tagged == SongTaggerStatus.False || song.Tagged == SongTaggerStatus.ODLC)
                return;

            if (File.Exists(song.FilePath)) //Just to be sure :)
            {
                using (CFSM.RSTKLib.PSARC.PSARC archive = new CFSM.RSTKLib.PSARC.PSARC())
                {
                    using (var fs = File.OpenRead(song.FilePath))
                        archive.Read(fs);

                    UntagSong(song, archive);
                    //   archive.TOC.Insert(0, new Entry() { Name = "NamesBlock.bin" });

                    using (var FS = File.Create(song.FilePath))
                        archive.Write(FS, true);

                    song.FileDate = File.GetLastWriteTimeUtc(song.FilePath);
                }
            }
        }

        public void TagSongs(SongData[] songs)
        {
            if (songs == null || songs.Length == 0)
                return;

            TaggerProgress tp = new TaggerProgress(0, songs.Length);
            using (var images = new BitmapHolder(tagsFolderFullPath))
                for (int i = 0; i < songs.Length; i++)
                {
                    if (OnProgress != null)
                    {
                        tp.SetPos(i);
                        tp.CurrentSong = songs[i];
                        OnProgress(this, tp);
                    }
                    TagSong(songs[i], images);
                }
            if (OnProgress != null)
            {
                tp.SetPos(songs.Length);
                OnProgress(this, tp);
            }
        }

        public void UntagSongs(SongData[] songs)
        {
            if (songs == null || songs.Length == 0)
                return;

            TaggerProgress tp = new TaggerProgress(0, songs.Length);
            for (int i = 0; i < songs.Length; i++)
            {
                if (OnProgress != null)
                {
                    tp.SetPos(i);
                    tp.CurrentSong = songs[i];
                    OnProgress(this, tp);
                }

                UntagSong(songs[i]);
            }
            if (OnProgress != null)
            {
                tp.SetPos(songs.Length);
                OnProgress(this, tp);
            }
        }
    }

    internal static class TagThemeRegistar
    {
        public static Type[] DrawTypes;

        public static void CFSM_INIT()
        {
            var xtypes = TypeExtensions.GetLoadableTypes().Where(type => { return type.IsSubclassOf(typeof (DrawInstruction)) && !type.IsAbstract; });
            DrawTypes = xtypes.ToArray();
        }
    }

    #region Tagger Theme File

    [Serializable, XmlRoot("CFSMTaggerTheme")]
    public class SongTaggerTheme
    {
        private SongData FSongData;
        private List<TaggerDrawer> Drawers;

        public TaggerDrawer DD { get; set; }
        public TaggerDrawer NDD { get; set; }
        public TaggerDrawer Custom { get; set; }

        [XmlAttribute]
        public string Name { get; set; }

        public SongTaggerTheme()
        {
            DD = new TaggerDrawer();
            NDD = new TaggerDrawer();
            Custom = new TaggerDrawer();
            CreateDrawerArray();
            Name = "Default";
        }

        private void CreateDrawerArray()
        {
            Drawers = new List<TaggerDrawer> {DD, NDD, Custom};
        }

        public static SongTaggerTheme Create(Stream AStream)
        {
            var result = AStream.DeserializeXml<SongTaggerTheme>(TagThemeRegistar.DrawTypes);
            result.Loaded();
            return result;
        }

        [XmlIgnore]
        public SongData Data
        {
            get { return FSongData; }
            set
            {
                FSongData = value;
                Drawers.ForEach(d => d.Data = Data);
            }
        }

        public void Loaded()
        {
            CreateDrawerArray();
            Drawers.ForEach(d => d.Loaded());
        }
    }

    public class SerializableObj
    {
        public Rectangle GetRect(string value)
        {
            var x = value.Split(',');
            int left = 0, top = 0, w = 0, h = 0;
            if (x.Count() > 0)
                left = Convert.ToInt32(x[0]);
            if (x.Count() > 1)
                top = Convert.ToInt32(x[1]);
            if (x.Count() > 2)
                w = Convert.ToInt32(x[2]);
            if (x.Count() > 3)
                h = Convert.ToInt32(x[3]);
            return new Rectangle(left, top, w, h);
        }

        public string GetRectString(Rectangle value)
        {
            return String.Format("{0},{1},{2},{3}", value.X, value.Y, value.Width, value.Height);
        }

        public Point GetPoint(string value)
        {
            var x = value.Split(',');
            return new Point(Convert.ToInt32(x[0]), Convert.ToInt32(x[1]));
        }

        public string GetPointString(Point value)
        {
            return String.Format("{0},{1}", value.X, value.Y);
        }
    }

    internal interface IDrawInstructionsHolder
    {
        DrawInstructions GetDrawing();
    }

    [Serializable, XmlRoot("XmlDrawer")]
    public class TaggerDrawer : SerializableObj, IDrawInstructionsHolder
    {
        public static TaggerDrawer Create(Stream AStream)
        {
            var result = AStream.DeserializeXml<TaggerDrawer>(TagThemeRegistar.DrawTypes);
            result.Loaded();
            return result;
        }

        [XmlAttribute("pos")]
        public string pos
        {
            get { return GetPointString(Position); }
            set
            {
                if (!String.IsNullOrEmpty(value))
                    Position = GetPoint(value);
            }
        }

        private SongData FSongData;

        [XmlIgnore]
        public SongData Data
        {
            get { return FSongData; }
            set
            {
                FSongData = value;
                Drawing.Instructions.ForEach(di => di.Data = value);
            }
        }

        [XmlIgnore] public Point Position;
        public DrawInstructions Drawing = new DrawInstructions();

        public void Draw(Graphics g, Bitmap b)
        {
            if (Drawing.Instructions.Count > 0)
            {
                g.TranslateTransform(Position.X, Position.Y);

                Drawing.Draw(g, b);

                g.ResetTransform();
            }
        }

        public void Loaded()
        {
            Drawing.Parent = this;
        }

        public DrawInstructions GetDrawing()
        {
            return Drawing;
        }
    }

    [Serializable]
    public class DrawInstructions : SerializableObj
    {
        public DrawInstructions()
        {
            Size = new Point(-1, -1);
        }

        private TaggerDrawer FParent;

        [XmlIgnore]
        public TaggerDrawer Parent
        {
            get { return FParent; }
            set
            {
                FParent = value;
                Instructions.ForEach(i => i.Instructions = this);
            }
        }

        [XmlAttribute]
        public string size
        {
            get { return GetPointString(Size); }
            set
            {
                if (!String.IsNullOrEmpty(value))
                    Size = GetPoint(value);
            }
        }

        [XmlIgnore]
        public Point Size { get; set; }

        [XmlArray("Instructions")] [XmlArrayItem("Draw")] public List<DrawInstruction> Instructions = new List<DrawInstruction>();

        public virtual void Draw(Graphics g, Bitmap b)
        {
            if (Size.X > -1 && Size.Y > -1)
                g.SetClip(new Rectangle(0, 0, Size.X, Size.Y));
            Instructions.ForEach(d =>
                {
                    if (d.CanDraw())
                    {
                        d.BeforeDraw(g, b);
                        d.Draw(g, b);
                        d.AfterDraw(g, b);
                    }
                });
            if (Size.X > -1 && Size.Y > -1)
                g.ResetClip();
        }
    }

    public enum GradientMode
    {
        Horizontal,
        Vertical,
        ForwardDiagonal,
        BackwardDiagonal,
        None
    }

    [Serializable]
    public abstract class DrawInstruction : SerializableObj
    {
        public DrawInstruction()
        {
            Color = Color.Black;
            Color2 = Color.Black;
            GradientMode = GradientMode.None;
            Condition = String.Empty;
        }

        [XmlIgnore]
        public DrawInstructions Instructions { get; set; }

        #region Xml Friendly Props

        [XmlAttribute("color")]
        public string color
        {
            get { return ColorTranslator.ToHtml(Color); }
            set { Color = ColorTranslator.FromHtml(value); }
        }

        [XmlAttribute("color2")]
        public string color2
        {
            get { return ColorTranslator.ToHtml(Color2); }
            set { Color2 = ColorTranslator.FromHtml(value); }
        }

        [XmlAttribute("rect")]
        public string rect
        {
            get { return GetRectString(Rect); }
            set
            {
                if (!String.IsNullOrEmpty(value))
                    Rect = GetRect(value);
            }
        }

        #endregion

        [XmlIgnore]
        public Color Color { get; set; }

        [XmlIgnore]
        public Color Color2 { get; set; }

        [XmlIgnore]
        public Rectangle Rect { get; set; }

        public string Condition { get; set; }

        [XmlAttribute("gradientMode")]
        public GradientMode GradientMode { get; set; }

        public virtual void Draw(Graphics g, Bitmap b)
        {
        }

        public virtual void BeforeDraw(Graphics g, Bitmap b)
        {
        }

        public virtual void AfterDraw(Graphics g, Bitmap b)
        {
        }

        public Brush GetBrush()
        {
            return GetBrush(Rect);
        }

        public virtual bool CanDraw()
        {
            if (String.IsNullOrEmpty(Condition))
                return true;

            var e = new Expression(Condition.Trim());
            try
            {
                Dictionary<string, object> template = GetTemplateDict();
                foreach (var x in template)
                    e.Parameters.Add(x.Key, x.Value);
                e.Parameters["dd"] = Data.DD > 0;
                e.Parameters.Add("Data", Data);
                return Convert.ToBoolean(e.Evaluate());
            }
            catch (Exception)
            {
                return true;
            }
        }

        public Brush GetBrush(Rectangle r)
        {
            Brush b = null;
            if (Color != Color2 && GradientMode != GradientMode.None)
                b = new LinearGradientBrush(r, Color, Color2, (LinearGradientMode) (int) GradientMode);
            else
                b = new SolidBrush(Color);
            return b;
        }

        [XmlIgnore]
        public SongData Data { get; set; }

        private Dictionary<string, object> GetTemplateDict()
        {
            Dictionary<string, object> template = new Dictionary<string, object>();
            template.Add("artist", Data.Artist);
            template.Add("title", Data.Title);
            template.Add("album", Data.Album);
            template.Add("filename", Data.FileName);
            template.Add("date", Data.LastConversionDateTime.ToShortDateString());
            template.Add("tuning", Data.Tuning.Split(new[] {", "}, StringSplitOptions.None).FirstOrDefault());
            template.Add("tempo", Data.SongAverageTempo);
            template.Add("appid", Data.AppID);
            var ts = TimeSpan.FromSeconds(Data.SongLength);
            template.Add("length", string.Format("{0}:{1}", ts.Minutes, ts.Seconds));
            if (Data.DD > 0)
                template.Add("dd", "DD");
            else
                template.Add("dd", "NDD");

            template.Add("year", Data.SongYear);
            template.Add("version", String.IsNullOrEmpty(Data.Version) ? "N/A" : Data.Version);
            template.Add("author", String.IsNullOrEmpty(Data.CharterName) ? "Unknown" : Data.CharterName);
            String arrInit = Data.ArrangementInitials;
            template.Add("arrangements", String.IsNullOrEmpty(arrInit) ? "" : arrInit);
            template.Add("\\n", Environment.NewLine);
            template.Add("\\t", (char) 9);
            return template;
        }

        public string GetTemplateText(string format)
        {
            if (Data != null)
            {
                Dictionary<string, object> template = GetTemplateDict();

                string s = format;
                foreach (var x in template)
                    s = s.Replace(String.Format("[{0}]", x.Key), x.Value.ToString());

                return s;
            }
            return format;
        }
    }

    #region Shapes

    public enum DrawShapeType
    {
        fill,
        outline
    }

    public abstract class DrawShape : DrawInstruction
    {
        public DrawShape()
        {
            PenSize = 1.0f;
        }

        [XmlAttribute("type")] public DrawShapeType DrawType;
        [XmlAttribute("pensize")] public float PenSize;

        public Pen GetPen()
        {
            return new Pen(GetBrush(), PenSize);
        }
    }

    [XmlType("rect")]
    public class DrawRectangle : DrawShape
    {
        public override void Draw(Graphics g, Bitmap bmp)
        {
            if (DrawType == DrawShapeType.fill)
                using (var b = GetBrush())
                    g.FillRectangle(b, Rect);
            else
                g.DrawRectangle(GetPen(), Rect);
        }
    }

    [XmlType("img")]
    public class DrawImage : DrawInstruction
    {
        private Rectangle SourceRect;

        public DrawImage()
        {
            opacity = 1.0f;
            SourceRect = new Rectangle(0, 0, 0, 0);
        }

        [XmlAttribute("src")]
        public string file { get; set; }

        [XmlAttribute]
        public float opacity { get; set; }

        [XmlAttribute("srcrect")]
        public string srcrect
        {
            get { return GetRectString(SourceRect); }
            set
            {
                if (!String.IsNullOrEmpty(value))
                    SourceRect = GetRect(value);
            }
        }

        public override void Draw(Graphics g, Bitmap bmp)
        {
            if (File.Exists(file))
            {
                var img = Image.FromFile(file);
                if (img != null)
                {
                    //if (opacity == 1.0)
                    //{
                    //    g.DrawImage(img, Rect);
                    //}
                    //else
                    {
                        //create image attributes  
                        ImageAttributes attributes = new ImageAttributes();

                        //set the color(opacity) of the image  
                        attributes.SetColorMatrix(new ColorMatrix() {Matrix33 = opacity}, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                        RectangleF rf = new RectangleF(SourceRect.X, SourceRect.Left, SourceRect.Width == 0 ? img.Width : SourceRect.Width, SourceRect.Height == 0 ? img.Height : SourceRect.Height);
                        var src = Rect;
                        if (src.Width == 0 || src.Height == 0)
                        {
                            src = new Rectangle(src.X, src.Y, img.Width, img.Height);
                        }

                        g.DrawImage(img, src, rf.X, rf.Y, rf.Width, rf.Height, GraphicsUnit.Pixel, attributes);
                    }
                }
            }
        }
    }

    [XmlType("ellipse")]
    public class DrawEllipse : DrawShape
    {
        public override void Draw(Graphics g, Bitmap bmp)
        {
            if (DrawType == DrawShapeType.fill)
                using (var b = GetBrush())
                    g.FillEllipse(b, Rect);
            else
                g.DrawEllipse(GetPen(), Rect);
        }
    }

    [XmlType("pie")]
    public class DrawPie : DrawShape
    {
        [XmlAttribute]
        public float startAngle { get; set; }

        [XmlAttribute]
        public float sweepAngle { get; set; }

        public override void Draw(Graphics g, Bitmap bmp)
        {
            if (DrawType == DrawShapeType.fill)
                using (var b = GetBrush())
                    g.FillPie(b, Rect, startAngle, sweepAngle);
            else
                g.DrawPie(GetPen(), Rect, startAngle, sweepAngle);
        }
    }

    [XmlType("line")]
    public class DrawLine : DrawShape
    {
        [XmlAttribute]
        public float x1 { get; set; }

        [XmlAttribute]
        public float x2 { get; set; }

        [XmlAttribute]
        public float y1 { get; set; }

        [XmlAttribute]
        public float y2 { get; set; }

        public override void Draw(Graphics g, Bitmap bmp)
        {
            g.DrawLine(GetPen(), x1, y1, x2, y2);
        }
    }

    #endregion

    #region Text

    [XmlType("text")]
    public class DrawText : DrawInstruction
    {
        public DrawText()
        {
            FontName = "Arial";
            FontSize = 9;
            alignment = StringAlignment.Center;
            bold = false;
            italic = false;
            underline = false;
            strikeout = false;
        }

        public string Text { get; set; }

        [XmlAttribute("fontname")]
        public string FontName { get; set; }

        [XmlAttribute("fontsize")]
        public int FontSize { get; set; }

        [XmlAttribute] public StringAlignment alignment;
        [XmlAttribute] public bool bold;
        [XmlAttribute] public bool italic;
        [XmlAttribute] public bool underline;
        [XmlAttribute] public bool strikeout;

        [XmlIgnore]
        public FontStyle FontStyle
        {
            get
            {
                FontStyle fs = FontStyle.Regular;
                if (bold)
                    fs = fs | FontStyle.Bold;
                if (italic)
                    fs = fs | FontStyle.Italic;
                if (underline)
                    fs = fs | FontStyle.Underline;
                if (strikeout)
                    fs = fs | FontStyle.Strikeout;

                return fs;
            }
        }

        public override void Draw(Graphics g, Bitmap bmp)
        {
            if (!String.IsNullOrEmpty(Text))
            {
                Font F = new Font(FontName, FontSize, FontStyle);
                using (var b = GetBrush())
                {
                    string atext = GetTemplateText(Text);
                    if (Rect.Width > 0 || Rect.Height > 0)
                    {
                        var z = g.MeasureString(atext, F);
                        PointF f = new Point(Rect.X, Rect.Y);
                        switch (alignment)
                        {
                            case StringAlignment.Near:
                                break;
                            case StringAlignment.Center:
                                f = new PointF(Rect.X + ((Rect.Width - z.Width)/2), Rect.Y);
                                break;
                            case StringAlignment.Far:
                                f = new PointF(Rect.Width - z.Width, Rect.Y);
                                break;
                        }
                        g.DrawString(atext, F, b, f);
                    }
                    else
                        g.DrawString(atext, F, b, new PointF(Rect.X, Rect.Y));
                }
            }
            //   throw new NotImplementedException();
        }
    }

    [XmlType("formattext")]
    public class FormatText : DrawInstruction
    {
        public FormatText()
        {
            FontName = "Arial";
            FontSize = 0;
            alignment = StringAlignment.Center;
            bold = false;
            italic = false;
            underline = false;
            strikeout = false;
            LineColor = Color.Black;
            LineSize = 1.0f;
        }

        [XmlAttribute("lineColor")]
        public string linecolor
        {
            get { return ColorTranslator.ToHtml(LineColor); }
            set { LineColor = ColorTranslator.FromHtml(value); }
        }

        [XmlIgnore]
        public Color LineColor { get; set; }

        [XmlAttribute("lineSize")]
        public float LineSize { get; set; }

        public string Text { get; set; }

        [XmlAttribute("fontname")]
        public string FontName { get; set; }

        [XmlAttribute("fontsize")]
        public int FontSize { get; set; }

        [XmlAttribute] public StringAlignment alignment;
        [XmlAttribute] public bool bold;
        [XmlAttribute] public bool italic;
        [XmlAttribute] public bool underline;
        [XmlAttribute] public bool strikeout;

        [XmlIgnore]
        public FontStyle FontStyle
        {
            get
            {
                FontStyle fs = FontStyle.Regular;
                if (bold)
                    fs = fs | FontStyle.Bold;
                if (italic)
                    fs = fs | FontStyle.Italic;
                if (underline)
                    fs = fs | FontStyle.Underline;
                if (strikeout)
                    fs = fs | FontStyle.Strikeout;

                return fs;
            }
        }

        public override void Draw(Graphics g, Bitmap bmp)
        {
            if (!String.IsNullOrEmpty(Text))
            {
                var XRect = new Rectangle(Rect.X, Rect.Y, Math.Max(Rect.Width, bmp.Width), Rect.Height);

                string atext = GetTemplateText(Text);
                int[] sizes = new int[] {100, 90, 80, 75, 70, 65, 60, 55, 50, 45, 42, 40, 38, 36, 34, 32, 30, 28, 26, 24, 22, 20, 16, 12, 8, 4};
                SizeF crSize = new SizeF();
                Font crFont = null;
                int fs = FontSize;
                if (fs <= 0)
                {
                    for (int i = 0; i < sizes.Length; i++)
                    {
                        crFont = new Font(FontName, sizes[i], FontStyle);
                        crSize = g.MeasureString(atext, crFont, Math.Max(Rect.Width, bmp.Width), new StringFormat() {Alignment = alignment});
                        if ((ushort) crSize.Width < ((ushort) bmp.Width))
                            break;
                    }
                }
                else
                {
                    crFont = new Font(FontName, fs, FontStyle);
                    crSize = g.MeasureString(atext, crFont, Math.Max(Rect.Width, bmp.Width), new StringFormat() {Alignment = alignment});
                }
                if (XRect.Width > 0)
                {
                    //Rect.Height = Math.Max
                    var arect = new Rectangle(XRect.X, XRect.Y, XRect.Width, Math.Max(Rect.Height, (int) crSize.Height));
                    using (GraphicsPath gp = new GraphicsPath())
                    using (Pen outline = new Pen(LineColor, LineSize) {LineJoin = LineJoin.Round})
                    using (StringFormat sf = new StringFormat() {Alignment = StringAlignment.Center})
                    using (var foreBrush = GetBrush(arect))
                    {
                        gp.AddString(atext, crFont.FontFamily, (int) crFont.Style, crFont.Size + 6, arect, sf);

                        g.SmoothingMode = SmoothingMode.HighQuality;
                        g.DrawPath(outline, gp);
                        g.FillPath(foreBrush, gp);
                    }
                }
            }
        }
    }

    #endregion

    public abstract class DrawInstParent : DrawInstruction, IDrawInstructionsHolder
    {
        public DrawInstructions Drawing = new DrawInstructions();

        public DrawInstructions GetDrawing()
        {
            return Drawing;
        }
    }

    [XmlType("if")]
    public class DrawIF : DrawInstParent
    {
    }

    #region Transform

    public abstract class DrawTransform : DrawInstParent
    {
        private GraphicsState ss;

        public override void Draw(Graphics g, Bitmap b)
        {
            base.Draw(g, b);
            Drawing.Draw(g, b);
        }

        public override void BeforeDraw(Graphics g, Bitmap b)
        {
            base.BeforeDraw(g, b);
            ss = g.Save();
        }

        public override void AfterDraw(Graphics g, Bitmap b)
        {
            g.Restore(ss);
            ss = null;
            base.AfterDraw(g, b);
        }
    }

    [XmlType("translate")]
    public class TranslateTransform : DrawTransform
    {
        [XmlAttribute]
        public float x { get; set; }

        [XmlAttribute]
        public float y { get; set; }

        public override void BeforeDraw(Graphics g, Bitmap b)
        {
            base.BeforeDraw(g, b);
            g.TranslateTransform(x, y);
        }
    }

    [XmlType("scale")]
    public class ScaleTransform : DrawTransform
    {
        [XmlAttribute("scaleX")]
        public float ScaleX { get; set; }

        [XmlAttribute("scaleY")]
        public float ScaleY { get; set; }

        public override void BeforeDraw(Graphics g, Bitmap b)
        {
            base.BeforeDraw(g, b);
            g.ScaleTransform(ScaleX, ScaleY);
        }
    }

    [XmlType("rotate")]
    public class RotateTransform : DrawTransform
    {
        [XmlAttribute("angle")]
        public float angle { get; set; }

        public override void BeforeDraw(Graphics g, Bitmap b)
        {
            base.BeforeDraw(g, b);
            g.RotateTransform(angle);
        }
    }

    #endregion

    #endregion

    public class TaggerProgress : EventArgs
    {
        private int max;

        public TaggerProgress(int Current, int Max)
        {
            max = Max;
            SetCurMax(Current, Max);
        }

        public SongData CurrentSong { get; set; }
        public int Progress { get; private set; }

        public void SetCurMax(int Current, int Max)
        {
            this.Progress = (Current/Max)*100;
        }

        public void SetPos(int Current)
        {
            SetCurMax(Current, max);
        }
    }

    internal sealed class BitmapHolder : IDisposable
    {
        public Bitmap backgroundLayer { get; private set; }
        public Bitmap customTagLayer { get; private set; }
        public Bitmap vocalLayer { get; private set; }
        public Bitmap leadLayer { get; private set; }
        public Bitmap rhythmLayer { get; private set; }
        public Bitmap bassLayer { get; private set; }
        public Bitmap leadBonusLayer { get; private set; }
        public Bitmap rhythmBonusLayer { get; private set; }
        public Bitmap bassBonusLayer { get; private set; }
        public Bitmap DDLayer { get; private set; }

        public BitmapHolder(string tagsFolderFullPath)
        {
            var p = Path.Combine(tagsFolderFullPath, "DD.png");
            if (!File.Exists(p))
                p = Path.Combine(Constants.TaggerTemplatesFolder, "DD.png");

            if (File.Exists(p))
                DDLayer = new Bitmap(p);
            else
                DDLayer = null;

            p = Path.Combine(tagsFolderFullPath, "Background.png");
            if (File.Exists(p))
                backgroundLayer = new Bitmap(p);
            else
                backgroundLayer = null;

            p = Path.Combine(tagsFolderFullPath, "Custom.png");
            if (File.Exists(p))
                customTagLayer = new Bitmap(p);
            else
                customTagLayer = null;

            p = Path.Combine(tagsFolderFullPath, "Vocal.png");
            if (File.Exists(p))
                vocalLayer = new Bitmap(p);
            else
                vocalLayer = null;

            p = Path.Combine(tagsFolderFullPath, "Lead.png");
            if (File.Exists(p))
                leadLayer = new Bitmap(p);
            else
                leadLayer = null;

            p = Path.Combine(tagsFolderFullPath, "Rhythm.png");
            if (File.Exists(p))
                rhythmLayer = new Bitmap(p);
            else
                rhythmLayer = null;

            p = Path.Combine(tagsFolderFullPath, "Bass.png");
            if (File.Exists(p))
                bassLayer = new Bitmap(p);
            else
                bassLayer = null;

            p = Path.Combine(tagsFolderFullPath, "Lead Bonus.png");
            if (File.Exists(p))
                leadBonusLayer = new Bitmap(p);
            else
                leadBonusLayer = null;

            p = Path.Combine(tagsFolderFullPath, "Rhythm Bonus.png");
            if (File.Exists(p))
                rhythmBonusLayer = new Bitmap(p);
            else
                rhythmBonusLayer = null;

            p = Path.Combine(tagsFolderFullPath, "Bass Bonus.png");
            if (File.Exists(p))
                bassBonusLayer = new Bitmap(p);
            else
                bassBonusLayer = null;
        }

        private void ClearImages()
        {
            if (backgroundLayer != null)
            {
                backgroundLayer.Dispose();
                backgroundLayer = null;
            }
            if (customTagLayer != null)
            {
                customTagLayer.Dispose();
                customTagLayer = null;
            }
            if (vocalLayer != null)
            {
                vocalLayer.Dispose();
                vocalLayer = null;
            }
            if (leadLayer != null)
            {
                leadLayer.Dispose();
                leadLayer = null;
            }
            if (rhythmLayer != null)
            {
                rhythmLayer.Dispose();
                rhythmLayer = null;
            }
            if (bassLayer != null)
            {
                bassLayer.Dispose();
                bassLayer = null;
            }
            if (leadBonusLayer != null)
            {
                leadBonusLayer.Dispose();
                leadBonusLayer = null;
            }
            if (rhythmBonusLayer != null)
            {
                rhythmBonusLayer.Dispose();
                rhythmBonusLayer = null;
            }
            if (bassBonusLayer != null)
            {
                bassBonusLayer.Dispose();
                bassBonusLayer = null;
            }
            if (DDLayer != null)
            {
                DDLayer.Dispose();
                DDLayer = null;
            }
        }

        public void Dispose()
        {
            ClearImages();
        }
    }
}