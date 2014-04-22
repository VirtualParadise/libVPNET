using System;
using VP.Native;

namespace VP
{
    /// <summary>
    /// Represents an immutable tuple of three floats, reperesenting an XYZ point in 3D
    /// space
    /// </summary>
    public struct Vector3
    {
        public static readonly Vector3 Zero = new Vector3(0, 0, 0);

        public float X;
        public float Y;
        public float Z;

        public Vector3(float x, float y, float z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        internal static Vector3 FromClick(IntPtr pointer)
        {
            return new Vector3
            {
                X = Functions.vp_float(pointer, FloatAttributes.ClickHitX),
                Y = Functions.vp_float(pointer, FloatAttributes.ClickHitY),
                Z = Functions.vp_float(pointer, FloatAttributes.ClickHitZ),
            };
        }

        internal static Vector3 FromObject(IntPtr pointer)
        {
            return new Vector3
            {
                X = Functions.vp_float(pointer, FloatAttributes.ObjectX),
                Y = Functions.vp_float(pointer, FloatAttributes.ObjectY),
                Z = Functions.vp_float(pointer, FloatAttributes.ObjectZ)
            };
        }

        internal void ToClick(IntPtr pointer)
        {
            Functions.vp_float_set(pointer, FloatAttributes.ClickHitX, X);
            Functions.vp_float_set(pointer, FloatAttributes.ClickHitY, Y);
            Functions.vp_float_set(pointer, FloatAttributes.ClickHitZ, Z);
        }
    }
}

