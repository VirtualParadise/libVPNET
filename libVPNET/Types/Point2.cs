using System;
using VP.Native;

namespace VP
{
    /// <summary>
    /// Represents an immutable tuple of two floats, reperesenting a point in 2D space on
    /// a XZ axis
    /// </summary>
    public struct Point2F
    {
        public static readonly Point2F Zero = new Point2F(0, 0);

        /// <summary>
        /// Gets the X value of this point
        /// </summary>
        public float X;
        /// <summary>
        /// Gets the Z value of this point
        /// </summary>
        public float Z;
        
        internal Point2F(float x, float z)
        {
            this.X = x;
            this.Z = z;
        }
    }

    /// <summary>
    /// Represents an immutable tuple of two ints, reperesenting a point in 2D space on a
    /// XZ axis
    /// </summary>
    public struct Point2I
    {
        public static readonly Point2I Zero = new Point2I(0, 0);

        /// <summary>
        /// Gets the X value of this point
        /// </summary>
        public int X;
        /// <summary>
        /// Gets the Z value of this point
        /// </summary>
        public int Z;

        internal Point2I(int x, int z)
        {
            this.X = x;
            this.Z = z;
        }
    }
}

