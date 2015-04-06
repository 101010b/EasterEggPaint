using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EggPainter
{
    class cmditem
    {
        public int rot;
        public int x;
        public int down;
        public int valid;

        public cmditem(int _rot, int _x, int _down)
        {
            rot = _rot;
            x = _x;
            down = _down;
            valid = 1;
        }

        public cmditem(double _rot, double _x, int _down)
        {
            rot = (int) Math.Floor(_rot+0.5);
            x = (int) Math.Floor(_x + 0.5);
            down = _down;
            valid = 1;
        }

        public cmditem(string s)
        {
            int i=0;
            valid = 0;

            if (s.Length < 1) return;
            if (s[0] == '#') return;

            while ((i < s.Length) && (s[i] != '\t'))
                i++;
            if (i >= s.Length)
                return;
            string s1 = s.Substring(0, i);
            int j=i+1;
            i++;
            while ((i < s.Length) && (s[i] != '\t'))
                i++;
            if (i >= s.Length)
                return;
            string s2 = s.Substring(j, i-j);
            string s3 = s.Substring(i + 1);

            if (!int.TryParse(s1, out rot)) return;
            if (!int.TryParse(s2, out x)) return;
            if (!int.TryParse(s3, out down)) return;

            valid = 1;
        }

        public string getline()
        {
            return string.Format("{0} {1} {2}", rot, x, down);
        }

        public string codeline()
        {
            return string.Format("{0}\t{1}\t{2}", rot, x, down);
        }

    }
}
