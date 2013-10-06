using System;
using VP.Native;

namespace VP
{
    /// <summary>
    /// Container for SDK methods, events and properties related to terrain, such as
    /// querying and terraforming
    /// </summary>
    public class TerrainContainer
    {
        static readonly int[,] allNodes = new int[4,4]
        {
            { -1, -1, -1, -1 },
            { -1, -1, -1, -1 },
            { -1, -1, -1, -1 },
            { -1, -1, -1, -1 },
        };

        #region Construction & disposal
        Instance instance;

        internal TerrainContainer(Instance instance)
        {
            this.instance = instance;
            instance.setNativeEvent(Events.TerrainNode, OnTerrainNode);
        }

        internal void Dispose()
        {
            GetNode = null;
        }  
        #endregion

        #region Events
        /// <summary>
        /// Encapsulates a method that accepts a source <see cref="Instance"/>, a
        /// <see cref="TerrainNode"/> definition and the X and Z of the tile it belongs
        /// to for the <see cref="GetNode"/> event
        /// </summary>
        public delegate void TerrainNodeArgs(Instance sender, TerrainNode node, int tileX, int tileZ);

        /// <summary>
        /// Fired for each node received after a call to
        /// <see cref="QueryTile(int, int)"/>, providing the node's data and the parent
        /// tile's coordinates
        /// </summary>
        public event TerrainNodeArgs GetNode;
        #endregion

        #region Event handlers
        internal void OnTerrainNode(IntPtr sender)
        {
            if (GetNode == null)
                return;

            var tileX = Functions.vp_int(sender, IntAttributes.TerrainTileX);
            var tileZ = Functions.vp_int(sender, IntAttributes.TerrainTileZ);

            var node = new TerrainNode(sender);
            GetNode(instance, node, tileX, tileZ);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Sets a terrain node's worth of data to the world's terrain. Thread-safe.
        /// </summary>
        public void SetNode(TerrainNode node, int tileX, int tileZ)
        {
            lock (instance.mutex)
                Functions.Call( () =>
                    Functions.vp_terrain_node_set(
                    instance.pointer,
                    tileX, tileZ,
                    node.X, node.Z,
                    DataHandlers.NodeToNodeData(node))
                );
        }

        /// <summary>
        /// Queries a tile, using a 2 dimensional array of node revisions for versioning.
        /// Thread-safe.
        /// </summary>
        /// <remarks>
        /// Only nodes that have a revision higher than the one provided for it will be
        /// sent via the <see cref="GetNode"/> event
        /// </remarks>
        public void QueryTile(int tileX, int tileZ, int[,] nodeRevision)
        {
            lock (instance.mutex)
                Functions.Call( () => Functions.vp_terrain_query(instance.pointer, tileX, tileZ, nodeRevision) );
        }

        /// <summary>
        /// Queries a tile for all its nodes. Thread-safe.
        /// </summary>
        public void QueryTile(int tileX, int tileZ)
        {
            lock (instance.mutex)
                Functions.Call( () => Functions.vp_terrain_query(instance.pointer, tileX, tileZ, allNodes) );
        }
        #endregion
    }
}
