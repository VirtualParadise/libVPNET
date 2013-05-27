using System;
using System.Globalization;

namespace VP
{
    public struct AvatarPosition
    {
        public static AvatarPosition GroundZero = new AvatarPosition();

        public float X;
        public float Y;
        public float Z;
        public float Pitch;
        public float Yaw;

        /// <summary>
        /// Gets or sets a Vector3 value for coordinates
        /// </summary>
        public Vector3 Coordinates
        {
            get { return new Vector3(X, Y, Z); }
            set
            {
                X = value.X;
                Y = value.Y;
                Z = value.Z;
            }
        }

        public AvatarPosition(Vector3 pos, float pitch, float yaw)
        {
            X     = pos.X;
            Y     = pos.Y;
            Z     = pos.Z;
            Pitch = pitch;
            Yaw   = yaw;
        }

        public AvatarPosition(float x, float y, float z, float pitch, float yaw)
        {
            X     = x;
            Y     = y;
            Z     = z;
            Pitch = pitch;
            Yaw   = yaw;
        }

        /// <summary>
        /// Formats the AvatarPosition to a human readable string
        /// </summary>
        public override string ToString()
        {
			return string.Format(CultureInfo.InvariantCulture, 
			                     "X: {0} Y: {1} Z: {2} Pitch: {3} Yaw: {4}", 
			                     X, Y, Z, Pitch, Yaw);
        }
    }
}