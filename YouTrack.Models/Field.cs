using System.Xml.Serialization;

namespace YouTrack.Models
{
    [XmlRoot("field")]
    public class Field<T>
    {
        public Field()
        {
        }

        public Field(T t)
        {
            Value = t;
        }

        [XmlElement("value")]
        public T Value { get; set; }
    }
}