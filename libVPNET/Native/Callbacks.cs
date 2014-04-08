namespace VP.Native
{
    internal enum Callbacks : int
    {
        ObjectAdd,
        ObjectChange,
        ObjectDelete,
        Friend,         // Unused
        FriendAdd,      // Unused
        FriendDelete,   // Unused
        TerrainQuery,   // Unused
        TerrainNodeSet, // Broken
        ObjectGet,
        Highest,
    }
}