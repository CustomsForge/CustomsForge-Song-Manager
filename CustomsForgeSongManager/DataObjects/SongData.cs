using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using CFSM.GenTools;
using DataGridViewTools;

namespace CustomsForgeSongManager.DataObjects
{
    public enum SongDataStatus : byte
    {
        None = 0,
        UpToDate = 1,
        OutDated = 2,
        NotFound = 3
    }

    public enum SongTaggerStatus : byte
    {
        [XmlEnum("0")]
        False = 0,
        [XmlEnum("1")]
        True = 1,
        [XmlEnum("2")]
        ODLC = 2
    }

    public enum RepairStatus : byte
    {
        NotRepaired = 0,
        Repaired = 1,
        RepairedDD = 2,
        RepairedMaxFive = 3,
        RepairedDDMaxFive = 4
    }

    //Static class seems more convenient to use, since the values are shown in DGV(s)
    //public static class RepairStatus
    //{
    //    public const string NotRepaired = "Not repaired";
    //    public const string Repaired = "Repaired";
    //    public const string RepairedDD = "Repaired + added DD";
    //    public const string RepairedMaxFive = "Repaired + fixed max 5 arr. error";
    //    public const string RepairedDDMaxFive = "Repaired + added DD + fixed max 5 arr. error";
    //}

    // only essential data needs to be saved to the XML songinfo file
    // order here determines order in xml file
    [Serializable]
    public class SongData : NotifyPropChangedBase
    {
        //ver 2 : SongKey changed to DLCKey.
        //ver 3 : removed DLCKey from arrangement
        //ver 4 : changed tagged to SongTaggerStatus
        //ver 5 : added ArtistSort TitleSort and AlbumSort variables
        //ver 6 : changed Path to FilePath to avoid conflict with reserved name System.IO.Path

        public const string SongDataListCurrentVersion = "6";

        public string DLCKey { get; set; }

        private bool _selected;
        [XmlIgnore]
        public bool Selected
        {
            get
            {
                // TODO: handle ODLC decisions at the datagrid level
                // allow non dgvMasterSongs ODLC to be deleted/moved/selected
                if (Globals.DgvCurrent.Name == "dgvMasterSongs")
                    return OfficialDLC ? false : _selected;

                return _selected;
            }
            set
            {
                if (!OfficialDLC || Globals.DgvCurrent.Name != "dgvMasterSongs")
                    _selected = value; // SetPropertyField("Selected", ref FSelected, value);              
                else
                    _selected = false;
            }
        }

        [XmlIgnore]
        public bool IsRsCompPack
        {
            get { return !String.IsNullOrEmpty(FilePath) && FilePath.Contains(Constants.RS1COMP); }
        }

        [XmlIgnore]
        public string Enabled
        {
            get { return (new FileInfo(FilePath).Name).ToLower().Contains("disabled") ? "No" : "Yes"; }
            set { } // required for XML file usage
        }

        public string AudioCache { get; set; }
        public string Artist { get; set; }
        public string ArtistSort { get; set; }
        public string Title { get; set; }
        public string TitleSort { get; set; }
        public string Album { get; set; }
        public string AlbumSort { get; set; }
        public Int32 SongYear { get; set; }
        public Single SongLength { get; set; }
        public Single SongAverageTempo { get; set; }
        public Single SongVolume { get; set; }
        public DateTime FileDate { get; set; }
        public int FileSize { get; set; }

        public void UpdateFileInfo()
        {
            var fi = new FileInfo(FilePath);
            FileDate = fi.LastWriteTimeUtc;
            FileSize = (int)fi.Length;
        }

        public void Delete()
        {
            if (!String.IsNullOrEmpty(AudioCache) && File.Exists(AudioCache))
                File.Delete(AudioCache);
            AudioCache = String.Empty;
            if (File.Exists(FilePath))
                File.Delete(FilePath);
        }

        // used by detail table
        [XmlArray("Arrangments")] // provides proper xml serialization
        [XmlArrayItem("Arrangement")] // provides proper xml serialization
        public FilteredBindingList<Arrangement> Arrangements2D { get; set; }

        public string FilePath { get; set; }

        [XmlIgnore]
        public string FileName
        {
            get { return (Path.Combine(Path.GetFileName(Path.GetDirectoryName(FilePath)), Path.GetFileName(FilePath))); }
            // set { } // required for XML file usage
        }

        [XmlIgnore]
        public string ArrangementInitials
        {
            get
            {
                string result = string.Empty;
                foreach (string arrangement in Arrangements2D.Select(x => x.Name.ToLower()).ToArray())
                {
                    if (arrangement.Contains("lead") && !arrangement.Contains("lead2"))
                        result += "L";
                    if (arrangement.Contains("lead2"))
                        result += "l";
                    if (arrangement.Contains("rhythm") && !arrangement.Contains("rhythm2"))
                        result += "R";
                    if (arrangement.Contains("rhythm2"))
                        result = "r";
                    if (arrangement.Contains("bass") && !arrangement.Contains("bass2"))
                        result += "B";
                    if (arrangement.Contains("bass2"))
                        result += "b";
                    if (arrangement.Contains("vocals"))
                        result += "V";
                    if (arrangement.Contains("combo"))
                        result += "C";
                }
                return result;
            }
        }

        [XmlIgnore]
        public bool OfficialDLC
        {
            get { return this.Tagged == SongTaggerStatus.ODLC; }
        }

        private string _charterName;
        public string CharterName
        {
            get
            {
                if (String.IsNullOrEmpty(_charterName))
                {
                    _charterName = "N/A";
                    if (OfficialDLC)
                        _charterName = "Ubisoft";
                }
                return _charterName;
            }
            set
            {
                _charterName = value;
            }
        }

        public DateTime LastConversionDateTime { get; set; }
        public string Version { get; set; }
        public string ToolkitVer { get; set; }
        public string AppID { get; set; }
        public SongDataStatus Status { get; set; }
        public string IgnitionID { get; set; }
        public string IgnitionAuthor { get; set; }
        public string IgnitionVersion { get; set; }
        public string IgnitionUpdated { get; set; }

        public SongTaggerStatus Tagged { get; set; }

        public RepairStatus RepairStatus { get; set; }

        [XmlIgnore]
        public string ArtistTitleAlbum
        {
            get { return String.Format("{0};{1};{2}", Artist, Title, Album); }
            // set { } // required for XML file usage
        }

        [XmlIgnore]
        public string ArtistTitleAlbumDate
        {
            get { return String.Format("{0};{1};{2};{3}", Artist, Title, Album, LastConversionDateTime.ToString("s")); }
            // set { } // required for XML file usage
        }


        [XmlIgnore] // preserves old 1D display method
        public string Arrangements
        {
            get { return String.Join(", ", Arrangements2D.Select(o => o.Name)); }
            set { }
        }

        [XmlIgnore] // preserves old 1D display method
        public string Tuning
        {
            get { return Arrangements2D.Select(o => o.Tuning).FirstOrDefault(); }
            // get { return String.Join(", ", Arrangements2D.Select(o => o.Tuning)); }
        }

        [XmlIgnore] // preserves old 1D display method show DMax
        public Int32 DD
        {
            get { return Convert.ToInt32(Arrangements2D.Max(o => o.DMax)); }
            //get { return String.Join(", ", Arrangements2D.Select(o => o.DMax)); }
        }

        [XmlIgnore] // preserves old 1D display method
        public Int32 Sections
        {
            get { return Arrangements2D.Max(o => o.SectionCount); }
        }

        // duplicate PID finder
        [XmlIgnore]
        public string PID { get; set; }

        [XmlIgnore]
        public string PIDArrangement { get; set; }
    }

    // detail table data
    [XmlRoot("Arrangment")] // provides proper xml serialization
    public class Arrangement
    {
        public Arrangement()
        {
        }

        public Arrangement(SongData Parent)
        {
            this.Parent = Parent;
        }

        [XmlIgnore]
        public string DLCKey
        {
            get
            {
                if (Parent != null)
                    return Parent.DLCKey;
                return string.Empty;
            }
        }

        [XmlIgnore]
        public SongData Parent { get; set; }

        public string PersistentID { get; set; }
        public string Name { get; set; } // arrangement name
        public string Tuning { get; set; }
        public Int32 DMax { get; set; }
        public string ToneBase { get; set; }
        public Int32 SectionCount { get; set; }
    }
}