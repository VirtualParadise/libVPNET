using System;
using VP.Native;

namespace VP
{
    /// <summary>
    /// Container class for Instance's avatar-related members
    /// </summary>
    public class InstanceAvatars : IDisposable
    {
        internal Instance instance;

        public InstanceAvatars(Instance instance)
        {
            this.instance = instance;
            instance.SetNativeEvent(Events.AvatarAdd, OnAvatarAdd);
            instance.SetNativeEvent(Events.AvatarChange, OnAvatarChange);
            instance.SetNativeEvent(Events.AvatarDelete, OnAvatarDelete);
            instance.SetNativeEvent(Events.AvatarClick, OnAvatarClicked);
        }

        public void Dispose()
        {
            Enter   = null;
            Change  = null;
            Leave   = null;
            Clicked = null;
        }

        #region Events
        public delegate void AvatarArgs(Instance sender, Avatar avatar);
        public delegate void AvatarClickedArgs(Instance sender, AvatarClick click);

        public event AvatarArgs        Enter;
        public event AvatarArgs        Change;
        public event AvatarArgs        Leave;
        public event AvatarClickedArgs Clicked;
        #endregion

        #region Methods
        /// <summary>
        /// Sends a click event to an avatar by session number
        /// </summary>
        public void Click(int session)
        {
            int rc;
            lock (instance)
                rc = Functions.vp_avatar_click(instance.pointer, session);

            if (rc != 0) throw new VPException((ReasonCode)rc);
        }

        /// <summary>
        /// Teleports a target session to a specified world and position
        /// </summary>
        public void Teleport(int session, string world, Vector3 pos, float yaw, float pitch)
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

        /// <summary>
        /// Teleports a target session to a specified world and AvatarPosition
        /// </summary>
        public void Teleport(int session, string world, AvatarPosition pos)
        {
            int rc;
            lock (instance)
                rc = Functions.vp_teleport_avatar(
                    instance.pointer,
                    session,
                    world,
                    pos.X, pos.Y, pos.Z,
                    pos.Yaw, pos.Pitch);

            if (rc != 0) throw new VPException((ReasonCode)rc);
        }

        /// <summary>
        /// Teleports a target session to a specified position in the same world
        /// </summary>
        public void Teleport(int session, Vector3 pos, float yaw, float pitch)
        { Teleport(session, "", pos, yaw, pitch); }

        /// <summary>
        /// Teleports a target session to a specified AvatarPosition in the same world
        /// </summary>
        public void Teleport(int session, AvatarPosition pos)
        { Teleport(session, "", pos); }
        #endregion

        #region Event handlers
        internal void OnAvatarAdd(IntPtr sender)
        {
            if (Enter == null) return;
            Avatar data;
            lock (instance)
                data = new Avatar(instance.pointer);

            Enter(instance, data);
        }

        internal void OnAvatarChange(IntPtr sender)
        {
            if (Change == null) return;
            Avatar data;
            lock (instance)
                data = new Avatar(instance.pointer);

            Change(instance, data);
        }

        internal void OnAvatarDelete(IntPtr sender)
        {
            if (Leave == null) return;
            Avatar data;
            lock (instance)
                data = new Avatar(instance.pointer);

            Leave(instance, data);
        }

        internal void OnAvatarClicked(IntPtr sender)
        {
            if (Clicked == null) return;
            AvatarClick click;
            lock (instance)
                click = new AvatarClick(sender);
                
            Clicked(instance, click);
        }
        #endregion
    }
}
