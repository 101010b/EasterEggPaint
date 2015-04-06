using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EggPainter
{
    class cmditemf
    {
        public double x;
        public double y;
        public int down;


        public cmditemf(double _x, double _y, int _down)
        {
            x=_x;
            y=_y;
            down = _down;
        }

        public cmditem toint(double scalex, double scaley, double ox, double oy)
        {
            return new cmditem(x * scalex + ox, y * scaley + oy, down);
        }

    }
}
