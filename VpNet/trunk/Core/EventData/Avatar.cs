namespace VpNet.Core.EventData
{
    public class Avatar
    {
        public string Name;
        public int Session;
        public int AvatarType;
        public float X, Y, Z;
        public float Yaw, Pitch;

        public Avatar(string name,int session,int avatarType,float x,float y,float z,float yaw,float pitch)
        {
            Name = name;
            Session = session;
            AvatarType = avatarType;
            X = x;
            Y = y;
            Z = z;
            Yaw = yaw;
            Pitch = pitch;
        }
    }
}
