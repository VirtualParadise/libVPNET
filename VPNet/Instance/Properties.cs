using System;
using System.Collections.Generic;
using VP.Native;

namespace VP
{
    public partial class Instance : IDisposable
    {
        #region World
        /// <summary>
        /// Gets this instance's current position in-world, including coordinates and
        /// rotation
        /// </summary>
        public AvatarPosition Position
        {
            get { return AvatarPosition.FromSelf(pointer); }
        }
        #endregion
    }
}
