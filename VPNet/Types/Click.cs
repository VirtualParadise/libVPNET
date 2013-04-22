using System;
using VP.Native;

namespace VP
{
    public struct ObjectClick
    {
        public int   Session;
        public int   Id;
        public float ClickX, ClickY, ClickZ;

        /// <summary>
        /// Creates a click from a native instance's attributes
        /// </summary>
        internal ObjectClick (IntPtr pointer)
        {
            Session = Functions.vp_int(pointer, IntAttributes.AvatarSession);
            Id      = Functions.vp_int(pointer, IntAttributes.ObjectId);
            ClickX  = Functions.vp_float(pointer, FloatAttributes.ClickHitX);
            ClickY  = Functions.vp_float(pointer, FloatAttributes.ClickHitY);
            ClickZ  = Functions.vp_float(pointer, FloatAttributes.ClickHitZ);
        }
    }
}
