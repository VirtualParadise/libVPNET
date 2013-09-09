using Nexus;
using System;
using VP.Native;

namespace VP
{
    internal class VPVector3D
    {
        internal static Vector3D FromClick(IntPtr pointer)
        {
            return new Vector3D
            {
                X = Functions.vp_float(pointer, FloatAttributes.ClickHitX),
                Y = Functions.vp_float(pointer, FloatAttributes.ClickHitY),
                Z = Functions.vp_float(pointer, FloatAttributes.ClickHitZ),
            };
        }
    }
}
