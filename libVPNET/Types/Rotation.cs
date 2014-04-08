using System;
using VP.Native;

namespace VP
{
    /// <summary>
    /// Represents an immutable tuple of four floats that represents an axis-angle
    /// rotation
    /// </summary>
    public struct Rotation
    {
        public float X;
        public float Y;
        public float Z;
        public float Angle;

        public Rotation(float x, float y, float z, float angle)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;

            this.Angle = angle;
        }

        internal static Rotation FromObject(IntPtr pointer)
        {
            return new Rotation
            {
                X = Functions.vp_float(pointer, FloatAttributes.ObjectRotationX),
                Y = Functions.vp_float(pointer, FloatAttributes.ObjectRotationY),
                Z = Functions.vp_float(pointer, FloatAttributes.ObjectRotationZ),

                Angle = Functions.vp_float(pointer, FloatAttributes.ObjectRotationAngle)
            };
        }

        internal void ToObject(IntPtr pointer)
        {
            Functions.vp_float_set (pointer, FloatAttributes.ObjectRotationX,     this.X);
            Functions.vp_float_set (pointer, FloatAttributes.ObjectRotationY,     this.Y);
            Functions.vp_float_set (pointer, FloatAttributes.ObjectRotationZ,     this.Z);
            Functions.vp_float_set (pointer, FloatAttributes.ObjectRotationAngle, this.Angle);
        }
    }
}
