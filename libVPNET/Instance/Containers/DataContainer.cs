using System;
using VP.Native;

namespace VP
{
    /// <summary>
    /// Container for SDK methods, events and properties related to data collection, such
    /// as world and user metadata
    /// </summary>
    public class DataContainer
    {
        #region Construction & disposal
        Instance instance;

        internal DataContainer(Instance instance)
        {
            this.instance = instance;
            instance.setNativeEvent(Events.WorldList, OnWorldList);
            instance.setNativeEvent(Events.WorldSetting, OnWorldSetting);
            instance.setNativeEvent(Events.WorldSettingsDone, OnWorldSettingsDone);
            instance.setNativeEvent(Events.UserAttributes, OnUserAttributes);
        }

        internal void Dispose()
        {
            WorldEntry        = null;
            WorldSetting      = null;
            WorldSettingsDone = null;
            UserAttributes    = null;
        }
        #endregion

        #region Events
        /// <summary>
        /// Encapsulates a method that accepts a source <see cref="Instance"/> and a
        /// <see cref="World"/> state entry for the <see cref="WorldEntry"/> event
        /// </summary>
        public delegate void WorldListArgs(Instance sender, World world);
        /// <summary>
        /// Encapsulates a method that accepts a source <see cref="Instance"/> and a
        /// string key and value representing a world setting for the
        /// <see cref="WorldSetting"/> event
        /// </summary>
        public delegate void WorldSettingArgs(Instance sender, string key, string value);
        /// <summary>
        /// Encapsulates a method that accepts a source <see cref="Instance"/> and a
        /// <see cref="User"/> attributes entry for the <see cref="UserAttributes"/>
        /// event
        /// </summary>
        public delegate void UserAttributesArgs(Instance sender, User user);

        /// <summary>
        /// Fired automatically for each world setting after a successful
        /// <see cref="Instance.Enter"/> call
        /// </summary>
        public event WorldSettingArgs WorldSetting;
        /// <summary>
        /// Fired automatically when the server has sent all world settings after a
        /// successful <see cref="Instance.Enter"/> call
        /// </summary>
        public event Instance.Event WorldSettingsDone;
        /// <summary>
        /// Fired automatically for any world that updates its state or for the
        /// destination world after a successful <see cref="Instance.Enter"/> call.
        /// Also called for each world from a <see cref="ListWorlds"/> call.
        /// </summary>
        public event WorldListArgs WorldEntry;
        /// <summary>
        /// Fired when the attributes of an existing user is requested from
        /// <see cref="GetUserAttributes(string)"/> or <see cref="GetUserAttributes(int)"/>
        /// </summary>
        public event UserAttributesArgs UserAttributes; 
        #endregion

        #region Event handlers
        internal void OnWorldSetting(IntPtr sender)
        {
            if (WorldSetting == null)
                return;

            var key   = Functions.vp_string(instance.Pointer, StringAttributes.WorldSettingKey);
            var value = Functions.vp_string(instance.Pointer, StringAttributes.WorldSettingValue);

            WorldSetting(instance, key, value);
        }

        internal void OnWorldSettingsDone(IntPtr sender)
        {
            if (WorldSettingsDone != null)
                WorldSettingsDone(instance);
        }

        internal void OnWorldList(IntPtr sender)
        {
            if (WorldEntry != null)
                WorldEntry( instance, new World(instance.Pointer) );
        }

        internal void OnUserAttributes(IntPtr sender)
        {
            if (UserAttributes != null)
                UserAttributes(instance, new User(instance.Pointer) );
        }
        #endregion

        #region Methods
        /// <summary>
        /// Requests user attributes by name. Currently not implemented natively.
        /// </summary>
        public void GetUserAttributes(string name)
        {
            throw new NotImplementedException();

            //lock (instance.mutex)
            //    Functions.Call( () => Functions.vp_user_attributes_by_name(instance.pointer, name) );
        }

        /// <summary>
        /// Requests user attributes by account unique ID number. Thread-safe.
        /// </summary>
        /// <seealso cref="UserAttributes"/>
        public void GetUserAttributes(int id)
        {
            lock (instance.Mutex)
                Functions.Call( () => Functions.vp_user_attributes_by_id(instance.Pointer, id) );
        }

        /// <summary>
        /// Requests the latest list of worlds and their states from the universe.
        /// Thread-safe.
        /// </summary>
        /// <seealso cref="WorldEntry"/>
        public void ListWorlds()
        {
            lock (instance.Mutex)
                Functions.Call( () => Functions.vp_world_list(instance.Pointer, 0) );
        } 
        #endregion
    }
}
