using System;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using VP.Native;

namespace VP
{
    /// <summary>
    /// Container class for Instance's terrain-related members
    /// </summary>
    public class InstanceTerrain : IDisposable
    {
        internal Instance instance;

        public InstanceTerrain(Instance instance)
        {
            this.instance = instance;
            instance.SetNativeEvent(Events.TerrainNode, OnTerrainNode);
        }

        public void Dispose()
        {
            GetNode = null;
        } 

        #region Events
        public delegate void TerrainNodeArgs(Instance sender, TerrainNode node, int tileX, int tileZ);

        public event TerrainNodeArgs GetNode;
        #endregion

        #region Methods
        /// <summary>
        /// Sets a terrain node data to world
        /// </summary>
        public void SetNode(TerrainNode node, int tileX, int tileZ)
        {
            int rc;

            lock (instance)
                rc = Functions.vp_terrain_node_set(
                    instance.pointer,
                    tileX, tileZ,
                    node.X, node.Z,
                    DataConverters.NodeToNodeData(node) );

            if (rc != 0)
                throw new VPException((ReasonCode)rc);
        }

        /// <summary>
        /// Queries a tile, using a 2 dimensional array of node revisions for versioning.
        /// Fires the GetNode event after each node received
        /// </summary>
        public void QueryTile(int tileX, int tileZ, int[,] nodeRevision)
        {
            int rc;

            lock (instance)
                rc = Functions.vp_terrain_query(instance.pointer, tileX, tileZ, nodeRevision);

            if (rc != 0)
                throw new VPException((ReasonCode)rc);
        }
        #endregion

        #region Event handlers
        internal void OnTerrainNode(IntPtr sender)
        {
            if (GetNode == null) return;
            var tileX = Functions.vp_int(sender, IntAttributes.TerrainTileX);
            var tileZ = Functions.vp_int(sender, IntAttributes.TerrainTileZ);

            var node = new TerrainNode(sender);
            GetNode(instance, node, tileX, tileZ);
        }
        #endregion
    }
}
