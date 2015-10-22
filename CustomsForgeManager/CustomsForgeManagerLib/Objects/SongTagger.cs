using CFMImageTools;
using RocksmithToolkitLib.PSARC;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CustomsForgeManager.CustomsForgeManagerLib.Objects
{

   
    public class SongTagger
    {
        private string[] defaultFiles = { 
                                            "info.txt", "Background.png", "Bass Bonus.png", 
                                            "Bass.png", "Custom.png", "Lead Bonus.png", "Lead.png", 
                                            "Rhythm Bonus.png", "Rhythm.png", "Vocal.png" 
                                        };
        private string[] defaultTagFolders = { 
                                                 "frackDefault", "motive_bl_", "motive_nv_", "motive_ws_",
                                                 "motive1" 
                                             };

        public List<String> Themes { get; private set; }

        public SongTagger()
        {
            ThemeName = defaultTagFolders[0];
            Themes = new List<string>();
            Populate();
        }

        public event EventHandler<TaggerProgress> OnProgress;


        private void Populate()
        {
            if (!Directory.Exists(Constants.TaggerTemplatesFolder) || Extensions.IsDirectoryEmpty(Constants.TaggerTemplatesFolder))
                CreateDefaultFolders();
            Themes.Clear();
            foreach (string tagPreview in
              Directory.EnumerateFiles(Constants.TaggerTemplatesFolder, "*.png").Where(
              file => file.ToLower().Contains("prev")))
                Themes.Add(Path.GetFileName(tagPreview).Replace(@"Tagger\", "").Replace("prev.png", ""));

        }

        private void CreateDefaultFolders()
        {
            if (!Directory.Exists(Constants.TaggerWorkingFolder))
                Directory.CreateDirectory(Constants.TaggerWorkingFolder);

            if (!Directory.Exists(Constants.TaggerExtractedFolder))
                Directory.CreateDirectory(Constants.TaggerExtractedFolder);

            if (!Directory.Exists(Constants.TaggerTemplatesFolder))
                Directory.CreateDirectory(Constants.TaggerTemplatesFolder);

            if (!Directory.Exists(Constants.TaggerPreviewsFolder))
                Directory.CreateDirectory(Constants.TaggerPreviewsFolder);

            foreach (string resourceDir in defaultTagFolders)
            {
                string folderPath = Path.Combine(Constants.TaggerTemplatesFolder, resourceDir);

                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                Extensions.ExtractEmbeddedResource(folderPath, "CustomsForgeManager.Resources.tags." + resourceDir, defaultFiles);
            }

            foreach (string previewFile in defaultTagFolders)
                Extensions.ExtractEmbeddedResource(Constants.TaggerTemplatesFolder, "CustomsForgeManager.Resources.tags", new string[] { previewFile + "prev.png" });
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

        public Image Preview(SongData sd)
        {
            if (ThemeName == String.Empty)
                ThemeName = defaultTagFolders[0];
            try
            {
                using (var images = new BitmapHolder(tagsFolderFullPath))
                {
                    string songPath = sd.Path;
                    using (PSARC archive = new PSARC())
                    {
                        using (var fs = File.OpenRead(songPath))
                            archive.Read(fs);

                        var imgEntry = archive.TOC.FirstOrDefault(entry => entry.Name.EndsWith("256.dds"));

                        if (imgEntry != null)
                        {
                            imgEntry.Data.Position = 0;
                            var albumArtDDS = new DDSImage(imgEntry.Data);
                            var AlbumArt = albumArtDDS.images[0];

                            bool lead = false;
                            bool rhythm = false;
                            bool bass = false;
                            bool vocals = false;
                            bool bonusLead = false;
                            bool bonusRhythm = false;
                            bool bonusBass = false;
                            //bool DD = false;

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
                            }
                            var fvalue = 0.5f;
                            using (Graphics gra = Graphics.FromImage(AlbumArt))
                            {
                                gra.DrawImage(images.backgroundLayer, 0, 0.5f);
                                if (vocals)
                                    gra.DrawImage(images.vocalLayer, 0, 0.5f);
                                if (bass)
                                    gra.DrawImage(images.bassLayer, 0, 0.5f);
                                if (bonusBass)
                                    gra.DrawImage(images.bassBonusLayer, 0, 0.5f);
                                if (rhythm)
                                    gra.DrawImage(images.rhythmLayer, 0, 0.5f);
                                if (bonusRhythm)
                                    gra.DrawImage(images.rhythmBonusLayer, 0, 0.5f);
                                if (lead)
                                    gra.DrawImage(images.leadLayer, 0, 0.5f);
                                if (bonusLead)
                                    gra.DrawImage(images.leadBonusLayer, 0, 0.5f);
                                gra.DrawImage(images.customTagLayer, 0, 0.5f);
                            }
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
            MessageBox.Show(string.Format("Make sure that you have all required files in the CFM\tags folder: \n" +
                "-Tagger/{0}/Background.png \n" +
                "-Tagger/{0}/Lead.png \n" +
                "-Tagger/{0}/Lead Bonus.png \n" +
                "-Tagger/{0}/Rhythm.png \n" +
                "-Tagger/{0}/Rhythm Bonus.png \n" +
                "-Tagger/{0}/Custom.png \n" +
                "-Tagger/{0}/Vocal.png", ThemeName));
        }

        private string tagsFolderFullPath
        {
            get { return Path.Combine(Constants.TaggerTemplatesFolder, ThemeName); }
        }

        private void TagSong(SongData song, BitmapHolder images)
        {
            if (ThemeName == String.Empty)
                ThemeName = defaultTagFolders[0];
            if (!Directory.Exists(Constants.TaggerTemplatesFolder) || !Directory.Exists(tagsFolderFullPath))
                CreateDefaultFolders();

            //   songTagged = songTagged || File.GetCreationTime(song.Path) == new DateTime(1990, 1, 1) ? true : false;

            if (File.Exists(song.Path)) //Just to be sure :)
            {
              //  if (!song.Tagged)
                {
                    string songPath = song.Path;
                    using (PSARC archive = new PSARC())
                    {
                        using (var fs = File.OpenRead(songPath))
                            archive.Read(fs);


                        var taggerOriginal = archive.TOC.FirstOrDefault(entry => entry.Name == "tagger.org");
                        if (taggerOriginal != null)
                        {
                            song.Tagged = true;
                            return;
                        }
                        Globals.Log("Tagging song: " + song.Title);

                        var toolKitEntry = archive.TOC.FirstOrDefault(entry => entry.Name == "toolkit.version");

                        if (toolKitEntry != null) //Very unlikely to happen since we don't parse official DLCs, but for safety
                        {
                            var albumSmallArtEntry = archive.TOC.FirstOrDefault(entry => entry.Name.EndsWith("64.dds")); //Get album art paths
                            var albumMidArtEntry = archive.TOC.FirstOrDefault(entry => entry.Name.EndsWith("128.dds"));
                            var albumBigArtEntry = archive.TOC.FirstOrDefault(entry => entry.Name.EndsWith("256.dds"));
                            albumBigArtEntry.Data.Position = 0;


                            //var albumArtDDS = new DDSImage(albumBigArtEntry.Data);
                            var bigAlbumArt = ImageExtensions.DDStoBitmap(albumBigArtEntry.Data);// albumArtDDS.images[0];
                            var orginalArtStream = new MemoryStream();
                            albumBigArtEntry.Data.Position = 0;
                            albumBigArtEntry.Data.CopyTo(orginalArtStream);
                            orginalArtStream.Position = 0;

                            //Check which arrangements it contains
                            bool lead = false;
                            bool rhythm = false;
                            bool bass = false;
                            bool vocals = false;
                            bool bonusLead = false;
                            bool bonusRhythm = false;
                            bool bonusBass = false;

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
                            }

                            //Add layers to big album art
                            using (Graphics gra = Graphics.FromImage(bigAlbumArt))
                            {
                                gra.DrawImage(images.backgroundLayer, 0, 0.5f);
                                if (vocals)
                                    gra.DrawImage(images.vocalLayer, 0, 0.5f);
                                if (bass)
                                    gra.DrawImage(images.bassLayer, 0, 0.5f);
                                if (bonusBass)
                                    gra.DrawImage(images.bassBonusLayer, 0, 0.5f);
                                if (rhythm)
                                    gra.DrawImage(images.rhythmLayer, 0, 0.5f);
                                if (bonusRhythm)
                                    gra.DrawImage(images.rhythmBonusLayer, 0, 0.5f);
                                if (lead)
                                    gra.DrawImage(images.leadLayer, 0, 0.5f);
                                if (bonusLead)
                                    gra.DrawImage(images.leadBonusLayer, 0, 0.5f);
                                gra.DrawImage(images.customTagLayer, 0, 0.5f);
                            }

                            var largeDDS = bigAlbumArt.ToDDS();
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
                            archive.TOC.Insert(0, new Entry() { Name = "NamesBlock.bin" });

                            archive.AddEntry("tagger.org", orginalArtStream);
                            songPath = song.Path;

                            using (var FS = File.Create(songPath))
                                archive.Write(FS, true);
                            song.Path = songPath;

                            song.FileDate = File.GetLastWriteTimeUtc(song.Path);

                            song.Tagged = true;

                            //  counter++;
                            //    gbTags.Text = string.Format("Tagged: {0}/{1}", counter, songCount);
                        }
                    }
                }
            }
        }

        public void TagSong(SongData song)
        {
            using (var images = new BitmapHolder(tagsFolderFullPath))
                TagSong(song, images);
        }

        public void UntagSong(SongData song)
        {
            if (File.Exists(song.Path)) //Just to be sure :)
            {
              //  if (song.Tagged)
                {
                    using (PSARC archive = new PSARC())
                    {
                        using (var fs = File.OpenRead(song.Path))
                            archive.Read(fs);
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

                            archive.TOC.Insert(0, new Entry() { Name = "NamesBlock.bin" });
                            archive.TOC.Remove(taggerOriginal);
                            using (var FS = File.Create(song.Path))
                                archive.Write(FS, true);
                            song.FileDate = File.GetLastWriteTimeUtc(song.Path);
                        }

                        song.Tagged = false;
                    }
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

    public class TaggerProgress : EventArgs
    {
        private int curr;
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
            this.Progress = (Current / Max) * 100;
        }

        public void SetPos(int Current)
        {
            SetCurMax(Current, max);
        }
    }

    sealed class BitmapHolder : IDisposable
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

        public BitmapHolder(string tagsFolderFullPath)
        {
            backgroundLayer = new Bitmap(Path.Combine(tagsFolderFullPath, "Background.png"));
            customTagLayer = new Bitmap(Path.Combine(tagsFolderFullPath, "Custom.png"));
            vocalLayer = new Bitmap(Path.Combine(tagsFolderFullPath, "Vocal.png"));
            leadLayer = new Bitmap(Path.Combine(tagsFolderFullPath, "Lead.png"));
            rhythmLayer = new Bitmap(Path.Combine(tagsFolderFullPath, "Rhythm.png"));
            bassLayer = new Bitmap(Path.Combine(tagsFolderFullPath, "Bass.png"));
            leadBonusLayer = new Bitmap(Path.Combine(tagsFolderFullPath, "Lead Bonus.png"));
            rhythmBonusLayer = new Bitmap(Path.Combine(tagsFolderFullPath, "Rhythm Bonus.png"));
            bassBonusLayer = new Bitmap(Path.Combine(tagsFolderFullPath, "Bass Bonus.png"));
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
        }

        public void Dispose()
        {
            ClearImages();
        }
    }
}
