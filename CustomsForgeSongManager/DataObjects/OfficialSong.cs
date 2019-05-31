using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace CustomsForgeSongManager.DataObjects
{
    public class OfficialSong
    {
        public string Title { get; set; }
        public string Artist { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Pack { get; set; }
        public string Link { get; set; }
    }
}
