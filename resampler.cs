using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EggPainter
{
    class resampler
    {
        List<cmditem> input;
        List<cmditem> output;

        public resampler(List<cmditem> inp)
        {
            input = inp;
            output = new List<cmditem>();
        }

        public void work(double dist, double aspect) {

            bool inpath=false;
            double stepl = 0;
            for (int i=0;i<input.Count;i++) {
                cmditem c = input[i];
            }




        }



    }
}
