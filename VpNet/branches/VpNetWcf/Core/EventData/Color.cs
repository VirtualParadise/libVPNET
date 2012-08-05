using System;

namespace VpNet.Core.EventData
{
    public class Color
    {
        public string Value { get; set; }

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
