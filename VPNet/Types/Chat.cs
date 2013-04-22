using System;
using VP.Native;

namespace VP
{
    public enum ChatType
    {
        Normal,
        ConsoleMessage,
        Private
    }

    [Flags]
    public enum ChatTextEffect : int
    {
        None = 0,
        Bold = 1,
        Italic
    }

    public class Chat
    {
        public string         Name, Message;
        public int            Session;
        public ChatType       Type = ChatType.Normal;
        public ChatTextEffect Effect;
        public Color          Color;

        /// <summary>
        /// Creates a Chat from a native instance's attributes
        /// </summary>
        internal Chat (IntPtr pointer)
        {
            Name    = Functions.vp_string(pointer, StringAttributes.AvatarName);
            Message = Functions.vp_string(pointer, StringAttributes.ChatMessage);
            Session = Functions.vp_int(pointer, IntAttributes.AvatarSession);
            Color   = new Color(pointer);

            Type   = (ChatType)       Functions.vp_int(pointer, IntAttributes.ChatType);
            Effect = (ChatTextEffect) Functions.vp_int(pointer, IntAttributes.ChatType);
        }
    }
}
