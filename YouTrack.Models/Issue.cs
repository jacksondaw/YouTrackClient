using System.Xml.Serialization;

namespace YouTrack.Models
{
    [XmlRoot("issue")]
    public class Issue
    {
        public Issue()
        {
            Attachments = new Attachment[0];
        }

        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlAttributeElement("field", "project")]
        public string Project { get; set; }

        [XmlAttributeElement("field", "summary")]
        public string Summary { get; set; }

        [XmlAttributeElement("field", "description")]
        public string Description { get; set; }

        [XmlAttributeElement("field", "permittedGroup")]
        public string PermittedGroup { get; set; }

        [XmlArray("attachments")]
        public Attachment[] Attachments { get; set; }
    }
}