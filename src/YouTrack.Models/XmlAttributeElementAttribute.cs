using System;

namespace YouTrack.Models
{
    public class XmlAttributeElementAttribute : Attribute
    {
        public XmlAttributeElementAttribute()
        {
        }

        public XmlAttributeElementAttribute(string elementName, string attributeName)
        {
            AttributeElement = new XmlAttributeElement(elementName, attributeName);
        }

        public XmlAttributeElement AttributeElement { get; set; }
    }
}