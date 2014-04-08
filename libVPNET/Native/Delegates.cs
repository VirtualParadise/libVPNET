using System;
using System.Runtime.InteropServices;

namespace VP.Native
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void CallbackDelegate(IntPtr sender, int rc, int reference);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void EventDelegate(IntPtr sender);
}
