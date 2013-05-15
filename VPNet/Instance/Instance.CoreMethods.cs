using System;
using System.Collections.Generic;
using VP.Native;

namespace VP
{
    public partial class Instance : IDisposable
    {
        #region SDK
        /// <summary>
        /// Pumps incoming events, fires any registered events or callbacks, then sleeps
        /// for the given amount of milliseconds.
        /// </summary>
        public void Wait(int milliseconds)
        {
            int rc = Functions.vp_wait(pointer, milliseconds);
            if (rc != 0) throw new VPException((ReasonCode)rc);
        } 
        #endregion

        #region Login
        /// <summary>
        /// Logs into a specified universe with the given authentication details and
        /// bot name. Chainable.
        /// </summary>
        public Instance Login(Uniserver universe, string username, string password, string botname)
        {
            int rc;
            lock (this)
                rc = Functions.vp_connect_universe(pointer, universe.Host, universe.Port);

            if (rc != 0) throw new VPException((ReasonCode)rc);

            lock (this)
                rc = Functions.vp_login(pointer, username, password, botname);

            if (rc != 0)
                throw new VPException((ReasonCode)rc);
            else
            {
                Name = botname;
                return this;
            }
        }

        /// <summary>
        /// Logs into the default Virtual Paradise universe with the given authentication
        /// details. Chainable.
        /// </summary>
        public Instance Login(string username, string password, string botname)
        { return Login(Uniserver.VirtualParadise, username, password, botname); }

        /// <summary>
        /// Logs into the default Virtual Paradise universe with the given authentication
        /// details and initialized bot name. Chainable.
        /// </summary>
        public Instance Login(string username, string password)
        { return Login(Uniserver.VirtualParadise, username, password, Name); }
        #endregion

        #region World
        /// <summary>
        /// Enters a given world, chainable
        /// </summary>
        public Instance Enter(string worldname)
        {
            int rc;
            lock (this)
                rc = Functions.vp_enter(pointer, worldname);

            if (rc != 0)
                throw new VPException((ReasonCode)rc);
            else
            {
                CurrentWorld = worldname;
                return this;
            }
        }

        /// <summary>
        /// Updates the bot's own position and rotation
        /// </summary>
        public void GoTo(float x = 0.0f, float y = 0.0f, float z = 0.0f,
            float yaw = 0.0f, float pitch = 0.0f)
        {
            int rc;
            lock (this)
            {
                Functions.vp_float_set(pointer, FloatAttributes.MyX, x);
                Functions.vp_float_set(pointer, FloatAttributes.MyY, y);
                Functions.vp_float_set(pointer, FloatAttributes.MyZ, z);
                Functions.vp_float_set(pointer, FloatAttributes.MyYaw, yaw);
                Functions.vp_float_set(pointer, FloatAttributes.MyPitch, pitch);
                rc = Functions.vp_state_change(pointer);
            }

            if (rc != 0) throw new VPException((ReasonCode)rc);
        }

        /// <summary>
        /// Updates the bot's own position and rotation using an AvatarPosition
        /// </summary>
        public void GoTo(AvatarPosition position)
        { GoTo(position.X, position.Y, position.Z, position.Yaw, position.Pitch); }

        /// <summary>
        /// Leaves the current world
        /// </summary>
        public void Leave()
        {
            int rc;
            lock (this)
                rc = Functions.vp_leave(pointer);

            if (rc != 0)
                throw new VPException((ReasonCode)rc);
            else
                CurrentWorld = null;
        } 
        #endregion

        #region Communication
        /// <summary>
        /// Sends a chat message to the current world
        /// </summary>
        public void Say(string message)
        {
            int rc;
            lock (this)
                rc = Functions.vp_say(pointer, message);

            if (rc != 0) throw new VPException( (ReasonCode) rc );
        }

        /// <summary>
        /// Sends a formatted chat message to current world
        /// </summary>
        public void Say(string message, params object[] parts)
        { Say(string.Format(message, parts)); }  

        /// <summary>
        /// Sends a broadcast-like message with custom styling to a specific session
        /// </summary>
        public void ConsoleMessage(int session, ChatEffect effects, Color color, string name, string message)
        {
            int rc;
            lock (this)
                rc = Functions.vp_console_message(pointer, session, name, message, (int) effects,
                    (byte) color.Red, (byte) color.Green, (byte) color.Blue);

            if (rc != 0) throw new VPException( (ReasonCode) rc );
        }

        /// <summary>
        /// Sends a formatted broadcast-like message with custom styling to a specific
        /// session
        /// </summary>
        public void ConsoleMessage(int session, ChatEffect effects, Color color, string name, string message, params object[] parts)
        { ConsoleMessage(session, effects, color, name, string.Format(message, parts)); }

        /// <summary>
        /// Sends a broadcast-like message with custom styling to everybody in-world
        /// </summary>
        public void ConsoleBroadcast(ChatEffect effects, Color color, string name, string message)
        { ConsoleMessage(0, effects, color, name, message); }

        /// <summary>
        /// Sends a formatted broadcast-like message with custom styling to everybody
        /// in-world
        /// </summary>
        public void ConsoleBroadcast(ChatEffect effects, Color color, string name, string message, params object[] parts)
        { ConsoleMessage(0, effects, color, name, string.Format(message, parts)); }

        /// <summary>
        /// Sends a broadcast-like message with default styling to a specific session
        /// </summary>
        public void ConsoleMessage(int session, string name, string message)
        { ConsoleMessage(session, ChatEffect.None, Color.Black, name, message); }

        /// <summary>
        /// Sends a formatted broadcast-like message with default styling to a specific
        /// session
        /// </summary>
        public void ConsoleMessage(int session, string name, string message, params object[] parts)
        { ConsoleMessage(session, ChatEffect.None, Color.Black, name, string.Format(message, parts)); }

        /// <summary>
        /// Sends a broadcast-like message with default styling to everybody in-world
        /// </summary>
        public void ConsoleBroadcast(string name, string message)
        { ConsoleMessage(0, ChatEffect.None, Color.Black, name, message); }

        /// <summary>
        /// Sends a formatted broadcast-like message with default styling to everybody
        /// in-world
        /// </summary>
        public void ConsoleBroadcast(string name, string message, params object[] parts)
        { ConsoleMessage(0, ChatEffect.None, Color.Black, name, string.Format(message, parts)); }
        #endregion
    }
}
