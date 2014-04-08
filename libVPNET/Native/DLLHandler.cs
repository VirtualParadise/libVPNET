using System;
using System.IO;
using System.Reflection;

namespace VP.Native
{
    internal static class DLLHandler
    {
        internal const string dllBase = "vpsdk";

        const string dllX86     = dllBase + ".x86.dll";
        const string dllX64     = dllBase + ".x64.dll";
        const string dllWindows = dllBase + ".dll";
        const string dllPrefix  = "VP.Libraries.";

        /// <summary>
        /// Unpacks the native VP SDK dll from the assembly upon first use for the
        /// correct platform
        /// http://weblogs.asp.net/ralfw/archive/2007/02/04/single-assembly-deployment-of-managed-and-unmanaged-code.aspx
        /// </summary>
        internal static void Unpack()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var native   = Environment.Is64BitProcess ? dllX64 : dllX86;

            unpackDll(assembly, native, dllWindows);
        }

        static void unpackDll(Assembly asm, string from, string to)
        {
            var embed = dllPrefix + from; 
            var info  = new FileInfo(to);

            if ( info.Exists && info.Length > 0 )
            if ( info.CreationTimeUtc > File.GetCreationTimeUtc(asm.Location) )
                return;

            using ( Stream     packed   = asm.GetManifestResourceStream(embed) )
            using ( FileStream unpacked = new FileStream(to, FileMode.Create) )
            {
                byte[] buffer = new byte[packed.Length];
                packed.Read(buffer, 0, buffer.Length);
                unpacked.Write(buffer, 0, buffer.Length);
            }
        }
    }
}
