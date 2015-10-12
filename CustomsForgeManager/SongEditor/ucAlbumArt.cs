using System;
using System.Collections.Generic;
using System.IO;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
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
                    using(var FS = File.OpenRead(artFile))
                    using (var MS = new MemoryStream())
                    {
                        if (DevILLite.ConvertImageType(FS, DevILLite.ImageType.Bmp, MS))
                        {
                            MS.Position = 0;
                            var image = Image.FromStream(MS);
                            picAlbumArt.Image = image;
                        }
                    }
                }


            }
        }
    }
}
