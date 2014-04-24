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
        public static readonly Rotation Zero = new Rotation(0, 1, 0, 0);

        /// <summary>
        /// Gets the X rotation
        /// </summary>
        public float X;
        /// <summary>
        /// Gets the Y rotation
        /// </summary>
        public float Y;
        /// <summary>
        /// Gets the Z rotation
        /// </summary>
        public float Z;
        /// <summary>
        /// Gets the angle of this rotation
        /// </summary>
        public float Angle;

        /// <summary>
        /// For compatiability with older objects, which may return a Euler rotation than
        /// an axis-angle one. This is internally determined in the standard Virtual
        /// Paradise client as a rotation with an infinite or max float value Angle.
        /// </summary>
        public bool IsEuler
        {
            get { return float.IsPositiveInfinity(Angle) || Angle == float.MaxValue; }
        }

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
