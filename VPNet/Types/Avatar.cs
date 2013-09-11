using System;
using System.Globalization;
using VP.Native;

namespace VP
{
    /// <summary>
    /// Represents an immutable record of an avatar's in-world state, including position
    /// data
    /// </summary>
    public struct Avatar
    {
        #region Public fields
        /// <summary>
        /// Gets the name of this avatar
        /// </summary>
        public string Name;
        /// <summary>
        /// Gets the ID of the user this avatar belongs to
        /// </summary>
        public int Id;
        /// <summary>
        /// Gets the unique session ID of this avatar
        /// </summary>
        public int Session;
        /// <summary>
        /// Gets the currently set type of this avatar
        /// </summary>
        public int Type;
        /// <summary>
        /// Gets the last known position of this avatar
        /// </summary>
        public AvatarPosition Position; 
        #endregion

        #region Public properties
        /// <summary>
        /// Checks if the avatar belongs to a bot instance, i.e. if the name is surrounded
        /// in square brackets
        /// </summary>
        public bool IsBot
        {
            get { return Name.StartsWith("[") && Name.EndsWith("]"); }
        } 
        #endregion

        /// <summary>
        /// Creates an Avatar from a native instance's attributes
        /// </summary>
        internal Avatar (IntPtr pointer)
        {
            Name     = Functions.vp_string(pointer, StringAttributes.AvatarName);
            Id       = Functions.vp_int(pointer, IntAttributes.UserId);
            Session  = Functions.vp_int(pointer, IntAttributes.AvatarSession);
            Type     = Functions.vp_int(pointer, IntAttributes.AvatarType);
            Position = AvatarPosition.FromAvatar(pointer);
        }

        const string format = "{0} (#{1}, Session #{2})";
        /// <summary>
        /// Formats this Avatar state to a human-readable string
        /// </summary>
        public override string ToString()
        {
			return string.Format(CultureInfo.InvariantCulture, format, Name, Id, Session);
        }
    }
}
