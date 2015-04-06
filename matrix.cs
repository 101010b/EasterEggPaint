using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EggPainter
{
    class matrix
    {
        public double[] a=new double[9];

        public matrix()
        {
            zero();
        }

        public matrix(matrix m) {
            for (int i=0;i<9;i++) a[i]=m.a[i];
        }


        public void zero()
        {
            for (int i = 0; i < 9; i++) a[i] = 0;
        }

        public void set(int y, int x, double d) { a[y * 3 + x] = d; }
        public double get(int y, int x) { return a[y * 3 + x]; }

        public void unity()
        {
            zero();
            set(0, 0, 1);
            set(1, 1, 1);
            set(2, 2, 1);
        }

        public void multiply_with(matrix m)
        {
            double[] e = new double[9];
            for (int col=0;col < 3;col++)
                for (int row = 0; row < 3; row++)
                {
                    double ee=0;
                    for (int i = 0; i < 3; i++)
                        ee += a[row * 3 + i] * m.a[i * 3 + col];
                    e[row * 3 + col] = ee;
                }
        }

        public matrix mult(matrix m)
        {
            matrix q = new matrix(this);
            q.multiply_with(m);
            return q;
        }

        public vector mult(vector m)
        {
            vector v = new vector();
            v.x = get(0, 0) * m.x + get(0, 1) * m.y + get(0, 2) * 1;
            v.y = get(1, 0) * m.x + get(1, 1) * m.y + get(1, 2) * 1;
            //double d = get(2, 0) * m.x + get(2, 1) * m.y + get(2, 2) * 1;
            //v.x /= d;
            //v.y /= d;
            return v;
        }








    }
}
