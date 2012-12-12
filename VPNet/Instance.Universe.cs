using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VP.Native;
using VP.Interfaces;

namespace VP
{
    public class InstanceUniverse : IInstanceContainer
    {
        internal Instance instance;

        #region IInstanceContainer
        public void SetNativeEvents()
        {
            instance.SetNativeEvent(Events.UniverseDisconnect, OnUniverseDisconnect);
            instance.SetNativeEvent(Events.WorldList, OnWorldList);
            instance.SetNativeEvent(Events.UserAttributes, OnUserAttributes);
        }

        public void Dispose()
        {
            WorldList = null;
            Disconnect = null;
            UserAttributes = null;
        }
        #endregion

        #region Events
        public delegate void WorldListArgs(Instance sender, World world);
        public delegate void UserAttributesArgs(Instance sender, User user);

        public event Instance.Event Disconnect;
        public event WorldListArgs WorldList;
        public event UserAttributesArgs UserAttributes; 
        #endregion

        #region Methods
        /// <summary>
        /// Logs into a specified universe with the given authentication details and
        /// bot name
        /// </summary>
        public void Login(Uniserver universe, string username, string password, string botname)
        {
            int rc;
            lock (instance)
                rc = Functions.vp_connect_universe(instance.pointer, universe.Host, universe.Port);

            if (rc != 0) throw new VPException((ReasonCode)rc);

            lock (instance)
                rc = Functions.vp_login(instance.pointer, username, password, botname);

            if (rc != 0) throw new VPException((ReasonCode)rc);
        }

        /// <summary>
        /// Logs into the default Virtual Paradise universe with the given authentication
        /// details
        /// </summary>
        public void Login(string username, string password, string botname)
        { Login(Uniserver.VirtualParadise, username, password, botname); }

        public void ListWorlds()
        {
            int rc;
            lock (instance)
                rc = Functions.vp_world_list(instance.pointer, 0);

            if (rc != 0) throw new VPException((ReasonCode)rc);
        } 
        #endregion

        #region Event handlers
        internal void OnUniverseDisconnect(IntPtr sender)
        {
            if (Disconnect == null) return;
            Disconnect(instance);
        } 

        internal void OnWorldList(IntPtr sender)
        {
            if (WorldList == null) return;
            World data;

            lock (instance)
                data = new World(instance.pointer);

            WorldList(instance, data);
        }

        internal void OnUserAttributes(IntPtr sender)
        {
            if (WorldList == null) return;
            User data;

            lock (instance)
                data = new User(instance.pointer);

            UserAttributes(instance, data);
        }
        #endregion
    }
}
