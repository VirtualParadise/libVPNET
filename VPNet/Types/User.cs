using System;
using VP.Native;

namespace VP
{
    public struct User
    {
        public string   Name;
        public string   Email;
        public int      ID;
        public DateTime OnlineTime;
        public DateTime RegistrationTime;
        public DateTime LastLogin;

        /// <summary>
        /// Creates user from native attributes
        /// </summary>
        internal User(IntPtr pointer)
        {
            Name             = Functions.vp_string(pointer, StringAttributes.UserName);
            Email            = Functions.vp_string(pointer, StringAttributes.UserEmail);
            ID               = Functions.vp_int(pointer, IntAttributes.UserId);
            OnlineTime       = DateTimeExt.FromUnixTimestampUTC(Functions.vp_int(pointer, IntAttributes.UserOnlineTime));
            RegistrationTime = DateTimeExt.FromUnixTimestampUTC(Functions.vp_int(pointer, IntAttributes.UserRegistrationTime));
            LastLogin        = DateTimeExt.FromUnixTimestampUTC(Functions.vp_int(pointer, IntAttributes.UserLastLogin));
        }
    }
}
