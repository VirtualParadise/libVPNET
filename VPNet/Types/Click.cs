using System;
using VP.Native;

namespace VP
{
    /// <summary>
    /// Represents a click on an object in 3D space
    /// </summary>
    public struct ObjectClick
    {
        public int   Session, Id;
        public float X, Y, Z;

        internal ObjectClick(IntPtr pointer)
        {
            Session = Functions.vp_int(pointer, IntAttributes.AvatarSession);
            Id      = Functions.vp_int(pointer, IntAttributes.ObjectId);
            X       = Functions.vp_float(pointer, FloatAttributes.ClickHitX);
            Y       = Functions.vp_float(pointer, FloatAttributes.ClickHitY);
            Z       = Functions.vp_float(pointer, FloatAttributes.ClickHitZ);
        }
    }

    /// <summary>
    /// Represents a click on an avatar in 3D space
    /// </summary>
    public struct AvatarClick
    {
        public int   SourceSession, TargetSession;
        public float X, Y, Z;

        internal AvatarClick(IntPtr pointer)
        {
            SourceSession = Functions.vp_int(pointer, IntAttributes.AvatarSession);
            TargetSession = Functions.vp_int(pointer, IntAttributes.ClickedSession);
            X             = Functions.vp_float(pointer, FloatAttributes.ClickHitX);
            Y             = Functions.vp_float(pointer, FloatAttributes.ClickHitY);
            Z             = Functions.vp_float(pointer, FloatAttributes.ClickHitZ);
        }
    }
}
