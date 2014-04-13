using System;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VP.Tests
{
    [TestClass]
    public class UtilityTests
    {
        [TestMethod]
        public void UnicodeSplit()
        {
            var chunks = Unicode.ChunkByByteLimit(Strings.TooLong);
            Assert.IsTrue(chunks.Length == 3);

            foreach (var chunk in chunks)
                Assert.IsTrue( Encoding.UTF8.GetByteCount(chunk) <= 255 );
        }
    }
}
