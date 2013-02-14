namespace VP
{
    public struct Quaternion
    {
        public static Quaternion Zero      = new Quaternion();
        public static Quaternion ZeroEuler = new Quaternion()
        {
            W = float.MaxValue
        };

        public float X;
        public float Y;
        public float Z;
        public float W;

        public Quaternion(float x = .0f, float y = .0f, float z = .0f, float w = float.MaxValue)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.W = w;
        }
    }
}