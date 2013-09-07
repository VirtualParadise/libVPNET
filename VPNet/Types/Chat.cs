using System;
using VP.Native;

namespace VP
{
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
        public string Name, Message;
        public int    Session;

        internal ChatMessage(IntPtr pointer)
        {
            Name    = Functions.vp_string(pointer, StringAttributes.AvatarName);
            Message = Functions.vp_string(pointer, StringAttributes.ChatMessage);
            Session = Functions.vp_int(pointer, IntAttributes.AvatarSession);
        }
    }

    /// <summary>
    /// Represents a console message
    /// </summary>
    public struct ConsoleMessage
    {
        public string     Name, Message;
        public int        Session;
        public ChatEffect Effect;
        public Color      Color;

        internal ConsoleMessage(IntPtr pointer)
        {
            Name    = Functions.vp_string(pointer, StringAttributes.AvatarName);
            Message = Functions.vp_string(pointer, StringAttributes.ChatMessage);
            Session = Functions.vp_int(pointer, IntAttributes.AvatarSession);
            Effect  = (ChatEffect) Functions.vp_int(pointer, IntAttributes.ChatEffects);
            Color   = new Color(pointer);
        }
    }
}
