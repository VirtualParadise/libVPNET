using System;
using System.Runtime.Serialization;

namespace VpNet.Core.Structs
{
#if (WCF)
[DataContract]
#endif
    public struct Color
    {
#if (WCF)
    [DataMember]
#endif
        public float Red { get; set; }
#if (WCF)
    [DataMember]
#endif    
        public float Green { get; set; }
#if (WCF)
    [DataMember]
#endif
        public float Blue { get; set; }
        
        public static Color FromName(string name)
        {
            throw new NotImplementedException();
        }

        public static Color FromHtml(string hex)
        {
            throw new NotImplementedException();            
        }
    }
}
