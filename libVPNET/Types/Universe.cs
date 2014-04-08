namespace VP
{
    /// <summary>
    /// Represents a universe that the SDK connects to
    /// </summary>
    public struct Uniserver
    {
        /// <summary>
        /// Constant value representing the official Virtual Paradise universe server
        /// </summary>
        public static readonly Uniserver VirtualParadise = new Uniserver
        {
            CanonicalName = "Virtual Paradise",
            Host          = "universe.virtualparadise.org",
            Port          = 57000
        };

        /// <summary>
        /// Gets the canonical name of this universe
        /// </summary>
        public string CanonicalName;
        /// <summary>
        /// Gets the hostname of this universe
        /// </summary>
        public string Host;
        /// <summary>
        /// Gets the port number of this universe
        /// </summary>
        public ushort Port;
    }
}
