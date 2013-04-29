using System.IO;
using System.Reflection;

namespace VP.Native
{
    internal static class DLLHandler
    {
        internal const string VPDLL         = "vpsdk.dll";
        internal const string VPDLLResource = "VP." + VPDLL;

        /// <summary>
        /// Unpacks the native VP SDK dll from the assembly upon first use
        /// http://weblogs.asp.net/ralfw/archive/2007/02/04/single-assembly-deployment-of-managed-and-unmanaged-code.aspx
        /// </summary>
        internal static void Unpack()
        {
            // TODO : *nix/Mono support
            var asm  = Assembly.GetExecutingAssembly();
            var info = new FileInfo(VPDLL);

            // Ignore null files made from exceptions and newer DLLs
            if ( File.Exists(VPDLL) && info.Length > 0 )
            if ( File.GetCreationTimeUtc(VPDLL) > File.GetCreationTimeUtc(asm.Location) )
                return;

            using ( Stream     packed   = asm.GetManifestResourceStream(VPDLLResource) )
            using ( FileStream unpacked = new FileStream(VPDLL, FileMode.Create) )
            {
                byte[] buffer = new byte[packed.Length];
                packed.Read(buffer, 0, buffer.Length);
                unpacked.Write(buffer, 0, buffer.Length);
            }
        }
    }
}
