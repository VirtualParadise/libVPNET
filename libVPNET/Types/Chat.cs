using System;
using VP.Native;

namespace VP
{
    internal enum ChatType : int
    {
        Normal,
        ConsoleMessage,
        Private
    }

    /// <summary>
    /// Bitfield of effects applied to a console message
    /// </summary>
    [Flags]
    public enum ChatEffect : int
    {
        /// <summary>
        /// No formatting applied
        /// </summary>
        None   = 0,
        /// <summary>
        /// Bold formatting
        /// </summary>
        Bold   = 1,
        /// <summary>
        /// Italics (emphasis) formatting
        /// </summary>
        Italic = 2,

        /// <summary>
        /// Both bold and italics formatting
        /// </summary>
        BoldItalic = Bold | Italic
    }

    /// <summary>
    /// Represents a chat message
    /// </summary>
    public struct ChatMessage
    {
        /// <summary>
        /// Gets the unique session ID of the source of this chat message
        /// </summary>
        public int Session;
        /// <summary>
        /// Gets the name of the source of this chat message
        /// </summary>
        public string Name;
        /// <summary>
        /// Gets the actual message itself
        /// </summary>
        public string Message;
        
        internal ChatMessage(IntPtr pointer)
        {
            Session = Functions.vp_int(pointer, IntAttributes.AvatarSession);
            Name    = Functions.vp_string(pointer, StringAttributes.AvatarName);
            Message = Functions.vp_string(pointer, StringAttributes.ChatMessage);
        }
    }

    /// <summary>
    /// Represents a console message
    /// </summary>
    public struct ConsoleMessage
    {
        /// <summary>
        /// Gets the unique session ID of the source of this console message
        /// </summary>
        public int Session;
        /// <summary>
        /// Gets the name attached to this console message
        /// </summary>
        /// <remarks>It is possible for this to be a blank string</remarks>
        public string Name;
        /// <summary>
        /// Gets the actual message itself
        /// </summary>
        public string Message;
        /// <summary>
        /// Gets the effects applied to this console message
        /// </summary>
        public ChatEffect Effect;
        /// <summary>
        /// Gets the color of this console message
        /// </summary>
        public Color Color;

        internal ConsoleMessage(IntPtr pointer)
        {
            Session = Functions.vp_int(pointer, IntAttributes.AvatarSession);
            Name    = Functions.vp_string(pointer, StringAttributes.AvatarName);
            Message = Functions.vp_string(pointer, StringAttributes.ChatMessage);
            Effect  = (ChatEffect) Functions.vp_int(pointer, IntAttributes.ChatEffects);
            Color   = Color.FromChat(pointer);
        }
    }
}
