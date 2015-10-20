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
        }

        public void Populate()
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

        public Image Preview(SongData sd)
        {
            if (ThemeName == String.Empty)
                ThemeName = defaultTagFolders[0];
            try
            {
                using (var images = new BitmapHolder(Path.Combine(Constants.TaggerTemplatesFolder, ThemeName)))
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
                            var midAlbumArt = albumArtDDS.images[0].ResizeImage(128, 128);

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
                                lead = (arrangement.Contains("lead") && !arrangement.Contains("lead2"));
                                
                                bonusLead = (arrangement.Contains("lead2"));

                                rhythm = (arrangement.Contains("rhythm") && !arrangement.Contains("rhythm2"));

                                bonusRhythm = (arrangement.Contains("rhythm2"));
                                bass = (arrangement.Contains("bass") && !arrangement.Contains("bass2"));
                                bonusBass = (arrangement.Contains("bass2"));
                                vocals = (arrangement.Contains("vocals"));
                            }

                            using (Graphics gra = Graphics.FromImage(midAlbumArt))
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
                            return midAlbumArt;

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


        private void TagSong(SongData song)
        {
            if (ThemeName == String.Empty)
                ThemeName = defaultTagFolders[0];

        }


        private void ReplaceImages(SongData song, Bitmap newImg, bool isTagged)
        {
            string songPath = song.Path;
            using (PSARC archive = new PSARC())
            {
                using (var fs = File.OpenRead(songPath))
                    archive.Read(fs);
                var toolKitEntry = archive.TOC.FirstOrDefault(entry => entry.Name == "toolkit.version");
                var orgDss = archive.TOC.FirstOrDefault(entry => entry.Name == "original_dss");

                if (toolKitEntry != null) //Very unlikely to happen since we don't parse official DLCs, but for safety
                {
                    var albumSmallArtEntry = archive.TOC.FirstOrDefault(entry => entry.Name.EndsWith("64.dds")); //Get album art paths
                    var albumMidArtEntry = archive.TOC.FirstOrDefault(entry => entry.Name.EndsWith("128.dds"));
                    var albumBigArtEntry = archive.TOC.FirstOrDefault(entry => entry.Name.EndsWith("256.dds"));
                    albumBigArtEntry.Data.Position = 0;


                    var largeDDS = newImg.ToDDS(256, 256);
                    var midDDS = newImg.ToDDS(128, 128);
                    var smallDDS = newImg.ToDDS(64, 64);

                    if (largeDDS == null || midDDS == null || smallDDS == null)
                        throw new Exception("unable to convert image steams.");
                    MemoryStream largeImg = null;
                    if (isTagged)
                    {
                        albumBigArtEntry.Data.Position = 0;
                        largeImg = new MemoryStream();
                        albumBigArtEntry.Data.CopyTo(largeImg);
                    }

                    albumSmallArtEntry.Data.Dispose();
                    albumSmallArtEntry.Data = smallDDS;

                    albumMidArtEntry.Data.Dispose();
                    albumMidArtEntry.Data = midDDS;

                    albumBigArtEntry.Data.Dispose();
                    albumBigArtEntry.Data = largeDDS;

  
                    // //If we add only "Tagged" without "true",
                    ////GetToolkitInfo might think that this is actually toolkit version -> it would probably cause problems
                    if (isTagged)
                    {
                        if (orgDss != null)
                            archive.TOC.Remove(orgDss);
                    }
                    else
                    {
                        if (orgDss == null)
                        {
                            orgDss = new Entry() { Name = "original_dss", Data = largeImg };
                            archive.AddEntry(orgDss);
                        }
                        else
                        {
                            orgDss.Data.Dispose();
                            orgDss.Data = largeImg;
                        }
                    }

                    songPath = song.Path;

                    using (var FS = File.Create(songPath))
                        archive.Write(FS, true);
                    song.Path = songPath;
                    song.Tagged = isTagged;
                }
              
            }

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
