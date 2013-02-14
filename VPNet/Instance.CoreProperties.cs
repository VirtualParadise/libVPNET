using System;
using System.Collections.Generic;
using VP.Native;

namespace VP
{
    public partial class Instance : IDisposable
    {
        #region World

        public AvatarPosition Position
        {
            get
            {
                return new AvatarPosition
                {
                    X     = Functions.vp_float(pointer, VPAttribute.MyX),
                    Y     = Functions.vp_float(pointer, VPAttribute.MyY),
                    Z     = Functions.vp_float(pointer, VPAttribute.MyZ),
                    Yaw   = Functions.vp_float(pointer, VPAttribute.MyYaw),
                    Pitch = Functions.vp_float(pointer, VPAttribute.MyPitch),
                };
            }
        }

        #endregion
    }
}
