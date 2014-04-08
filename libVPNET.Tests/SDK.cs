using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VP.Tests
{
    [TestClass]
    public class SDK
    {
        [TestMethod]
        public void EnvironmentTest()
        {
            var instance = new Instance();
            var paths    = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Process).Split(';');
            var lastPath = paths[paths.Length - 1];

            string file;
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.MacOSX:
                    file = "libvpsdk.dylib";
                    break;

                case PlatformID.Unix:
                    file = "libvpsdk.so";
                    break;

                default:
                    file = "vpsdk.dll";
                    break;
            }

            var filePath = Path.Combine(lastPath, file);

            Assert.IsTrue( Directory.Exists(lastPath) );
            Assert.IsTrue( File.Exists(filePath) );

            Console.WriteLine("PATH: {0}", Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Process));
            Console.WriteLine("Added VP native path: {0}", lastPath);
            Console.WriteLine("Platform VP native file: {0}", filePath);
        }
    }
}
