using System;
using VP.Native;

namespace VP
{
    public struct User
    {
        public string Name;
        public string Email;
        public int ID;
        public DateTime OnlineTime;
        public DateTime RegistrationTime;
        public DateTime LastLogin;

        /// <summary>
        /// Creates user from native attributes
        /// </summary>
        internal User(IntPtr pointer)
        {
            Name = Functions.vp_string(pointer, VPAttribute.UserName);
            Email = Functions.vp_string(pointer, VPAttribute.UserEmail);
            ID = Functions.vp_int(pointer, VPAttribute.UserId);
            OnlineTime = DateTimeExt.FromUnixTimestampUTC(Functions.vp_int(pointer, VPAttribute.UserOnlineTime));
            RegistrationTime = DateTimeExt.FromUnixTimestampUTC(Functions.vp_int(pointer, VPAttribute.UserRegistrationTime));
            LastLogin = DateTimeExt.FromUnixTimestampUTC(Functions.vp_int(pointer, VPAttribute.UserLastLogin));
        }
    }
}
