using System;
using System.Collections.Generic;
using VP.Native;

namespace VP
{
    public partial class Instance : IDisposable
    {
        public const int VPSDK_VERSION = 1;
        static bool _isInitialized;

        #region Member containers
        /// <summary>
        /// Methods, events and properties related to metadata collection
        /// </summary>
        public InstanceData Data;
        /// <summary>
        /// Methods, events and properties related to users and avatars
        /// </summary>
        public InstanceAvatars Avatars;
        /// <summary>
        /// Methods, events and properties related to property and object handling,
        /// including queries
        /// </summary>
        public InstanceProperty Property;
        /// <summary>
        /// Methods, events and properties related to terrain modificaton and queries
        /// </summary>
        public InstanceTerrain Terrain;
        #endregion

        #region Constructor, setup & deconstructor
        internal readonly IntPtr pointer;

        /// <summary>
        /// Creates a bot instance, initializing the SDK automatically
        /// </summary>
        public Instance()
        {
            if (!_isInitialized)
            {
                int rc = Functions.vp_init(VPSDK_VERSION);
                if (rc != 0)
                    throw new VPException((ReasonCode)rc);

                _isInitialized = true;
            }

            pointer = Functions.vp_create();
            setup();
            setupEvents();
        }

        /// <summary>
        /// Creates a bot instance with a given name, initializing the SDK automatically
        /// </summary>
        public Instance(string name)
            : this()
        {
            this.Name = name;
        }

        ~Instance()
        {
            if (pointer != IntPtr.Zero)
                lock (this)
                    Functions.vp_destroy(pointer);
        }

        /// <summary>
        /// Disposes of the bot by destorying it natively, then disposes of all
        /// containers.
        /// </summary>
        public void Dispose()
        {
            if (pointer != IntPtr.Zero)
                Functions.vp_destroy(pointer);

            Data.Dispose();
            Avatars.Dispose();
            Property.Dispose();
            Terrain.Dispose();
            disposeEvents();
            GC.SuppressFinalize(this);
        }

        void setup()
        {
            this.Data = new InstanceData(this);
            this.Avatars = new InstanceAvatars(this);
            this.Property = new InstanceProperty(this);
            this.Terrain = new InstanceTerrain(this);
        }
        #endregion

        #region Public properties
        public string Name;
        public string CurrentWorld;
        #endregion
    }
}
