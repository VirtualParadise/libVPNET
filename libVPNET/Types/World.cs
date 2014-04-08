using System;
using VP.Native;

namespace VP
{
    /// <summary>
    /// Represents the possible states of a world
    /// </summary>
    public enum WorldState : int
    {
        /// <summary>
        /// Online world
        /// </summary>
        Online,
        /// <summary>
        /// Offline world
        /// </summary>
        Stopped,
        /// <summary>
        /// No known state
        /// </summary>
        Unknown
    }

    /// <summary>
    /// Represents a world's state in a universe
    /// </summary>
    public struct World
    {
        /// <summary>
        /// Gets the name of this world
        /// </summary>
        public string Name;
        /// <summary>
        /// Gets the current amount of users in-world
        /// </summary>
        public int UserCount;
        /// <summary>
        /// Gets the current state of this world
        /// </summary>
        public WorldState State;

        internal World(IntPtr pointer)
        {
            Name      = Functions.vp_string(pointer, StringAttributes.WorldName);
            UserCount = Functions.vp_int(pointer, IntAttributes.WorldUsers);
            State     = (WorldState) Functions.vp_int(pointer, IntAttributes.WorldState);
        }
    }
}
