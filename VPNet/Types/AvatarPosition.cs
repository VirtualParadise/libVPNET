using System;

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

        /// <summary>
        /// Creates an avatar position from a comma-seperated list
        /// </summary>
        public AvatarPosition(string csv)
        {
            var parts = csv.Split(new[] { ',' }, 5, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length != 5)
                throw new ArgumentException("Not a valid AvatarPosition CSV string");

            X = float.Parse(parts[0]);
            Y = float.Parse(parts[1]);
            Z = float.Parse(parts[2]);
            Pitch = float.Parse(parts[3]);
            Yaw = float.Parse(parts[4]);
        }

        public AvatarPosition(Vector3 pos, float pitch, float yaw)
        {
            X = pos.X;
            Y = pos.Y;
            Z = pos.Z;
            Pitch = pitch;
            Yaw = yaw;
        }

        public AvatarPosition(float x, float y, float z, float pitch, float yaw)
        {
            X = x;
            Y = y;
            Z = z;
            Pitch = pitch;
            Yaw = yaw;
        }

        /// <summary>
        /// Formats the position to a comma-seperated list
        /// </summary>
        public override string ToString()
        {
            return string.Format("{0},{1},{2},{3},{4}", X, Y, Z, Pitch, Yaw);
        }
    }
}