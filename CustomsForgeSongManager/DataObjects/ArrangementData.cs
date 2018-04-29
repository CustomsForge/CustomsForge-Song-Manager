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
    /// flattened SongData for direct use with dgvArrangments
    /// </summary>
    [Serializable]
    public class ArrangementData
    {
        // song key
        public string DLCKey { get; set; }

        // song elements
        public string Artist { get; set; }
        public string ArtistSort { get; set; }
        public string Title { get; set; }
        public string TitleSort { get; set; }
        public string Album { get; set; }
        public string AlbumSort { get; set; } // older CDLC do not have this
        public int SongYear { get; set; }
        public double SongLength { get; set; }
        public float SongAverageTempo { get; set; }
        public float? SongVolume { get; set; } // older CDLC do not have this
        public DateTime LastConversionDateTime { get; set; }
        public string AppID { get; set; }
        public string PackageAuthor { get; set; }
        public string PackageVersion { get; set; }
        public string PackageComment { get; set; }
        public string ToolkitVersion { get; set; }
        public string FilePath { get; set; }
        public DateTime FileDate { get; set; }
        public int FileSize { get; set; }

        // arrangement elements
        public string PersistentID { get; set; } // arrangment key
        public string Name { get; set; } // arrangement name
        public string Tuning { get; set; }
        public string ToneBase { get; set; }
        public string Tones { get; set; } // concatinated string of the tones used in arrangement
        public int? DDMax { get; set; } // null value is not serialized
        public int? SectionCount { get; set; } // null value is not serialized
        public int? BassPick { get; set; } // null value is not serialized
        public double? TuningPitch { get; set; } // tuning frequency, see Cents2Frequency method
        public int? CapoFret { get; set; }
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
        public int? PullOffCount { get; set; }
        public int? SlapCount { get; set; }
        public int? SlideCount { get; set; }
        public int? SustainCount { get; set; }
        public int? TapCount { get; set; }
        public int? TremoloCount { get; set; }
        public int? UnpitchedSlideCount { get; set; }
        public int? VibratoCount { get; set; }
        // calculated content taken from SongData
        public string ChordNamesCounts { get; set; }
        public bool Selected { get; set; }
        public bool OfficialDLC { get; set; }
        public bool IsRsCompPack { get; set; }
        public string ArtistTitleAlbum { get; set; }
        public string ArtistTitleAlbumDate { get; set; }
        public string FileName { get; set; }
        public SongTaggerStatus Tagged { get; set; }
        public RepairStatus RepairStatus { get; set; }
    }
}

/*
 *         // song key
        DLCKey

        // song elements
        Artist
        ArtistSort
        Title
        TitleSort
        Album
        AlbumSort // older CDLC do not have this
        SongYear
        SongLength
        SongAverageTempo
        SongVolume // older CDLC do not have this
        LastConversionDateTime
        AppID
        PackageAuthor
        PackageVersion
        PackageComment
        ToolkitVersion
        FilePath
        FileDate
        FileSize

        // arrangement elements
        PersistentID // arrangment key
        Name // arrangement name
        Tuning
        ToneBase
        Tones // concatinated string of the tones used in arrangement
         DDMax // null value is not serialized
         SectionCount // null value is not serialized
         BassPick // null value is not serialized
         TuningPitch // tuning frequency, see Cents2Frequency method
         CapoFret
         ChordCount
         NoteCount
         BendCount
         FretHandMuteCount
         HammerOnCount
         HarmonicCount
         HarmonicPinchCount
         HighestFretUsed
         OctaveCount
         PalmMuteCount
         PluckCount
         PullOffCount
         SlapCount
         SlideCount
         SustainCount
         TapCount
         TremoloCount
         UnpitchedSlideCount
         VibratoCount
        //

 */
