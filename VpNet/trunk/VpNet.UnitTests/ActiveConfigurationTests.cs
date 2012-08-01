using System.IO;
using NUnit.Framework;
using VPNetExamples.Common;
using VPNetExamples.TextRotatorBot;

namespace VpNet.UnitTests
{
    [TestFixture]
    public class ActiveConfigurationTests
    {
        [Test]
        public void TextRotatorConfigTest()
        {
            var config = new TextRotatorConfig {ObjectId = int.MaxValue};
            config.TextItems.Add(new TextRotatorConfigItem(){Delay=1000,Text="Hello world!\r\n"});
            SerializationHelpers.Serialize<TextRotatorConfig>(config, new FileInfo("TextRotatorConfigTest.xml"));
            var target = SerializationHelpers.Deserialize<TextRotatorConfig>(new FileInfo("TextRotatorConfigTest.xml"));
            Assert.AreEqual(config.ObjectId, target.ObjectId);
        }
    }
}
