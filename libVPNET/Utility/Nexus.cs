using Nexus;
using Nexus.Graphics.Colors;
using System;
using VP.Native;

namespace VP
{
    internal static class VPVector3D
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

        internal static void ToClick(this Vector3D vec, IntPtr pointer)
        {
            Functions.vp_float_set(pointer, FloatAttributes.ClickHitX, vec.X);
            Functions.vp_float_set(pointer, FloatAttributes.ClickHitY, vec.Y);
            Functions.vp_float_set(pointer, FloatAttributes.ClickHitZ, vec.Z);
        }
    }

    internal static class VPColor
    {
        internal static ColorRgb FromChat(IntPtr pointer)
        {
            return new ColorRgb
            {
                R = (byte) Functions.vp_int(pointer, IntAttributes.ChatColorRed),
                G = (byte) Functions.vp_int(pointer, IntAttributes.ChatColorGreen),
                B = (byte) Functions.vp_int(pointer, IntAttributes.ChatColorBlue),
            };
        }
    }
}
