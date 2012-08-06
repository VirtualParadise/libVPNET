
using System.Runtime.Serialization;

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
        [DataMember]
#endif
            Online,
#if (WCF)
        [DataMember]
#endif
            Stopped,
#if (WCF)
        [DataMember]
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
