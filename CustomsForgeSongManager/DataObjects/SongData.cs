using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;
using GenTools;
using DataGridViewTools;
using System.Collections.Generic;
using System.ComponentModel;

// DO NOT USE RESHAPER TO SORT THIS CLASS -- HAND SORT ONLY

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
        ODLC = 5,
        Unknown = 6
    }

    // only essential data should be saved to the XML songinfo file (NO BLOAT)
    // NOTE: custom object order here determines order of elements in the xml file
    [Serializable]
    public class SongData : NotifyPropChangedBase
    {
        // version 1 - 10: recyclable vers numbers
        // incrementing version forces songInfo.xml and appSettings.xml to reset/update to defaults
        public const string SongDataListCurrentVersion = "7";

        public string FilePath { get; set; }
        public DateTime FileDate { get; set; }
        public int FileSize { get; set; }
        public string DLCKey { get; set; }
        public string Artist { get; set; }
        public string ArtistSort { get; set; }
        public string Title { get; set; }
        public string TitleSort { get; set; }
        public string Album { get; set; }
        public string AlbumSort { get; set; }
        public int SongYear { get; set; }
        public float SongLength { get; set; }
        public float SongAverageTempo { get; set; }
        public float SongVolume { get; set; }
        public DateTime LastConversionDateTime { get; set; }
        public string Version { get; set; }

        // used by detail table
        [XmlArray("Arrangements")] // provides proper xml serialization
        [XmlArrayItem("Arrangement")] // provides proper xml serialization
        public FilteredBindingList<Arrangement> Arrangements2D { get; set; }

        public string ToolkitVer { get; set; }

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

        public string AppID { get; set; }
        //
        public string IgnitionID { get; set; } // not serialized if empty
        public string IgnitionVersion { get; set; } // not serialized if empty
        public string IgnitionAuthor { get; set; }
        public string IgnitionUpdated { get; set; } // not serialized if empty
        public string AudioCache { get; set; } // not serialized if empty
        public SongDataStatus Status { get; set; }
        public SongTaggerStatus Tagged { get; set; }
        public RepairStatus RepairStatus { get; set; }
        public bool ExtraMetaDataScanned { get; set; }
        //
        // these elements are not serialized only used to display data in datagridview
        //
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
                    SetPropertyField("Selected", ref _selected, value); // _selected = value;          
                else
                    SetPropertyField("Selected", ref _selected, false); //_selected = false;
            }
        }

        [XmlIgnore]
        public bool OfficialDLC
        {
            get { return this.Tagged == SongTaggerStatus.ODLC; }
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

        [XmlIgnore]
        public string ArrangementsInitials
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
        // preserves 1D display methods
        [XmlIgnore]
        public string Arrangements1D { get { return String.Join(", ", Arrangements2D.Select(o => o.Name)).TrimEnd(" ,".ToCharArray()); } }
        [XmlIgnore]
        public string Tunings1D { get { return String.Join(", ", Arrangements2D.Select(o => o.Tuning)).TrimEnd(" ,".ToCharArray()); } }
        [XmlIgnore]
        public string Tones1D { get { return String.Join(", ", Arrangements2D.Select(o => o.Tones)).TrimEnd(" ,".ToCharArray()); } }
        [XmlIgnore]
        public string ArtistTitleAlbum { get { return String.Format("{0};{1};{2}", Artist, Title, Album); } }
        [XmlIgnore]
        public string ArtistTitleAlbumDate { get { return String.Format("{0};{1};{2};{3}", Artist, Title, Album, LastConversionDateTime.ToString("s")); } }
        [XmlIgnore]
        public string FileName { get { return (Path.Combine(Path.GetFileName(Path.GetDirectoryName(FilePath)), Path.GetFileName(FilePath))); } }

        // duplicate PID finder
        [XmlIgnore]
        public string PID { get; set; }

        [XmlIgnore]
        public string PIDArrangement { get; set; }

        //extra arrangemenet data for Analyzer
        [XmlIgnore]
        public int? DD { get { return Convert.ToInt32(Arrangements2D.Max(o => o.DMax)); } }
        [XmlIgnore]
        public int? Sections { get { return Arrangements2D.Max(o => o.SectionCount); } }
        [XmlIgnore]
        public string CapoFret1D { get { return String.Join(", ", Arrangements2D.Select(o => o.CapoFret)).TrimEnd(" ,".ToCharArray()); } }
        [XmlIgnore]
        public string TuningPitch1D { get { return String.Join(", ", Arrangements2D.Select(o => o.TuningPitch)).TrimEnd(" ,".ToCharArray()); } }
        [XmlIgnore]
        public int? BassPick { get { return Arrangements2D.Max(a => a.BassPick); } }
        [XmlIgnore]
        public string ChordNamesCounts { get { return Arrangements2D[0].ChordNamesCounts.ToString(); } }
        [XmlIgnore]
        public int? ChordCount { get { return Arrangements2D.Sum(a => a.ChordCount); } }
        [XmlIgnore]
        public int? NoteCount { get { return Arrangements2D.Sum(a => a.NoteCount); } }
        [XmlIgnore]
        public int? OctaveCount { get { return Arrangements2D.Sum(a => a.OctaveCount); } }
        [XmlIgnore]
        public int? BendCount { get { return Arrangements2D.Sum(a => a.BendCount); } }
        [XmlIgnore]
        public int? HammerOnCount { get { return Arrangements2D.Sum(a => a.HammerOnCount); } }
        [XmlIgnore]
        public int? PullOffCount { get { return Arrangements2D.Sum(a => a.PullOffCount); } }
        [XmlIgnore]
        public int? HarmonicCount { get { return Arrangements2D.Sum(a => a.HarmonicCount); } }
        [XmlIgnore]
        public int? FretHandMuteCount { get { return Arrangements2D.Sum(a => a.FretHandMuteCount); } }
        [XmlIgnore]
        public int? PalmMuteCount { get { return Arrangements2D.Sum(a => a.PalmMuteCount); } }
        [XmlIgnore]
        public int? PluckCount { get { return Arrangements2D.Sum(a => a.PluckCount); } }
        [XmlIgnore]
        public int? SlapCount { get { return Arrangements2D.Sum(a => a.SlapCount); } }
        [XmlIgnore]
        public int? PopCount { get { return Arrangements2D.Sum(a => a.PopCount); } }
        [XmlIgnore]
        public int? SlideCount { get { return Arrangements2D.Sum(a => a.SlideCount); } }
        [XmlIgnore]
        public int? SustainCount { get { return Arrangements2D.Sum(a => a.SustainCount); } }
        [XmlIgnore]
        public int? TremoloCount { get { return Arrangements2D.Sum(a => a.TremoloCount); } }
        [XmlIgnore]
        public int? HarmonicPinchCount { get { return Arrangements2D.Sum(a => a.HarmonicCount); } }
        [XmlIgnore]
        public int? UnpitchedSlideCount { get { return Arrangements2D.Sum(a => a.UnpitchedSlideCount); } }
        [XmlIgnore]
        public int? TapCount { get { return Arrangements2D.Sum(a => a.TapCount); } }
        [XmlIgnore]
        public int? VibratoCount { get { return Arrangements2D.Sum(a => a.VibratoCount); } }
        [XmlIgnore]
        public int? HighestFretUsed { get { return Arrangements2D.Max(a => a.HighestFretUsed); } }


        public void Delete()
        {
            if (!String.IsNullOrEmpty(AudioCache) && File.Exists(AudioCache))
                File.Delete(AudioCache);

            AudioCache = String.Empty;

            if (File.Exists(FilePath))
                File.Delete(FilePath);
        }

        public void UpdateFileInfo()
        {
            var fi = new FileInfo(FilePath);
            FileDate = fi.LastWriteTimeUtc;
            FileSize = (int)fi.Length;
        }
    }

    // used for Analyzer data
    [XmlRoot("Arrangment")]
    public class Arrangement
    {
        public string PersistentID { get; set; }
        public string Name { get; set; } // arrangement name
        public string Tuning { get; set; }
        public string ToneBase { get; set; }
        public string Tones { get; set; }
        public int? DMax { get; set; } // null value is not serialized
        public int? SectionCount { get; set; } // null value is not serialized
        public int? BassPick { get; set; } // null value is not serialized
        public string TuningPitch { get; set; } // tuning frequency, see Cents2Frequency method
        public string CapoFret { get; set; }

        public bool ShouldSerializeDMax()
        {
            return DMax.HasValue;
        }

        public bool ShouldSerializeSectionCount()
        {
            return SectionCount.HasValue;
        }

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
        [XmlIgnore]
        public List<string> ChordNames { get; set; }
        [XmlIgnore]
        public List<int> ChordCounts { get; set; }
        //
        private string _chordNamesCounts;
        public string ChordNamesCounts
        {
            get
            {
                if (String.IsNullOrEmpty(_chordNamesCounts))
                {
                    if (ChordNames == null || ChordNames.Count == 0)
                        _chordNamesCounts = "";
                    else
                    {
                        var sep = " | ";
                        for (int i = 0; i < ChordNames.Count(); i++)
                            _chordNamesCounts += ChordNames[i] + "-" + ChordCounts[i].ToString() + sep;

                        _chordNamesCounts = _chordNamesCounts.TrimEnd(sep.ToCharArray());
                    }
                }

                return _chordNamesCounts;
            }
            set
            {
                _chordNamesCounts = value;
            }
        }

        public bool ShouldSerializeChordNamesCounts()
        {
            return ChordCount > 0;
        }

        public int? ChordCount { get; set; }
        public int? NoteCount { get; set; }
        public int? BendCount { get; set; }
        public int? FretHandMuteCount { get; set; }
        public int? HammerOnCount { get; set; }
        public int? HarmonicCount { get; set; }
        public int? HarmonicPinchCount { get; set; }
        public int? HighestFretUsed { get; set; }
        public int? OctaveCount { get; set; }
        public int? PalmMuteCount { get; set; }
        public int? PluckCount { get; set; }
        public int? PopCount { get; set; }
        public int? PullOffCount { get; set; }
        public int? SlapCount { get; set; }
        public int? SlideCount { get; set; }
        public int? SustainCount { get; set; }
        public int? TapCount { get; set; }
        public int? TremoloCount { get; set; }
        public int? UnpitchedSlideCount { get; set; }
        public int? VibratoCount { get; set; }

    }



}