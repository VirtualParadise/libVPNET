namespace VP
{
    public struct Quaternion
    {
        public static Quaternion Zero = new Quaternion();
        public static Quaternion ZeroEuler = new Quaternion()
        {
            W = float.MaxValue
        };

        public float X;
        public float Y;
        public float Z;
        public float W;
    }
}