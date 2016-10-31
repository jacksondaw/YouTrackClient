using System.IO;
using System.Xml;
using System.Xml.Serialization;
using NUnit.Framework;
using YouTrack.Models;
using YouTrack.Models.Serialization;

namespace YouTrack.Tests
{
    [TestFixture]
    public class SerializationTests
    {
        [Test]
        public void TestFieldDeserialization()
        {
            const string fieldXml =
                "<?xml version='1.0' encoding='UTF-8' standalone='yes' ?><field name='summary'><value>summary</value></field>";

            var serializer = new XmlSerializer(typeof(Field<string>));
            using (TextReader reader = new StringReader(fieldXml))
            {
                var retval = (Field<string>) serializer.Deserialize(reader);

                Assert.IsTrue(string.Equals(retval.Value, "summary"));
            }
        }

        [Test]
        public void TestIssueDeserialization()
        {
            var serializer = new XmlSerializer(typeof(Issue));
            var context = TestContext.CurrentContext.TestDirectory;
            var file = Path.Combine(context, "TestIssue.xml");
            using (var fileStream = new FileStream(file, FileMode.Open))
            {
                var fieldDeserializer = new XmlAttributeElementDeserializer<Issue>();
                var xmlReader = new XmlTextReader(fileStream);
                var retval = (Issue) serializer.Deserialize(xmlReader, fieldDeserializer.DeserializationEvents);

                Assert.NotNull(retval);
                Assert.IsTrue(!string.IsNullOrEmpty(retval.Summary));
            }
        }
    }
}