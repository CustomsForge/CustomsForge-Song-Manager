using System;
using System.IO;
using System.Windows.Forms;
using RocksmithToolkitLib.DLCPackage;
using RocksmithToolkitLib.PSARC;

namespace CustomsForgeSongManager.SongEditor
{
    //public interface IAttributesEditor
    //{
    //    void EditSongAttributes(RocksmithToolkitLib.DLCPackage.Manifest2014.Attributes2014 attr);
    //}

    public class DLCPackageEditorControlBase : UserControl
    {
        public DLCPackageEditorControlBase()
        {
            Dirty = false;
        }

        public string FilePath;
        public DLCPackageData SongData;
        public virtual bool Dirty { get; set; }
        public virtual bool HaltOnError { get; set; }

        public virtual void Save()
        {
            Dirty = false;
        }

        public virtual bool AfterSave(PSARC packagePath)
        {
            Dirty = false;
            return false;
        }

        public virtual bool NeedsAfterSave()
        {
            return false;
        }

        public virtual void DoInit()
        {
        }

        public string TempToolkitPath
        {
            get
            {
                if (SongData != null)
                {
                    if (String.IsNullOrEmpty(SongData.AlbumArtPath))
                        return Path.GetDirectoryName(SongData.OggPath);
                    return Path.GetDirectoryName(SongData.AlbumArtPath);
                }
                return string.Empty;
            }
        }

        public string TempEOFPath
        {
            get
            {
                if (SongData != null)
                    return Path.GetDirectoryName(SongData.OggPath);
                return string.Empty;
            }
        }
    }
}