using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;
using GenTools;
using DataGridViewTools;
using System.Collections.Generic;

namespace CustomsForgeSongManager.DataObjects
{
    [Obfuscation(Exclude = false, Feature = "-rename")]
    public enum SongDataStatus : byte
    {
        None = 0,
        UpToDate = 1,
        OutDated = 2,
        NotFound = 3
    }

    [Obfuscation(Exclude = false, Feature = "-rename")]
    public enum SongTaggerStatus : byte
    {
        [XmlEnum("0")]
        False = 0,
        [XmlEnum("1")]
        True = 1,
        [XmlEnum("2")]
        ODLC = 2
    }

    // TODO: get custom filter to work with Enums, i.e. p.PropertyType.IsEnum
    [Obfuscation(Exclude = false, Feature = "-rename")]
    public enum RepairStatus : byte
    {
        NotRepaired = 0,
        Repaired = 1,
        RepairedDD = 2,
        RepairedMaxFive = 3,
        RepairedDDMaxFive = 4,
        ODLC = 5
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
        //ver 1 : Initial release
        //ver 2 : SongKey changed to DLCKey
        //ver 3 : removed DLCKey from arrangement
        //ver 4 : changed tagged to SongTaggerStatus
        //ver 5 : added ArtistSort TitleSort and AlbumSort variables
        //ver 6 : changed Path to FilePath to avoid conflict with reserved name System.IO.Path
        //ver 7 : add RepairStatus
        //ver 8 : add RepairStatus 'ODLC'
        //ver 9 : all app reference files moved to 'My Documents/CFSM'
        //ver 10 : force create a fresh 'My Documents/CFSM' folder'
        //ver 1 - 10: time to recycle some ver numbers

        // incrimenting forces songInfo.xml to update
        public const string SongDataListCurrentVersion = "9";

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

        //extra arrangemenet data

        [XmlIgnore]
        public Int32 ChordCount { get { return Arrangements2D.Sum(a => a.ChordCount); } }
        [XmlIgnore]
        public Int32 NoteCount { get { return Arrangements2D.Sum(a => a.NoteCount); } }
        [XmlIgnore]
        public Int32 OctaveCount { get { return Arrangements2D.Sum(a => a.OctaveCount); } }
        [XmlIgnore]
        public Int32 BendCount { get { return Arrangements2D.Sum(a => a.BendCount); } }
        [XmlIgnore]
        public Int32 HammerOnCount { get { return Arrangements2D.Sum(a => a.HammerOnCount); } }
        [XmlIgnore]
        public Int32 PullOffCount { get { return Arrangements2D.Sum(a => a.PullOffCount); } }
        [XmlIgnore]
        public Int32 HarmonicCount { get { return Arrangements2D.Sum(a => a.HarmonicCount); } }
        [XmlIgnore]
        public Int32 FretHandMuteCount { get { return Arrangements2D.Sum(a => a.FretHandMuteCount); } }
        [XmlIgnore]
        public Int32 PalmMuteCount { get { return Arrangements2D.Sum(a => a.PalmMuteCount); } }
        [XmlIgnore]
        public Int32 PluckCount { get { return Arrangements2D.Sum(a => a.PluckCount); } }
        [XmlIgnore]
        public Int32 SlapCount { get { return Arrangements2D.Sum(a => a.SlapCount); } }
        [XmlIgnore]
        public Int32 PopCount { get { return Arrangements2D.Sum(a => a.PopCount); } }
        [XmlIgnore]
        public Int32 SlideCount { get { return Arrangements2D.Sum(a => a.SlideCount); } }
        [XmlIgnore]
        public Int32 SustainCount { get { return Arrangements2D.Sum(a => a.SustainCount); } }
        [XmlIgnore]
        public Int32 TremoloCount { get { return Arrangements2D.Sum(a => a.TremoloCount); } }
        [XmlIgnore]
        public Int32 HarmonicPinchCount { get { return Arrangements2D.Sum(a => a.HarmonicCount); } }
        [XmlIgnore]
        public Int32 UnpitchedSlideCount { get { return Arrangements2D.Sum(a => a.UnpitchedSlideCount); } }
        [XmlIgnore]
        public Int32 TapCount { get { return Arrangements2D.Sum(a => a.TapCount); } }
        [XmlIgnore]
        public Int32 VibratoCount { get { return Arrangements2D.Sum(a => a.VibratoCount); } }
        public bool ExtraMetaDataScanned { get; set; }
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

        //[XmlIgnore]
        //public Dictionary<string, int> ChordList { get; set; }

        public List<string> ChordNames { get; set; }
        public List<int> ChordCounts { get; set; }
        [XmlIgnore]
        public string ChordCountsCombined
        {
            get
            {
                if (ChordNames == null)
                    return "";

                string stringList = "";

                for (int i = 0; i < ChordNames.Count(); i++)
                {
                    stringList += (ChordNames[i] + "-" + ChordCounts[i].ToString() + "| ");
                }

                return stringList;
            }
        }

        public Int32 ChordCount { get; set; }
        public Int32 NoteCount { get; set; }
        public Int32 OctaveCount { get; set; }
        public Int32 BendCount { get; set; }
        public Int32 HammerOnCount { get; set; }
        public Int32 PullOffCount { get; set; }
        public Int32 HarmonicCount { get; set; }
        public Int32 FretHandMuteCount { get; set; }
        public Int32 PalmMuteCount { get; set; }
        public Int32 PluckCount { get; set; }
        public Int32 SlapCount { get; set; }
        public Int32 PopCount { get; set; }
        public Int32 SlideCount { get; set; }
        public Int32 SustainCount { get; set; }
        public Int32 TremoloCount { get; set; }
        public Int32 HarmonicPinchCount { get; set; }
        public Int32 UnpitchedSlideCount { get; set; }
        public Int32 TapCount { get; set; }
        public Int32 VibratoCount { get; set; }
    }
}