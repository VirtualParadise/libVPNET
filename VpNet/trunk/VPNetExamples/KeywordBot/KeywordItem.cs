using System.Xml.Serialization;

namespace VPNetExamples.KeywordBot
{
    public class KeywordItem
    {
        [XmlAttribute]
        public string Keyword { get; set; }
        [XmlAttribute]
        public string Response { get; set; }
    }
}
