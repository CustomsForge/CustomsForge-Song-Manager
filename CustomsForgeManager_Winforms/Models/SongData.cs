using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CustomsForgeManager_Winforms
{
    [Serializable]
    public class SongData
    {
        public string Enabled { get; set; }
        public string Artist { get; set; }
        public string Song { get; set; }
        public string Album { get; set; }
        public string Tuning { get; set; }
        public string DD { get; set; }
        public string SongYear { get; set; }
        public string Updated { get; set; }

        public string IgnitionID { get; set; }
        public string IgnitionVersion { get; set; }
        public string IgnitionUpdated { get; set; }
        public string IgnitionAuthor { get; set; }

        public string Arrangements
        {
            get
            {
                return _arrangements != null ? String.Join(",", _arrangements.Select(x => x.Name).ToArray()) : "";
            }
        }

        public string Author { get; set; }
        public string Path { get; set; }

        public string FileName
        {
            get { return (new FileInfo(Path).Name); }
        }


        public string Version { get; set; }
        public string ToolkitVer { get; set; }


        private List<SongDataArrangement> _arrangements;


        public void AddArrangement(SongDataArrangement arrangement)
        {
            if (_arrangements == null)
                _arrangements = new List<SongDataArrangement>();
            _arrangements.Add(arrangement);
        }

    }

    [Serializable]
    public class SongDataArrangement
    {
        public string Name { get; set; }

    }


}
