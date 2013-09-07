using System;
using System.Collections.Generic;
using VP.Native;

namespace VP
{
    public partial class Instance : IDisposable
    {
        const  int  sdkversion = 1;
        static bool isInitialized;

        #region Member containers
        /// <summary>
        /// Methods, events and properties related to user or world list data
        /// </summary>
        public DataContainer Data;
        /// <summary>
        /// Methods, events and properties related to users and avatars
        /// </summary>
        public AvatarsContainer Avatars;
        /// <summary>
        /// Methods, events and properties related to property and object handling,
        /// including queries
        /// </summary>
        public PropertyContainer Property;
        /// <summary>
        /// Methods, events and properties related to terrain modificaton and queries
        /// </summary>
        public TerrainContainer Terrain;
        #endregion

        #region Public properties
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
        #endregion

        #region Constructor, setup & deconstructor
        internal readonly IntPtr pointer;

        /// <summary>
        /// Creates a bot instance, initializing the SDK automatically
        /// </summary>
        public Instance()
        {
            if (!isInitialized)
            {
                // Unpack DLL
                DLLHandler.Unpack();

                // Init SDK
                int rc = Functions.vp_init(sdkversion);
                if (rc != 0)
                    throw new VPException((ReasonCode)rc);

                isInitialized = true;
            }

            pointer = Functions.vp_create();
            setup();
            setupEvents();
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
            this.Data     = new DataContainer(this);
            this.Avatars  = new AvatarsContainer(this);
            this.Property = new PropertyContainer(this);
            this.Terrain  = new TerrainContainer(this);
        }
        #endregion
        
    }
}
