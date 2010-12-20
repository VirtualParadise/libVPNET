using System;
using VpNet.NativeApi;

namespace VpNet.Core
{
    class VpException : Exception
    {
        public ReasonCode Reason;

        public VpException(ReasonCode reason) : base(string.Format("VP SDK Error: {0}({1})", reason, (int)reason))
        {
            Reason = reason;
        }
    }
}
