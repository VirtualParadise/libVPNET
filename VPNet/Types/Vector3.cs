using System;
using System.Globalization;

namespace VP
{
    public struct Vector3
    {
        public static Vector3 Zero = new Vector3();

        public float X;
        public float Y;
        public float Z;

        public Vector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vector3(float a)
        {
            X = a;
            Y = a;
            Z = a;
        }

        public Vector3(string csv)
        {
            var parts = csv.Split(new[] { ',' }, 3, StringSplitOptions.RemoveEmptyEntries);
			X = float.Parse(parts[0], CultureInfo.InvariantCulture);
			Y = float.Parse(parts[1], CultureInfo.InvariantCulture);
			Z = float.Parse(parts[2], CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Formats the vector to a comma-seperated list
        /// </summary>
        public override string ToString()
        {
			return string.Format(CultureInfo.InvariantCulture,
			                     "{0},{1},{2}", X, Y, Z);
        }
    }
}