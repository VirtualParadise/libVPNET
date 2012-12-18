using System.Runtime.InteropServices;
using System;

namespace VP
{
    public enum TerrainRotation
    {
        North,
        West,
        South,
        East
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct NativeTerrainNode
    {
        public float Height;
        public ushort Attributes;
    }

    public class TerrainNode
    {
        internal NativeTerrainNode Native;
        public bool Hole;
        public TerrainRotation Rotation;
        public ushort Texture;

        public float Height;
        public int TileX;
        public int TileZ;
        public int X;
        public int Z;

        public void Pack()
        {
            Native = new NativeTerrainNode
            {
                Attributes = (ushort)(
                    (Texture & 0x1FFF)
                    | ((Hole ? 1 : 0) << 15)
                    | ((int)Rotation << 13)),
                Height = Height
            };
        }
    }
}
