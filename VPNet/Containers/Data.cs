using System;
using VP.Native;

namespace VP
{
    /// <summary>
    /// Container for SDK methods and properties related to data collection, such as
    /// world and user metadata
    /// </summary>
    public class InstanceData : IDisposable
    {
        internal Instance instance;

        public InstanceData(Instance instance)
        {
            this.instance = instance;
            instance.SetNativeEvent(Events.WorldList, OnWorldList);
            instance.SetNativeEvent(Events.WorldSetting, OnWorldSetting);
            instance.SetNativeEvent(Events.WorldSettingsChanged, OnWorldSettingsChanged);
            instance.SetNativeEvent(Events.UserAttributes, OnUserAttributes);
        }

        public void Dispose()
        {
            GetWorldEntry = null;
            GetWorldSetting = null;
            GetUserAttributes = null;

            WorldSettingsChanged = null;
        }

        #region Events
        public delegate void WorldListArgs(Instance sender, World world);
        public delegate void UserAttributesArgs(Instance sender, User user);
        public delegate void WorldSettingArgs(Instance sender, string key, string value);

        public event Instance.Event WorldSettingsChanged;
        public event WorldSettingArgs GetWorldSetting;
        public event WorldListArgs GetWorldEntry;
        public event UserAttributesArgs GetUserAttributes; 
        #endregion

        #region Methods
        internal void OnWorldSetting(IntPtr sender)
        {
            if (GetWorldSetting == null) return;
            string key, value;
            lock (instance)
            {
                key = Functions.vp_string(instance.pointer, VPAttribute.WorldSettingKey);
                value = Functions.vp_string(instance.pointer, VPAttribute.WorldSettingValue);
            }

            GetWorldSetting(instance, key, value);
        }

        internal void OnWorldSettingsChanged(IntPtr sender)
        {
            if (WorldSettingsChanged == null) return;
            WorldSettingsChanged(instance);
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
        internal void OnWorldList(IntPtr sender)
        {
            if (GetWorldEntry == null) return;
            World data;

            lock (instance)
                data = new World(instance.pointer);

            GetWorldEntry(instance, data);
        }

        internal void OnUserAttributes(IntPtr sender)
        {
            if (GetWorldEntry == null) return;
            User data;

            lock (instance)
                data = new User(instance.pointer);

            GetUserAttributes(instance, data);
        }
        #endregion
    }
}
