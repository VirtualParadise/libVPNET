using System.Xml.Serialization;

namespace VPNetExamples.TextRotatorBot
{
    public class TextRotatorConfigItem
    {
        [XmlAttribute]
        public string Text { get; set; }
        [XmlAttribute]
        public long Delay { get; set; }
    }
}
