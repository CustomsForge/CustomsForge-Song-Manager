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
// TODO: CLEANUP SONGDATA OBJECT - DEPRICATE OLD/UNUSED STUFF

namespace CustomsForgeSongManager.DataObjects
{

    /// <summary>
    /// flattened SongData for direct use with dgvArrangements
    /// </summary>
    [Serializable]
    public class ArrangementData
    {
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

        // Arrangement Attributes
        public string PersistentID { get; set; } // unique ID
        public string Name { get; set; } // arrangement name
        public int? CapoFret { get; set; }
        public int? DDMax { get; set; }
        public string Tuning { get; set; }
        public double? TuningPitch { get; set; } // tuning frequency, see Cents2Frequency method
        public string ToneBase { get; set; }
        public string Tones { get; set; } // concatinated string of the tones used in arrangement
        public int? SectionsCount { get; set; }
        public int? TonesCount { get; set; }

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
        public int? HopoCount { get; set; }
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
        // useful calculated content taken from SongData
        public bool Selected { get; set; }
        public string ChordNamesCounts { get; set; }
        public bool IsOfficialDLC { get; set; }
        public bool IsRsCompPack { get; set; }
        public string IsBassPick { get; set; }
        public string IsDefaultBonusAlternate { get; set; }
        public string ArtistTitleAlbum { get; set; }
        public string ArtistTitleAlbumDate { get; set; }
        public string FileName { get; set; }

        // TODO: future expansion analyze Arrangement Properties
        // Arrangement Properties
        //public int? BassPick { get; set; }
        //public int? BarreChords { get; set; }
        //public int? Bends { get; set; }
        //public int? BonusArr { get; set; }
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
        //public int? Represent { get; set; }
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

    }
}


