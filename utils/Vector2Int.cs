using System;

namespace AoC2022.utils
{
    public class Vector2Int
    {
        public int x;
        public int y;

        public Vector2Int(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public static Vector2Int operator +(Vector2Int a, Vector2Int b)
        {
            return new Vector2Int(a.x + b.x, a.y + b.y);
        }

        public static Vector2Int operator -(Vector2Int a, Vector2Int b)
        {
            return new Vector2Int(a.x - b.x, a.y - b.y);
        }

        public static Vector2Int operator *(Vector2Int a, int b)
        {
            return new Vector2Int(a.x * b, a.y * b);
        }

        public static Vector2Int operator *(int a, Vector2Int b)
        {
            return new Vector2Int(a * b.x, a * b.y);
        }

        public static Vector2Int operator /(Vector2Int a, int b)
        {
            return new Vector2Int(a.x / b, a.y / b);
        }

        public static Vector2Int operator /(int a, Vector2Int b)
        {
            return new Vector2Int(a / b.x, a / b.y);
        }

        public static bool operator ==(Vector2Int a, Vector2Int b)
        {
            return a.x == b.x && a.y == b.y;
        }

        public static bool operator !=(Vector2Int a, Vector2Int b)
        {
            return a.x != b.x || a.y != b.y;
        }

        public override bool Equals(object obj)
        {
            return obj is Vector2Int vector &&
                   x == vector.x &&
                   y == vector.y;
        }

        public bool Equals(Vector2Int other)
        {
            return x == other.x && y == other.y;
        }
        
        public float Length()
        {
            return (float)Math.Sqrt(x * x + y * y);
        }
        
        public int ManhattanDistance(Vector2Int other)
        {
            return Math.Abs(x - other.x) + Math.Abs(y - other.y);
        }

        public Vector2Int Normalize()
        {
            return this / ManhattanDistance(new Vector2Int(0, 0));
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(x, y);
        }

        public override string ToString()
        {
            return $"({x}, {y})";
        }
    }
}