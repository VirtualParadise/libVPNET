using System;

namespace VpNet.NativeApi
{
    public delegate void CallbackDelegate(IntPtr sender, int rc, int reference);
    public delegate void EventDelegate(IntPtr sender);
}
