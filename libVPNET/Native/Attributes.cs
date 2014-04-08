namespace VP.Native
{
    internal enum IntAttributes : int
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
        DisconnectErrorCode,
        UrlTarget,
        IntegerAttributeHighest, 
    }

    internal enum FloatAttributes : int
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

    internal enum StringAttributes : int
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
        Url,
        StringAttributeHighest,
    }

    internal enum DataAttributes : int
    {
        ObjectData = 0,
        TerrainNodeData,
        VpHighestData 
    }
}
