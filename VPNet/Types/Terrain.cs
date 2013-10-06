using System;
using System.Runtime.InteropServices;
using VP.Native;

namespace VP
{
    /// <summary>
    /// Specifies constants that represent the rotation of a terrain cell's texture
    /// </summary>
    public enum TerrainRotation
    {
        /// <summary>
        /// Texture rotated north-wise
        /// </summary>
        North,
        /// <summary>
        /// Texture rotated west-wise
        /// </summary>
        West,
        /// <summary>
        /// Texture rotated south-wise
        /// </summary>
        South,
        /// <summary>
        /// Texture rotated east-wise
        /// </summary>
        East
    }

    /// <summary>
    /// Represents an immutable definition of a terrain cell, typically belonging to a
    /// <see cref="TerrainNode"/>
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct TerrainCell
    {
        /// <summary>
        /// Gets or sets the height of this cell
        /// </summary>
        public float Height;
        /// <summary>
        /// Gets or sets the raw bitfield attributes of this cell
        /// </summary>
        public ushort Attributes;

        /// <summary>
        /// Gets or sets whether this cell is a hole (no drawn or physical geometry)
        /// </summary>
        public bool Hole
        {
            get { return (Attributes & 0x8000) >> 15 == 1; }
            set { Attributes = (ushort) (Attributes | ((value ? 1 : 0) << 15)); }
        }

        /// <summary>
        /// Gets or sets the rotation of this cell
        /// </summary>
        public TerrainRotation Rotation
        {
            get { return (TerrainRotation) ((Attributes & 0x6000) >> 13); }
            set { Attributes = (ushort) (Attributes | ((int)value << 13)); }
        }

        /// <summary>Gets or sets the texture used by this cell</summary>
        /// <remarks>
        /// The VP client uses this value by looking for a texture with the name
        /// "terrain#.jpg", where '#' is this value.
        /// </remarks>
        public ushort Texture
        {
            get { return (ushort) (Attributes & 0x0FFF); }
            set { Attributes = (ushort) (Attributes | (value & 0x1FFF)); }
        }
    }

    /// <summary>
    /// Represents a node of terrain, which can hold an 8 by 8 grid of cells
    /// </summary>
    public class TerrainNode
    {
        /// <summary>
        /// Gets the terrain cell grid of this node
        /// </summary>
        public readonly TerrainCell[,] Cells = new TerrainCell[8, 8];
        /// <summary>
        /// Gets or sets the X coordinate of this node in relation to its parent tile
        /// </summary>
        public int X;
        /// <summary>
        /// Gets or sets the Z coordinate of this node in relation to its parent tile
        /// </summary>
        public int Z;
        /// <summary>
        /// Gets the revision count of this node if it came from a query
        /// </summary>
        public readonly int Revision = 0;

        /// <summary>
        /// Creates a terrain node
        /// </summary>
        public TerrainNode() { }

        /// <summary>
        /// Creates a terrain node from an instances' attributes and byte array
        /// </summary>
        public TerrainNode(IntPtr pointer)
        {
            X        = Functions.vp_int(pointer, IntAttributes.TerrainNodeX);
            Z        = Functions.vp_int(pointer, IntAttributes.TerrainNodeZ);
            Revision = Functions.vp_int(pointer, IntAttributes.TerrainNodeRevision);

            var data = DataHandlers.GetData(pointer, DataAttributes.TerrainNodeData);
            Cells    = DataHandlers.NodeDataTo2DArray(data);
        }

        /// <summary>
        /// Gets or sets a TerrainCell value based on one-dimensional index, in X-major
        /// order (e.g. TerrainNode[5] = col 5, row 0 or X5 Z0)
        /// </summary>
        public TerrainCell this[int i]
        {
            get
            {
                int x = i % 8;
                int z = (i - x) / 8;
                return this[x, z];
            }

            set
            {
                int x = i % 8;
                int z = (i - x) / 8;
                this[x, z] = value;
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
}
