using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VP.Tests
{
    [TestClass]
    public class SDKTests
    {
        [TestMethod]
        public void EnvironmentSetup()
        {
            new Instance().Dispose();
            var lastPath = getLastPATH();

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

            Console.WriteLine("PATH: {0}", Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Process));
            Console.WriteLine("Added VP native path: {0}", lastPath);
            Console.WriteLine("Platform VP native file: {0}", filePath);

            Assert.IsTrue( Directory.Exists(lastPath) );
            Assert.IsTrue( File.Exists(filePath) );            
        }

        [TestMethod]
        public void CheckNatives()
        {
            new Instance().Dispose();
            var lastPath  = getLastPATH();
            var libWinx86 = Path.Combine(lastPath, "..", "x86", "vpsdk.dll");
            var libWinx64 = Path.Combine(lastPath, "..", "x64", "vpsdk.dll");
            var libNixx64 = Path.Combine(lastPath, "..", "x64", "libvpsdk.so");

            Assert.IsTrue( File.Exists(libWinx86) );
            Assert.IsTrue( File.Exists(libWinx64) );
            Assert.IsTrue( File.Exists(libNixx64) );
        }

        string getLastPATH()
        {
            var path = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Process);

            if (path == null)
                throw new PlatformNotSupportedException("PATH variable is missing");

            var paths = path.Split(';');

            return paths[paths.Length - 1];
        }
    }
}
