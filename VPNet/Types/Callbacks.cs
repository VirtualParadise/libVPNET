using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VP;
using VP.Native;

namespace VP
{
    public abstract class CallbackData : EventArgs
    {
        public ReasonCode Reason;
        public bool Success { get { return Reason == ReasonCode.Success; } }
    }

    public class ObjectCallbackData : CallbackData
    {
        public VPObject Object;

        public ObjectCallbackData(ReasonCode rc, VPObject vpObject)
        {
            Object = vpObject;
            Reason = rc;
        }
    }
}
