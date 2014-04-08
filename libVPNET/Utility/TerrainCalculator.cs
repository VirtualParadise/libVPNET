using System;

namespace VP
{
    /// <summary>
    /// Utility struct for calculating coordinates and relative values of terrain cells,
    /// nodes and tiles
    /// </summary>
    public struct TerrainCalculator
    {
        /// <summary>
        /// Global cell X coordinate
        /// </summary>
        public int GlobalCellX;
        /// <summary>
        /// Global cell Z coordinate
        /// </summary>
        public int GlobalCellZ;
        /// <summary>
        /// Cell X coordinate relative to node
        /// </summary>
        public int CellX;
        /// <summary>
        /// Cell Z coordinate relative to node
        /// </summary>
        public int CellZ;
        /// <summary>
        /// Node X relative to tile
        /// </summary>
        public int NodeX;
        /// <summary>
        /// Node Z relative to tile
        /// </summary>
        public int NodeZ;
        /// <summary>
        /// Global tile X relative to world
        /// </summary>
        public int TileX;
        /// <summary>
        /// Global tile Z relative to world
        /// </summary>
        public int TileZ;

        /// <summary>
        /// Creates a fully calculated terrain position from world coordinates
        /// </summary>
        public TerrainCalculator(float x, float z, float scaleFactor = 1.0f)
        {
            GlobalCellX = (int)Math.Floor(x * scaleFactor);
            GlobalCellZ = (int)Math.Floor(z * scaleFactor);
            TileX = CellToTile(GlobalCellX);
            TileZ = CellToTile(GlobalCellZ);
            CellX = CellToNode(GlobalCellX);
            CellZ = CellToNode(GlobalCellZ);
            NodeX = CellToLocal(GlobalCellX) / 8;
            NodeZ = CellToLocal(GlobalCellZ) / 8;
        }

        /// <summary>
        /// Gets tile number from a global cell
        /// </summary>
        public static int CellToTile(int global)
        {
            if (global < 0)
                return (global + 1) / 32 - 1;
            else
                return global / 32;
        }

        /// <summary>
        /// Gets a global cell's local coordinate reletive to tile
        /// </summary>
        public static int CellToLocal(int global)
        {
            if (global < 0)
                return 32 - Math.Abs(global % 32);
            else
                return global % 32;
        }

        /// <summary>
        /// Gets a global cell's local coordinate relative to node
        /// </summary>
        public static int CellToNode(int global)
        {
            if (global < 0)
                return 32 - Math.Abs(global % 8);
            else
                return global % 8;
        }
    }
}