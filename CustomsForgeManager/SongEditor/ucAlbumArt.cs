using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System.Linq;
using CustomsForgeManagerTools;

namespace CustomsForgeManager.SongEditor
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
                string artFile = Directory.GetFiles(TempToolkitPath, "*_256.dds").FirstOrDefault();
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
}
