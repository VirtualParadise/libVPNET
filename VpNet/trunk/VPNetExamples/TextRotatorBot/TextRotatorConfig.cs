using System.Collections.Generic;
using System.Xml.Serialization;

namespace VPNetExamples.TextRotatorBot
{
    public class TextRotatorConfig
    {
        [XmlAttributeAttribute]
        public int CellX { get; set; }
        [XmlAttributeAttribute]
        public int CellY { get; set; }
        [XmlAttribute]
        public string Name { get; set; }
        public List<TextRotatorConfigItem> TextItems { get; set; }

        public TextRotatorConfig()
        {
            TextItems = new List<TextRotatorConfigItem>();
        }
    }
}
