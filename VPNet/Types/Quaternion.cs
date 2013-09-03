using System;

namespace VP
{
    public struct Quaternion
    {
        public static Quaternion Zero      = new Quaternion();
        public static Quaternion ZeroEuler = new Quaternion()
        {
            W = float.MaxValue
        };

        public float X;
        public float Y;
        public float Z;
        public float W;

        public Quaternion(float x = .0f, float y = .0f, float z = .0f, float w = float.MaxValue)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.W = w;
        }

        public static Quaternion FromEulerR(float x, float y, float z)
        {
            // Assuming the angles are in radians.
            double c1 = Math.Cos(y/2);
            double s1 = Math.Sin(y/2);
            double c2 = Math.Cos(z/2);
            double s2 = Math.Sin(z/2);
            double c3 = Math.Cos(x/2);
            double s3 = Math.Sin(x/2);
            double c1c2 = c1*c2;
            double s1s2 = s1*s2;

            return new Quaternion
            {
                X = (float) (c1c2*s3 + s1s2*c3),
                Y = (float) (s1*c2*c3 + c1*s2*s3),
                Z = (float) (c1*s2*c3 - s1*c2*s3),
                W = (float) (c1c2*c3 - s1s2*s3)
            };
        }
    }
}