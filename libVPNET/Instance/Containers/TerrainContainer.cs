using System;
using System.Collections.Generic;
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

            instance.setNativeCallback(Callbacks.TerrainNodeSet, OnNodeSetCallback);
        }

        internal void Dispose()
        {
            GetNode = null;

            CallbackNodeSet = null;

            nodeReferences.Clear();
        }  
        #endregion

        #region Callback references
        Dictionary<int, Tuple<TerrainNode,int,int>> nodeReferences = new Dictionary<int, Tuple<TerrainNode,int,int>>();

        int nextRef = int.MinValue;
        int nextReference
        {
            get
            {
                if (nextRef < int.MaxValue)
                    nextRef++;
                else
                    nextRef = int.MinValue;

                return nextRef;
            }
        } 
        #endregion

        #region Events and callbacks
        /// <summary>
        /// Encapsulates a method that accepts a source <see cref="Instance"/>, a
        /// <see cref="TerrainNode"/> definition and the X and Z of the tile it belongs
        /// to for the <see cref="GetNode"/> event
        /// </summary>
        public delegate void GetNodeArgs(Instance sender, TerrainNode node, int tileX, int tileZ);
        /// <summary>
        /// Encapsulates a method that accepts a source <see cref="Instance"/>, a
        /// reason code and a <see cref="TerrainNode"/> reference for
        /// <see cref="CallbackNodeSet"/>
        /// </summary>
        public delegate void NodeSetCallbackArgs(Instance sender, ReasonCode result, TerrainNode node, int tileX, int tileZ);

        /// <summary>
        /// Fired for each node received after a call to
        /// <see cref="QueryTile(int, int)"/>, providing the node's data and the parent
        /// tile's coordinates
        /// </summary>
        public event GetNodeArgs GetNode;
        /// <summary>
        /// Fired after a call to the asynchronous <see cref="SetNode"/>, providing a
        /// result code and, if successful, the set <see cref="TerrainNode"/>
        /// </summary>
        public event NodeSetCallbackArgs CallbackNodeSet;
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

        #region Callback handlers
        void OnNodeSetCallback(IntPtr sender, int rc, int reference)
        {
            var node = nodeReferences[reference];
            nodeReferences.Remove(reference);

            if (CallbackNodeSet == null)
                return;

            CallbackNodeSet(instance, (ReasonCode) rc, node.Item1, node.Item2, node.Item3);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Sets a terrain node's worth of data to the world's terrain. Thread-safe.
        /// </summary>
        public void SetNode(TerrainNode node, int tileX, int tileZ)
        {
            lock (instance.Mutex)
            {
                var refNum  = nextReference;
                var refNode = new Tuple<TerrainNode, int, int> (node, tileX, tileZ);
                nodeReferences.Add(refNum, refNode);

                Functions.vp_int_set(instance.Pointer, IntAttributes.ReferenceNumber, refNum);

                try {
                    Functions.Call( () =>
                        Functions.vp_terrain_node_set(
                        instance.Pointer,
                        tileX, tileZ,
                        node.X, node.Z,
                        DataHandlers.NodeToNodeData(node))
                    );
                }
                catch
                {
                    nodeReferences.Remove(refNum);
                    throw;
                }
            }
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
            lock (instance.Mutex)
                Functions.Call( () => Functions.vp_terrain_query(instance.Pointer, tileX, tileZ, nodeRevision) );
        }

        /// <summary>
        /// Queries a tile for all its nodes. Thread-safe.
        /// </summary>
        public void QueryTile(int tileX, int tileZ)
        {
            lock (instance.Mutex)
                Functions.Call( () => Functions.vp_terrain_query(instance.Pointer, tileX, tileZ, allNodes) );
        }
        #endregion
    }
}
