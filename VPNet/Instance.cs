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
        /// Methods, events and properties related to universe connectivity,
        /// authentication and metadata
        /// </summary>
        public InstanceUniverse Universe;
        /// <summary>
        /// Methods, events and properties related to world connectivity
        /// </summary>
        public InstanceWorld World;
        /// <summary>
        /// Methods, events and properties related to communications
        /// </summary>
        public InstanceComms Comms;
        /// <summary>
        /// Methods, events and properties related to property and object handling,
        /// including queries
        /// </summary>
        public InstanceProperty Property;
        #endregion

        internal readonly IntPtr pointer;

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
        }

        ~Instance()
        {
            if (pointer != IntPtr.Zero)
                lock (this)
                    Functions.vp_destroy(pointer);
        }

        #region Public instantation properties
        public string Name;
        public string UserName;
        public string Password; 
        #endregion

        #region Public SDK methods
        public void Wait(int milliseconds)
        {
            int rc = Functions.vp_wait(pointer, milliseconds);
            if (rc != 0) throw new VPException((ReasonCode)rc);
        }

        /// <summary>
        /// Logs into the default Virtual Paradise universe with pre-set authentication
        /// details (clears UserName and Password) and automatically enters the preset
        /// world. Chainable.
        /// </summary>
        public Instance Login()
        {
            Universe.Login(UserName, Password, Name);
            UserName = null;
            Password = null;

            return this;
        }

        public void Dispose()
        {
            if (pointer != IntPtr.Zero)
                Functions.vp_destroy(pointer);

            Universe.Dispose();
            World.Dispose();
            Comms.Dispose();
            Property.Dispose();
            GC.SuppressFinalize(this);
        }
        #endregion

        void setup()
        {
            this.Universe = new InstanceUniverse { instance = this };
            this.World = new InstanceWorld { instance = this };
            this.Comms = new InstanceComms { instance = this };
            this.Property = new InstanceProperty { instance = this };

            this.Universe.SetNativeEvents();
            this.World.SetNativeEvents();
            this.Comms.SetNativeEvents();
            this.Property.SetNativeEvents();
        }

        #region Native events and callbacks
        /// <summary>
        /// Generic event for all containers
        /// </summary>
        public delegate void Event(Instance sender);

        Dictionary<Events, EventDelegate> _nativeEvents = new Dictionary<Events, EventDelegate>();
        Dictionary<Callbacks, CallbackDelegate> _nativeCallbacks = new Dictionary<Callbacks, CallbackDelegate>();
        
        internal void SetNativeEvent(Events eventType, EventDelegate eventFunction)
        {
            _nativeEvents[eventType] = eventFunction;
            Functions.vp_event_set(pointer, (int)eventType, eventFunction);
        }

        internal void SetNativeCallback(Callbacks callbackType, CallbackDelegate callbackFunction)
        {
            _nativeCallbacks[callbackType] = callbackFunction;
            Functions.vp_callback_set(pointer, (int)callbackType, callbackFunction);
        }
        #endregion
    }
}
