using System;
using System.IO;

namespace CustomsForgeSongManager.DataObjects
{
    public class ProfileData
    {
        public bool Selected { get; set; }
        public DateTime ArchiveDate { get; set; }
        public string ArchivePath { get; set; }
        public string ArchiveName
        {
            get { return (Path.GetFileName(ArchivePath)); }
        }

    }
}
