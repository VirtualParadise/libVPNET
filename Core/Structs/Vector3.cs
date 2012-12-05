namespace VpNet.Core.Structs
{
    public struct Vector3 : IVector3
    {
        public static Vector3 Zero = new Vector3(0, 0, 0);
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public Vector3(float x, float y, float z) : this()
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
}
