using System;
using System.Runtime.InteropServices;

namespace VP.Native
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void CallbackDelegate(IntPtr sender, int rc, int reference);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void EventDelegate(IntPtr sender);
}
