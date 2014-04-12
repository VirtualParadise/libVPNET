using System;
using VP.Extensions;
using VP.Native;

namespace VP
{
    /// <summary>
    /// Represents a user and its attributes from the universe
    /// </summary>
    public struct User
    {
        /// <summary>
        /// Gets the unique ID number of this user
        /// </summary>
        public int ID;
        /// <summary>
        /// Gets the canonical login name of this user
        /// </summary>
        public string Name;
        /// <summary>
        /// Gets the email address of this user
        /// </summary>
        /// <remarks>
        /// This value will be a blank string for users the bot has no permission to
        /// access the attributes of
        /// </remarks>
        public string Email;
        /// <summary>
        /// Gets the approximate amount of time this user has been online
        /// </summary>
        /// <remarks>
        /// This value will be a zero value for users the bot has no permission to access
        /// the attributes of
        /// </remarks>
        public TimeSpan OnlineTime;
        /// <summary>
        /// Gets the date and time this user registered
        /// </summary>
        /// <remarks>
        /// This value will be equivalent to the UNIX epoch (Janurary 1st 1970 00:00) for
        /// users the bot has no permission to access the attributes of
        /// </remarks>
        public DateTime RegistrationTime;
        /// <summary>
        /// Gets the date and time this user last logged in
        /// </summary>
        /// <remarks>
        /// This value will be equivalent to the UNIX epoch (Janurary 1st 1970 00:00) for
        /// users the bot has no permission to access the attributes of
        /// </remarks>
        public DateTime LastLogin;

        internal User(IntPtr pointer)
        {
            Name             = Functions.vp_string(pointer, StringAttributes.UserName);
            Email            = Functions.vp_string(pointer, StringAttributes.UserEmail);
            ID               = Functions.vp_int(pointer, IntAttributes.UserId);
            OnlineTime       = TimeSpan.FromSeconds( Functions.vp_int(pointer, IntAttributes.UserOnlineTime) );
            RegistrationTime = DateTimeExt.FromUnixTimestampUTC(Functions.vp_int(pointer, IntAttributes.UserRegistrationTime));
            LastLogin        = DateTimeExt.FromUnixTimestampUTC(Functions.vp_int(pointer, IntAttributes.UserLastLogin));
        }
    }
}
