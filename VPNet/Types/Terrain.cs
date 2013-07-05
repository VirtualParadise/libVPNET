﻿using System;
using System.Runtime.InteropServices;
using VP.Native;

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
        public float  Height;
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
        public TerrainCell[,] Cells = new TerrainCell[8, 8];
        public int X;
        public int Z;
        public int Revision;

        public TerrainNode() { }

        /// <summary>
        /// Creates a terrain node from an instances' attributes and byte array
        /// </summary>
        public TerrainNode(IntPtr pointer)
        {
            X        = Functions.vp_int(pointer, IntAttributes.TerrainNodeX);
            Z        = Functions.vp_int(pointer, IntAttributes.TerrainNodeZ);
            Revision = Functions.vp_int(pointer, IntAttributes.TerrainNodeRevision);
            var data = Functions.GetData(pointer, DataAttributes.TerrainNodeData);
            Cells    = DataConverters.NodeDataTo2DArray(data);
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

    public class TerrainTile
    {
        /// <summary>
        /// A 2D array of revision numbers to force the server to send even unmodified
        /// terrain nodes back
        /// </summary>
        public static int[,] BaseRevision = new int[4, 4]
        {
            { -1, -1, -1, -1 },
            { -1, -1, -1, -1 },
            { -1, -1, -1, -1 },
            { -1, -1, -1, -1 }
        };

        public TerrainNode[,] Nodes = new TerrainNode[4,4];
        public int X;
        public int Z;

        /// <summary>
        /// Gets or sets a TerrainNode object based on one-dimensional index, in column-major
        /// order (e.g. TerrainTile[4] = col 1, row 0)
        /// </summary>
        public TerrainNode this[int i]
        {
            get
            {
                int row = i % 4;
                int col = (i - row) / 4;
                return this[col, row];
            }

            set
            {
                int row = i % 4;
                int col = (i - row) / 4;
                this[col, row] = value;
            }
        }

        /// <summary>
        /// Gets or sets a TerrainNode object based on two-dimensional index.
        /// Automatically sets the node's X, Y and Parent value
        /// </summary>
        public TerrainNode this[int x, int z]
        {
            get { return Nodes[x, z]; }
            set {
                value.X = x;
                value.Z = z;
                value.Parent = this;
                Nodes[x, z] = value;
            }
        }
    }
}
