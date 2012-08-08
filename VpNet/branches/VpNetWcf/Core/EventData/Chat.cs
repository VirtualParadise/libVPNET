#if (WCF)
using System.Runtime.Serialization;
#endif

namespace VpNet.Core.EventData
{
#if (WCF)
    [DataContract]
#endif
    public struct Chat
    {
#if (WCF)
        [DataMember]
#endif
        public string Username { get; set; }
#if (WCF)
        [DataMember]
#endif
        public string Message { get; set; }
#if (WCF)
        [DataMember]
#endif
        public int Session { get; set; }

        public Chat(string username, string message, int session) : this()
        {
            Username = username;
            Message = message;
            Session = session;
        }
    }
}
