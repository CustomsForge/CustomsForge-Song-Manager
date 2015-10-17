using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using CustomsForgeManagerTools;
using CustomsForgeManager.CustomsForgeManagerLib.Objects;
using System.IO;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using CustomsForgeManager.CustomsForgeManagerLib;
using RocksmithToolkitLib.DLCPackage;
using RocksmithToolkitLib.Extensions;

namespace CustomsForgeManager.UControls
{
    public partial class Tagger : UserControl
    {
        Stopwatch stopWatch = new Stopwatch();
        private string[] defaultFiles = { "info.txt", "Background.png", "Bass Bonus.png", "Bass.png", "Custom.png", "Lead Bonus.png", "Lead.png", "Rhythm Bonus.png", "Rhythm.png", "Vocal.png" };
        private string[] defaultTagFolders = { "frackDefault", "motive_bl_", "motive_nv_", "motive_ws_", "motive1" };

        int songCount = 0;
        int counter = 0;
        string songExtractedPath = "";
        string manifestsFolderPath = "";
        string toolkitVersionFilePath = "";
        string albumArtFolderPath = "";
        string albumSmallArtPath = "";
        string albumMidArtPath = "";
        string albumBigArtPath = "";
        string tagsFolder = "frackDefault";
        string tagsFolderFullPath = Path.Combine(Constants.TaggerTemplatesFolder, "frackDefault");
        string taggedPreviewPath = "";
        string cleanPreviewPath = "";
        string workingFolderPath = "";
        string songPath = "";
        string pathExtension = "_";

        Bitmap smallAlbumArt;
        Bitmap midAlbumArt;
        Bitmap bigAlbumArt;
        DDSImage albumArtDDS;

        Bitmap backgroundLayer;
        Bitmap customTagLayer;
        Bitmap vocalLayer;
        Bitmap leadLayer;
        Bitmap rhythmLayer;
        Bitmap bassLayer;
        Bitmap leadBonusLayer;
        Bitmap rhythmBonusLayer;
        Bitmap bassBonusLayer;

        bool lead = false;
        bool rhythm = false;
        bool bass = false;
        bool vocals = false;
        bool bonusLead = false;
        bool bonusRhythm = false;
        bool bonusBass = false;
        bool DD = false;
        bool allSelected = false;
        bool songTagged = false;
        bool overwriteTags = false;

        public Tagger()
        {
            InitializeComponent();
        }

        public void PopulateTagger()
        {
            Globals.Log("Populating Tagger GUI...");

            if (!Directory.Exists(Constants.TaggerTemplatesFolder) || Extensions.IsDirectoryEmpty(Constants.TaggerTemplatesFolder))
                CreateDefaultFolders();

            LoadThemes();
            comboTagPacks.SelectedIndex = 0;
        }

        public void UpdateToolStrip()
        {
        }

        private void LoadThemes()
        {
            foreach (string tagPreview in Directory.EnumerateFiles(Constants.TaggerTemplatesFolder, "*.png").Where(file => file.ToLower().Contains("prev")))
            {
                comboTagPacks.Items.Add(Path.GetFileName(tagPreview).Replace(@"tags\", "").Replace("prev.png", ""));
            }
        }

        private bool HasDD(SongData song)
        {
            if (song.DD != 0)
                return true;
            else
                return false;
        }

        //Credits to Mark from StackOverflow
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }
            return destImage;
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

        private void btnTagDLCs_Click(object sender, EventArgs e)
        {
            pathExtension = "_";
            counter = 0;

            try
            {
                if (!Directory.Exists(Constants.TaggerTemplatesFolder) || !Directory.Exists(tagsFolderFullPath))
                    CreateDefaultFolders();

                backgroundLayer = new Bitmap(Path.Combine(tagsFolderFullPath, "Background.png"));
                customTagLayer = new Bitmap(Path.Combine(tagsFolderFullPath, "Custom.png"));
                vocalLayer = new Bitmap(Path.Combine(tagsFolderFullPath, "Vocal.png"));
                leadLayer = new Bitmap(Path.Combine(tagsFolderFullPath, "Lead.png"));
                rhythmLayer = new Bitmap(Path.Combine(tagsFolderFullPath, "Rhythm.png"));
                bassLayer = new Bitmap(Path.Combine(tagsFolderFullPath, "Bass.png"));
                leadBonusLayer = new Bitmap(Path.Combine(tagsFolderFullPath, "Lead Bonus.png"));
                rhythmBonusLayer = new Bitmap(Path.Combine(tagsFolderFullPath, "Rhythm Bonus.png"));
                bassBonusLayer = new Bitmap(Path.Combine(tagsFolderFullPath, "Bass Bonus.png"));

                overwriteTags = checkOverwriteTagsOnTaggedSongs.Checked;
                songCount = Globals.SongCollection.Where(song => !song.Tagged).Count();

                lblStatus.Text = "Tagged: 0/" + songCount;

                // if (Globals.SongCollection.Where(song => song.Tagged).Count() == Globals.SongCollection.Count() && !overwriteTags) //Check if all songs have been tagged before
                if (0 == 0) // just a place holder for the if check above
                {
                    if (MessageBox.Show("Do you really want to tag all your CDLC songs?", "Tag CDLCs?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        Globals.Log("CDLC tagging started...");
                        foreach (SongData song in Globals.SongCollection.ToList())
                        {
                            songTagged = songTagged || File.GetCreationTime(song.Path) == new DateTime(1990, 1, 1) ? true : false;

                            if (File.Exists(song.Path)) //Just to be sure :)
                            {
                                if (!songTagged || (songTagged && overwriteTags))
                                {
                                    pathExtension = "_";

                                    songExtractedPath = Path.Combine(Constants.TaggerExtractedFolder, Path.GetFileName(song.Path.Replace(".psarc", "_Pc")));
                                    manifestsFolderPath = Path.Combine(songExtractedPath, "manifests");
                                    albumArtFolderPath = Path.Combine(songExtractedPath, "gfxassets", "album_art");
                                    toolkitVersionFilePath = Path.Combine(songExtractedPath, "toolkit.version");
                                    taggedPreviewPath = Path.Combine(Constants.TaggerPreviewsFolder, Path.GetFileName(song.Path.Replace("_p.psarc", "")) + "_tagged.png");
                                    cleanPreviewPath = Path.Combine(Constants.TaggerPreviewsFolder, Path.GetFileName(song.Path.Replace("_p.psarc", "")) + "_clean.png");

                                    Packer.Unpack(song.Path, Constants.TaggerExtractedFolder);

                                    if (File.Exists(toolkitVersionFilePath)) //Very unlikely to happen since we don't parse official DLCs, but for safety
                                    {
                                        albumSmallArtPath = Directory.EnumerateFiles(albumArtFolderPath, "*64.dds").ToList()[0]; //Get album art paths
                                        albumMidArtPath = Directory.EnumerateFiles(albumArtFolderPath, "*128.dds").ToList()[0];
                                        albumBigArtPath = Directory.EnumerateFiles(albumArtFolderPath, "*256.dds").ToList()[0];

                                        albumArtDDS = new DDSImage(File.ReadAllBytes(albumBigArtPath));
                                        bigAlbumArt = albumArtDDS.images[0];

                                        midAlbumArt = ResizeImage(bigAlbumArt, 128, 128); //Make a copy of the biggest album art .dds (other two might already have tags on them) 
                                        smallAlbumArt = ResizeImage(bigAlbumArt, 64, 64);

                                        midAlbumArt.Save("albumMidArt.png", ImageFormat.Png);
                                        File.Copy("albumMidArt.png", cleanPreviewPath, true);
                                        File.Delete("albumMidArt.png");

                                        //Check which arrangements it contains
                                        lead = false;
                                        rhythm = false;
                                        bass = false;
                                        vocals = false;
                                        bonusLead = false;
                                        bonusRhythm = false;
                                        bonusBass = false;
                                        DD = false;

                                        string[] arrangements = song.Arrangements.Split(',');

                                        foreach (string arrangement in arrangements.Cast<string>().Select(arr => arr.ToLower()).ToList())
                                        {
                                            if (arrangement.Contains("lead") && !arrangement.Contains("lead2"))
                                            {
                                                lead = true;
                                                pathExtension += "L";
                                            }
                                            if (arrangement.Contains("lead2"))
                                            {
                                                bonusLead = true;
                                                pathExtension += "l";
                                            }
                                            if (arrangement.Contains("rhythm") && !arrangement.Contains("rhythm2"))
                                            {
                                                rhythm = true;
                                                pathExtension += "R";
                                            }
                                            if (arrangement.Contains("rhythm2"))
                                            {
                                                bonusRhythm = true;
                                                pathExtension = "r";
                                            }
                                            if (arrangement.Contains("bass") && !arrangement.Contains("bass2"))
                                            {
                                                bass = true;
                                                pathExtension += "B";
                                            }
                                            if (arrangement.Contains("bass2"))
                                            {
                                                bonusBass = true;
                                                pathExtension += "b";
                                            }
                                            if (arrangement.Contains("vocals"))
                                            {
                                                vocals = true;
                                                pathExtension += "V";
                                            }
                                        }

                                        //Add layers to big album art
                                        using (Graphics gra = Graphics.FromImage(midAlbumArt))
                                        {
                                            gra.DrawImage(backgroundLayer, 0, 0.5f);
                                            if (vocals)
                                                gra.DrawImage(vocalLayer, 0, 0.5f);
                                            if (bass)
                                                gra.DrawImage(bassLayer, 0, 0.5f);
                                            if (bonusBass)
                                                gra.DrawImage(bassBonusLayer, 0, 0.5f);
                                            if (rhythm)
                                                gra.DrawImage(rhythmLayer, 0, 0.5f);
                                            if (bonusRhythm)
                                                gra.DrawImage(rhythmBonusLayer, 0, 0.5f);
                                            if (lead)
                                                gra.DrawImage(leadLayer, 0, 0.5f);
                                            if (bonusLead)
                                                gra.DrawImage(leadBonusLayer, 0, 0.5f);
                                            gra.DrawImage(customTagLayer, 0, 0.5f);
                                        }

                                        //Draw layers to small album art
                                        using (Graphics gra = Graphics.FromImage(smallAlbumArt))
                                        {
                                            gra.DrawImage(new Bitmap(backgroundLayer, backgroundLayer.Width / 2, backgroundLayer.Height / 2), 0, 1.0f);
                                            if (vocals)
                                                gra.DrawImage(new Bitmap(vocalLayer, vocalLayer.Width / 2, vocalLayer.Height / 2), 0, 1.0f);
                                            if (bass)
                                                gra.DrawImage(new Bitmap(bassLayer, vocalLayer.Width / 2, vocalLayer.Height / 2), 0, 1.0f);
                                            if (bonusBass)
                                                gra.DrawImage(new Bitmap(bassBonusLayer, vocalLayer.Width / 2, vocalLayer.Height / 2), 0, 1.0f);
                                            if (rhythm)
                                                gra.DrawImage(new Bitmap(rhythmLayer, vocalLayer.Width / 2, vocalLayer.Height / 2), 0, 1.0f);
                                            if (bonusRhythm)
                                                gra.DrawImage(new Bitmap(rhythmLayer, vocalLayer.Width / 2, vocalLayer.Height / 2), 0, 1.0f);
                                            if (lead)
                                                gra.DrawImage(new Bitmap(leadLayer, vocalLayer.Width / 2, vocalLayer.Height / 2), 0, 1.0f);
                                            if (bonusLead)
                                                gra.DrawImage(new Bitmap(leadBonusLayer, vocalLayer.Width / 2, vocalLayer.Height / 2), 0, 1.0f);
                                            gra.DrawImage(new Bitmap(customTagLayer, customTagLayer.Width / 2, customTagLayer.Height / 2), 0, 1.0f);
                                        }

                                        //Delete existing album art
                                        if (File.Exists(albumMidArtPath))
                                            File.Delete(albumMidArtPath);

                                        if (File.Exists(albumSmallArtPath))
                                            File.Delete(albumSmallArtPath);

                                        //Save modified album art
                                        midAlbumArt.Save("albumMidArt.png", ImageFormat.Png);

                                        ExternalApps.Png2Dds("albumMidArt.png", albumMidArtPath, 128, 128);
                                        File.Copy("albumMidArt.png", taggedPreviewPath, true);

                                        File.Delete("albumMidArt.png");

                                        smallAlbumArt.Save("albumSmallArt.png", ImageFormat.Png);

                                        ExternalApps.Png2Dds("albumSmallArt.png", albumSmallArtPath, 64, 64);

                                        File.Delete("albumSmallArt.png");

                                        // Delete existing song & repack it
                                        if (File.Exists(song.Path))
                                            File.Delete(song.Path);

                                        //Add file name tags if user wants to add tags to the file name, too
                                        if (checkAddTagsToFileName.Checked)
                                            songPath = song.Path.Replace("_p.", pathExtension + "_p.");
                                        else
                                            songPath = song.Path;

                                        File.AppendAllText(toolkitVersionFilePath, "\nTagged: true"); //If we add only "Tagged" without "true",
                                        //GetToolkitInfo might think that this is actually toolkit version -> it would probably cause problems

                                        Packer.Pack(songExtractedPath, songPath);

                                        //Delete extracted folders if needed
                                        if (checkDeleteExtractedWhenDone.Checked)
                                            Directory.Delete(songExtractedPath, true);

                                        var songVar = Globals.SongCollection.FirstOrDefault(sng => sng.Path == songPath);
                                        songVar.Path = songPath;
                                        songVar.Tagged = true;

                                        counter++;
                                        lblStatus.Text = "Tagged: " + counter + "/" + songCount;
                                    }
                                    else
                                    {
                                        Directory.Delete(songExtractedPath, true);
                                        songCount -= 1;
                                    }
                                }
                            }
                        }
                        Globals.Log("CDLC tagging done!");
                        MessageBox.Show("CDLC tagging done!", "Done!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                    MessageBox.Show("All CDLC songs in your song collection have been tagged before, if you want to tag them again, tick \"Overwrite tags on tagged songs\" and try again!",
                                    "All songs tagged", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (ArgumentException)
            {
                MessageBox.Show("Make sure that you have all required files in the CFM\tags folder: \n" +
                                "-tags/" + tagsFolder + "/Background.png \n" +
                                "-tags/" + tagsFolder + "/Lead.png \n" +
                                "-tags/" + tagsFolder + "/Lead Bonus.png \n" +
                                "-tags/" + tagsFolder + "/Rhythm.png \n" +
                                "-tags/" + tagsFolder + "/Rhythm Bonus.png \n" +
                                "-tags/" + tagsFolder + "/Custom.png \n" +
                                "-tags/" + tagsFolder + "/Vocal.png");
            }
            catch (IOException ex)
            {
                MessageBox.Show("Error: \n\n" + ex.Message.ToString(), "IO Error");
            }
        }

        private void comboTagPacks_SelectedIndexChanged(object sender, EventArgs e)
        {
            string packName = comboTagPacks.SelectedItem.ToString();
            string packFolder = Path.Combine("tags", packName);
            string preview = Path.Combine("tags", packName + "prev_tagged.png");
            string info = Path.Combine("tags", packName, "info.txt");

            if (Directory.Exists(packFolder))
            {
                tagsFolder = packFolder.Replace(@"tags\", "");
                tagsFolderFullPath = Path.Combine(Constants.TaggerTemplatesFolder, tagsFolder);

                if (File.Exists(preview))
                {
                    pictureBoxPreview.Image = Bitmap.FromFile(preview);
                }

                if (File.Exists(info))
                    tbThemeLegend.Text = File.ReadAllText(info);
            }
        }

        private void btnRemoveTags_Click(object sender, EventArgs e)
        {
            counter = 0;

            try
            {
                songCount = Globals.SongCollection.Where(song => song.Tagged).Count();
                lblStatus.Text = "Tags removed on: 0/" + songCount;

                if (songCount > 0)
                {
                    if (MessageBox.Show("Do you really want to remove tags from your CDLC songs?", "Untag CDLCs?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        Globals.Log("Removing tags on your CDLC songs!");
                        foreach (SongData song in Globals.SongCollection.ToList())
                        {
                            if (File.Exists(song.Path))
                            {
                                songTagged = song.Tagged || File.GetCreationTime(song.Path) == new DateTime(1990, 1, 1) ? true : false;

                                if (songTagged)
                                {
                                    songExtractedPath = Path.Combine(Constants.TaggerExtractedFolder, Path.GetFileName(song.Path.Replace(".psarc", "_Pc")));
                                    manifestsFolderPath = Path.Combine(songExtractedPath, "manifests");
                                    albumArtFolderPath = Path.Combine(songExtractedPath, "gfxassets", "album_art");
                                    toolkitVersionFilePath = Path.Combine(songExtractedPath, "toolkit.version");
                                    taggedPreviewPath = Path.Combine(Constants.TaggerPreviewsFolder, Path.GetFileName(song.Path.Replace("_p.psarc", "")) + "_tagged.png");

                                    Packer.Unpack(song.Path, Constants.TaggerExtractedFolder);

                                    if (File.Exists(toolkitVersionFilePath))
                                    {
                                        string songPath = song.Path;

                                        albumSmallArtPath = Directory.EnumerateFiles(albumArtFolderPath, "*64.dds").ToList()[0];
                                        albumMidArtPath = Directory.EnumerateFiles(albumArtFolderPath, "*128.dds").ToList()[0];
                                        albumBigArtPath = Directory.EnumerateFiles(albumArtFolderPath, "*256.dds").ToList()[0];

                                        albumArtDDS = new DDSImage(File.ReadAllBytes(albumBigArtPath));
                                        bigAlbumArt = albumArtDDS.images[0];

                                        midAlbumArt = ResizeImage(bigAlbumArt, 128, 128);
                                        smallAlbumArt = ResizeImage(bigAlbumArt, 64, 64);

                                        //Delete existing album art
                                        if (File.Exists(albumMidArtPath))
                                            File.Delete(albumMidArtPath);

                                        if (File.Exists(albumSmallArtPath))
                                            File.Delete(albumSmallArtPath);

                                        //Save modified album art
                                        albumArtDDS.images[0] = midAlbumArt;
                                        albumArtDDS.images[0].Save("albumMidArt.png", ImageFormat.Png);

                                        ExternalApps.Png2Dds("albumMidArt.png", albumMidArtPath, 128, 128);

                                        if (!Directory.Exists(Path.Combine(workingFolderPath, "previews")))
                                            Directory.CreateDirectory(Path.Combine(workingFolderPath, "previews"));
                                        File.Copy("albumMidArt.png", taggedPreviewPath, true);
                                        File.Delete("albumMidArt.png");

                                        albumArtDDS.images[0] = smallAlbumArt;
                                        albumArtDDS.images[0].Save("albumSmallArt.png", ImageFormat.Png);

                                        ExternalApps.Png2Dds("albumSmallArt.png", albumSmallArtPath, 64, 64);

                                        File.Delete("albumSmallArt.png");

                                        //Delete existing song & repack it
                                        if (File.Exists(song.Path))
                                            File.Delete(song.Path);

                                        //Replace arrangement tags in file name, if they exist
                                        string[] split = song.Path.Split('_');
                                        foreach (string part in split)
                                        {
                                            if (!part.Except("LlVvBbRr-").Any())
                                            {
                                                songPath = songPath.Replace(part, "-").Replace("_-", "").Replace("-_p.psarc", "_p.psarc");
                                            }
                                        }

                                        //Just in case that the changes don't go too well, remove "-" again
                                        if (songPath.EndsWith("-_p.psarc"))
                                            songPath = songPath.Replace("-_p.psarc", "_p.psarc");

                                        if (songPath.EndsWith("-_p.disabled.psarc"))
                                            songPath = songPath.Replace("-_p.disabled.psarc", "_p.disabled.psarc");

                                        var lines = File.ReadAllLines(toolkitVersionFilePath);
                                        var newLines = lines.Where(line => !line.Contains("Tagged")).ToList();
                                        File.WriteAllLines(toolkitVersionFilePath, newLines);

                                        Packer.Pack(songExtractedPath, songPath);

                                        if (File.GetCreationTime(song.Path) == new DateTime(1990, 1, 1))
                                            File.SetCreationTime(songPath, DateTime.Now);

                                        var songVar = Globals.SongCollection.FirstOrDefault(sng => sng.Path == song.Path);
                                        songVar.Path = songPath;

                                        counter += 1;
                                        lblStatus.Text = "Tags removed on: " + counter + "/" + songCount;

                                        //Delete extracted folders if needed
                                        if (checkDeleteExtractedWhenDone.Checked)
                                            Directory.Delete(songExtractedPath, true);
                                    }
                                    else
                                        songCount -= 1;
                                }
                            }
                        }

                        Globals.Log("Removing tags done!");
                        MessageBox.Show("Tags removed from your CDLC songs!", "Done!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (ArgumentException)
            {
                MessageBox.Show("Make sure that you have all required files in the CFM\tags folder: \n" +
                                "-tags/" + tagsFolder + "/Background.png \n" +
                                "-tags/" + tagsFolder + "/Lead.png \n" +
                                "-tags/" + tagsFolder + "/Lead Bonus.png \n" +
                                "-tags/" + tagsFolder + "/Rhythm.png \n" +
                                "-tags/" + tagsFolder + "/Rhythm Bonus.png \n" +
                                "-tags/" + tagsFolder + "/Custom.png \n" +
                                "-tags/" + tagsFolder + "/Vocal.png");
            }
            catch (IOException ex)
            {
                MessageBox.Show("Error: \n\n" + ex.Message.ToString(), "IO Error");
            }
        }

        private void btnPreviewSelected_Click(object sender, EventArgs e) //TODO: figure out if we can use this one
        {
            try
            {
                backgroundLayer = new Bitmap(Path.Combine(Constants.TaggerTemplatesFolder, tagsFolder, "Background.png"));
                customTagLayer = new Bitmap(Path.Combine(Constants.TaggerTemplatesFolder, tagsFolder, "Custom.png"));
                vocalLayer = new Bitmap(Path.Combine(Constants.TaggerTemplatesFolder, tagsFolder, "Vocal.png"));
                leadLayer = new Bitmap(Path.Combine(Constants.TaggerTemplatesFolder, tagsFolder, "Lead.png"));
                rhythmLayer = new Bitmap(Path.Combine(Constants.TaggerTemplatesFolder, tagsFolder, "Rhythm.png"));
                bassLayer = new Bitmap(Path.Combine(Constants.TaggerTemplatesFolder, tagsFolder, "Bass.png"));
                leadBonusLayer = new Bitmap(Path.Combine(Constants.TaggerTemplatesFolder, tagsFolder, "Lead Bonus.png"));
                rhythmBonusLayer = new Bitmap(Path.Combine(Constants.TaggerTemplatesFolder, tagsFolder, "Rhythm Bonus.png"));
                bassBonusLayer = new Bitmap(Path.Combine(Constants.TaggerTemplatesFolder, tagsFolder, "Bass Bonus.png"));

                string songPath = "";

                songExtractedPath = Path.Combine(Constants.TaggerExtractedFolder, Path.GetFileName(songPath.Replace(".psarc", "_Pc")));
                manifestsFolderPath = Path.Combine(songExtractedPath, "manifests");
                albumArtFolderPath = Path.Combine(songExtractedPath, "gfxassets", "album_art");
                toolkitVersionFilePath = Path.Combine(songExtractedPath, "toolkit.version");

                Packer.Unpack(songPath, Constants.TaggerExtractedFolder);

                if (File.Exists(toolkitVersionFilePath))
                {
                    albumBigArtPath = Directory.EnumerateFiles(albumArtFolderPath, "*256.dds").ToList()[0];

                    albumArtDDS = new DDSImage(File.ReadAllBytes(albumBigArtPath));
                    midAlbumArt = ResizeImage(albumArtDDS.images[0], 128, 128);

                    lead = false;
                    rhythm = false;
                    bass = false;
                    vocals = false;
                    bonusLead = false;
                    bonusRhythm = false;
                    bonusBass = false;

                    var arrangements = Directory.EnumerateFiles(manifestsFolderPath, "*.json", SearchOption.AllDirectories);

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

                    using (Graphics gra = Graphics.FromImage(midAlbumArt))
                    {
                        gra.DrawImage(backgroundLayer, 0, 0.5f);
                        if (vocals)
                            gra.DrawImage(vocalLayer, 0, 0.5f);
                        if (bass)
                            gra.DrawImage(bassLayer, 0, 0.5f);
                        if (bonusBass)
                            gra.DrawImage(bassBonusLayer, 0, 0.5f);
                        if (rhythm)
                            gra.DrawImage(rhythmLayer, 0, 0.5f);
                        if (bonusRhythm)
                            gra.DrawImage(rhythmBonusLayer, 0, 0.5f);
                        if (lead)
                            gra.DrawImage(leadLayer, 0, 0.5f);
                        if (bonusLead)
                            gra.DrawImage(leadBonusLayer, 0, 0.5f);
                        gra.DrawImage(customTagLayer, 0, 0.5f);
                    }

                    pictureBoxPreview.Image = midAlbumArt;
                    //Clear dirs
                    Directory.Delete(songExtractedPath, true);
                }
            }
            catch (ArgumentException)
            {
                MessageBox.Show("Make sure that you have all required files in the app folder: \n" +
                                "-tags/" + tagsFolder + "/Background.png \n" +
                                "-tags/" + tagsFolder + "/Lead.png \n" +
                                "-tags/" + tagsFolder + "/Lead Bonus.png \n" +
                                "-tags/" + tagsFolder + "/Rhythm.png \n" +
                                "-tags/" + tagsFolder + "/Rhythm Bonus.png \n" +
                                "-tags/" + tagsFolder + "/Custom.png \n" +
                                "-tags/" + tagsFolder + "/Vocal.png");
            }
        }
    }
}
