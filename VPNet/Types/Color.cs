using System;
using VP.Native;

namespace VP
{
    public struct Color
    {
        public static Color Black = new Color(0,0,0);

        public byte Red;
        public byte Green;
        public byte Blue;

        public Color (byte red, byte green, byte blue)
        {
            this.Red   = red;
            this.Green = green;
            this.Blue  = blue;
        }

        internal Color (IntPtr pointer)
        {
            Red   = (byte) Functions.vp_int(pointer, IntAttributes.ChatColorRed);
            Green = (byte) Functions.vp_int(pointer, IntAttributes.ChatColorGreen);
            Blue  = (byte) Functions.vp_int(pointer, IntAttributes.ChatColorBlue);
        }
    }
}
