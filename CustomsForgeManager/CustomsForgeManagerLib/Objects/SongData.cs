﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;
using DataGridViewTools;


namespace CustomsForgeManager.CustomsForgeManagerLib.Objects // .DataClass
{
    // required because Dotfuscator renames enums if Exclude is not present
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = true, Feature = "renaming")]
    public enum SongDataStatus : byte
    {
        None = 0,
        UpToDate = 1,
        OutDated = 2,
        NotFound = 3
    }

    // only essential data needs to be saved to the XML songinfo file
    [Serializable]
    public class SongData
    {
        public string SongKey { get; set; }

        [XmlIgnore]
        public bool Selected { get; set; }

        [XmlIgnore]
        public string Enabled
        {
            get { return (new FileInfo(Path).Name).ToLower().Contains("disabled") ? "No" : "Yes"; }
            // set { } // required for XML file usage
        }

        public string Artist { get; set; }
        public string Title { get; set; }
        public string Album { get; set; }
        public string SongYear { get; set; }
        public string SongLength { get; set; } // single (seconds)
        public string SongAverageTempo { get; set; } // single
        public string SongVolume { get; set; } // float
        public DateTime FileDate { get; set; }
        public int FileSize { get; set; }

        // used by detail table
        [XmlArray("Arrangments")] // provides proper xml serialization
        [XmlArrayItem("Arrangement")] // provides proper xml serialization
        public FilteredBindingList<Arrangement> Arrangements2D { get; set; }

        public string Path { get; set; }

        [XmlIgnore]
        public string FileName
        {
            get { return (new FileInfo(Path).Name); }
            // set { } // required for XML file usage
        }

        public string Charter { get; set; }
        public string LastConversionDateTime { get; set; }
        public string Version { get; set; }
        public string ToolkitVer { get; set; }
        public string AppID { get; set; }
        public SongDataStatus Status { get; set; }
        public string IgnitionID { get; set; }
        public string IgnitionAuthor { get; set; }
        public string IgnitionVersion { get; set; }
        public string IgnitionUpdated { get; set; }

        [XmlIgnore]
        public string ArtistTitleAlbum
        {
            get { return String.Format("{0};{1};{2}", Artist, Title, Album); }
            // set { } // required for XML file usage
        }

        [XmlIgnore]  // preserves old 1D display method
        public string Arrangements
        {
            get { return String.Join(", ", Arrangements2D.Select(o => o.Name)); }
        }

        [XmlIgnore]  // preserves old 1D display method
        public string Tuning
        {
            get { return Arrangements2D.Select(o => o.Tuning).FirstOrDefault(); }
            // get { return String.Join(", ", Arrangements2D.Select(o => o.Tuning)); }
        }

        [XmlIgnore]  // preserves old 1D display method
        public string DD
        {
            get { return Arrangements2D.Max(o => o.DMax); }
            //get { return String.Join(", ", Arrangements2D.Select(o => o.DMax)); }
        }

        [XmlIgnore]  // preserves old 1D display method
        public string Sections
        {
            get { return Arrangements2D.Max(o => o.SectionCount).ToString(); }
        }
    }



    // detail table data
    [XmlRoot("Arrangment")] // provides proper xml serialization
    public class Arrangement
    {
        public string SongKey { get; set; }
        public string PersistentID { get; set; }
        public string Name { get; set; }
        public string Tuning { get; set; }
        public string DMax { get; set; }
        public string ToneBase { get; set; }
        public int SectionCount { get; set; }
    }

}