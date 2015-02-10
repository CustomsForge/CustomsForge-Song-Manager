using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomsForgeManager_Winforms
{
    [Serializable]
    class SongData
    {
        public string Preview { get; set; }
        public string Artist { get; set; }
        public string Song { get; set; }
        public string Album { get; set; }
        public string Tuning { get; set; }
        public string DD { get; set; }
        public string SongYear { get; set; }
        public string Updated { get; set; }
        public string Arrangements { get; set; }
        public string User { get; set; }
        public string NewAvailable { get; set; }
        public string Author { get; set; }
        public string Path { get; set; }
    }
}
