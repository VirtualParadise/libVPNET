using System;
using System.IO;
using System.Reflection;

namespace VP.Native
{
    internal static class DLLHandler
    {
        internal const string dllBase = "vpsdk";

        const string dllWindows = "vpsdk.dll";
        const string dllUnix    = "libvpsdk.so";
        const string dllPrefix  = "VP.Libraries.";


        /// <summary>
        /// Unpacks the native VP SDK dll from the assembly upon first use for the
        /// correct platform
        /// http://weblogs.asp.net/ralfw/archive/2007/02/04/single-assembly-deployment-of-managed-and-unmanaged-code.aspx
        /// </summary>
        internal static void Unpack()
        {
            var platform = Environment.OSVersion.Platform;
            var assembly = Assembly.GetExecutingAssembly();

            switch (platform)
            {
                case PlatformID.MacOSX:
                    throw new PlatformNotSupportedException("This SDK does not contain a native library for MacOS X");

                case PlatformID.Unix:
                    unpackDll(assembly, dllUnix);
                    break;

                case PlatformID.Win32NT:
                case PlatformID.Win32S:
                case PlatformID.Win32Windows:
                default:
                    unpackDll(assembly, dllWindows);
                    break;
            }
        }

        static void unpackDll(Assembly asm, string dll)
        {
            var embed = dllPrefix + dll; 
            var info  = new FileInfo(dll);

            // Ignore null files made from exceptions
            if ( info.Exists && info.Length > 0 )
            if ( info.CreationTimeUtc > File.GetCreationTimeUtc(asm.Location) )
                return;

            using ( Stream     packed   = asm.GetManifestResourceStream(embed) )
            using ( FileStream unpacked = new FileStream(dll, FileMode.Create) )
            {
                byte[] buffer = new byte[packed.Length];
                packed.Read(buffer, 0, buffer.Length);
                unpacked.Write(buffer, 0, buffer.Length);
            }
        }
    }
}
