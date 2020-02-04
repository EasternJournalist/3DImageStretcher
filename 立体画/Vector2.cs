using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 立体画
{
    //一个简单的二维向量类
    public struct Vector2
    {
        public double X, Y;
        public Vector2(double _X, double _Y)
        {
            X = _X;
            Y = _Y;
        }
        public static Vector2 operator +(Vector2 a, Vector2 b)
        {
            return new Vector2(a.X + b.X, a.Y + b.Y);
        }
        public static Vector2 operator -(Vector2 a, Vector2 b)
        {
            return new Vector2(a.X - b.X, a.Y - b.Y);
        }
        public static Vector2 operator *(Vector2 a, Vector2 b)
        {
            return new Vector2(a.X * b.X, a.Y * b.Y);
        }
        public static Vector2 operator *(double l, Vector2 b)
        {
            return new Vector2(l * b.X, l * b.Y);
        }
        public static implicit operator System.Drawing.PointF(Vector2 a)
        {
            return new System.Drawing.PointF((float)a.X, (float)a.Y);
        }
        public double Mod()
        {
            return Math.Sqrt(X * X + Y * Y);
        }
    }
}
