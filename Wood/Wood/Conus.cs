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
        public override int Set(float R1, float R2, float L)
        {
            int value = base.Set(R1, R2, L);

            if (value == 0)
            {

            }
            else
                return value;

            if (L < ((R2 - R1) / 2))
                return 4;

            this.H = (float) Math.Sqrt((float)(Math.Pow(L, 2) - Math.Pow((R2 - R1) / 2, 2)));
            return 0;
        }

        public override float V()
        {
            float value = (float)(Math.PI * H * (Math.Pow(R1, 2) + Math.Pow(R2, 2) + R2 * R1)) / 3;

            return value;
        }
    }
}
