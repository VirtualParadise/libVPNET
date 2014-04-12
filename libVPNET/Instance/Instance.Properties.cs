using System;
using System.Collections.Generic;
using VP.Native;

namespace VP
{
    public partial class Instance : IDisposable
    {
        string name = "";
        /// <summary>
        /// Logged in bot name, or blank if not logged in at least once
        /// </summary>
        public string Name
        {
            get { return name; }
        }

        string world = "";
        /// <summary>
        /// World currently logged into, or blank if not logged into a world
        /// </summary>
        public string World
        {
            get { return world; }
        }

        /// <summary>
        /// Gets this instance's current position in-world, including coordinates and
        /// rotation
        /// </summary>
        public AvatarPosition Position
        {
            get { return AvatarPosition.FromSelf(Pointer); }
        }
    }
}
