using Nexus;
using System;
using VP.Native;

namespace VP
{
    /// <summary>
    /// Represents a click on an object in 3D space
    /// </summary>
    public struct ObjectClick
    {
        /// <summary>
        /// Gets the unique ID of the object that is the target of this click
        /// </summary>
        public int Id;
        /// <summary>
        /// Gets the unique session ID of the user that is the source of this click
        /// </summary>
        public int Session;
        /// <summary>
        /// Gets the Vector3D representing the coordinates of this click
        /// </summary>
        public Vector3D Position;

        internal ObjectClick(IntPtr pointer)
        {
            Id       = Functions.vp_int(pointer, IntAttributes.ObjectId);
            Session  = Functions.vp_int(pointer, IntAttributes.AvatarSession);
            Position = VPVector3D.FromClick(pointer);
        }
    }

    /// <summary>
    /// Represents a click on an avatar in 3D space
    /// </summary>
    public struct AvatarClick
    {
        /// <summary>
        /// Gets the unique session ID of the user that is the source of this click
        /// </summary>
        public int SourceSession;
        /// <summary>
        /// Gets the unique session ID of the user that is the target of this click
        /// </summary>
        public int TargetSession;
        /// <summary>
        /// Gets the Vector3D representing the coordinates of this click
        /// </summary>
        public Vector3D Position;

        internal AvatarClick(IntPtr pointer)
        {
            SourceSession = Functions.vp_int(pointer, IntAttributes.AvatarSession);
            TargetSession = Functions.vp_int(pointer, IntAttributes.ClickedSession);
            Position = VPVector3D.FromClick(pointer);
        }
    }
}
