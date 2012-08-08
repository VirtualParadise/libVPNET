namespace VpNetFramework
{
    public class ReconnectSettings
    {
        private readonly uint _milliseconds;
        private readonly uint _multiplicationFactor;
        private readonly uint _maximumTimeMs;

        public ReconnectSettings(uint milliseconds, uint multiplicationFactor, uint maximumTimeMs)
        {
            _milliseconds = milliseconds;
            _multiplicationFactor = multiplicationFactor;
            _maximumTimeMs = maximumTimeMs;
        }

        public uint MaximumTimeMs
        {
            get { return _maximumTimeMs; }
        }

        public uint MultiplicationFactor
        {
            get { return _multiplicationFactor; }
        }

        public uint Milliseconds
        {
            get { return _milliseconds; }
        }
    }
}
