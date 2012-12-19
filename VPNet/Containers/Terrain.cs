using System;
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
        public delegate void TerrainNodeArgs(Instance sender);

        public event TerrainNodeArgs GetNode;
        #endregion

        #region Methods
        /// <summary>
        /// Sets a terrain node data to world
        /// HIGHLY UNSTABLE
        /// </summary>
        public void SetNode(TerrainNode node, int tileX, int tileZ)
        {
            int rc;
            Console.WriteLine("Setting node: {0}x{1} at tile {2}x{3}", node.X, node.Z, tileX, tileZ);

            lock (instance)
                rc = Functions.vp_terrain_node_set(
                    instance.pointer,
                    tileX, tileZ,
                    node.X, node.Z,
                    node.Cells);

            if (rc != 0)
                throw new VPException((ReasonCode)rc);
        }

        /// <summary>
        /// Queries a tile, using a 2 dimensional array of node revisions for versioning
        /// INCOMPLETE EVENT; DO NOT USE
        /// </summary>
        public void Query(int tileX, int tileZ, int[,] nodeRevision)
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
            int dataLength;
            var data = Functions.vp_data(sender, VPAttribute.TerrainNodeData, out dataLength);
            var tileX = Functions.vp_int(sender, VPAttribute.TerrainTileX);
            var tileZ = Functions.vp_int(sender, VPAttribute.TerrainTileZ);
            var nodeX = Functions.vp_int(sender, VPAttribute.TerrainNodeX);
            var nodeZ = Functions.vp_int(sender, VPAttribute.TerrainNodeZ);
            var revision = Functions.vp_int(sender, VPAttribute.TerrainNodeRevision);

            var terrainNode = Functions.GetData(sender, VPAttribute.TerrainNodeData);
            var terr = (TerrainCell) Marshal.PtrToStructure(data, typeof(TerrainCell));
            Console.WriteLine("Terrain: height: {0} attr: {1} x: {2} y: {3}", terr.Height, terr.Attributes, nodeX, nodeZ);
        }
        #endregion
    }
}
