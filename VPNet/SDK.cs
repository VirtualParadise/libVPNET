using System;
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
            DLLHandler.Unpack();
            Functions.Call( () => Functions.vp_init(version) );

            isInitialized = true;
        }
    }
}
