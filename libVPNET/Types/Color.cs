using System;
using VP.Native;

namespace VP
{
    /// <summary>
    /// Represents an immutable tuple of three bytes to represent a color in RGB format
    /// </summary>
    public struct Color
    {
        public byte R;
        public byte G;
        public byte B;

        public Color(byte red, byte green, byte blue)
        {
            this.R = red;
            this.G = green;
            this.B = blue;
        }

        public Color(int red, int green, int blue)
        {
            this.R = (byte) red;
            this.G = (byte) green;
            this.B = (byte) blue;
        }

        internal static Color FromChat(IntPtr pointer)
        {
            return new Color
            {
                R = (byte) Functions.vp_int(pointer, IntAttributes.ChatColorRed),
                G = (byte) Functions.vp_int(pointer, IntAttributes.ChatColorGreen),
                B = (byte) Functions.vp_int(pointer, IntAttributes.ChatColorBlue),
            };
        }
    }
}
