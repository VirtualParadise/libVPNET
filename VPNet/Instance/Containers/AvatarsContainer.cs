using Nexus;
using System;
using VP.Native;

namespace VP
{
    /// <summary>
    /// Container for SDK methods, events and properties related to avatars, such as
    /// state changes, clicks and teleport requests
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
        /// <summary>
        /// Encapsulates a method that accepts a source <see cref="Instance"/> and an
        /// <see cref="Avatar"/> state for the <see cref="Enter"/> and
        /// <see cref="Change"/> events
        /// </summary>
        public delegate void StateArgs(Instance sender, Avatar avatar);
        /// <summary>
        /// Encapsulates a method that accepts a source <see cref="Instance"/> and a
        /// unique session ID for the <see cref="Leave"/> event
        /// </summary>
        public delegate void LeaveArgs(Instance sender, int session);
        /// <summary>
        /// Encapsulates a method that accepts a source <see cref="Instance"/> and an
        /// <see cref="AvatarClick"/> for the <see cref="Clicked"/> event
        /// </summary>
        public delegate void ClickedArgs(Instance sender, AvatarClick click);
        /// <summary>
        /// Encapsulates a method that accepts a source <see cref="Instance"/>, a source
        /// unique session ID, an <see cref="AvatarPosition"/> and optional world string
        /// for the <see cref="Teleported"/> event
        /// </summary>
        public delegate void TeleportedArgs(Instance sender, int session, AvatarPosition position, string world);

        /// <summary>
        /// Fired when an avatar enters the world, providing its initial state
        /// </summary>
        /// <remarks>
        /// Technically, this is when the avatar calls vp_state_change() (or on this SDK,
        /// <see cref="Instance.GoTo(float, float, float, float, float)"/>) for the first
        /// time. It is possible for bots to enter a world without having this event
        /// fired for them.
        /// </remarks>
        public event StateArgs Enter;
        /// <summary>
        /// Fired when an avatar's state (e.g. position) is changed, providing all of the
        /// avatar's latest state
        /// </summary>
        public event StateArgs Change;
        /// <summary>
        /// Fired when an avatar exits the world, providing only its session ID
        /// </summary>
        public event LeaveArgs Leave;
        /// <summary>
        /// Fired when this instance is clicked by another avatar in-world, providing
        /// click coordinates and source ID
        /// </summary>
        public event ClickedArgs Clicked;
        /// <summary>
        /// Fired when an avatar sends this instance a request to teleport to the
        /// given position and, optionally, world. Also provides the source session ID.
        /// </summary>
        public event TeleportedArgs Teleported;
        #endregion

        #region Event handlers
        internal void OnAvatarAdd(IntPtr sender)
        {
            if (Enter != null)
                Enter( instance, new Avatar(sender) );
        }

        internal void OnAvatarChange(IntPtr sender)
        {
            if (Change != null)
                Change( instance, new Avatar(sender) );
        }

        internal void OnAvatarDelete(IntPtr sender)
        {
            if (Leave != null)
            {
                int session = Functions.vp_int(instance.pointer, IntAttributes.AvatarSession);
                Leave(instance, session);
            }
        }

        internal void OnAvatarClicked(IntPtr sender)
        {
            if (Clicked != null)
                Clicked( instance, new AvatarClick(sender) );
        }

        internal void OnAvatarTeleported(IntPtr sender)
        {
            if (Teleported == null)
                return;

            var pos     = AvatarPosition.FromTeleport(sender);
            var session = Functions.vp_int(sender, IntAttributes.AvatarSession);
            var world   = Functions.vp_string(sender, StringAttributes.TeleportWorld);
  
            Teleported(instance, session, pos, world);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Sends a click event to an avatar by session number on the specified
        /// coordinates using a <see cref="Vector3D"/>
        /// </summary>
        public void Click(int session, Vector3D coordinates)
        {
            lock (instance.mutex)
            {
                coordinates.ToClick(instance.pointer);
                Functions.Call( () => Functions.vp_avatar_click(instance.pointer, session) );
            }
        }

        /// <summary>
        /// Sends a click event to an avatar by session number
        /// </summary>
        public void Click(int session)
        {
            Click(session, Vector3D.Zero);
        }

        /// <summary>
        /// Sends a request for a target session to teleport to a specified world and
        /// position
        /// </summary>
        public void Teleport(int session, string world, Vector3D pos, float yaw, float pitch)
        {
            lock (instance.mutex)
                Functions.Call( () =>
                    Functions.vp_teleport_avatar(
                    instance.pointer,
                    session,
                    world,
                    pos.X, pos.Y, pos.Z,
                    yaw, pitch)
                );
        }

        /// <summary>
        /// Sends a request for a target session to teleport to a specified world and
        /// <see cref="AvatarPosition"/>
        /// </summary>
        public void Teleport(int session, string world, AvatarPosition pos)
        {
            lock (instance.mutex)
                Functions.Call( () =>
                    Functions.vp_teleport_avatar(
                    instance.pointer,
                    session,
                    world,
                    pos.X, pos.Y, pos.Z,
                    pos.Yaw, pos.Pitch)
                );
        }

        /// <summary>
        /// Sends a request for a target session to teleport to a specified position in
        /// the same world
        /// </summary>
        public void Teleport(int session, Vector3D pos, float yaw, float pitch)
        {
            Teleport(session, "", pos, yaw, pitch);
        }

        /// <summary>
        /// Sends a request for a target session to teleport to a specified
        /// <see cref="AvatarPosition"/> in the same world
        /// </summary>
        public void Teleport(int session, AvatarPosition pos)
        {
            Teleport(session, "", pos);
        }
        #endregion
    }
}
