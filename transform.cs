using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EggPainter
{


    class transform
    {
        public matrix M;

        bool iswhite(char c)
        {
            if ((c == ' ') || (c == '\t') || (c == ',')) return true;
            return false;
        }

        bool isnumeric(char c)
        {
            if (((c >= '0') && (c <= '9')) || (c == 'e') || (c == 'E') || (c == '.') || (c == '-')) return true;
            return false;
        }

        bool isempty(ref string s)
        {
            if (s.Length < 1) return true;
            while ((s.Length > 0) && iswhite(s[0])) s = s.Substring(1);
            if (s.Length < 1) return true;
            return false;
        }

        double get_double(ref string s)
        {
            int n = 0;
            while ((s.Length > 0) && iswhite(s[0])) s = s.Substring(1);
            if (s.Length < 1) throw new Exception("Bad number");
            while ((n < s.Length) && isnumeric(s[n])) n++;
            string sn = s.Substring(0, n);
            s = s.Substring(n);
            double d = 0;
            try
            {
                d = Convert.ToDouble(sn);
            }
            catch (Exception e)
            {
                throw new Exception("Bad number");
            }
            return d;
        }

        void clearstring(ref string s)
        {
            while ((s.Length > 0) && iswhite(s[0])) s = s.Substring(1);
            while ((s.Length > 0) && iswhite(s[s.Length-1])) s=s.Substring(0,s.Length-1);
        }

        matrix gettrf(ref string trf)
        {
            matrix N = new matrix();
            clearstring(ref trf);
            if (trf.Length < 1) return N;
            int i = 0;
            while ((i < trf.Length) && (trf[i] != '(')) i++;
            if (i >= trf.Length)
                throw new Exception("Bad Transform String");
            string cmd = trf.Substring(0,i);
            trf = trf.Substring(i + 1, trf.Length - (i + 1));
            clearstring(ref cmd);
            clearstring(ref trf);
            i = 0;
            while ((i < trf.Length) && (trf[i] != ')')) i++;
            if (i >= trf.Length)
                throw new Exception("Bad Transform String");
            string param = trf.Substring(0, i);
            trf = trf.Substring(i + 1, trf.Length - (i + 1));
            clearstring(ref param);
            clearstring(ref trf);

            N.unity();
            if (cmd.Equals("translate"))
            {
                double ox = get_double(ref param);
                double oy = 0;
                if (!isempty(ref trf)) oy = get_double(ref param);
                N.set(0, 2, ox);
                N.set(1, 2, oy);
            }
            else if (cmd.Equals("matrix"))
            {
                double a = get_double(ref param);
                double b = get_double(ref param);
                double c = get_double(ref param);
                double d = get_double(ref param);
                double e = get_double(ref param);
                double f = get_double(ref param);
                N.set(0, 0, a);
                N.set(1, 0, b);
                N.set(0, 1, c);
                N.set(1, 1, d);
                N.set(0, 2, e);
                N.set(1, 2, f);
            }
            else if (cmd.Equals("scale"))
            {
                double sx = get_double(ref param);
                double sy = sx;
                if (!isempty(ref trf))
                    sy = get_double(ref param);
                N.set(0, 0, sx);
                N.set(1, 1, sy);
            }
            else if (cmd.Equals("rotate"))
            {
                trf = trf.Substring(7);
                trf = trf.Substring(0, trf.Length - 1);
                double a = get_double(ref param);
                N.set(0, 0, Math.Cos(a));
                N.set(0, 1, -Math.Sin(a));
                N.set(1, 0, Math.Sin(a));
                N.set(1, 1, Math.Cos(a));
            }
            else if (cmd.Equals("skewX"))
            {
                trf = trf.Substring(6);
                trf = trf.Substring(0, trf.Length - 1);
                double a = get_double(ref param);
                N.set(0, 1, Math.Tan(a));
            }
            else if (cmd.Equals("skewY"))
            {
                trf = trf.Substring(6);
                trf = trf.Substring(0, trf.Length - 1);
                double a = get_double(ref param);
                N.set(1, 0, Math.Tan(a));
            }
            else
            {
                throw new Exception("Bad transformation (unsupported)");
            }
            return N;
        }

        public transform(transform last, string trf)
        {
            matrix N = new matrix();
            N.unity();
            clearstring(ref trf);
            while (trf.Length > 0)
            {
                matrix Q = gettrf(ref trf);
                N.multiply_with(Q);
            }
            if (last != null)
            {
                M = new matrix(last.M);
                M.multiply_with(N);
            }
            else
            {
                M = new matrix(N);
            }
        }

        public void process(ref double x, ref double y) {
            vector erg=M.mult(new vector(x,y));
            x = erg.x;
            y = erg.y;
        }


    }
}
