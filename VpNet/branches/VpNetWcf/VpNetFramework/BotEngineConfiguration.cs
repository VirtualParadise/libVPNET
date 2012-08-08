using VpNet.Core.Structs;

namespace VpNetFramework
{
    public struct BotEngineConfiguration
    {
        public string Server { get;set; }
        public ushort ServerPort { get; set; }
        public string LoginUser { get; set; }
        public string LoginPassword { get; set; }
        public string BotName { get; set; }
        public string World { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }

        public ReconnectSettings ReconnectUniverse { get; set; }
        public ReconnectSettings ReconnecWorld { get; set; }
    }
}
