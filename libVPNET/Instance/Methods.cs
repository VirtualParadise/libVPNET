using Nexus.Graphics.Colors;
using System;
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
        /// and for most function calls to go through. Chainable and thread-safe.
        /// </summary>
        /// <remarks>
        /// This function cannot be called from an event handler that has been fired by a
        /// prior call to Pump.
        /// </remarks>
        public Instance Pump(int milliseconds = 25)
        {
            lock (mutex)
            {
                if (isPumping)
                    throw new InvalidOperationException("Cannot pump whilst handling a pumped event");
                else
                    isPumping = true;

                try     { Functions.Call( () => Functions.vp_wait(pointer, milliseconds) ); }
                finally { isPumping = false; }
                return this;
            }
        } 
        #endregion

        #region Login
        /// <summary>
        /// Logs into a specified universe with the given authentication details and bot
        /// name. Chainable and thread-safe.
        /// </summary>
        /// <remarks>
        /// This function makes an internal call to <see cref="Pump"/>. Due to the same
        /// limitations, this function cannot be called from within an event handler that
        /// has been fired by a prior call to Pump.
        /// 
        /// Servers always add square brackets around a bot's name.
        /// </remarks>
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
        /// <remarks>
        /// This function makes an internal call to <see cref="Pump"/>. Due to the same
        /// limitations, this function cannot be called from within an event handler that
        /// has been fired by a prior call to Pump.
        /// 
        /// Servers always add square brackets around a bot's name.
        /// </remarks>
        public Instance Login(string username, string password, string botname)
        {
            return Login(Uniserver.VirtualParadise, username, password, botname);
        }
        #endregion

        #region World
        /// <summary>
        /// Enters a given world. Chainable and thread-safe.
        /// </summary>
        /// <remarks>
        /// This function makes an internal call to <see cref="Pump"/>. Due to the same
        /// limitations, this function cannot be called from within an event handler that
        /// has been fired by a prior call to Pump.
        /// </remarks>
        /// <param name="worldname">Target world to enter</param>
        /// <param name="setState">
        /// If true (default), the bot will automatically call
        /// <see cref="GoTo(float,float,float,float,float)"/> to set state upon entry.
        /// Set to false for shadow operations that do not require a presence.
        /// </param>
        public Instance Enter(string worldname, bool setState = true)
        {
            lock (mutex)
            {
                Functions.Call( () => Functions.vp_enter(pointer, worldname) );
                world = worldname;

                if (setState)
                    GoTo();

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
        /// Sends a formatted chat message to current world. Chainable, thread-safe and
        /// splits messages over 255 bytes in length automatically.
        /// </summary>
        /// <seealso cref="string.Format(string, Object)"/>
        public Instance Say(string message, params object[] parts)
        {
            var formatted = string.Format(message, parts);
            var chunks    = Unicode.ChunkByByteLimit(formatted);

            lock (mutex)
                foreach (var chunk in chunks)
                    Functions.Call( () => Functions.vp_say(pointer, chunk) );

            return this;
        }  

        /// <summary>
        /// Sends a formattable and broadcast-like message with custom styling to a
        /// specific session. Chainable, thread-safe and splits messages over 255 bytes
        /// in length automatically.
        /// </summary>
        /// <param name="session">
        /// Target session, or use 0 to broadcast to everybody. Alternatively, use 
        /// <see cref="ConsoleBroadcast(ChatEffect, Color, string, string)"/>
        /// </param>
        /// <param name="name">
        /// Name to use for message, or blank string for a standalone message
        /// </param>
        /// <param name="effects">Effects to use on this message</param>
        /// <param name="color">Color to use on this message</param>
        /// <param name="message">Message to send</param>
        public Instance ConsoleMessage(int session, ChatEffect effects, ColorRgb color, string name, string message, params object[] parts)
        {
            var formatted = string.Format(message, parts);
            var chunks    = Unicode.ChunkByByteLimit(formatted);

            lock (mutex)
                foreach (var chunk in chunks)
                    Functions.Call( () => Functions.vp_console_message(pointer, session, name, chunk, (int) effects, color.R, color.G, color.B) );

            return this;
        }

        /// <summary>
        /// Sends a formattable and broadcast-like message with custom styling to
        /// everybody in-world. Chainable, thread-safe and splits messages over 255 bytes
        /// in length automatically.
        /// </summary>
        /// <param name="name">
        /// Name to use for message, or blank string for a standalone message
        /// </param>
        /// <param name="effects">Effects to use on this message</param>
        /// <param name="color">Color to use on this message</param>
        /// <param name="message">Message to send</param>
        public Instance ConsoleBroadcast(ChatEffect effects, ColorRgb color, string name, string message, params object[] parts)
        {
            return ConsoleMessage( 0, effects, color, name, string.Format(message, parts) );
        }

        /// <summary>
        /// Sends a formattable and broadcast-like message with default styling to a
        /// specific session. Chainable, thread-safe and splits messages over 255 bytes
        /// in length automatically.
        /// </summary>
        /// <param name="session">
        /// Target session, or use 0 to broadcast to everybody. Alternatively, use 
        /// <see cref="ConsoleBroadcast(string, string)"/>
        /// </param>
        /// <param name="name">
        /// Name to use for message, or blank string for a standalone message
        /// </param>
        /// <param name="message">Message to send</param>
        public Instance ConsoleMessage(int session, string name, string message, params object[] parts)
        { 
            return ConsoleMessage(session, ChatEffect.None, new ColorRgb(0,0,0), name, message, parts);
        }

        /// <summary>
        /// Sends a formattable broadcast-like message with default styling to everybody
        /// in-world. Chainable, thread-safe and splits messages over 255 bytes in length
        /// automatically.
        /// </summary>
        /// <param name="name">
        /// Name to use for message, or blank string for a standalone message
        /// </param>
        /// <param name="message">Message to send</param>
        public Instance ConsoleBroadcast(string name, string message, params object[] parts)
        {
            return ConsoleMessage(0, ChatEffect.None, new ColorRgb(0,0,0), name, message, parts);
        }
        #endregion
    }
}
