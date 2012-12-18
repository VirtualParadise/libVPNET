using System;
using VP;
using VP.Native;

namespace VP
{
    public struct Avatar
    {
        public string Name;
        public int Session;
        public int AvatarType;
        public float X, Y, Z;
        public float Yaw, Pitch;

        /// <summary>
        /// Checks if the avatar belongs to a bot instance, i.e. if the name is surrounded
        /// in square brackets
        /// </summary>
        public bool IsBot
        {
            get { return Name.StartsWith("[") && Name.EndsWith("]"); }
        }

        /// <summary>
        /// Creates an Avatar from a native instance's attributes
        /// </summary>
        internal Avatar (IntPtr pointer)
        {
            Name = Functions.vp_string(pointer, VPAttribute.AvatarName);
            Session = Functions.vp_int(pointer, VPAttribute.AvatarSession);
            AvatarType = Functions.vp_int(pointer, VPAttribute.AvatarType);
            X = Functions.vp_float(pointer, VPAttribute.AvatarX);
            Y = Functions.vp_float(pointer, VPAttribute.AvatarY);
            Z = Functions.vp_float(pointer, VPAttribute.AvatarZ);
            Yaw = Functions.vp_float(pointer, VPAttribute.AvatarYaw);
            Pitch = Functions.vp_float(pointer, VPAttribute.AvatarPitch);
        }
    }
}
