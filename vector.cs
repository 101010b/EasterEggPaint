using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EggPainter
{
    class vector
    {
        public double x;
        public double y;

        public vector(double _x, double _y)
        {
            x = _x;
            y = _y;
        }

        public vector(vector v)
        {
            x = v.x;
            y = v.y;
        }

        public vector()
        {
            x = y = 0;
        }

        public void add_from(double _x, double _y)
        {
            x += _x;
            y += _y;
        }

        public void add_from(vector v)
        {
            x += v.x;
            y += v.y;
        }

        public vector add(double _x, double _y)
        {
            return new vector(x + _x, y + _y);
        }

        public vector add(vector v)
        {
            return new vector(x + v.x, y + v.y);
        }

        public void sub_from(double _x, double _y)
        {
            x -= _x;
            y -= _y;
        }

        public void sub_from(vector v)
        {
            x -= v.x;
            y -= v.y;
        }

        public vector sub(double _x, double _y)
        {
            return new vector(x - _x, y - _y);
        }

        public vector sub(vector v)
        {
            return new vector(x - v.x, y - v.y);
        }

        public vector neg()
        {
            return new vector(-x, -y);
        }

        public double len()
        {
            return Math.Sqrt(x * x + y * y);
        }

        public vector get_norm()
        {
            double l = len();
            if (l > 0)
                return new vector(x / l, y / l);
            return new vector(0, 0);
        }

        public void norm()
        {
            double l = len();
            if (l > 0)
            {
                x /= l;
                y /= l;
            }
        }


        public void rot90()
        {
            double t = x;
            x = -y;
            y = t;
        }

        public vector get_rot90()
        {
            return new vector(-y, x);
        }

        public void mul_with(double n)
        {
            x *= n;
            y *= n;
        }

        public vector mul(double n)
        {
            return new vector(x * n, y * n);
        }

        public double scalar_with(double _x, double _y)
        {
            return x * _x + y * _y;
        }

        public double scalar_with(vector v)
        {
            return x * v.x + y * v.y;
        }

        public double cross_with(double _x, double _y)
        {
            return x * _y - y * _x;
        }

        public double cross_with(vector v)
        {
            return x * v.y - y * v.x;
        }

        public void rotate_by(double phi)
        {
            double xn = x * Math.Cos(phi) - y * Math.Sin(phi);
            double yn = x * Math.Sin(phi) + y * Math.Cos(phi);
        }

        public vector get_rotate(double phi)
        {
            return new vector(x * Math.Cos(phi) - y * Math.Sin(phi), x * Math.Sin(phi) + y * Math.Cos(phi));
        }

    }
}
