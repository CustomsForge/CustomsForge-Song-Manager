using System;
using System.Xml.Serialization;

namespace CustomsForgeSongManager.DataObjects
{
    [Serializable]
    public class IgnitionData
    {
        [XmlElement("odlc")]
        public bool ODLC { get; set; }

        [XmlElement("cfid")]
        public long CFID { get; set; }

        [XmlElement("artist")]
        public string Artist { get; set; }

        [XmlElement("title")]
        public string Title { get; set; }

        [XmlElement("album")]
        public string Album { get; set; }

        [XmlElement("downloads")]
        public long Downloads { get; set; }

        [XmlElement("tuning")]
        public string Tuning { get; set; }

        [XmlElement("dd")]
        public bool DD { get; set; }

        [XmlElement("charter")]
        public string Charter { get; set; }

        [XmlElement("version")]
        public string Version { get; set; }

        [XmlElement("createdate")]
        public DateTime Created { get; set; }

        [XmlElement("updated")]
        public DateTime Updated { get; set; }

        [XmlElement("lead")]
        public bool Lead { get; set; }

        [XmlElement("rhythm")]
        public bool Rhythm { get; set; }

        [XmlElement("bass")]
        public bool Bass { get; set; }

        [XmlElement("vocals")]
        public bool Vocals { get; set; }
        [XmlElement("pc")]
        public bool PC { get; set; }

        [XmlElement("mac")]
        public bool Mac { get; set; }

        [XmlElement("xbox")]
        public bool Xbox { get; set; }

        [XmlElement("ps3")]
        public bool PS3 { get; set; }
    }
}
