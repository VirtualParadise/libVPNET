using System.Runtime.Serialization;

namespace VpNet.Core.Structs
{
#if (WCF)
[DataContract]
#endif
    public struct Vector3 : IVector3
    {
#if (WCF)
    [DataMember]
#endif
        public float X { get; set; }
#if (WCF)
    [DataMember]
#endif
        public float Y { get; set; }
#if (WCF)
    [DataMember]
#endif
        public float Z { get; set; }

        public Vector3(float x, float y, float z) : this()
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
}
