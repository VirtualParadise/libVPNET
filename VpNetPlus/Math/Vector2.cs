using System;

namespace VpNetPlus.Math
{
    public class Vector2
    {
        public double x;
        public double y;

        public Vector2()
        {
        }
        public Vector2(Vector2 orig)
        {
            x = orig.x;
            y = orig.y;
        }
        public Vector2(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
        static public Vector2 operator +(Vector2 first, Vector2 second)
        {
            return new Vector2(first.x + second.x, first.y + second.y);
        }
        static public Vector2 operator -(Vector2 first, Vector2 second)
        {
            return new Vector2(first.x - second.x, first.y - second.y);
        }
        static public double DistanceSquared(Vector2 one, Vector2 two)
        {
            return (one.x - two.x) * (one.x - two.x) + (one.y - two.y) * (one.y - two.y);
        }
        public double Det()
        {
            return System.Math.Sqrt(x * x + y * y);
        }
        public override String ToString()
        {
            return "<" + x.ToString() + "," + y.ToString() + ">";
        }
        public bool InRange(int minx, int miny, int maxx, int maxy)
        {
            return (x >= minx && x <= maxx && y >= miny && y <= maxy);
        }
        public override bool Equals(object two)
        {
            return ((Vector2)two).x == this.x && ((Vector2)two).y == this.y;
        }
        static public bool operator ==(Vector2 first, Vector2 second)
        {
            return first.x == second.x && first.y == second.y;
        }
        static public bool operator !=(Vector2 first, Vector2 second)
        {
            return first.x != second.x || first.y != second.y;
        }
        public override int GetHashCode()
        {
            return x.GetHashCode() / 3 + y.GetHashCode() / 3;
        }
    }
}
