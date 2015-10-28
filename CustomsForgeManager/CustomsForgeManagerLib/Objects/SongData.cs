using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;
using DataGridViewTools;
using System.ComponentModel;


namespace CustomsForgeManager.CustomsForgeManagerLib.Objects // .DataClass
{
    public enum SongDataStatus : byte
    {
        None = 0,
        UpToDate = 1,
        OutDated = 2,
        NotFound = 3
    }

    // only essential data needs to be saved to the XML songinfo file
    [Serializable]
    public class SongData : NotifyPropChangedBase
    {
        //version 2 : SongKey changed to DLCKey.
        //version 3 : removed DLCKey from arrangement
        public const string SongDataListCurrentVersion = "3";

        public string DLCKey { get; set; }

        private bool FSelected;
        [XmlIgnore]
        public bool Selected {
            get { return FSelected; }
            set { SetPropertyField("Selected", ref FSelected, value); }
        }

        [XmlIgnore]
        public string Enabled
        {
            get { return (new FileInfo(Path).Name).ToLower().Contains("disabled") ? "No" : "Yes"; }
            set { } // required for XML file usage
        }

        [XmlIgnore]
        public bool IsMine
        {
            get
            {
                if (!String.IsNullOrEmpty(AppSettings.Instance.CreatorName) && !String.IsNullOrEmpty(Charter))
                {
                    string[] creatorNames = AppSettings.Instance.CreatorName.ToLower().Split(new char[] {';',','});
                    return creatorNames.Any(z => z == Charter.ToLower());
                }
                return false;
            }
        }

        public string Artist { get; set; }
        public string Title { get; set; }
        public string Album { get; set; }
        public Int32 SongYear { get; set; }
        public Single SongLength { get; set; }
        public Single SongAverageTempo { get; set; }
        public Single SongVolume { get; set; }
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
                }
                return result;
            }
        }


        public string Charter { get; set; }
        public DateTime LastConversionDateTime { get; set; }
        public string Version { get; set; }
        public string ToolkitVer { get; set; }
        public string AppID { get; set; }
        public SongDataStatus Status { get; set; }
        public string IgnitionID { get; set; }
        public string IgnitionAuthor { get; set; }
        public string IgnitionVersion { get; set; }
        public string IgnitionUpdated { get; set; }
        public bool Tagged { get; set; }

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

        [XmlIgnore]  // preserves old 1D display method show DMax
        public Int32 DD
        {
            get { return Convert.ToInt32(Arrangements2D.Max(o => o.DMax)); }
            //get { return String.Join(", ", Arrangements2D.Select(o => o.DMax)); }
        }

        [XmlIgnore]  // preserves old 1D display method
        public Int32 Sections
        {
            get { return Arrangements2D.Max(o => o.SectionCount); }
        }

        // duplicate PID finder
        [XmlIgnore]
        public string PID { get; set; }
        [XmlIgnore]
        public string ArrangementPID { get; set; }
    }



    // detail table data
    [XmlRoot("Arrangment")] // provides proper xml serialization
    public class Arrangement
    {
        public Arrangement() { }


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
