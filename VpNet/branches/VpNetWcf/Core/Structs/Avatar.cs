using System.Runtime.Serialization;

namespace VpNet.Core.Structs
{
#if (WCF)
[DataContract]
#endif
    public struct Avatar
    {
#if (WCF)
    [DataMember]
#endif
    public string Name { get; set; }
#if (WCF)
    [DataMember]
#endif
    public int Session { get; set; }
#if (WCF)
    [DataMember]
#endif
    public int AvatarType { get; set; }
#if (WCF)
    [DataMember]
#endif
    public Vector3 Position { get; set; }
#if (WCF)
    [DataMember]
#endif
        public float Yaw { get; set; }
#if (WCF)
    [DataMember]
#endif
    public float Pitch { get; set; }

    public Avatar(string name,int session,int avatarType,float x,float y,float z,float yaw,float pitch) : this()
        {
            Name = name;
            Session = session;
            AvatarType = avatarType;
            Position = new Vector3(x, y, z);
            Yaw = yaw;
            Pitch = pitch;
        }
    }
}
