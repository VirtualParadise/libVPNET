namespace VpNet.NativeApi
{
    /// <summary>
    /// Callback Types
    /// </summary>
    public enum Callbacks
    {
        /// <summary>
        /// Object add callback
        /// </summary>
        ObjectAdd=0,
        /// <summary>
        /// Object change callback
        /// </summary>
        ObjectChange,
        /// <summary>
        /// Get friend info callback
        /// </summary>
        Friend,
        /// <summary>
        /// Friend addition callback
        /// </summary>
        FriendAdd,
        /// <summary>
        /// Friend deletion callback
        /// </summary>
        FriendDelete,
        /// <summary>
        /// Terrain query callback
        /// </summary>
        TerrainQuery,
        /// <summary>
        /// Terrain node set callback
        /// </summary>
        TerrainNodeSet,
        /// <summary>
        /// Highest callback
        /// </summary>
        Highest,
    }
}
