using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

// Based on the SVG-Spec from http://www.w3.org/TR/SVG11/

namespace EggPainter
{
    class svg_read
    {
        string fn;
        public bool valid=false;
        public string error=null;
        public List<cmditemf> cl;

        double startx, starty;
        double lastx, lasty;
        double bezier_ax, bezier_ay;
        bool pen_isdown = false;

        public double scaleto=200;
        public double aspect = 1.0;
        public double posx = 0;
        public double posy = 0;

        List<transform> trf = new List<transform>();

        void addcmd(double x, double y, int down)
        {
            if (trf.Count < 1)
                cl.Add(new cmditemf(x, y, down));
            else
            {
                trf[trf.Count - 1].process(ref x, ref y);
                cl.Add(new cmditemf(x, y, down));
            }
        }

        void pen_up()
        {
            if (pen_isdown)
            {
                addcmd(lastx, lasty, 0);
            }
            pen_isdown = false;

        }

        void pen_down()
        {
            if (!pen_isdown)
            {
                addcmd(lastx, lasty, 0);
                addcmd(lastx, lasty, 1);
                startx = lastx;
                starty = lasty;
            }
            pen_isdown = true;
        }


        void moveto(bool relative, double x, double y)
        {
            pen_up();
            if (relative) {
                lastx += x;
                lasty += y;
            } else {
                lastx = x;
                lasty = y;
            }
            bezier_ax = lastx;
            bezier_ay = lasty;
        }

        void lineto(bool relative, double x, double y)
        {
            if (!pen_isdown) 
                pen_down();
            if (relative)
            {
                lastx += x;
                lasty += y;
            }
            else
            {
                lastx = x;
                lasty = y;
            }
            bezier_ax = lastx;
            bezier_ay = lasty;
            addcmd(lastx, lasty, 1);
        }

        void lineto_h(bool relative, double x) {
            if (relative) 
                lineto(false,lastx+x,lasty);
            else
                lineto(false,x,lasty);
        }

        void lineto_v(bool relative, double y) {
            if (relative) 
                lineto(false,lastx,lasty+y);
            else
                lineto(false,lastx,y);
        }

        void lineto_a(bool relative, bool horz, double d)
        {
            if (horz)
                lineto_h(relative, d);
            else
                lineto_v(relative, d);
        }

        void bezierto(bool relative, double ax, double ay, double bx, double by, double x, double y)
        {
            if (!pen_isdown)
                pen_down();

            if (relative)
            {
                ax += lastx;
                ay += lasty;
                bx += lastx;
                by += lasty;
                x += lastx;
                y += lasty;
            }
            bezier_ax = bx;
            bezier_ay = by;

            for (int i = 1; i <= 20; i++)
            {
                double t = (double)i / 20.0;
                double cx = (1 - t) * (1 - t) * (1 - t) * lastx + 3 * t * (1 - t) * (1 - t) * ax + 3 * t * t * (1 - t) * bx + t * t * t * x;
                double cy = (1 - t) * (1 - t) * (1 - t) * lasty + 3 * t * (1 - t) * (1 - t) * ay + 3 * t * t * (1 - t) * by + t * t * t * y;
                addcmd(cx,cy,1);
            }
            lastx = x;
            lasty = y;
        }

        void sbezierto(bool relative, double bx, double by, double x, double y)
        {
            if (relative)
            {
                bx += lastx;
                by += lasty;
                x += lastx;
                y += lasty;
            }
            double ax = -(bezier_ax - lastx) + lastx;
            double ay = -(bezier_ay - lasty) + lasty;
            bezierto(false, ax, ay, bx, by, x, y);
        }

        void qbezierto(bool relative, double ax, double ay, double x, double y)
        {
            if (!pen_isdown)
                pen_down();

            if (relative)
            {
                ax += lastx;
                ay += lasty;
                x += lastx;
                y += lasty;
            }
            bezier_ax = ax;
            bezier_ay = ay;

            for (int i = 1; i <= 20; i++)
            {
                double t = (double)i / 20.0;
                double cx = (1 - t) * (1 - t) * lastx + 2 * t * (1 - t) * ax + t * t * x;
                double cy = (1 - t) * (1 - t) * lasty + 2 * t * (1 - t) * ay + t * t * y;
                addcmd(cx, cy, 1);
            }
            lastx = x;
            lasty = y;
        }

        void qsbezierto(bool relative, double x, double y)
        {
            if (relative)
            {
                x += lastx;
                y += lasty;
            }
            double ax = -(bezier_ax - lastx) + lastx;
            double ay = -(bezier_ay - lasty) + lasty;
            qbezierto(false, ax, ay, x, y);
        }

        void ellipticarcto(bool relative, double rx, double ry, double xrot, bool larf, bool sf)
        {
            throw new Exception("Not Implemented");
            // Use http://www.w3.org/TR/SVG11/implnote.html#ArcImplementationNotes for implementation notes
        }



        public svg_read(string fin)
        {
            fn = fin;
        }

        bool iswhite(char c) {
            if ((c == ' ') || (c == '\t') || (c == ',')) return true;
            return false;
        }

        bool isnumeric(char c) {
            if (((c >= '0') && (c <= '9')) || (c == 'e') || (c == 'E') || (c == '.') || (c == '-')) return true;
            return false;
        }

        void clear_white(out bool eos, ref string o)
        {
            eos = false;
            while ((o.Length > 0) && (iswhite(o[0]))) o = o.Substring(1);
            if (o.Length < 1) { eos = true; return; }
        }

        void next_char(out bool eos, out char c, ref string o)
        {
            eos=false;
            c='\0';
            while ((o.Length > 0) && (iswhite(o[0]))) o=o.Substring(1);
            if (o.Length < 1) { eos=true; return; }
            c=o[0];
            o=o.Substring(1);
        }

        void next_val(out bool eos, out double d, ref string o)
        {
            eos=false;
            d = Double.NaN;
            while ((o.Length > 0) && (iswhite(o[0]))) o=o.Substring(1);
            if (o.Length < 1) { eos=true; return; }
            int n=0;
            while ((n < o.Length) && (isnumeric(o[n]))) n++;
            string sn = o.Substring(0, n);
            o = o.Substring(n);
            try
            {
                d = Convert.ToDouble(sn);
            }
            catch (Exception e)
            {
                throw new Exception("Bad Number Format");
                // d = Double.NaN;
            }
        }

        void parse(string s)
        {
            bool eos = false;
            bool first=true;
            double tx = 0;
            double ty = 0;
            double ax = 0;
            double ay = 0;
            double bx = 0;
            double by = 0;
            double d = 0;
            bool relative=false;
            bool horz=false;
            double rx=0;
            double ry = 0;
            double xrot = 0;
            double larf = 0;
            double sf = 0;

            pen_isdown = false;
            while (!eos)
            {
                char c;
                next_char(out eos, out c, ref s);
                if (!eos)
                {
                    switch (c)
                    {
                        case 'm':
                        case 'M':
                        case 'l':
                        case 'L':
                            next_val(out eos, out tx, ref s);
                            if (eos) throw new Exception("Bad Number in File");
                            next_val(out eos, out ty, ref s);
                            if (eos) throw new Exception("Bad Number in File");
                            if (c == 'm') // Relative
                            {
                                relative = true;
                                if (first) // Exception for the first coordinate pair - must be absulte!
                                    moveto(false, tx, ty);
                                else 
                                    moveto(relative, tx,ty);
                            }
                            else if (c == 'M') 
                            { // Absolute
                                relative = false;
                                moveto(relative, tx, ty);
                            }
                            else if (c == 'l')
                            {
                                relative = true;
                                lineto(relative, tx, ty);
                            }
                            else if (c == 'L')
                            {
                                relative = false;
                                lineto(relative, tx, ty);
                            }
                            first = false;
                            clear_white(out eos, ref s);
                            if (!eos && isnumeric(s[0]))
                            {
                                // More pairs
                                bool eoc = false;
                                while (!eoc)
                                {
                                    next_val(out eos, out tx, ref s);
                                    if (eos) throw new Exception("Bad Number in File");
                                    next_val(out eos, out ty, ref s);
                                    if (eos) throw new Exception("Bad Number in File");
                                    // This is a line to
                                    lineto(relative, tx, ty);
                                    clear_white(out eos, ref s);
                                    if (eos || !isnumeric(s[0]))
                                        eoc = true;
                                }
                            }
                            break;
                        case 'h':
                        case 'H':
                        case 'v':
                        case 'V':
                            next_val(out eos, out d, ref s);
                            if (eos) throw new Exception("Bad Number in File");
                            if (c == 'H')
                            {
                                relative = false;horz=true;
                                lineto_a(relative, horz, d);
                            }
                            else if (c == 'h')
                            {
                                relative = true;horz=true;
                                lineto_a(relative, horz, d);
                            }
                            else if (c == 'V')
                            {
                                relative = false;horz=false;
                                lineto_a(relative, horz, d);
                            }
                            else if (c == 'v')
                            {
                                relative = true;horz=false;
                                lineto_a(relative, horz, d);
                            }
                            clear_white(out eos, ref s);
                            if (!eos && isnumeric(s[0]))
                            {
                                // More values
                                bool eoc = false;
                                while (!eoc)
                                {
                                    next_val(out eos, out d, ref s);
                                    if (eos) throw new Exception("Bad Number in File");
                                    lineto_a(relative, horz, d);
                                    clear_white(out eos, ref s);
                                    if (eos || !isnumeric(s[0]))
                                        eoc = true;
                                }
                            }
                            break;

                        case 'Z':
                        case 'z':
                            lineto(false, startx, starty);
                            pen_up();
                            // first = true;
                            break;

                        case 'C':
                        case 'c':
                            next_val(out eos, out ax, ref s);
                            if (eos) throw new Exception("Bad Number in File");
                            next_val(out eos, out ay, ref s);
                            if (eos) throw new Exception("Bad Number in File");
                            next_val(out eos, out bx, ref s);
                            if (eos) throw new Exception("Bad Number in File");
                            next_val(out eos, out by, ref s);
                            if (eos) throw new Exception("Bad Number in File");
                            next_val(out eos, out tx, ref s);
                            if (eos) throw new Exception("Bad Number in File");
                            next_val(out eos, out ty, ref s);
                            if (eos) throw new Exception("Bad Number in File");

                            if (c == 'C')
                            {
                                relative = false;
                            }
                            else
                            {
                                relative = true;
                            }
                            bezierto(relative,ax,ay,bx,by,tx,ty);

                            clear_white(out eos, ref s);
                            if (!eos && isnumeric(s[0]))
                            {
                                // More values
                                bool eoc = false;
                                while (!eoc)
                                {
                                    next_val(out eos, out ax, ref s);
                                    if (eos) throw new Exception("Bad Number in File");
                                    next_val(out eos, out ay, ref s);
                                    if (eos) throw new Exception("Bad Number in File");
                                    next_val(out eos, out bx, ref s);
                                    if (eos) throw new Exception("Bad Number in File");
                                    next_val(out eos, out by, ref s);
                                    if (eos) throw new Exception("Bad Number in File");
                                    next_val(out eos, out tx, ref s);
                                    if (eos) throw new Exception("Bad Number in File");
                                    next_val(out eos, out ty, ref s);
                                    if (eos) throw new Exception("Bad Number in File");
                                    
                                    bezierto(relative, ax, ay, bx, by, tx, ty);

                                    clear_white(out eos, ref s);
                                    if (eos || !isnumeric(s[0]))
                                        eoc = true;
                                }
                            }
                            break;
                        case 'S':
                        case 's':
                            next_val(out eos, out bx, ref s);
                            if (eos) throw new Exception("Bad Number in File");
                            next_val(out eos, out by, ref s);
                            if (eos) throw new Exception("Bad Number in File");
                            next_val(out eos, out tx, ref s);
                            if (eos) throw new Exception("Bad Number in File");
                            next_val(out eos, out ty, ref s);
                            if (eos) throw new Exception("Bad Number in File");

                            if (c == 'S')
                                relative = false;
                            else
                                relative = true;
                            sbezierto(relative, bx, by, tx, ty);

                            clear_white(out eos, ref s);
                            if (!eos && isnumeric(s[0]))
                            {
                                // More values
                                bool eoc = false;
                                while (!eoc)
                                {
                                    next_val(out eos, out bx, ref s);
                                    if (eos) throw new Exception("Bad Number in File");
                                    next_val(out eos, out by, ref s);
                                    if (eos) throw new Exception("Bad Number in File");
                                    next_val(out eos, out tx, ref s);
                                    if (eos) throw new Exception("Bad Number in File");
                                    next_val(out eos, out ty, ref s);
                                    if (eos) throw new Exception("Bad Number in File");
                                    
                                    sbezierto(relative, bx, by, tx, ty);

                                    clear_white(out eos, ref s);
                                    if (eos || !isnumeric(s[0]))
                                        eoc = true;
                                }
                            }
                            break;
                        case 'Q':
                        case 'q':
                            next_val(out eos, out ax, ref s);
                            if (eos) throw new Exception("Bad Number in File");
                            next_val(out eos, out ay, ref s);
                            if (eos) throw new Exception("Bad Number in File");
                            next_val(out eos, out tx, ref s);
                            if (eos) throw new Exception("Bad Number in File");
                            next_val(out eos, out ty, ref s);
                            if (eos) throw new Exception("Bad Number in File");
                            if (c == 'Q')
                                relative = true;
                            else
                                relative = false;
                            qbezierto(relative, ax, ay, tx, ty);

                            clear_white(out eos, ref s);
                            if (!eos && isnumeric(s[0]))
                            {
                                // More values
                                bool eoc = false;
                                while (!eoc)
                                {
                                    next_val(out eos, out ax, ref s);
                                    if (eos) throw new Exception("Bad Number in File");
                                    next_val(out eos, out ay, ref s);
                                    if (eos) throw new Exception("Bad Number in File");
                                    next_val(out eos, out tx, ref s);
                                    if (eos) throw new Exception("Bad Number in File");
                                    next_val(out eos, out ty, ref s);
                                    if (eos) throw new Exception("Bad Number in File");
                                    
                                    qbezierto(relative, bx, by, tx, ty);

                                    clear_white(out eos, ref s);
                                    if (eos || !isnumeric(s[0]))
                                        eoc = true;
                                }
                            }
                            break;

                        case 'T':
                        case 't':
                            next_val(out eos, out tx, ref s);
                            if (eos) throw new Exception("Bad Number in File");
                            next_val(out eos, out ty, ref s);
                            if (eos) throw new Exception("Bad Number in File");
                            if (c == 'T')
                                relative = true;
                            else
                                relative = false;

                            qsbezierto(relative, tx, ty);

                            clear_white(out eos, ref s);
                            if (!eos && isnumeric(s[0]))
                            {
                                // More values
                                bool eoc = false;
                                while (!eoc)
                                {
                                    next_val(out eos, out tx, ref s);
                                    if (eos) throw new Exception("Bad Number in File");
                                    next_val(out eos, out ty, ref s);
                                    if (eos) throw new Exception("Bad Number in File");
                                    
                                    qsbezierto(relative, tx, ty);

                                    clear_white(out eos, ref s);
                                    if (eos || !isnumeric(s[0]))
                                        eoc = true;
                                }
                            }
                            break;

                        case 'A':
                        case 'a':

                            next_val(out eos, out rx, ref s);
                            if (eos) throw new Exception("Bad Number in File");
                            next_val(out eos, out ry, ref s);
                            if (eos) throw new Exception("Bad Number in File");
                            next_val(out eos, out xrot, ref s);
                            if (eos) throw new Exception("Bad Number in File");
                            next_val(out eos, out larf, ref s);
                            if (eos) throw new Exception("Bad Number in File");
                            next_val(out eos, out sf, ref s);
                            if (eos) throw new Exception("Bad Number in File");
                            next_val(out eos, out tx, ref s);
                            if (eos) throw new Exception("Bad Number in File");
                            next_val(out eos, out ty, ref s);
                            if (eos) throw new Exception("Bad Number in File");
                            if (c == 'A')
                                relative = false;
                            else
                                relative = true;
                            ellipticarcto(relative, rx, ry, xrot, (larf > 0.5) ? true : false, (sf > 0.5) ? true : false);
                            clear_white(out eos, ref s);
                            if (!eos && isnumeric(s[0]))
                            {
                                // More values
                                bool eoc = false;
                                while (!eoc)
                                {
                                    next_val(out eos, out rx, ref s);
                                    if (eos) throw new Exception("Bad Number in File");
                                    next_val(out eos, out ry, ref s);
                                    if (eos) throw new Exception("Bad Number in File");
                                    next_val(out eos, out xrot, ref s);
                                    if (eos) throw new Exception("Bad Number in File");
                                    next_val(out eos, out larf, ref s);
                                    if (eos) throw new Exception("Bad Number in File");
                                    next_val(out eos, out sf, ref s);
                                    if (eos) throw new Exception("Bad Number in File");
                                    next_val(out eos, out tx, ref s);
                                    if (eos) throw new Exception("Bad Number in File");
                                    next_val(out eos, out ty, ref s);
                                    if (eos) throw new Exception("Bad Number in File");

                                    ellipticarcto(relative, rx, ry, xrot, (larf > 0.5) ? true : false, (sf > 0.5) ? true : false); 

                                    clear_white(out eos, ref s);
                                    if (eos || !isnumeric(s[0]))
                                        eoc = true;
                                }
                            }
                            break;
                        default:
                            throw new Exception("Bad Flag in path definition");
                    }
                }
            }
            pen_up();
        }

        void read_group(XmlTextReader xtr, bool root)
        {
            while (xtr.Read())
            {
                switch (xtr.NodeType)
                {
                    case XmlNodeType.Element:
                        if (xtr.Name.Equals("g"))
                        {
                            int trfadded = 0;
                            for (int i = 0; i < xtr.AttributeCount; i++)
                            {
                                xtr.MoveToAttribute(i);
                                if (xtr.Name.Equals("transform"))
                                {
                                    if (trf.Count == 0)
                                        trf.Add(new transform(null,xtr.Value));
                                    else
                                        trf.Add(new transform(trf[trf.Count - 1], xtr.Value));
                                    trfadded++;
                                }
                            }
                            read_group(xtr, false);
                            for (int i=0;i<trfadded;i++) trf.RemoveAt(trf.Count - 1);
                        }
                        else if (xtr.Name.Equals("path"))
                        {
                            string tt = null;
                            string pdef = "";
                            for (int i = 0; i < xtr.AttributeCount; i++)
                            {
                                xtr.MoveToAttribute(i);
                                if (xtr.Name.Equals("d")) pdef = xtr.Value;
                                if (xtr.Name.Equals("transform")) tt = xtr.Value;
                            }
                            if ((tt != null) && ( tt.Length > 0))
                            {
                                if (trf.Count == 0)
                                    trf.Add(new transform(null, tt));
                                else
                                    trf.Add(new transform(trf[trf.Count-1],tt));
                                parse(pdef);
                                trf.RemoveAt(trf.Count - 1);
                            }
                            else
                            {
                                parse(pdef);
                            }

                        }
                        break;
                    case XmlNodeType.EndElement:
                        if (xtr.Name.Equals("g"))
                        {
                            if (!root)
                                return;
                        }
                        break;
                }
            }
        }

        public List<cmditem> read()
        {

            XmlTextReader xtr=null;
            error = null;
            valid = false;
            cl = new List<cmditemf>();
            trf = new List<transform>();
            try
            {
                xtr = new XmlTextReader(fn);
                xtr.WhitespaceHandling = WhitespaceHandling.None;
                read_group(xtr,true);
            }
            catch (Exception e)
            {
                error = e.Message;
                valid = false;
            }

            if (xtr != null)
                xtr.Close();

            if (cl.Count > 0)
            {
                double minx, miny, maxx, maxy;
                minx = maxx = cl[0].x;
                miny = maxy = cl[0].y;
                for (int i = 1; i < cl.Count; i++)
                {
                    if (cl[i].x < minx) minx = cl[i].x;
                    if (cl[i].x > maxx) maxx = cl[i].x;
                    if (cl[i].y < miny) miny = cl[i].y;
                    if (cl[i].y > maxy) maxy = cl[i].y;
                }

                double scaley = scaleto / ((maxy - miny)/2);
                double scalex = scaley * aspect;

                List<cmditem> el = new List<cmditem>();
                for (int i = 0; i < cl.Count; i++)
                {
                    double rx = -(cl[i].x - (maxx + minx) / 2) * scalex + posx;
                    double ry = (cl[i].y - (maxy + miny) / 2) * scaley + posy;
                    el.Add(new cmditem(rx,ry,cl[i].down));
                }
                return el;

            }
            else
            {
                return new List<cmditem>();    
            }


        }




    }
}
