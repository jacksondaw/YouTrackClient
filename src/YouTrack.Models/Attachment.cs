using System;
using System.Xml.Serialization;

namespace YouTrack.Models
{
    [XmlRoot("attachment")]
    public class Attachment
    {
        [XmlIgnore]
        public byte[] File { get; set; }

        [XmlElement("value")]
        public string FileName { get; set; }

        [XmlAttribute("url")]
        public string Url { get; set; }

    }
}