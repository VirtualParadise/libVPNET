using System;
using VP.Interfaces;
using VP.Native;

namespace VP
{
    /// <summary>
    /// Container class for Instance's world-related members
    /// </summary>
    public class InstanceWorld : IInstanceContainer
    {
        internal Instance instance;

        #region IInstanceContainer
        public void SetNativeEvents()
        {
            instance.SetNativeEvent(Events.WorldDisconnect, OnWorldDisconnect);
            instance.SetNativeEvent(Events.WorldSetting, OnSetting);
            instance.SetNativeEvent(Events.WorldSettingsChanged, OnSettingsChanged);
            instance.SetNativeEvent(Events.AvatarAdd, OnAvatarAdd);
            instance.SetNativeEvent(Events.AvatarChange, OnAvatarChange);
            instance.SetNativeEvent(Events.AvatarDelete, OnAvatarDelete);
        }

        public void Dispose()
        {
            Disconnect = null;
            Setting = null;
            SettingsChanged = null;
            AvatarAdd = null;
            AvatarChange = null;
            AvatarDelete = null;
        }
        #endregion

        #region Events
        public delegate void AvatarArgs(Instance sender, Avatar eventData);
        public delegate void SettingArgs(Instance sender, string key, string value);

        public event Instance.Event Disconnect;
        public event SettingArgs Setting;
        public event Instance.Event SettingsChanged;
        public event AvatarArgs AvatarAdd;
        public event AvatarArgs AvatarChange;
        public event AvatarArgs AvatarDelete;
        #endregion

        /// <summary>
        /// Gets the currently entered world, or null if none
        /// </summary>
        public string Current;

        #region Methods
        /// <summary>
        /// Enters a given world, chainable
        /// TODO: check if leaves current world
        /// </summary>
        public Instance Enter(string worldname)
        {
            int rc;
            lock (instance)
                rc = Functions.vp_enter(instance.pointer, worldname);

            if (rc != 0)
                throw new VPException((ReasonCode)rc);
            else
                Current = worldname;

            return instance;
        }

        /// <summary>
        /// Leaves the current world
        /// </summary>
        public void Leave()
        {
            int rc;
            lock (instance)
                rc = Functions.vp_leave(instance.pointer);

            if (rc != 0)
                throw new VPException((ReasonCode)rc);
            else
                Current = null;
        }

        /// <summary>
        /// Updates the bot's own position and rotation
        /// </summary>
        public void UpdateAvatar(float x = 0.0f, float y = 0.0f, float z = 0.0f,
            float yaw = 0.0f, float pitch = 0.0f)
        {
            int rc;
            lock (instance)
            {
                Functions.vp_float_set(instance.pointer, VPAttribute.MyX, x);
                Functions.vp_float_set(instance.pointer, VPAttribute.MyY, y);
                Functions.vp_float_set(instance.pointer, VPAttribute.MyZ, z);
                Functions.vp_float_set(instance.pointer, VPAttribute.MyYaw, yaw);
                Functions.vp_float_set(instance.pointer, VPAttribute.MyPitch, pitch);
                rc = Functions.vp_state_change(instance.pointer);
            }

            if (rc != 0) throw new VPException((ReasonCode)rc);
        }

        /// <summary>
        /// Sends a click event to an avatar by session number
        /// </summary>
        public void ClickAvatar(int session)
        {
            int rc;
            lock (instance)
                rc = Functions.vp_avatar_click(instance.pointer, session);

            if (rc != 0) throw new VPException((ReasonCode)rc);
        }

        public void TeleportAvatar(int session, string world, Vector3 pos, float yaw, float pitch)
        {
            int rc;
            lock (instance)
                rc = Functions.vp_teleport_avatar(
                    instance.pointer,
                    session,
                    world,
                    pos.X, pos.Y, pos.Z,
                    yaw, pitch);

            if (rc != 0) throw new VPException((ReasonCode)rc);
        }
        #endregion

        #region Event handlers
        internal void OnWorldDisconnect(IntPtr sender)
        {
            if (Disconnect == null) return;
            Disconnect(instance);
        }

        internal void OnSetting(IntPtr sender)
        {
            if (Setting == null) return;
            string key, value;
            lock (instance)
            {
                key = Functions.vp_string(instance.pointer, VPAttribute.WorldSettingKey);
                value = Functions.vp_string(instance.pointer, VPAttribute.WorldSettingValue);
            }

            Setting(instance, key, value);
        }

        internal void OnSettingsChanged(IntPtr sender)
        {
            if (SettingsChanged == null) return;
            SettingsChanged(instance);
        }

        internal void OnAvatarAdd(IntPtr sender)
        {
            if (AvatarAdd == null) return;
            Avatar data;
            lock (instance)
                data = new Avatar
                {
                    Name = Functions.vp_string(instance.pointer, VPAttribute.AvatarName),
                    Session = Functions.vp_int(instance.pointer, VPAttribute.AvatarSession),
                    AvatarType = Functions.vp_int(instance.pointer, VPAttribute.AvatarType),
                    X = Functions.vp_float(instance.pointer, VPAttribute.AvatarX),
                    Y = Functions.vp_float(instance.pointer, VPAttribute.AvatarY),
                    Z = Functions.vp_float(instance.pointer, VPAttribute.AvatarZ),
                    Yaw = Functions.vp_float(instance.pointer, VPAttribute.AvatarYaw),
                    Pitch = Functions.vp_float(instance.pointer, VPAttribute.AvatarPitch)
                };

            AvatarAdd(instance, data);
        }

        internal void OnAvatarChange(IntPtr sender)
        {
            if (AvatarChange == null) return;
            Avatar data;
            lock (instance)
                data = new Avatar
                {
                    Name = Functions.vp_string(instance.pointer, VPAttribute.AvatarName),
                    Session = Functions.vp_int(instance.pointer, VPAttribute.AvatarSession),
                    AvatarType = Functions.vp_int(instance.pointer, VPAttribute.AvatarType),
                    X = Functions.vp_float(instance.pointer, VPAttribute.AvatarX),
                    Y = Functions.vp_float(instance.pointer, VPAttribute.AvatarY),
                    Z = Functions.vp_float(instance.pointer, VPAttribute.AvatarZ),
                    Yaw = Functions.vp_float(instance.pointer, VPAttribute.AvatarYaw),
                    Pitch = Functions.vp_float(instance.pointer, VPAttribute.AvatarPitch)
                };

            AvatarChange(instance, data);
        }

        internal void OnAvatarDelete(IntPtr sender)
        {
            if (AvatarDelete == null) return;
            Avatar data;
            lock (instance)
                data = new Avatar
                {
                    Name = Functions.vp_string(instance.pointer, VPAttribute.AvatarName),
                    Session = Functions.vp_int(instance.pointer, VPAttribute.AvatarSession)
                };

            AvatarDelete(instance, data);
        }
        #endregion
    }
}
