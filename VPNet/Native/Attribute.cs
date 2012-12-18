namespace VP.Native
{
    public enum VPAttribute
    {
        #region Integer attributes
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
        IntegerAttributeHighest, 
        #endregion

        #region Float attributes
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
        ObjectYaw = ObjectRotationX,
        ObjectPitch = ObjectRotationY,
        ObjectRoll = ObjectRotationZ,
        ObjectRotationAngle,
        TeleportX,
        TeleportY,
        TeleportZ,
        TeleportYaw,
        TeleportPitch,
        FloatAttributeHighest, 
        #endregion

        #region String attributes
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
        #endregion

        #region Data attributes
        ObjectData = 0,
        TerrainNodeData,
        VpHighestData 
        #endregion
    }
}
