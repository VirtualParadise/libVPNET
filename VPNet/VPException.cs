using System;
using VP.Native;

namespace VP
{
    public sealed class VPException : Exception
    {
        public ReasonCode Reason;

        public VPException(ReasonCode reason) : base(string.Format("VP SDK Error: {0} ({1})", reason, (int)reason))
        {
            Reason = reason;
        }
    }
}
