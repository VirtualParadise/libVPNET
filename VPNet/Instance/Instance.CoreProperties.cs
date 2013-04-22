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
                    X     = Functions.vp_float(pointer, FloatAttributes.MyX),
                    Y     = Functions.vp_float(pointer, FloatAttributes.MyY),
                    Z     = Functions.vp_float(pointer, FloatAttributes.MyZ),
                    Yaw   = Functions.vp_float(pointer, FloatAttributes.MyYaw),
                    Pitch = Functions.vp_float(pointer, FloatAttributes.MyPitch),
                };
            }
        }

        #endregion
    }
}
