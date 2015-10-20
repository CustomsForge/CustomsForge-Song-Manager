using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using CFMImageTools;
using CustomsForgeManager.CustomsForgeManagerLib.Objects;
using System.IO;
using CustomsForgeManager.CustomsForgeManagerLib;
using System.Windows.Forms.Design;
using RocksmithToolkitLib.PSARC;

namespace CustomsForgeManager.UControls
{

    public partial class Tagger : UserControl
    {
    #if TAGGER
        Stopwatch stopWatch = new Stopwatch();
        private string[] defaultFiles = { "info.txt", "Background.png", "Bass Bonus.png", "Bass.png", "Custom.png", "Lead Bonus.png", "Lead.png", "Rhythm Bonus.png", "Rhythm.png", "Vocal.png" };
        private string[] defaultTagFolders = { "frackDefault", "motive_bl_", "motive_nv_", "motive_ws_", "motive1" };

        int songCount = 0;
        int counter = 0;

        string tagsFolder = "frackDefault";
        string tagsFolderFullPath = Path.Combine(Constants.TaggerTemplatesFolder, "frackDefault");
        string pathExtension = "_";


        bool allSelected = false;
        bool songTagged = false;
        bool deleteExtractedWhenDone = false;
        bool addTagsToFilename = false;
        bool overwriteTags = false;


        private ToolStrip ThisToolstrip;
        private ToolStripComboBox themeCombo;

        public Tagger()
        {
            InitializeComponent();
            themeCombo = new ToolStripComboBox("themeCombo")
            {
                ToolTipText = "Tag packs",
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            themeCombo.SelectedIndexChanged += comboTagPacks_SelectedIndexChanged;
        }

        public void PopulateTagger()
        {
            Globals.Log("Populating Tagger GUI...");

            if (!Directory.Exists(Constants.TaggerTemplatesFolder) || Extensions.IsDirectoryEmpty(Constants.TaggerTemplatesFolder))
                CreateDefaultFolders();

            LoadThemes();
            themeCombo.SelectedIndex = 0;
        }

        public void UpdateToolStrip()
        {
        }

        private void LoadThemes()
        {
            foreach (string tagPreview in
                Directory.EnumerateFiles(Constants.TaggerTemplatesFolder, "*.png").Where(
                file => file.ToLower().Contains("prev")))
                themeCombo.Items.Add(Path.GetFileName(tagPreview).Replace(@"Tagger\", "").Replace("prev.png", ""));
        }

        private bool HasDD(SongData song)
        {
            if (song.DD != 0)
                return true;
            else
                return false;
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

        private void ReplaceImages(SongData song, Bitmap newImg, bool isTagged)
        {
            pathExtension = "_";


            string songPath = song.Path;
            using (PSARC archive = new PSARC())
            {
                using (var fs = File.OpenRead(songPath))
                    archive.Read(fs);
                var toolKitEntry = archive.TOC.FirstOrDefault(entry => entry.Name == "toolkit.version");

                if (toolKitEntry != null) //Very unlikely to happen since we don't parse official DLCs, but for safety
                {
                    var albumSmallArtEntry = archive.TOC.FirstOrDefault(entry => entry.Name.EndsWith("64.dds")); //Get album art paths
                    var albumMidArtEntry = archive.TOC.FirstOrDefault(entry => entry.Name.EndsWith("128.dds"));
                    var albumBigArtEntry = archive.TOC.FirstOrDefault(entry => entry.Name.EndsWith("256.dds"));
                    albumBigArtEntry.Data.Position = 0;


                    var largeDDS = newImg.ToDDS(256, 256);
                    var midDDS = newImg.ToDDS(128, 128);
                    var smallDDS = newImg.ToDDS(64, 64);

                    if (largeDDS == null || midDDS == null || smallDDS == null )
                        throw new Exception("unable to convert image steams.");

                    albumSmallArtEntry.Data.Dispose();
                    albumSmallArtEntry.Data = smallDDS;

                    albumMidArtEntry.Data.Dispose();
                    albumMidArtEntry.Data = midDDS;

                    albumBigArtEntry.Data.Dispose();
                    albumBigArtEntry.Data = largeDDS;

                    toolKitEntry.Data.Position = 0;
                    string toolKitData = string.Empty;
                    using (StreamReader reader = new StreamReader(toolKitEntry.Data))
                    {
                        toolKitData = reader.ReadToEnd();
                    }
                    // //If we add only "Tagged" without "true",
                    ////GetToolkitInfo might think that this is actually toolkit version -> it would probably cause problems
                    if (isTagged)
                        toolKitData += "\nTagged: true";
                    else
                        toolKitData.Replace("\nTagged: true", "");
                    byte[] byteArray = Encoding.ASCII.GetBytes(toolKitData);
                    toolKitEntry.Data = new MemoryStream(byteArray);

                    ////Add file name tags if user wants to add tags to the file name, too
                    if (addTagsToFilename && isTagged)
                        songPath = song.Path.Replace("_p.", pathExtension + "_p.");
                    else
                        songPath = song.Path;

                    using (var FS = File.Create(songPath))
                    {
                        archive.Write(FS, true);
                    }

                    if (addTagsToFilename && isTagged)
                        File.Delete(song.Path);

                    song.Path = songPath;
                    song.Tagged = isTagged;

                    gbTags.Text = string.Format("Tagged: {0}/{1}", counter, songCount);
                }
                else
                {
                    songCount -= 1;
                }
            }

        }

        private void TagSong(SongData song, BitmapHolder images)
        {
            songTagged = songTagged || File.GetCreationTime(song.Path) == new DateTime(1990, 1, 1) ? true : false;

            if (File.Exists(song.Path)) //Just to be sure :)
            {
                if (!songTagged || (songTagged && overwriteTags))
                {
                    pathExtension = "_";

                    string songPath = song.Path;
                    using (PSARC archive = new PSARC())
                    {
                        using (var fs = File.OpenRead(songPath))
                            archive.Read(fs);
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
                            bool DD = false;
                            if (addTagsToFilename)
                            {
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
                            albumSmallArtEntry.Data = smallDDS;

                            albumMidArtEntry.Data.Dispose();
                            albumMidArtEntry.Data = midDDS;

                            albumBigArtEntry.Data.Dispose();
                            albumBigArtEntry.Data = largeDDS;

                            toolKitEntry.Data.Position = 0;
                            string toolKitData = string.Empty;
                            using (StreamReader reader = new StreamReader(toolKitEntry.Data))
                            {
                                toolKitData = reader.ReadToEnd();
                            }
                            // //If we add only "Tagged" without "true",
                            ////GetToolkitInfo might think that this is actually toolkit version -> it would probably cause problems
                            toolKitData += "\nTagged: true";
                            byte[] byteArray = Encoding.ASCII.GetBytes(toolKitData);
                            toolKitEntry.Data = new MemoryStream(byteArray);

                            archive.AddEntry("taggerOriginal.dds", orginalArtStream);

                            
                            ////Add file name tags if user wants to add tags to the file name, too
                            if (addTagsToFilename)
                                songPath = song.Path.Replace("_p.", pathExtension + "_p.");
                            else
                                songPath = song.Path;

                            using (var FS = File.Create(songPath))
                                archive.Write(FS, true);

                            if (addTagsToFilename)
                                File.Delete(song.Path);

                            song.Path = songPath;
                            song.Tagged = true;

                            counter++;
                            gbTags.Text = string.Format("Tagged: {0}/{1}", counter, songCount);
                        }
                        else
                        {
                            songCount -= 1;
                        }
                    }
                }
            }
        }


        public void TagSong(SongData song, string packName)
        {
            if (!Directory.Exists(Constants.TaggerTemplatesFolder) || !Directory.Exists(tagsFolderFullPath))
                CreateDefaultFolders();


            if (!String.IsNullOrEmpty(packName))
                tagsFolderFullPath = Path.Combine(Constants.TaggerTemplatesFolder, packName);

            using (var images = new BitmapHolder(tagsFolderFullPath))
                TagSong(song, images);
        }

        public void TagSongs(SongData[] song, string packName)
        {
            if (!Directory.Exists(Constants.TaggerTemplatesFolder) || !Directory.Exists(tagsFolderFullPath))
                CreateDefaultFolders();


            if (!String.IsNullOrEmpty(packName))
                tagsFolderFullPath = Path.Combine(Constants.TaggerTemplatesFolder, packName);

            using (var images = new BitmapHolder(tagsFolderFullPath))
                song.ToList().ForEach(sng => TagSong(sng, images));
        }


        private void btnTagDLCs_Click(object sender, EventArgs e)
        {
            pathExtension = "_";
            counter = 0;
            try
            {
                if (!Directory.Exists(Constants.TaggerTemplatesFolder) || !Directory.Exists(tagsFolderFullPath))
                    CreateDefaultFolders();
                //
                using (var images = new BitmapHolder(tagsFolderFullPath))
                {
                    songCount = Globals.SongCollection.Where(song => !song.Tagged).Count();

                    gbTags.Text = "Tagged: 0/" + songCount;

                    // if (Globals.SongCollection.Where(song => song.Tagged).Count() == Globals.SongCollection.Count() && !overwriteTags) //Check if all songs have been tagged before
                    if (0 == 0) // just a place holder for the if check above
                    {
                        if (MessageBox.Show("Do you really want to tag all your CDLC songs?", "Tag CDLCs?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            Globals.Log("CDLC tagging started...");
                            foreach (SongData song in Globals.SongCollection)
                                TagSong(song, images);
                            Globals.Log("CDLC tagging done!");
                            MessageBox.Show("CDLC tagging done!", "Done!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }

                    else
                        MessageBox.Show("All CDLC songs in your song collection have been tagged before, if you want to tag them again, tick \"Overwrite tags on tagged songs\" and try again!",
                                        "All songs tagged", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (ArgumentException)
            {
                ShowTaggerError();
            }
            catch (IOException ex)
            {
                MessageBox.Show("Error: \n\n" + ex.Message.ToString(), "IO Error");
            }
        }

        private void comboTagPacks_SelectedIndexChanged(object sender, EventArgs e)
        {
            tagsFolder = themeCombo.SelectedItem.ToString();
            string packFolder = Path.Combine(Constants.TaggerTemplatesFolder, tagsFolder);
            string preview = Path.Combine(Constants.TaggerTemplatesFolder, tagsFolder + "prev.png");
            string info = Path.Combine(Constants.TaggerTemplatesFolder, tagsFolder, "info.txt");

            if (Directory.Exists(packFolder))
            {
                tagsFolderFullPath = packFolder;
                if (File.Exists(preview))
                {
                    pictureBoxPreview.Image = Bitmap.FromFile(preview);
                }

                if (File.Exists(info))
                    tbThemeLegend.Text = File.ReadAllText(info);
            }
        }


        private void UntagSong(SongData song, BitmapHolder images)
        {
            if (File.Exists(song.Path))
            {
                songTagged = song.Tagged || File.GetCreationTime(song.Path) == new DateTime(1990, 1, 1) ? true : false;

                if (songTagged)
                {
                    using (PSARC archive = new PSARC())
                    {
                        using (var fs = File.OpenRead(song.Path))
                            archive.Read(fs);
                        var toolKitEntry = archive.TOC.FirstOrDefault(entry => entry.Name == "toolkit.version");
                        var taggerOriginal = archive.TOC.FirstOrDefault(entry => entry.Name == "taggerOriginal.dds");
                        if (toolKitEntry != null)
                        {
                            if (taggerOriginal != null)
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

                                toolKitEntry.Data.Position = 0;
                                string toolKitData = string.Empty;
                                using (StreamReader reader = new StreamReader(toolKitEntry.Data))
                                {
                                    toolKitData = reader.ReadToEnd();
                                }
                                toolKitData = toolKitData.Replace("\nTagged: true", "");
                                byte[] byteArray = Encoding.ASCII.GetBytes(toolKitData);
                                toolKitEntry.Data = new MemoryStream(byteArray);
                                using (var FS = File.Create(song.Path))
                                    archive.Write(FS, true);


                                song.Tagged = false;

                                counter++;
                                gbTags.Text = string.Format("Tagged: {0}/{1}", counter, songCount);

                            }
                        }
                    }
                }
            }
        }


        private void btnRemoveTags_Click(object sender, EventArgs e)
        {
            counter = 0;
            try
            {
                songCount = Globals.SongCollection.Where(song => song.Tagged).Count();
                gbTags.Text = "Tags removed on: 0/" + songCount;

                if (songCount > 0)
                {
                    if (MessageBox.Show("Do you really want to remove tags from your CDLC songs?", "Untag CDLCs?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        Globals.Log("Removing tags on your CDLC songs!");
                        using (var images = new BitmapHolder(tagsFolderFullPath))
                        {
                            foreach (SongData song in Globals.SongCollection.ToList())
                            {
                                UntagSong(song, images);
                            }
                        }
                        Globals.Log("Removing tags done!");
                        MessageBox.Show("Tags removed from your CDLC songs!", "Done!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (ArgumentException)
            {
                ShowTaggerError();
            }
            catch (IOException ex)
            {
                MessageBox.Show("Error: \n\n" + ex.Message.ToString(), "IO Error");
            }
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
                "-Tagger/{0}/Vocal.png", tagsFolder));
        }

        private void btnPreviewSelected_Click(object sender, EventArgs e)
        {
            try
            {
                using (var images = new BitmapHolder(Path.Combine(Constants.TaggerTemplatesFolder, tagsFolder)))
                {

                    var data = Globals.SongManager.GetFirstSelected();
                    if (data == null)
                    {
                        MessageBox.Show("No songs selected in Song Manager.");
                        return;
                    }

                    string songPath = data.Path;
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
                            bool DD = false;

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
                            pictureBoxPreview.Image = midAlbumArt;
                        }
                    }
                }
            }
            catch (ArgumentException)
            {
                ShowTaggerError();
            }
        }


        public void TabEnter()
        {
            //show menu
            CustomsForgeManager.Forms.frmMain f = Globals.MainForm;
            if (f != null)
            {
                f.tstripContainer.TopToolStripPanel.Visible = true;
                if (ThisToolstrip == null)
                {
                    List<ToolStripItem> items = new List<ToolStripItem>();

                    items.Add(themeCombo);

                    items.Add(new ToolStripSeparator());

                    ToolStripButton btn = new ToolStripButton("TagCDLC");
                    btn.Click += btnTagDLCs_Click;
                    items.Add(btn);

                    btn = new ToolStripButton("Remove tags");
                    btn.Click += btnRemoveTags_Click;
                    items.Add(btn);

                    //TODO:save preview button


                    btn = new ToolStripButton("Preview") { ToolTipText = "Preview selected song in Song Manager;" };
                    btn.Click += btnPreviewSelected_Click;
                    items.Add(btn);

                    items.Add(new ToolStripSeparator());

                    ToolStripCheckBox cb = new ToolStripCheckBox() { Text = "Delete extracted folders when done." };
                    cb.CheckedChanged += (s, e) =>
                    {
                        deleteExtractedWhenDone = ((ToolStripCheckBox)s).Checked;
                    };
                    items.Add(cb);

                    cb = new ToolStripCheckBox() { Text = "Add tags to file name." };
                    cb.CheckedChanged += (s, e) =>
                    {
                        addTagsToFilename = ((ToolStripCheckBox)s).Checked;
                    };
                    items.Add(cb);

                    cb = new ToolStripCheckBox() { Text = "Overwrite tags on tagged songs." };
                    cb.CheckedChanged += (s, e) =>
                    {
                        overwriteTags = ((ToolStripCheckBox)s).Checked;
                    };
                    items.Add(cb);


                    ThisToolstrip = new ToolStrip(items.ToArray());

                }

                f.tstripContainer.TopToolStripPanel.Join(ThisToolstrip, 0);
            }
        }


        public void TabLeave()
        {
            //hide the menu strip
            if (ThisToolstrip != null)
            {
                CustomsForgeManager.Forms.frmMain f = Globals.MainForm;
                if (f != null)
                {
                    f.tstripContainer.TopToolStripPanel.Controls.Remove(ThisToolstrip);
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


    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip)]
    public partial class ToolStripCheckBox : ToolStripControlHost
    {
        public CheckBox CheckBoxControl
        {
            get
            {
                return Control as CheckBox;
            }
        }

        /// <summary>
        /// Is check box ticked
        /// </summary>
        [Category("Appearance")]
        public bool Checked
        {
            get
            {
                return CheckBoxControl.Checked;
            }
            set
            {
                CheckBoxControl.Checked = value;
            }
        }

        /// <summary>
        /// Checked state
        /// </summary>
        [Category("Appearance")]
        public CheckState CheckState
        {
            get
            {
                return CheckBoxControl.CheckState;
            }
            set
            {
                CheckBoxControl.CheckState = value;
            }
        }

        /// <summary>
        /// Label text
        /// </summary>
        [Category("Appearance")]
        public override string Text
        {
            get
            {
                return CheckBoxControl.Text;
            }
            set
            {
                CheckBoxControl.Text = value;
            }
        }

        /// <summary>
        /// Occurs when check property is changed
        /// </summary>
        [Category("Misc")]
        public event EventHandler CheckedChanged;

        /// <summary>
        /// Occurs when check state of the control changes
        /// </summary>
        [Category("Misc")]
        public event EventHandler CheckStateChanged;

        public ToolStripCheckBox()
            : base(new CheckBox())
        {
            CheckBoxControl.MouseHover += new EventHandler(chk_MouseHover);
        }

        void chk_MouseHover(object sender, EventArgs e)
        {
            this.OnMouseHover(e);
        }

        protected override void OnSubscribeControlEvents(Control c)
        {
            base.OnSubscribeControlEvents(c);
            ((CheckBox)c).CheckedChanged += ToolStripCheckBox_CheckedChanged;
            ((CheckBox)c).CheckStateChanged += ToolStripCheckBox_CheckStateChanged;
        }

        protected override void OnUnsubscribeControlEvents(Control c)
        {
            base.OnUnsubscribeControlEvents(c);
            ((CheckBox)c).CheckedChanged -= ToolStripCheckBox_CheckedChanged;
            ((CheckBox)c).CheckStateChanged -= ToolStripCheckBox_CheckStateChanged;
        }

        void ToolStripCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckedChanged != null)
                CheckedChanged(this, e);
        }

        void ToolStripCheckBox_CheckStateChanged(object sender, EventArgs e)
        {
            if (CheckStateChanged != null)
                CheckStateChanged(this, e);
        }
#endif
    }
}
