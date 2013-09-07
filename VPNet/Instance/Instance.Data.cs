using System;
using VP.Native;

namespace VP
{
    /// <summary>
    /// Container for SDK methods, events and properties related to data collection, such
    /// as world and user metadata
    /// </summary>
    public class InstanceData
    {
        Instance instance;

        public InstanceData(Instance instance)
        {
            this.instance = instance;
            instance.SetNativeEvent(Events.WorldList, OnWorldList);
            instance.SetNativeEvent(Events.WorldSetting, OnWorldSetting);
            instance.SetNativeEvent(Events.WorldSettingsDone, OnWorldSettingsDone);
            instance.SetNativeEvent(Events.UserAttributes, OnUserAttributes);
        }

        internal void Dispose()
        {
            WorldEntry     = null;
            WorldSetting   = null;
            UserAttributes = null;

            WorldSettingsChanged = null;
        }

        #region Events
        public delegate void WorldListArgs(Instance sender, World world);
        public delegate void UserAttributesArgs(Instance sender, User user);
        public delegate void WorldSettingArgs(Instance sender, string key, string value);

        public event Instance.Event     WorldSettingsChanged;
        public event WorldSettingArgs   WorldSetting;
        public event WorldListArgs      WorldEntry;
        public event UserAttributesArgs UserAttributes; 
        #endregion

        #region Methods
        public void GetUserAttributes(string name)
        {
            int rc;
            lock (instance)
                rc = Functions.vp_user_attributes_by_name(instance.pointer, name);

            if (rc != 0) throw new VPException((ReasonCode)rc);
        }

        public void GetUserAttributes(int id)
        {
            int rc;
            lock (instance)
                rc = Functions.vp_user_attributes_by_id(instance.pointer, id);

            if (rc != 0) throw new VPException((ReasonCode)rc);
        }

        public void ListWorlds()
        {
            int rc;
            lock (instance)
                rc = Functions.vp_world_list(instance.pointer, 0);

            if (rc != 0) throw new VPException((ReasonCode)rc);
        } 
        #endregion

        #region Event handlers
        internal void OnWorldSetting(IntPtr sender)
        {
            if (WorldSetting == null) return;
            string key, value;
            lock (instance)
            {
                key   = Functions.vp_string(instance.pointer, StringAttributes.WorldSettingKey);
                value = Functions.vp_string(instance.pointer, StringAttributes.WorldSettingValue);
            }

            WorldSetting(instance, key, value);
        }

        internal void OnWorldSettingsDone(IntPtr sender)
        {
            if (WorldSettingsChanged == null) return;
            WorldSettingsChanged(instance);
        }

        internal void OnWorldList(IntPtr sender)
        {
            if (WorldEntry == null) return;
            World data;

            lock (instance)
                data = new World(instance.pointer);

            WorldEntry(instance, data);
        }

        internal void OnUserAttributes(IntPtr sender)
        {
            if (UserAttributes == null) return;
            User data;

            lock (instance)
                data = new User(instance.pointer);

            UserAttributes(instance, data);
        }
        #endregion
    }
}
