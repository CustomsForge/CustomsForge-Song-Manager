using System;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CFSM.ImageTools;
using CFSM.RSTKLib.PSARC;
using CustomsForgeSongManager.DataObjects;

namespace CustomsForgeSongManager.SongEditor
{
    public partial class ucAlbumArt : DLCPackageEditorControlBase
    {
        public ucAlbumArt()
        {
            InitializeComponent();
        }

        public override void DoInit()
        {
            base.DoInit();
            LoadAlbumArt();
        }

        public void LoadAlbumArt()
        {
            if (SongData != null)
            {
                var artFile = Directory.GetFiles(TempToolkitPath, "*_256.dds").FirstOrDefault();
                if (artFile != null)
                {
                    if (!string.IsNullOrEmpty(artFile))
                    {
                        byte[] data = File.ReadAllBytes(artFile);
                        DDSImage dds = new DDSImage(data);
                        if (dds.images.Length > 0)
                            picAlbumArt.Image = dds.images[0];
                    }
                }
            }
        }

        public override void Save()
        {
            //dont' do anything, Dirty needs to be true for NeedsAfterSave
        }

        public override bool NeedsAfterSave()
        {
            return Dirty;
        }

        // TODO: consider move to PsarcExtensions.cs
        private bool ReplaceImagesExtracted(PSARC p, string imgName, int width, int height)
        {
            var newImageStream = picAlbumArt.Image.ToDDS(width, height);

            if (newImageStream.Length == 0)
            {
                Globals.Log(String.Format("Unable to convert {0}x{1} image.", width, height));
                return false;
            }

            newImageStream.Position = 0;

            var ent = p.TOC.Where(entry => entry.Name.ToLower().Equals(imgName.ToLower())).FirstOrDefault();
            if (ent == null)
            {
                ent = new Entry() {Name = imgName, Data = newImageStream};
                p.AddEntry(ent);
            }
            else
            {
                ent.Data.Dispose();
                ent.Data = newImageStream;
            }
            return true;
        }

        private bool ReplaceImages(PSARC p)
        {
            var x = "gfxassets/album_art/album_{0}_{1}.dds";
            var large = String.Format(x, this.SongData.Name.ToLower(), 256);
            var mid = String.Format(x, this.SongData.Name.ToLower(), 128);
            var small = String.Format(x, this.SongData.Name.ToLower(), 64);

            if (!ReplaceImagesExtracted(p, large, 256, 256))
                return false;

            if (!ReplaceImagesExtracted(p, mid, 128, 128))
                return false;

            if (!ReplaceImagesExtracted(p, small, 64, 64))
                return false;

            return true;
        }

        public override bool AfterSave(PSARC archive)
        {
            if (Dirty)
            {
                Dirty = false;
                return ReplaceImages(archive);
            }
            return base.AfterSave(archive);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var od = new OpenFileDialog() {Filter = "Image Files|*.bmp;*.gif;*.jpeg;*.jpg;*.png;*.dds"})
            {
                if (od.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        if (Path.GetExtension(od.FileName).ToLower() == ".dds")
                        {
                            using (var fs = File.OpenRead(od.FileName))
                            {
                                using (var img = ImageExtensions.DDStoBitmap(fs))
                                    picAlbumArt.Image = img.ScaleImage(256);
                            }
                        }
                        else
                        {
                            var art = Image.FromFile(od.FileName);
                            if (art.Width > 256 || art.Height > 256)
                            {
                                var resizeart = art.ScaleImage(256);
                                art.Dispose();
                                art = resizeart;
                            }
                            picAlbumArt.Image = art;
                        }
                    }
                    catch (Exception ex)
                    {
                        Globals.Log(String.Format("{0}: {1}", Properties.Resources.ERROR, ex.Message));
                        return;
                    }
                    Dirty = true;
                }
            }
        }
    }
}