using System;
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
        public string Project
        {
            get { return $"{ShortProjectName}-{NumberInProject}"; }
            set { ParseProject(value); }
        }

        [XmlAttributeElement("field", "projectShortName")]
        public string ShortProjectName { get; set; }

        [XmlAttributeElement("field", "numberInProject")]
        public int NumberInProject { get; set; }

        [XmlAttributeElement("field", "summary")]
        public string Summary { get; set; }

        [XmlAttributeElement("field", "description")]
        public string Description { get; set; }

        [XmlAttributeElement("field", "permittedGroup")]
        public string PermittedGroup { get; set; }

        [XmlAttributeElement("field", "created")]
        public DateTime Created { get; set; }

        [XmlAttributeElement("field", "updated")]
        public DateTime Updated { get; set; }

        [XmlAttributeElement("field", "updaterName")]
        public string Updater { get; set; }

        [XmlAttributeElement("field", "reporterName")]
        public string Reporter { get; set; }

        [XmlAttributeElement("field", "resolved")]
        public DateTime Resolved { get; set; }

        [XmlAttributeElement("field", "commentsCount")]
        public int CommentsCount { get; set; }

        [XmlArray("attachments")]
        public Attachment[] Attachments { get; set; }

        private void ParseProject(string project)
        {
            if (project == null)
            {
                ShortProjectName = string.Empty;
                NumberInProject = 0;
                return;
            }
            var parts = project.Split('-');

            if (parts.Length == 0)
            {
                ShortProjectName = project;
                return;
            }

            ShortProjectName = parts[0];

            if (parts.Length == 2)
            {
                int number;
                var success = int.TryParse(parts[1], out number);

                if (!success) return;

                NumberInProject = number;
            }
        }
    }
}