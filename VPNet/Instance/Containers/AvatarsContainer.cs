using Nexus;
using System;
using VP.Native;

namespace VP
{
    /// <summary>
    /// Container class for Instance's avatar-related members
    /// </summary>
    public class AvatarsContainer
    {
        #region Construction & disposal
        Instance instance;

        internal AvatarsContainer(Instance instance)
        {
            this.instance = instance;
            instance.setNativeEvent(Events.AvatarAdd, OnAvatarAdd);
            instance.setNativeEvent(Events.AvatarChange, OnAvatarChange);
            instance.setNativeEvent(Events.AvatarDelete, OnAvatarDelete);
            instance.setNativeEvent(Events.AvatarClick, OnAvatarClicked);
            instance.setNativeEvent(Events.Teleport, OnAvatarTeleported);
        }

        internal void Dispose()
        {
            Enter      = null;
            Change     = null;
            Leave      = null;
            Clicked    = null;
            Teleported = null;
        }
        #endregion

        #region Events
        public delegate void StateArgs(Instance sender, Avatar avatar);
        public delegate void LeaveArgs(Instance sender, int session);
        public delegate void ClickedArgs(Instance sender, AvatarClick click);
        public delegate void TeleportedArgs(Instance sender, int session, AvatarPosition position, string world);

        /// <summary>
        /// Fired when an avatar enters the world, providing its initial state
        /// </summary>
        /// <remarks>
        /// Technically, this is when the avatar calls vp_state_change() (or on this SDK,
        /// <seealso cref="Instance.GoTo"/>) for the first time. It is possible for bots
        /// to enter a world without having this event fired for them.
        /// </remarks>
        public event StateArgs      Enter;
        /// <summary>
        /// Fired when an avatar's state (e.g. position) is changed, providing all of the
        /// avatar's latest state
        /// </summary>
        public event StateArgs      Change;
        /// <summary>
        /// Fired when an avatar exits the world, providing only its session ID
        /// </summary>
        public event LeaveArgs      Leave;
        public event ClickedArgs    Clicked;
        public event TeleportedArgs Teleported;
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
        public void Teleport(int session, string world, Vector3D pos, float yaw, float pitch)
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
        public void Teleport(int session, Vector3D pos, float yaw, float pitch)
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
            int session;
            lock (instance)
                session = Functions.vp_int(instance.pointer, IntAttributes.AvatarSession);

            Leave(instance, session);
        }

        internal void OnAvatarClicked(IntPtr sender)
        {
            if (Clicked == null) return;
            AvatarClick click;
            lock (instance)
                click = new AvatarClick(sender);
                
            Clicked(instance, click);
        }

        internal void OnAvatarTeleported(IntPtr sender)
        {
            if (Teleported == null)
                return;

            AvatarPosition pos;
            int            session;
            string         world;

            lock (instance)
            {
                pos     = AvatarPosition.FromTeleport(sender);
                session = Functions.vp_int(sender, IntAttributes.AvatarSession);
                world   = Functions.vp_string(sender, StringAttributes.TeleportWorld);
            }
                
            Teleported(instance, session, pos, world);
        }
        #endregion
    }
}
