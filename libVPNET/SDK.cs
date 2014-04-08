using System;
using System.IO;
using VP.Native;

namespace VP
{
    internal class SDK
    {
        const  int    version = 1;
        static object mutex   = new object();
        static bool   isInitialized;

        /// <summary>
        /// Makes the native SDK create an instance in memory and returns the pointer.
        /// Initializes the SDK if not already initialized. Thread-safe.
        /// </summary>
        internal static IntPtr CreateInstance()
        {
            lock (mutex)
            {
                if (!isInitialized)
                    initialize();

                return Functions.vp_create();
            }
        }

        static void initialize()
        {
            try
            {
                var path    = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Process);
                var local   = Environment.Is64BitProcess ? "x64/" : "x86/";
                var newPath = String.Format( "{0};{1}", path, Path.Combine(Environment.CurrentDirectory, local) );

                Environment.SetEnvironmentVariable("PATH", newPath, EnvironmentVariableTarget.Process);

                Functions.Call( () => Functions.vp_init(version) );

                isInitialized = true;
            }
            catch (BadImageFormatException ex)
            {
                throw new BadImageFormatException("libVPNET does not support this machine's architecture and/or operating system", ex.FileName, ex); 
            }
            catch (DllNotFoundException ex)
            {
                throw new DllNotFoundException("libVPNET does not support this machine's architecture and/or operating system", ex); 
            }
        }
    }
}
