namespace VP.Native
{
    /// <summary>
    /// Native callback types
    /// </summary>
    public enum Callbacks : int
    {
        ObjectAdd,
        ObjectChange,
        ObjectDelete,
        Friend,
        FriendAdd,
        FriendDelete,
        TerrainQuery,
        TerrainNodeSet,
        Highest,
    }
}