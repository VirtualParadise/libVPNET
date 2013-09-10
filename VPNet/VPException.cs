using System;
using VP.Native;

namespace VP
{
    /// <summary>
    /// Exception that is thrown when an SDK method fails for any reason
    /// </summary>
    public class VPException : Exception
    {
        /// <summary>
        /// Gets the code of this exception. This field is read-only.
        /// </summary>
        public readonly ReasonCode Reason;

        internal VPException(ReasonCode reason) : base(string.Format("VP SDK Error: {0} (#{1})", reason, (int) reason))
        {
            Reason = reason;
        }
    }
}
