using System.Collections.Generic;
using System.Reflection;
using System.Xml.Serialization;

namespace YouTrack.Models.Serialization
{
    public class XmlAttributeElementDeserializer<TType>
    {
        private readonly XmlDeserializationEvents _deserializationEvents;

        private readonly Dictionary<XmlAttributeElement, PropertyInfo> _elementAttributeProperties;

        public XmlAttributeElementDeserializer()
        {
            IList<PropertyInfo> properties = typeof(TType).GetProperties();

            _elementAttributeProperties = new Dictionary<XmlAttributeElement, PropertyInfo>();

            foreach (var propertyInfo in properties)
            {
                var attribute = propertyInfo.GetCustomAttribute<XmlAttributeElementAttribute>();

                if (attribute == null) continue;

                _elementAttributeProperties.Add(attribute.AttributeElement, propertyInfo);
            }

            _deserializationEvents = new XmlDeserializationEvents();

            _deserializationEvents.OnUnknownElement += OnUnknownElement;
        }

        public XmlDeserializationEvents DeserializationEvents => _deserializationEvents;

        private void OnUnknownElement(object sender, XmlElementEventArgs xmlElementEventArgs)
        {
            var element = xmlElementEventArgs.Element;

            var nameAttribute = element.GetAttribute("name");

            if (string.IsNullOrEmpty(nameAttribute)) return;

            var tuple = new XmlAttributeElement
            {
                ElementName = element.Name,
                AttributeName = nameAttribute
            };

            var success = _elementAttributeProperties.ContainsKey(tuple);

            if (!success) return;

            var valueNode = xmlElementEventArgs.Element.SelectSingleNode("value");

            if (valueNode == null) return;

            var property = _elementAttributeProperties[tuple];
            property.SetValue(xmlElementEventArgs.ObjectBeingDeserialized, valueNode.InnerText);
        }
    }
}