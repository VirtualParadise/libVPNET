#if (WCF)
using System.Runtime.Serialization;
#endif

namespace VpNet.Core.EventData
{
#if (WCF)
        [DataContract]
#endif
    public struct World
    {
        public World(string worldName, WorldState state, int userCount) : this()
        {
            Name = worldName;
            UserCount = userCount;
            State = state;
        }
#if (WCF)
        [DataContract]
#endif
        public enum WorldState : int
        {
#if (WCF)
        [EnumMember]
#endif
            Online,
#if (WCF)
        [EnumMember]
#endif
            Stopped,
#if (WCF)
        [EnumMember]
#endif
            Unknown
        }

#if (WCF)
        [DataMember]
#endif
        public string Name { get; set; }
#if (WCF)
        [DataMember]
#endif
        public int UserCount { get; set; }
#if (WCF)
        [DataMember]
#endif
        public WorldState State { get; set; }
    }
}
