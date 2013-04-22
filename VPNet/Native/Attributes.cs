namespace VP.Native
{
    public enum IntAttributes : int
    {
        AvatarSession = 0,
        AvatarType,
        MyType,
        ObjectId,
        ObjectType,
        ObjectTime,
        ObjectUserId,
        WorldState,
        WorldUsers,
        ReferenceNumber,
        Callback,
        UserId,
        UserRegistrationTime,
        UserOnlineTime,
        UserLastLogin,
        FriendId,
        FriendUserId,
        FriendOnline,
        MyUserId,
        ProxyType,
        ProxyPort,
        CellX,
        CellZ,
        TerrainTileX,
        TerrainTileZ,
        TerrainNodeX,
        TerrainNodeZ,
        TerrainNodeRevision,
        ClickedSession,
        ChatType,
        ChatColorRed,
        ChatColorGreen,
        ChatColorBlue,
        ChatEffects,
        IntegerAttributeHighest, 
    }

    public enum FloatAttributes : int
    {
        AvatarX = 0,
        AvatarY,
        AvatarZ,
        AvatarYaw,
        AvatarPitch,

        MyX,
        MyY,
        MyZ,
        MyYaw,
        MyPitch,

        ObjectX,
        ObjectY,
        ObjectZ,
        ObjectRotationX,
        ObjectRotationY,
        ObjectRotationZ,
        ObjectRotationAngle,

        TeleportX,
        TeleportY,
        TeleportZ,
        TeleportYaw,
        TeleportPitch,

        ClickHitX,
        ClickHitY,
        ClickHitZ,

        FloatAttributeHighest, 
    }

    public enum StringAttributes : int
    {
        AvatarName = 0,
        ChatMessage,
        ObjectModel,
        ObjectAction,
        ObjectDescription,
        WorldName,
        UserName,
        UserEmail,
        WorldSettingKey,
        WorldSettingValue,
        FriendName,
        ProxyHost,
        TeleportWorld,
        StringAttributeHighest,
    }

    public enum DataAttributes : int
    {
        ObjectData = 0,
        TerrainNodeData,
        VpHighestData 
    }
}
