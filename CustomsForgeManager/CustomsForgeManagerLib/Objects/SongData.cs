using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CustomsForgeManager.CustomsForgeManagerLib.Objects
{
    #region SongDataStatus
    public enum SongDataStatus : byte
    {
        None = 0,
        UpToDate = 1,
        OutDated = 2,
        NotFound = 3
    }
    #endregion

    [Serializable]
    public class SongData
    {
        public bool Selected { get; set; }
        public string Enabled { get; set; }
        public string Artist { get; set; }
        public string Song { get; set; }
        public string Album { get; set; }
        public string Tuning { get; set; }
        public string DD { get; set; }
        public string SongYear { get; set; }
        public string Updated { get; set; }
        public string IgnitionID { get; set; }
        public string IgnitionUpdated { get; set; }
        public string IgnitionAuthor { get; set; }
        public string IgnitionVersion { get; set; }
        public string Version { get; set; }
        public string Author { get; set; }
        public string Path { get; set; }
        public string ToolkitVer { get; set; }
        public SongDataStatus Status { get; set; }

        public string Arrangements
        {
            get { return _arrangements != null ? String.Join(",", _arrangements.Select(x => x.Name).ToArray()) : String.Empty; }
            set { XmlArrangementsHelper(value); } // required for XML file usage 
            // TODO: convert XmlArrangementsHelper to LINQ
            // set {AddArrangement(value.Split(',').Select((x) => new SongDataArrangement {Name = x});]
        }

        public string FileName
        {
            get { return (new FileInfo(Path).Name); }
            set { } // required for XML file usage
        }

        private List<SongDataArrangement> _arrangements;
        public void AddArrangement(SongDataArrangement arrangement)
        {
            if (_arrangements == null)
                _arrangements = new List<SongDataArrangement>();
            _arrangements.Add(arrangement);
        }

        private void XmlArrangementsHelper(string xmlArrangements)
        {
            var arrangements = xmlArrangements.Split(',');
            foreach (var arrangment in arrangements)
                AddArrangement(new SongDataArrangement { Name = arrangment });
        }
    }

    [Serializable]
    public class SongDataArrangement
    {
        public string Name { get; set; }

    }

    // TODO: impliment INotifyPropertyChanged 
    /*    public class SongData : INotifyPropertyChanged
    impliment INotifyProperyChanged for BindingList
     * {
        public event PropertyChangedEventHandler PropertyChanged;
        private string _artist;

        private void NotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs( name));
        }

        public string Artist
        {
            get { return _artist; }
            set
            {
                _artist = value;
                this.NotifyPropertyChanged("Artist");
            }
        }
  
     */
}
