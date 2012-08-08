using System;
#if (WCF)
using System.Runtime.Serialization;
#endif

namespace VpNet.Core.Structs
{
#if (WCF)
    [DataContract]
#endif
    public class VpObject
    {
#if (WCF)
    [DataMember]
#endif
        public int Id { get; set; }
#if (WCF)
    [DataMember]
#endif
    public int Type { get; set; }
#if (WCF)
    [DataMember]
#endif
    public DateTime Time { get; set; }
#if (WCF)
    [DataMember]
#endif
    public int Owner { get; set; }
#if (WCF)
    [DataMember]
#endif
    public Vector3 Position { get; set; }
#if (WCF)
    [DataMember]
#endif
    public Vector3 Rotation { get; set; }
#if (WCF)
    [DataMember]
#endif
    public float Angle { get; set; }
#if (WCF)
    [DataMember]
#endif

    public string Action { get; set; }
#if (WCF)
    [DataMember]
#endif
    public string Description { get; set; }
#if (WCF)
    [DataMember]
#endif
    public int ObjectType { get; set; }
#if (WCF)
    [DataMember]
#endif
        public string Model { get; set; }

        public VpObject(int id, int type, DateTime time, int owner, Vector3 position, Vector3 rotation, float angle, string action, string description, int objectType, string model)
        {
            Id = id;
            Type = type;
            Time = time;
            Owner = owner;
            Position = position;
            Rotation = rotation;
            Angle = angle;
            Action = action;
            Description = description;
            ObjectType = objectType;
            Model = model;
        }

        public VpObject()
        {
            Angle = float.MaxValue;
        }
    }
}
