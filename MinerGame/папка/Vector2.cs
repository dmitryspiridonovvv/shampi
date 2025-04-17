using OpenTK.Mathematics;

namespace MinerGameLib
{
    public struct Vector2
    {
        public float X { get; set; }
        public float Y { get; set; }

        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }

        public static Vector2 operator +(Vector2 a, Vector2 b) => new Vector2(a.X + b.X, a.Y + b.Y);
        public static Vector2 operator -(Vector2 a, Vector2 b) => new Vector2(a.X - b.X, a.Y - b.Y);
        public static Vector2 operator *(Vector2 a, float scalar) => new Vector2(a.X * scalar, a.Y * scalar);
        public static Vector2 operator *(float scalar, Vector2 a) => a * scalar;

        public static implicit operator OpenTK.Mathematics.Vector2(Vector2 v) => new OpenTK.Mathematics.Vector2(v.X, v.Y);
        public static implicit operator Vector2(OpenTK.Mathematics.Vector2 v) => new Vector2(v.X, v.Y);
    }
}