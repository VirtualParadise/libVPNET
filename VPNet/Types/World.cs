using System;
using VP.Native;

namespace VP
{
    public enum WorldState : int
    {
        Online,
        Stopped,
        Unknown
    }

    public struct World
    {
        public string     Name;
        public int        UserCount;
        public WorldState State;

        /// <summary>
        /// Creates world metadata from native attributes
        /// </summary>
        internal World(IntPtr pointer)
        {
            Name      = Functions.vp_string(pointer, StringAttributes.WorldName);
            State     = (WorldState) Functions.vp_int(pointer, IntAttributes.WorldState);
            UserCount = Functions.vp_int(pointer, IntAttributes.WorldUsers);
        }
    }
}
