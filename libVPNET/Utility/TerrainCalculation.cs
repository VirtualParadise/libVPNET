using System;

namespace VP
{
    /// <summary>
    /// Utility struct for calculating coordinates and relative values of terrain cells,
    /// nodes and tiles
    /// </summary>
    /// <remarks>
    /// Acknowledgement to Edwin for the remarkable patience shown in assisting me with
    /// understanding and writing the code and mathematics below
    /// </remarks>
    public struct TerrainCalculation
    {
        /// <summary>
        /// Gets the world position XZ point this calculation was created for
        /// </summary>
        public Point2F Position;

        /// <summary>
        /// Gets the scale of terrain relative to world positions this calculation uses,
        /// where a scale factor of 1 is equal to 10 meters in-world (or: 1 world
        /// position unit)
        /// </summary>
        /// <example>
        /// If the scale factor is 0.5, it means that a terrain cell spans 5 meters
        /// in-world (or: 1/2 a world position unit).
        /// 
        /// If the scale factor is 2, it means that a terrain cell spans 20 meters
        /// in-world (or: 2 world position units).
        /// </example>
        public float ScaleFactor;

        /// <summary>
        /// Gets the calculated terrain cell XZ point relative to origin of terrain grid
        /// </summary>
        public Point2I OriginCell;

        /// <summary>
        /// Gets the calculated tile XZ relative to origin of terrain grid
        /// </summary>
        public Point2I Tile;

        /// <summary>
        /// Gets the calculated terrain cell XZ point relative to its parent tile (also
        /// known as the tile cell offset)
        /// </summary>
        public Point2I TileCell;

        /// <summary>
        /// Gets the calculated terrain node XZ point relative to its parent tile
        /// </summary>
        public Point2I Node;

        /// <summary>
        /// Gets the calculated terrain cell XZ point relative to its parent node (also
        /// known as the node cell offset)
        /// </summary>
        public Point2I NodeCell;        

        /// <summary>
        /// Creates a fully calculated terrain position from world coordinates
        /// </summary>
        public TerrainCalculation(float x, float z, float scaleFactor = 1.0f)
        {
            // Save calculation inputs
            this.Position    = new Point2F(x, z);
            this.ScaleFactor = scaleFactor;

            // Snap coordinates to global cell
            this.OriginCell = new Point2I()
            {
                X = (int) Math.Floor(x / scaleFactor),
                Z = (int) Math.Floor(z / scaleFactor)
            };

            // Calculate tile number
            this.Tile = new Point2I()
            {
                X = originCell2Tile(OriginCell.X),
                Z = originCell2Tile(OriginCell.Z)
            };

            // Calculate cell offset relative to tile origin
            this.TileCell = new Point2I()
            {
                X = originCell2TileOffset(OriginCell.X),
                Z = originCell2TileOffset(OriginCell.Z)
            };

            // Calculate node relative to tile origin
            this.Node = new Point2I()
            {
                X = tileCell2Node(TileCell.X),
                Z = tileCell2Node(TileCell.Z)
            };

            // Calculate node offset relative to node origin
            this.NodeCell = new Point2I()
            {
                X = TileCell.X % 8,
                Z = TileCell.Z % 8
            };
        }

        static int originCell2Tile(int cell)
        {
            return cell < 0
                ? (int) Math.Ceiling( (cell + 1) / 32d - 1 )
                : (int) Math.Floor(cell / 32d);
        }

        static int originCell2TileOffset(int cell)
        {
            int offset = cell % 32;

            return offset < 0
                ? offset + 32
                : offset;
        }

        static int tileCell2Node(int cell)
        {
            return (int) Math.Floor(cell / 8d);
        }
    }

}