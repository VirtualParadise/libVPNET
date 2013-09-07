using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VP
{
    public struct Uniserver
    {
        public static readonly Uniserver VirtualParadise = new Uniserver
        {
            CanonicalName = "Virtual Paradise",
            Host          = "universe.virtualparadise.org",
            Port          = 57000
        };

        public string CanonicalName;
        public string Host;
        public ushort Port;
    }
}
