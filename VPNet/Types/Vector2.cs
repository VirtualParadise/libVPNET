namespace VP
{
    public struct Vector2
    {
        public static Vector2 Zero = new Vector2();

        public float X;
        public float Y;

        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }

        public Vector2(float a)
        {
            X = a;
            Y = a;
        }
    }
}