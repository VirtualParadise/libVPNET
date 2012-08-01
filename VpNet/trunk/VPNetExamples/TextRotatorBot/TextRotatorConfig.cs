using System.Collections.Generic;
using System.Xml.Serialization;

namespace VPNetExamples.TextRotatorBot
{
    public class TextRotatorConfig
    {
        [XmlAttribute]
        public int ObjectId { get; set; }
        public List<TextRotatorConfigItem> TextItems { get; set; }

        public TextRotatorConfig()
        {
            TextItems = new List<TextRotatorConfigItem>();
        }
    }
}
