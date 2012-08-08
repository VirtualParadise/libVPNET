using System;
#if (WCF)
using System.Runtime.Serialization;
#endif
using VpNet.NativeApi;

namespace VpNet.Core
{
#if (WCF)
        [DataContract]
#endif
    public sealed class VpException : Exception
    {
#if (WCF)
        [DataMember]
#endif
        public ReasonCode Reason { get; set; }

        public VpException(ReasonCode reason) : base(string.Format("VP SDK Error: {0}({1})", reason, (int)reason))
        {
            Reason = reason;
        }
    }
}
