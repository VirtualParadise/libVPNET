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
    public struct TerrainCell
    {
        public float Height;
        public ushort Attributes;

        public bool Hole
        {
            get { return (Attributes & 0x8000) >> 15 == 1; }
            set { Attributes = (ushort) (Attributes | ((value ? 1 : 0) << 15)); }
        }

        public TerrainRotation Rotation
        {
            get { return (TerrainRotation) ((Attributes & 0x6000) >> 13); }
            set { Attributes = (ushort) (Attributes | ((int)value << 13)); }
        }

        public ushort Texture
        {
            get { return (ushort) (Attributes & 0x0FFF); }
            set { Attributes = (ushort) (Attributes | (value & 0x1FFF)); }
        }
    }

    public class TerrainNode
    {
        public TerrainTile Parent;
        public TerrainCell[,] Cells = new TerrainCell[8,8];
        public int X;
        public int Z;

        /// <summary>
        /// Gets or sets a TerrainCell value based on one-dimensional index, in row-major
        /// order (e.g. TerrainNode[5] = col 5, row 0)
        /// </summary>
        public TerrainCell this[int i]
        {
            get
            {
                int col = i % 8;
                int row = (i - col) / 8;
                return Cells[col, row];
            }

            set
            {
                int col = i % 8;
                int row = (i - col) / 8;
                Cells[col, row] = value;
            }
        }

        /// <summary>
        /// Gets or sets a TerrainCell value based on two-dimensional index
        /// </summary>
        public TerrainCell this[int x, int z]
        {
            get { return Cells[x, z]; }
            set { Cells[x, z] = value; }
        }
    }

    public class TerrainTile
    {
        public TerrainNode[,] Nodes = new TerrainNode[4,4];
        public int X;
        public int Z;
    }
}
