using System;
using Nexus.Graphics.Colors;
using VP.Native;

namespace VP
{
    public partial class Instance : IDisposable
    {
        #region SDK
        bool isPumping;

        /// <summary>
        /// Pumps incoming events from and outgoing calls to the server, for the maximum
        /// amount of given milliseconds. This is nessecary in order to fire most events
        /// and for most function calls to go through.
        /// </summary>
        /// <remarks>Equivalent of C SDK's vp_wait()</remarks>
        public void Pump(int milliseconds = 0)
        {
            lock (mutex)
            {
                if (isPumping)
                    throw new InvalidOperationException("Cannot pump whilst handling a pumped event");
                else
                    isPumping = true;

                try     { Functions.Call( () => Functions.vp_wait(pointer, milliseconds) ); }
                finally { isPumping = false; }
            }
        } 
        #endregion

        #region Login
        /// <summary>
        /// Logs into a specified universe with the given authentication details and bot
        /// name. Chainable and thread-safe.
        /// </summary>
        /// <remarks>Servers always add square brackets around a bot's name</remarks>
        public Instance Login(Uniserver universe, string username, string password, string botname)
        {
            lock (mutex)
            {
               Functions.Call( () => Functions.vp_connect_universe(pointer, universe.Host, universe.Port) );
               Functions.Call( () => Functions.vp_login(pointer, username, password, botname)             );

               name = botname;
               return this;
            }
        }

        /// <summary>
        /// Logs into the default Virtual Paradise universe with the given authentication
        /// details and bot name. Chainable and thread-safe.
        /// </summary>
        /// <remarks>Servers always add square brackets around a bot's name</remarks>
        public Instance Login(string username, string password, string botname)
        {
            return Login(Uniserver.VirtualParadise, username, password, botname);
        }
        #endregion

        #region World
        /// <summary>
        /// Enters a given world, Chainable and thread-safe.
        /// </summary>
        public Instance Enter(string worldname)
        {
            lock (mutex)
            {
                Functions.Call( () => Functions.vp_enter(pointer, worldname) );

                world = worldname;
                return this;
            }
        }

        /// <summary>
        /// Updates the bot's own position and rotation. If called for the first time
        /// after entering the world, this makes this instance's presence known to other
        /// avatars. Chainable and thread-safe.
        /// </summary>
        /// <remarks>
        /// This needs to be called at least once in order to receive certain events in
        /// the world
        /// </remarks>
        public Instance GoTo(float x = 0.0f, float y = 0.0f, float z = 0.0f,
            float yaw = 0.0f, float pitch = 0.0f)
        {
            lock (mutex)
            {
                Functions.vp_float_set(pointer, FloatAttributes.MyX, x);
                Functions.vp_float_set(pointer, FloatAttributes.MyY, y);
                Functions.vp_float_set(pointer, FloatAttributes.MyZ, z);
                Functions.vp_float_set(pointer, FloatAttributes.MyYaw, yaw);
                Functions.vp_float_set(pointer, FloatAttributes.MyPitch, pitch);
                Functions.Call( () => Functions.vp_state_change(pointer) );

                return this;
            }
        }

        /// <summary>
        /// Updates the bot's own position and rotation using a given
        /// <see cref="AvatarPosition"/>. Chainable and thread-safe.
        /// </summary>
        public Instance GoTo(AvatarPosition position)
        {
            return GoTo(position.X, position.Y, position.Z, position.Yaw, position.Pitch);
        }

        /// <summary>
        /// Leaves the current world. Chainable and thread-safe.
        /// </summary>
        public Instance Leave()
        {
            lock (mutex)
            {
                Functions.Call( () => Functions.vp_leave(pointer) );

                world = "";
                return this;
            }
        } 
        #endregion

        #region Communication
        /// <summary>
        /// Sends a chat message to the current world. Chainable and thread-safe.
        /// </summary>
        public Instance Say(string message)
        {
            lock (mutex)
                Functions.Call( () => Functions.vp_say(pointer, message) );

            return this;
        }

        /// <summary>
        /// Sends a formatted chat message to current world. Chainable and thread-safe.
        /// </summary>
        /// <seealso cref="string.Format"/>
        public Instance Say(string message, params object[] parts)
        {
            Say( string.Format(message, parts) );

            return this;
        }  

        /// <summary>
        /// Sends a broadcast-like message with custom styling to a specific session
        /// </summary>
        public void ConsoleMessage(int session, ChatEffect effects, ColorRgb color, string name, string message)
        {
            int rc;
            lock (mutex)
                rc = Functions.vp_console_message(pointer, session, name, message, (int) effects, color.R, color.G, color.B);

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
