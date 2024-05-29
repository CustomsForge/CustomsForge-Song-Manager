using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;
using GenTools;
using DataGridViewTools;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using System.Drawing;

// DO NOT USE RESHAPER TO SORT THIS CLASS -- HAND SORT ONLY
// TODO: CLEANUP SONGDATA OBJECT - DEPRICATE OLD/UNUSED STUFF

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

    // NOTE: custom object order here determines order of elements in the xml file
    // the use of nullables is compatible with filtering/sorting ... NOW
    [Serializable]
    public class SongData : NotifyPropChangedBase
    {
        // version 0 - 9: recyclable version number
        // incrementing version forces songInfo.xml and appSettings.xml to reset/update to defaults
        public const string SongDataVersion = "0"; // Devs change when needed to force user update

        // Unique Song Key
        public string DLCKey { get; set; }
        // Song Attributes
        public string Artist { get; set; }
        public string Title { get; set; }
        public string Album { get; set; }
        public string ArtistSort { get; set; }
        public string TitleSort { get; set; }
        public string AlbumSort { get; set; } // older CDLC do not have this
        public int? SongYear { get; set; }
        public double SongLength { get; set; }
        public float SongAverageTempo { get; set; }
        public float? SongVolume { get; set; } // older CDLC do not have this
        public DateTime LastConversionDateTime { get; set; }
        public string AppID { get; set; }
        public string ToolkitVersion { get; set; }
        public string PackageAuthor { get; set; }
        public string PackageVersion { get; set; }
        public string PackageComment { get; set; }
        public string PackageRating { get; set; }
        public string FilePath { get; set; }
        public DateTime FileDate { get; set; }
        public int FileSize { get; set; }
        // TODO: impliment from Ignition API if it ever gets finished
        public string IgnitionID { get; set; } // not serialized if empty
        public string IgnitionVersion { get; set; }
        public string IgnitionAuthor { get; set; }
        public string IgnitionDate { get; set; }
        //
        public SongDataStatus Status { get; set; }
        public SongTaggerStatus Tagged { get; set; }
        public RepairStatus RepairStatus { get; set; }

        public bool HasCustomFont { get; set; }
        public int AudioBitRate { get; set; }       
        public bool NeedsUpdate { get; set; }

        // used by detail table
        [XmlArray("Arrangements")] // provides proper xml serialization
        [XmlArrayItem("Arrangement")] // provides proper xml serialization
        public List<Arrangement> Arrangements2D { get; set; }

        public string AudioCache { get; set; } // not serialized if empty

        //
        // these elements are not serialized only used to display data in datagridview
        //
        [XmlIgnore]
        public bool IsODLC
        {
            get { return PackageAuthor == "Ubisoft" ? true : false; }
        }

        [XmlIgnore]
        public bool IsSongsPsarc
        {
            get { return !String.IsNullOrEmpty(FilePath) && (FileName.ToLower().EndsWith(Constants.BASESONGS) || FileName.ToLower().EndsWith(Constants.BASESONGSDISABLED)); }
        }

        [XmlIgnore]
        public bool IsRsCompPack
        {
            get { return !String.IsNullOrEmpty(FilePath) && FileName.ToLower().Contains(Constants.RS1COMP); }
        }

        [XmlIgnore]
        public bool IsSongPack
        {
            get { return !String.IsNullOrEmpty(FilePath) && (FileName.ToLower().Contains(Constants.SONGPACK) || FileName.ToLower().Contains(Constants.ABVSONGPACK)); }
        }

        [XmlIgnore]
        public string ArtistTitleAlbum { get { return String.Format("{0};{1};{2}", Artist != null ? Artist.Trim() : "", Title != null ? Title.Trim() : "", Album != null ? Album.Trim() : ""); } }
        [XmlIgnore]
        public string ArtistTitleAlbumDate { get { return String.Format("{0};{1};{2};{3}", Artist != null ? Artist.Trim() : "", Title != null ? Title.Trim() : "", Album != null ? Album.Trim() : "", LastConversionDateTime.ToString("s")); } }
        [XmlIgnore]
        public string FileName { get { return (Path.Combine(Path.GetFileName(Path.GetDirectoryName(FilePath)), Path.GetFileName(FilePath))); } }

        private bool _selected;
        [XmlIgnore]
        public bool Selected
        {
            get
            {
                // allow non dgvMasterSongs ODLC to be deleted/moved/selected
                //if (Globals.DgvCurrent.Name == "dgvMasterSongs")
                //    return IsOfficialDLC ? false : _selected;

                return _selected;
            }
            set
            {
                //if (!IsOfficialDLC || Globals.DgvCurrent.Name != "dgvMasterSongs")
                SetPropertyField("Selected", ref _selected, value); // _selected = value;          
                //else
                //    SetPropertyField("Selected", ref _selected, false); //_selected = false;
            }
        }

        [XmlIgnore]
        public string Enabled
        {
            get
            {
                // individual non-songpack songs and songpacks may be entirely disabled by filename
                if ((new FileInfo(FilePath).Name).ToLower().Contains("disabled"))
                    return "No";
                // songpack song status is unknown if not entirely disabled by filename
                if (IsRsCompPack || IsSongPack || IsSongsPsarc)
                    return "SongPack";
                // individual non-songpack songs
                return "Yes";
            }
            set { } // required for XML file usage
        }

        [XmlIgnore]
        public string ArrangementsInitials
        {
            get
            {
                string result = string.Empty;
                foreach (string arrangement in Arrangements2D.Select(x => x.ArrangementName.ToLower()).ToArray())
                {
                    // create BLRV short acronym
                    if (arrangement.ToLower().Contains("bass") && !result.Contains("B"))
                        result += "B";
                    else if (arrangement.ToLower().Contains("lead") && !result.Contains("L"))
                        result += "L";
                    else if ((arrangement.ToLower().Contains("rhythm") || arrangement.ToLower().Contains("combo")) && !result.Contains("R"))
                        result += "R";
                    else if (arrangement.ToLower().Contains("vocals"))
                        result += "V";
                }

                return result;
            }
        }

        private int? _ratingStars;
        [XmlIgnore]
        public int RatingStars  // { get; set; }
        {
            get
            {
                if (_ratingStars == null)
                {
                    if (String.IsNullOrEmpty(PackageRating) || PackageRating.ToLower().Contains("null"))
                        _ratingStars = 0;
                    else
                        _ratingStars = int.Parse(PackageRating);
                }

                return (int)_ratingStars;
            }

            set { _ratingStars = value; }
        }

        // preserves 1D display methods
        [XmlIgnore]
        public string Arrangements1D { get { return String.Join(", ", Arrangements2D.Select(o => o.ArrangementName)).TrimEnd(" ,".ToCharArray()); } }
        [XmlIgnore]
        public string Tunings1D { get { return String.Join(", ", Arrangements2D.Select(o => o.Tuning)).TrimEnd(" ,".ToCharArray()); } }
        [XmlIgnore]
        public string Tones1D { get { return String.Join(", ", Arrangements2D.Select(o => o.Tones)).TrimEnd(" ,".ToCharArray()); } }

        // used by duplicate finder
        [XmlIgnore]
        public string PID { get; set; }
        [XmlIgnore]
        public string PIDArrangement { get; set; }


        // used by Tagger and Renamer
        [XmlIgnore]
        public int DD { get { return Convert.ToInt32(Arrangements2D.Max(o => o.DDMax)); } }

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
            FileDate = fi.LastWriteTime;
            FileSize = (int)fi.Length;
        }
    }

    [Serializable]
    public class Arrangement
    {
        // Arrangement Attributes
        public string PersistentID { get; set; } // unique ID
        public string ArrangementName { get; set; }
        public int? CapoFret { get; set; }
        public int? DDMax { get; set; }
        public float? ScrollSpeed { get; set; } // stored as int in toolkit (x10)
        public string Tuning { get; set; }
        public double? TuningPitch { get; set; } // tuning frequency, see Cents2Frequency method
        public string ToneBase { get; set; }
        public string Tones { get; set; } // concatinated string of the tones used in arrangement
        public int? TonesCount { get; set; }
        public int? SectionsCount { get; set; }

        // Arrangement Attributes from HSAN file data
        public double? SongDifficulty { get; set; }

        // Arrangement Levels
        public int? ChordCount { get; set; }
        public int? NoteCount { get; set; }
        public int? AccentCount { get; set; }
        public int? BendCount { get; set; }
        public int? FretHandMuteCount { get; set; } // maxLevelNotes.Count(n => n.Mute > 0) + maxLevelChords.Count(c => c.FretHandMute > 0)
        public int? HammerOnCount { get; set; }
        public int? HarmonicCount { get; set; }
        public int? HarmonicPinchCount { get; set; }
        public int? HighestFretUsed { get; set; }
        public int? HopoCount { get; set; } // FIXME
        public int? IgnoreCount { get; set; }
        public int? LinkNextCount { get; set; }
        public int? OctaveCount { get; set; }
        public int? PalmMuteCount { get; set; } // maxLevelNotes.Count(n => n.PalmMute > 0) + maxLevelChords.Count(c => c.PalmMute > 0)
        public int? PluckCount { get; set; }
        public int? PullOffCount { get; set; }
        public int? SlapCount { get; set; }
        public int? SlideCount { get; set; }
        public int? SlideUnpitchToCount { get; set; }
        public int? SustainCount { get; set; }
        public int? TapCount { get; set; }
        public int? TremoloCount { get; set; }
        public int? VibratoCount { get; set; }
        public int? ThumbCount { get; set; }
        public int? PitchedChordSlideCount { get; set; }
        public int? TimeSignatureChangeCount { get; set; }
        public int? BPMChangeCount { get; set; }
        public float? MinBPM { get; set; }
        public float? MaxBPM { get; set; }

        // TODO: future expansion analyzer Arrangement Properties
        // Arrangement Properties 

        public int? BassPick { get; set; }
        //public int? BarreChords { get; set; }
        //public int? Bends { get; set; }
        public int? BonusArr { get; set; }
        //public int? DoubleStops { get; set; }
        //public int? DropDPower { get; set; }
        //public int? FifthsAndOctaves { get; set; }
        //public int? FingerPicking { get; set; }
        //public int? FretHandMutes { get; set; }
        //public int? Harmonics { get; set; }
        //public int? Hopo { get; set; }
        //public int? Metronome { get; set; }
        //public int? NonStandardChords { get; set; }
        //public int? OpenChords { get; set; }
        //public int? PalmMutes { get; set; }
        //public int? PathBass { get; set; }
        //public int? PathLead { get; set; }
        //public int? PathRhythm { get; set; }
        //public int? PickDirection { get; set; }
        //public int? PinchHarmonics { get; set; }
        //public int? PowerChords { get; set; }
        public int? Represent { get; set; }
        //public int? RouteMask { get; set; } // 0 = bass, 1 = lead, 2 = rhytmm
        //public int? SlapPop { get; set; }
        //public int? Slides { get; set; }
        //public int? StandardTuning { get; set; }
        //public int? Sustain { get; set; }
        //public int? Syncopation { get; set; }
        //public int? Tapping { get; set; }
        //public int? Tremolo { get; set; }
        //public int? TwoFingerPicking { get; set; }
        //public int? UnpitchedSlides { get; set; }
        //public int? Vibrato { get; set; }

        //public bool ShouldSerializeBassPick()
        //{
        //    return BassPick.HasValue;
        //}

        [XmlIgnore]
        public string IsBassPick
        {
            get
            {
                if (BassPick == null)
                    return null;

                return BassPick == 0 ? "Fingered" : "Picked";
            }
        }

        [XmlIgnore]
        public string IsDefaultBonusAlternate
        {
            // Default => represent is true and bonus is false 
            // Bonus => represent is false and bonus is true     
            // Alternate => both represent and bonus are false
            // Unknown => both represent and bonus are true
            get
            {
                if (Represent == null || BonusArr == null)
                    return "Null";
                else if (Represent == 1 && BonusArr == 0)
                    return "Default";
                else if (Represent == 0 && BonusArr == 1)
                    return "Bonus";
                else if (Represent == 0 && BonusArr == 0)
                    return "Alternate";
                else if (Represent == 1 && BonusArr == 1)
                    return "Unknown"; // appears in-game as default arrangement
                else
                    throw new DataException("<ERROR> Invalid Represent/BonusArr condition ...");
            }
        }


        // concatinated string of chord names and cord counts
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

        [XmlIgnore]
        public List<string> ChordNames { get; set; }
        [XmlIgnore]
        public List<int> ChordCounts { get; set; }

        public Arrangement()
        {
        }

        public Arrangement(SongData Parent)
        {
            this.Parent = Parent;
        }

        [JsonIgnore]
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

        [JsonIgnore]
        [XmlIgnore]
        public SongData Parent { get; set; }

    }



}