using System.Collections.Generic;

namespace YouTrack.Models
{
    public class XmlAttributeElement : IEqualityComparer<XmlAttributeElement>
    {

        public XmlAttributeElement()
        {
        }

        public XmlAttributeElement(string elementName, string attributeName)
        {
            ElementName = elementName;
            AttributeName = attributeName;
        }

        public string ElementName { get; set; }

        public string AttributeName { get; set; }



        public bool Equals(XmlAttributeElement x, XmlAttributeElement y)
        {
            return x.Equals(y);
        }

        public int GetHashCode(XmlAttributeElement obj)
        {
            return obj.GetHashCode();
        }

        public bool Equals(XmlAttributeElement other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(ElementName, other.ElementName) && string.Equals(AttributeName, other.AttributeName);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((XmlAttributeElement)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((ElementName?.GetHashCode() ?? 0) * 397) ^ (AttributeName?.GetHashCode() ?? 0);
            }
        }
    }
}