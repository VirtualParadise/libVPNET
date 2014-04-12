using System;
using System.Globalization;
using VP.Native;

namespace VP
{
    /// <summary>
    /// Represents an immutable value of 3D Cartesian coordinates and rotations of any
    /// avatar
    /// </summary>
    public struct AvatarPosition
    {
        /// <summary>
        /// Represents an avatar position at a world's ground zero (zero position and
        /// rotations)
        /// </summary>
        public static readonly AvatarPosition GroundZero = new AvatarPosition(); 

        #region Native creators
        internal static AvatarPosition FromSelf(IntPtr pointer)
        {
            return new AvatarPosition
            {
                X     = Functions.vp_float(pointer, FloatAttributes.MyX),
                Y     = Functions.vp_float(pointer, FloatAttributes.MyY),
                Z     = Functions.vp_float(pointer, FloatAttributes.MyZ),
                Yaw   = Functions.vp_float(pointer, FloatAttributes.MyYaw),
                Pitch = Functions.vp_float(pointer, FloatAttributes.MyPitch),
            };
        }

        internal static AvatarPosition FromAvatar(IntPtr pointer)
        {
            return new AvatarPosition
            {
                X     = Functions.vp_float(pointer, FloatAttributes.AvatarX),
                Y     = Functions.vp_float(pointer, FloatAttributes.AvatarY),
                Z     = Functions.vp_float(pointer, FloatAttributes.AvatarZ),
                Yaw   = Functions.vp_float(pointer, FloatAttributes.AvatarYaw),
                Pitch = Functions.vp_float(pointer, FloatAttributes.AvatarPitch),
            };
        }

        internal static AvatarPosition FromTeleport(IntPtr pointer)
        {
            return new AvatarPosition
            {
                X     = Functions.vp_float(pointer, FloatAttributes.TeleportX),
                Y     = Functions.vp_float(pointer, FloatAttributes.TeleportY),
                Z     = Functions.vp_float(pointer, FloatAttributes.TeleportZ),
                Yaw   = Functions.vp_float(pointer, FloatAttributes.TeleportYaw),
                Pitch = Functions.vp_float(pointer, FloatAttributes.TeleportPitch),
            };
        }
        #endregion

        #region Public fields
        /// <summary>
        /// Gets the X (east-west) coordinate of this position
        /// </summary>
        public float X;
        /// <summary>
        /// Gets the Y (altitude) coordinate of this position
        /// </summary>
        public float Y;
        /// <summary>
        /// Gets the Z (south-north) coordinate of this position
        /// </summary>
        public float Z;
        /// <summary>
        /// Gets the yaw (left-right) rotation of this position in degrees
        /// </summary>
        public float Yaw; 
        /// <summary>
        /// Gets the pitch (down-up) rotation of this position in degrees
        /// </summary>
        public float Pitch;
        #endregion

        #region Public properties
        /// <summary>
        /// Gets a Vector3D value for coordinates
        /// </summary>
        public Vector3 Coordinates
        {
            get { return new Vector3(X, Y, Z); }
        }
        #endregion

        #region Public constructors
        /// <summary>
        /// Creates a new AvatarPosition from a given Vector3D for coordinates and pitch
        /// and yaw values for rotation
        /// </summary>
        /// <param name="pos">Coordinates of position using a Vector3D</param>
        /// <param name="yaw">Yaw (left-right) rotation in degrees</param>
        /// <param name="pitch">Pitch (down-up) rotation in degrees</param>
        public AvatarPosition(Vector3 pos, float yaw, float pitch)
        {
            X     = pos.X;
            Y     = pos.Y;
            Z     = pos.Z;
            Yaw   = yaw;
            Pitch = pitch;
        }

        /// <summary>
        /// Creates a new AvatarPosition from a given set of coordinates and pitch and
        /// yaw values for rotation
        /// </summary>
        /// <param name="x">X (east-west) coordinate of position</param>
        /// <param name="y">Y (altitude) coordinate of position</param>
        /// <param name="z">Z (south-north) coordinate of position</param>
        /// <param name="yaw">Yaw (left-right) rotation in degrees</param>
        /// <param name="pitch">Pitch (down-up) rotation in degrees</param>
        public AvatarPosition(float x, float y, float z, float yaw, float pitch)
        {
            X     = x;
            Y     = y;
            Z     = z;
            Yaw   = yaw;
            Pitch = pitch;
        }
        #endregion

        const string format = "X: {0} Y: {1} Z: {2} Yaw: {3}° Pitch: {4}°";
        /// <summary>
        /// Formats this AvatarPosition to a human-readable string
        /// </summary>
        public override string ToString()
        {
			return string.Format(CultureInfo.InvariantCulture, format, X, Y, Z, Yaw, Pitch);
        }
    }
}