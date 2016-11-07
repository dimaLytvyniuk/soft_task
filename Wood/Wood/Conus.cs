using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wood
{
    class Conus:Cilinder
    {
        private float H = 0;

        public Conus()
        {

        }

        /*
         *4 - L < (R2 - R1) / 2
        */
        public override int Set(float r1, float r2, float l)
        {
            int value = base.Set(r1, r2, l);

            if (value == 0)
            {

            }
            else
                return value;

            if (l < ((r2 - r1) / 2))
                return 4;

            this.H = (float) Math.Sqrt((float)(Math.Pow(l, 2) - Math.Pow((r2 - r1) / 2, 2)));
            return 0;
        }

        public override float V()
        {
            float value = (float)(Math.PI * H * (Math.Pow(r1, 2) + Math.Pow(r2, 2) + r2 * r1)) / 3;

            return value;
        }
    }
}
