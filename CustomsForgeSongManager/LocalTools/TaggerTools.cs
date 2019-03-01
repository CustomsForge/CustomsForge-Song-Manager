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
using CustomsForgeSongManager.DataObjects;
using GenTools;
using CFSM.ImageTools;
using CFSM.NCalc;
using RocksmithToolkitLib.DLCPackage;
using RocksmithToolkitLib.Extensions;
using System.Diagnostics;
using CustomsForgeSongManager.Forms;
using System.Text;

namespace CustomsForgeSongManager.LocalTools
{
    public class TaggerTools
    {
        private static StringBuilder sbErrors;
        private static bool isFirstError;

        // set global default theme name (folder) here
        public static string DefaultThemeName
        {
            get { return "default_tags_stars"; }
        }

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

        public TaggerTools()
        {
            ThemeName = DefaultThemeName;
            Themes = new List<string>();
            XmlThemes = new List<XmlThemeStorage>();
            Populate();
        }

        private void Populate()
        {
            // start fresh for debugging
            if (Constants.DebugMode)
                GenExtensions.DeleteDirectory(Constants.TaggerWorkingFolder);

            if (!Directory.Exists(Constants.TaggerTemplatesFolder) || GenExtensions.IsDirectoryEmpty(Constants.TaggerTemplatesFolder) || !File.Exists(Path.Combine(Constants.TaggerTemplatesFolder, "Default.tagtheme")))
                CreateDefaultFolders();

            if (!File.Exists(Path.Combine(Constants.TaggerTemplatesFolder, "User.Default.tagtheme")))
                File.Copy(Path.Combine(Constants.TaggerTemplatesFolder, "Default.tagtheme"), Path.Combine(Constants.TaggerTemplatesFolder, "User.Default.tagtheme"));

            Themes.Clear();

            var tagPreviews = Directory.EnumerateFiles(Constants.TaggerTemplatesFolder, "*.png").Where(file => file.ToLower().Contains("_prev.png")).ToList();
            foreach (string tagPreview in tagPreviews)
                Themes.Add(Path.GetFileName(tagPreview).Replace(@"Tagger\", "").Replace("_prev.png", ""));

            var tagThemes = Directory.EnumerateFiles(Constants.TaggerTemplatesFolder, "*.tagtheme", SearchOption.AllDirectories).Where(file => !file.ToLower().Contains("default") && !file.ToLower().Contains("example")).ToList();
            foreach (string tagTheme in tagThemes)
                XmlThemes.Add(new XmlThemeStorage(Path.GetFileNameWithoutExtension(tagTheme), Path.GetDirectoryName(tagTheme)));

            Themes.AddRange(XmlThemes.Select(f => f.Name));
        }

        private void CreateDefaultFolders()
        {
            if (!Directory.Exists(Constants.TaggerWorkingFolder))
                Directory.CreateDirectory(Constants.TaggerWorkingFolder);

            // create tagger directories and files
            GenExtensions.ExtractEmbeddedResources(Constants.TaggerTemplatesFolder, Assembly.GetExecutingAssembly(), "CustomsForgeSongManager.Resources.tags");
        }

        public string ThemeName { get; set; }

        /// <summary>
        /// Opens the Tagger Theme *_prev.png file
        /// </summary>
        /// <param name="themeName"></param>
        /// <returns></returns>
        public void GetThemePreview(string themeName)
        {
            var previewPath = Path.Combine(Constants.TaggerTemplatesFolder, themeName) + "_prev.png";
            if (File.Exists(previewPath))
                frmNoteViewer.ViewExternalImageFile(previewPath, "Tag Example: " + themeName);
        }

        /// <summary>
        /// Opens the Tagger Theme info.txt file
        /// </summary>
        /// <param name="themeName"></param>
        /// <returns></returns>
        public void GetThemeInfo(string themeName)
        {
            var infoPath = Path.Combine(Constants.TaggerTemplatesFolder, themeName, "info.txt");
            if (File.Exists(infoPath))
                frmNoteViewer.ViewExternalFile(infoPath, "Tag Theme Info: " + themeName);
        }

        public Image Preview(SongData sd, string themeName = "")
        {
            if (String.IsNullOrEmpty(themeName))
            {
                if (ThemeName == String.Empty)
                    ThemeName = DefaultThemeName;

                themeName = ThemeName;
            }

            var themePath = Path.Combine(Constants.TaggerTemplatesFolder, themeName);
            var xTheme = XmlThemes.Find(x => x.Name == themeName);
            if (xTheme != null)
                themePath = xTheme.Folder;

            try
            {
                using (var images = new BitmapHolder(themePath))
                {
                    var songPath = sd.FilePath;
                    using (CFSM.RSTKLib.PSARC.PSARC archive = new CFSM.RSTKLib.PSARC.PSARC(true))
                    using (var fs = File.OpenRead(songPath))
                    {
                        archive.Read(fs, true);

                        CFSM.RSTKLib.PSARC.Entry imgEntry;
                        if (sd.Tagged == SongTaggerStatus.True)
                            imgEntry = archive.TOC.FirstOrDefault(entry => entry.Name.Equals("tagger.org"));
                        else
                            imgEntry = archive.TOC.FirstOrDefault(entry => entry.Name.EndsWith("256.dds"));

                        if (imgEntry != null)
                        {
                            archive.InflateEntry(imgEntry);
                            if (imgEntry.Data == null)
                            {
                                Globals.Log("<Error>: Inflating image entry ...");
                                return null;
                            }

                            imgEntry.Data.Position = 0;
                            var albumArtDDS = new DDSImage(imgEntry.Data);
                            var albumArt = albumArtDDS.images[0];

                            TagDrawImage(sd, images, xTheme, archive, albumArt, themePath);

                            return albumArt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowTaggerError(ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine);
            }

            return null;
        }

        private void ShowTaggerError(string errMsg)
        {
            GenExtensions.DeleteDirectory(Constants.TaggerWorkingFolder);
            Populate();

            MessageBox.Show(
                "<ERROR> Tagger encountered an critical error ..." + Environment.NewLine +
                errMsg + "  " + Environment.NewLine +
                "CFSM has restored all tagger template files in an" + Environment.NewLine +
                "attempt to fix the error.  Please try tagger again.  ", "Album Artwork Tagger", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }

        private string TagsFolderFullPath
        {
            get
            {
                var xTheme = XmlThemes.Find(x => x.Name == ThemeName);
                if (xTheme != null)
                    return xTheme.Folder;

                return Path.Combine(Constants.TaggerTemplatesFolder, ThemeName);
            }
        }

        private void TagDrawImage(SongData song, BitmapHolder images, XmlThemeStorage xTheme, CFSM.RSTKLib.PSARC.PSARC archive, Bitmap bigAlbumArt, String folderFullPath)
        {
            var lead = false;
            var rhythm = false;
            var bass = false;
            var vocals = false;
            var bonusLead = false;
            var bonusRhythm = false;
            var bonusBass = false;
            var DD = song.DD > 0;

            var arrangements = archive.TOC.Where(entry => entry.Name.ToLower().EndsWith(".json")).Select(entry => entry.Name).ToList();
            foreach (string arr in arrangements)
            {
                if (arr.Contains("lead") && !arr.Contains("lead2"))
                    lead = true;
                if (arr.Contains("lead2"))
                    bonusLead = true;
                if (arr.Contains("rhythm") && !arr.Contains("rhythm2"))
                    rhythm = true;
                if (arr.Contains("rhythm2"))
                    bonusRhythm = true;
                if (arr.Contains("bass") && !arr.Contains("bass2"))
                    bass = true;
                if (arr.Contains("bass2"))
                    bonusBass = true;
                if (arr.Contains("vocals"))
                    vocals = true;
                if (arr.Contains("combo") && !arr.Contains("combo2"))
                {
                    lead = true;
                    rhythm = true;
                }
                if (arr.Contains("combo2"))
                {
                    bonusLead = true;
                    bonusRhythm = true;
                }
            }

            var rating = 0;
            var tkEntry = archive.TOC.FirstOrDefault(x => x.Name.Equals("toolkit.version"));
            if (tkEntry != null)
            {
                if (tkEntry.Data != null)
                    tkEntry.Data.Position = 0;
                else
                    archive.InflateEntry(tkEntry);

                ToolkitInfo tkInfo = GeneralExtensions.GetToolkitInfo(new StreamReader(tkEntry.Data));
                rating = int.Parse(tkInfo.PackageRating == null ? "0" : tkInfo.PackageRating);
            }

            SongTaggerTheme tt = null;
            var ttpath = TagsFolderFullPath;
            if (xTheme != null)
            {
                using (FileStream fs1 = File.OpenRead(Path.Combine(xTheme.Folder, xTheme.Name + ".tagtheme")))
                    tt = SongTaggerTheme.Create(fs1);
            }
            else if (File.Exists(Path.Combine(folderFullPath, "Default.tagtheme")))
            {
                using (FileStream fs1 = File.OpenRead(Path.Combine(folderFullPath, "Default.tagtheme")))
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
                // Add layers to big album art
                using (Graphics gra = Graphics.FromImage(bigAlbumArt))
                {
                    // Image Layers are BottomMost to TopMost Order
                    // Background Layers
                    if (images.BackgroundLayer != null)
                        gra.DrawImage(images.BackgroundLayer, 0, 0.5f);
                    if (images.CustomStarsLayer != null)
                        gra.DrawImage(images.CustomStarsLayer, 0, 0.5f);
                    if (images.CustomTagsLayer != null)
                        gra.DrawImage(images.CustomTagsLayer, 0, 0.5f);
                    if (images.CustomTagsStarsLayer != null)
                        gra.DrawImage(images.CustomTagsStarsLayer, 0, 0.5f);
                    if (DD && images.DDLayer != null)
                        gra.DrawImage(images.DDLayer, 0, 0.5f);

                    // Arrangement Layers
                    if (vocals && images.VocalLayer != null)
                        gra.DrawImage(images.VocalLayer, 0, 0.5f);
                    if (bass && images.BassLayer != null)
                        gra.DrawImage(images.BassLayer, 0, 0.5f);
                    if (bonusBass && images.BassBonusLayer != null)
                        gra.DrawImage(images.BassBonusLayer, 0, 0.5f);
                    if (rhythm && images.RhythmLayer != null)
                        gra.DrawImage(images.RhythmLayer, 0, 0.5f);
                    if (bonusRhythm && images.RhythmBonusLayer != null)
                        gra.DrawImage(images.RhythmBonusLayer, 0, 0.5f);
                    if (lead && images.LeadLayer != null)
                        gra.DrawImage(images.LeadLayer, 0, 0.5f);
                    if (bonusLead && images.LeadBonusLayer != null)
                        gra.DrawImage(images.LeadBonusLayer, 0, 0.5f);

                    // Rating Layers
                    if (rating > 4 && images.Stars5Layer != null)
                        gra.DrawImage(images.Stars5Layer, 0, 0.5f);
                    if (rating > 3 && images.Stars4Layer != null)
                        gra.DrawImage(images.Stars4Layer, 0, 0.5f);
                    if (rating > 2 && images.Stars3Layer != null)
                        gra.DrawImage(images.Stars3Layer, 0, 0.5f);
                    if (rating > 1 && images.Stars2Layer != null)
                        gra.DrawImage(images.Stars2Layer, 0, 0.5f);
                    if (rating > 0 && images.Stars1Layer != null)
                        gra.DrawImage(images.Stars1Layer, 0, 0.5f);

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
                ThemeName = DefaultThemeName;

            if (!Directory.Exists(Constants.TaggerTemplatesFolder) || !Directory.Exists(TagsFolderFullPath))
                CreateDefaultFolders();

            var xTheme = XmlThemes.Find(x => x.Name == ThemeName);

            if (File.Exists(song.FilePath)) //Just to be sure :)
            {
                try
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

                        // do not tag Official DLC songs
                        if (!AllowEditingOfODLC && toolKitEntry == null)
                        {
                            Globals.Log("CFSM can not be used to tag Official DLC songs.");
                            song.Tagged = SongTaggerStatus.ODLC;
                            return;
                        }

                        var albumSmallArtEntry = archive.TOC.FirstOrDefault(entry => entry.Name.EndsWith("64.dds")); // get album art paths
                        var albumMidArtEntry = archive.TOC.FirstOrDefault(entry => entry.Name.EndsWith("128.dds"));
                        var albumBigArtEntry = archive.TOC.FirstOrDefault(entry => entry.Name.EndsWith("256.dds"));
                        albumBigArtEntry.Data.Position = 0;

                        var bigAlbumArt = ImageExtensions.DDStoBitmap(albumBigArtEntry.Data); // albumArtDDS.images[0];
                        var orginalArtStream = new MemoryStream();
                        albumBigArtEntry.Data.Position = 0;
                        albumBigArtEntry.Data.CopyTo(orginalArtStream);
                        orginalArtStream.Position = 0;

                        //Check which arrangements it contains
                        TagDrawImage(song, images, xTheme, archive, bigAlbumArt, TagsFolderFullPath);

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

                        // archive.TOC.Insert(0, new Entry() { Name = "NamesBlock.bin" });
                        archive.AddEntry("tagger.org", orginalArtStream);
                        songPath = song.FilePath;

                        using (var FS = File.Create(songPath))
                            archive.Write(FS, true);

                        song.FilePath = songPath;
                        song.FileDate = File.GetLastWriteTime(song.FilePath);
                        song.Tagged = SongTaggerStatus.True;
                    }

                    // Globals.Log("Finished tagging song ...");
                }
                catch (Exception ex)
                {
                    // commented out per user request ... silly users ;>)
                    // MessageBox.Show(Path.GetFileName(song.FilePath) + " is corrupt.  " + Environment.NewLine +
                    // "The CDLC contains no album artwork." + Environment.NewLine + ex.Message,
                    // Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Error);

                    if (isFirstError)
                    {
                        Globals.Log("<ERROR>: Tagger could not find album artwork ...");
                        Globals.Log(" - Use the Repairs Mastery 100% Bug option to restore default album artwork ...");
                        Globals.Log(" - For file and error details, see: " + Constants.RepairsErrorLogPath);
                        sbErrors.Insert(0, "File Name, Error Message" + Environment.NewLine);
                        sbErrors.Insert(0, DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + Environment.NewLine);
                        isFirstError = false;
                    }

                    sbErrors.AppendLine(String.Format("{0}, Tagger could not find album artwork", Path.GetFileName(song.FilePath)));

                    using (TextWriter tw = new StreamWriter(Constants.RepairsErrorLogPath, true))
                    {
                        tw.WriteLine(sbErrors);
                        tw.Close();
                    }
                }
            }
        }

        public void TagSong(SongData song)
        {
            using (var images = new BitmapHolder(TagsFolderFullPath))
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
                // Globals.Log("Finished untagging song ...");
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

                    song.FileDate = File.GetLastWriteTime(song.FilePath);
                }
            }
        }

        public void TagUntagSongs(List<SongData> selectedSongs, bool isUnTag = false)
        {
            if (!selectedSongs.Any())
                return;

            // initialize error tracker
            isFirstError = true;
            sbErrors = new StringBuilder();

            if (!isUnTag)
                Globals.Log("Tagging selected CDLC files ...");
            else
                Globals.Log("Untagging selected CDLC files ...");

            var total = selectedSongs.Count(); // (sd => !sd.IsODLC);
            int processed = 0, failed = 0, skipped = 0;
            GenericWorker.InitReportProgress();

            using (var images = new BitmapHolder(TagsFolderFullPath))
            {
                foreach (var song in selectedSongs)
                {
                    processed++;
                    GenericWorker.ReportProgress(processed, total, skipped, failed);

                    // skip ODLC
                    if (song.IsODLC)
                    {
                        skipped++;
                        continue;
                    }

                    try
                    {
                        if (isUnTag)
                            UntagSong(song);
                        else
                            TagSong(song, images);
                    }
                    catch (Exception ex)
                    {
                        failed++;
                        Debug.WriteLine(String.Format("<ERROR>: Could not {0} CDLC file {1}", isUnTag ? "UnTag" : "Tag", song.FileName + Environment.NewLine + ex.Message));
                    }
                }
            }

            GenericWorker.ReportProgress(processed, total, skipped, failed);
        }
    }

    internal static class TagThemeRegistar
    {
        public static Type[] DrawTypes;

        public static void CFSM_INIT()
        {
            var xtypes = TypeExtensions.GetLoadableTypes().Where(type => { return type.IsSubclassOf(typeof(DrawInstruction)) && !type.IsAbstract; });
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
        public TaggerDrawer Custom { get; set; } // Tag and Rating

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
            Drawers = new List<TaggerDrawer> { DD, NDD, Custom };
        }

        public static SongTaggerTheme Create(Stream stream)
        {
            var result = stream.DeserializeXml<SongTaggerTheme>(TagThemeRegistar.DrawTypes);
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
        public static TaggerDrawer Create(Stream stream)
        {
            var result = stream.DeserializeXml<TaggerDrawer>(TagThemeRegistar.DrawTypes);
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

        private SongData songData;
        [XmlIgnore]
        public SongData Data
        {
            get { return songData; }
            set
            {
                songData = value;
                Drawing.Instructions.ForEach(di => di.Data = value);
            }
        }

        [XmlIgnore]
        public Point Position;
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

        private TaggerDrawer mParent;

        [XmlIgnore]
        public TaggerDrawer Parent
        {
            get { return mParent; }
            set
            {
                mParent = value;
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

        [XmlArray("Instructions")]
        [XmlArrayItem("Draw")]
        public List<DrawInstruction> Instructions = new List<DrawInstruction>();

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

    [Obfuscation(Exclude = false, Feature = "-rename")]
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
                b = new LinearGradientBrush(r, Color, Color2, (LinearGradientMode)(int)GradientMode);
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
            template.Add("tuning", Data.Tunings1D.Split(new[] { ", " }, StringSplitOptions.None).FirstOrDefault());
            template.Add("tempo", Data.SongAverageTempo);
            template.Add("appid", Data.AppID);
            var ts = TimeSpan.FromSeconds(Data.SongLength);
            template.Add("length", String.Format("{0}:{1}", ts.Minutes, ts.Seconds));
            if (Data.DD > 0)
                template.Add("dd", "DD");
            else
                template.Add("dd", "NDD");

            template.Add("year", Data.SongYear);
            template.Add("version", String.IsNullOrEmpty(Data.PackageVersion) ? "" : Data.PackageVersion);
            template.Add("author", String.IsNullOrEmpty(Data.PackageAuthor) ? "" : Data.PackageAuthor);
            String arrInit = Data.ArrangementsInitials;
            template.Add("arrangements", String.IsNullOrEmpty(arrInit) ? "" : arrInit);
            template.Add("\\n", Environment.NewLine);
            template.Add("\\t", (char)9);

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

    [Obfuscation(Exclude = false, Feature = "-rename")]
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

        [XmlAttribute("type")]
        public DrawShapeType DrawType;
        [XmlAttribute("pensize")]
        public float PenSize;

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
                        attributes.SetColorMatrix(new ColorMatrix() { Matrix33 = opacity }, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

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

        [XmlAttribute]
        public StringAlignment alignment;
        [XmlAttribute]
        public bool bold;
        [XmlAttribute]
        public bool italic;
        [XmlAttribute]
        public bool underline;
        [XmlAttribute]
        public bool strikeout;

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
                                f = new PointF(Rect.X + ((Rect.Width - z.Width) / 2), Rect.Y);
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

        [XmlAttribute]
        public StringAlignment alignment;
        [XmlAttribute]
        public bool bold;
        [XmlAttribute]
        public bool italic;
        [XmlAttribute]
        public bool underline;
        [XmlAttribute]
        public bool strikeout;

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
                int[] sizes = new int[] { 100, 90, 80, 75, 70, 65, 60, 55, 50, 45, 42, 40, 38, 36, 34, 32, 30, 28, 26, 24, 22, 20, 16, 12, 8, 4 };
                SizeF crSize = new SizeF();
                Font crFont = null;
                int fs = FontSize;
                if (fs <= 0)
                {
                    for (int i = 0; i < sizes.Length; i++)
                    {
                        crFont = new Font(FontName, sizes[i], FontStyle);
                        crSize = g.MeasureString(atext, crFont, Math.Max(Rect.Width, bmp.Width), new StringFormat() { Alignment = alignment });
                        if ((ushort)crSize.Width < ((ushort)bmp.Width))
                            break;
                    }
                }
                else
                {
                    crFont = new Font(FontName, fs, FontStyle);
                    crSize = g.MeasureString(atext, crFont, Math.Max(Rect.Width, bmp.Width), new StringFormat() { Alignment = alignment });
                }
                if (XRect.Width > 0)
                {
                    //Rect.Height = Math.Max
                    var arect = new Rectangle(XRect.X, XRect.Y, XRect.Width, Math.Max(Rect.Height, (int)crSize.Height));
                    using (GraphicsPath gp = new GraphicsPath())
                    using (Pen outline = new Pen(LineColor, LineSize) { LineJoin = LineJoin.Round })
                    using (StringFormat sf = new StringFormat() { Alignment = StringAlignment.Center })
                    using (var foreBrush = GetBrush(arect))
                    {
                        gp.AddString(atext, crFont.FontFamily, (int)crFont.Style, crFont.Size + 6, arect, sf);

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

    internal sealed class BitmapHolder : IDisposable
    {
        // Background Layers
        public Bitmap BackgroundLayer { get; private set; }
        public Bitmap CustomTagsLayer { get; private set; }
        public Bitmap CustomStarsLayer { get; private set; }
        public Bitmap CustomTagsStarsLayer { get; private set; }
        // Arrangement Layers
        public Bitmap LeadLayer { get; private set; }
        public Bitmap RhythmLayer { get; private set; }
        public Bitmap BassLayer { get; private set; }
        public Bitmap LeadBonusLayer { get; private set; }
        public Bitmap RhythmBonusLayer { get; private set; }
        public Bitmap BassBonusLayer { get; private set; }
        public Bitmap VocalLayer { get; private set; }
        public Bitmap DDLayer { get; private set; } // (optional) all Remastered CDLC must have DD
        // Rating Layers
        public Bitmap Stars1Layer { get; private set; }
        public Bitmap Stars2Layer { get; private set; }
        public Bitmap Stars3Layer { get; private set; }
        public Bitmap Stars4Layer { get; private set; }
        public Bitmap Stars5Layer { get; private set; }

        public BitmapHolder(string tagsFolderFullPath)
        {
            ClearImages();

            // Background Layers
            var p = Path.Combine(tagsFolderFullPath, "Background.png");
            if (File.Exists(p))
                BackgroundLayer = new Bitmap(p);
            else
                BackgroundLayer = null;

            p = Path.Combine(tagsFolderFullPath, "Custom_Stars.png");
            if (File.Exists(p))
                CustomStarsLayer = new Bitmap(p);
            else
                CustomStarsLayer = null;

            p = Path.Combine(tagsFolderFullPath, "Custom_Tags.png");
            if (File.Exists(p))
                CustomTagsLayer = new Bitmap(p);
            else
                CustomTagsLayer = null;

            p = Path.Combine(tagsFolderFullPath, "Custom_Tags_Stars.png");
            if (File.Exists(p))
                CustomTagsStarsLayer = new Bitmap(p);
            else
                CustomTagsStarsLayer = null;


            // Arrangement Layers             
            p = Path.Combine(tagsFolderFullPath, "Lead.png");
            if (File.Exists(p))
                LeadLayer = new Bitmap(p);
            else
                LeadLayer = null;

            p = Path.Combine(tagsFolderFullPath, "Lead_Bonus.png");
            if (File.Exists(p))
                LeadBonusLayer = new Bitmap(p);
            else
                LeadBonusLayer = null;

            p = Path.Combine(tagsFolderFullPath, "Rhythm.png");
            if (File.Exists(p))
                RhythmLayer = new Bitmap(p);
            else
                RhythmLayer = null;

            p = Path.Combine(tagsFolderFullPath, "Rhythm_Bonus.png");
            if (File.Exists(p))
                RhythmBonusLayer = new Bitmap(p);
            else
                RhythmBonusLayer = null;

            p = Path.Combine(tagsFolderFullPath, "Bass.png");
            if (File.Exists(p))
                BassLayer = new Bitmap(p);
            else
                BassLayer = null;

            p = Path.Combine(tagsFolderFullPath, "Bass_Bonus.png");
            if (File.Exists(p))
                BassBonusLayer = new Bitmap(p);
            else
                BassBonusLayer = null;


            p = Path.Combine(tagsFolderFullPath, "Vocals.png");
            if (File.Exists(p))
                VocalLayer = new Bitmap(p);
            else
                VocalLayer = null;

            p = Path.Combine(tagsFolderFullPath, "DD.png");
            if (!File.Exists(p))
                p = Path.Combine(Constants.TaggerTemplatesFolder, "DD.png");
            if (File.Exists(p))
                DDLayer = new Bitmap(p);
            else
                DDLayer = null;

            // Rating Layers           
            p = Path.Combine(tagsFolderFullPath, "Stars_1.png");
            if (File.Exists(p))
                Stars1Layer = new Bitmap(p);
            else
                Stars1Layer = null;

            p = Path.Combine(tagsFolderFullPath, "Stars_2.png");
            if (File.Exists(p))
                Stars2Layer = new Bitmap(p);
            else
                Stars2Layer = null;

            p = Path.Combine(tagsFolderFullPath, "Stars_3.png");
            if (File.Exists(p))
                Stars3Layer = new Bitmap(p);
            else
                Stars3Layer = null;

            p = Path.Combine(tagsFolderFullPath, "Stars_4.png");
            if (File.Exists(p))
                Stars4Layer = new Bitmap(p);
            else
                Stars4Layer = null;

            p = Path.Combine(tagsFolderFullPath, "Stars_5.png");
            if (File.Exists(p))
                Stars5Layer = new Bitmap(p);
            else
                Stars5Layer = null;
        }

        private void ClearImages()
        {
            // Background Layers
            if (BackgroundLayer != null)
            {
                BackgroundLayer.Dispose();
                BackgroundLayer = null;
            }
            if (CustomTagsLayer != null)
            {
                CustomTagsLayer.Dispose();
                CustomTagsLayer = null;
            }
            if (CustomStarsLayer != null)
            {
                CustomStarsLayer.Dispose();
                CustomStarsLayer = null;
            }
            if (CustomTagsStarsLayer != null)
            {
                CustomTagsStarsLayer.Dispose();
                CustomTagsStarsLayer = null;
            }

            // Arrangement Layers
            if (LeadLayer != null)
            {
                LeadLayer.Dispose();
                LeadLayer = null;
            }
            if (LeadBonusLayer != null)
            {
                LeadBonusLayer.Dispose();
                LeadBonusLayer = null;
            }
            if (RhythmLayer != null)
            {
                RhythmLayer.Dispose();
                RhythmLayer = null;
            }
            if (RhythmBonusLayer != null)
            {
                RhythmBonusLayer.Dispose();
                RhythmBonusLayer = null;
            }
            if (BassLayer != null)
            {
                BassLayer.Dispose();
                BassLayer = null;
            }
            if (BassBonusLayer != null)
            {
                BassBonusLayer.Dispose();
                BassBonusLayer = null;
            }
            if (VocalLayer != null)
            {
                VocalLayer.Dispose();
                VocalLayer = null;
            }
            if (DDLayer != null)
            {
                DDLayer.Dispose();
                DDLayer = null;
            }

            // Ratings Layers
            if (Stars1Layer != null)
            {
                Stars1Layer.Dispose();
                Stars1Layer = null;
            }
            if (Stars2Layer != null)
            {
                Stars2Layer.Dispose();
                Stars2Layer = null;
            }
            if (Stars3Layer != null)
            {
                Stars3Layer.Dispose();
                Stars3Layer = null;
            }
            if (Stars4Layer != null)
            {
                Stars4Layer.Dispose();
                Stars4Layer = null;
            }
            if (Stars5Layer != null)
            {
                Stars5Layer.Dispose();
                Stars5Layer = null;
            }
        }

        public void Dispose()
        {
            ClearImages();
        }
    }
}