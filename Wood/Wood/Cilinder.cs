using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Wood
{
    class Cilinder
    {
        protected float R1 = 0,R2 = 0,L = 0;

        public Cilinder()
        {
            
        }

        /*
         * возращает коды ошибок
         * 1 - R1 <= 0
         * 2 - R2 <= 0
         * 3 - L <= 0
        */
        public virtual int Set(float R1,float R2,float L)
        {
            if (R1 <= 0)
                return 1;
            if (R2 <= 0)
                return 2;
            if (L <= 0)
                return 3;

            if (R1 < R2)
            {
                this.R1 = R1;
                this.R2 = R2;
            }
            else
            {
                this.R1 = R2;
                this.R2 = R1;
            }
            this.L = L;

            return 0;
        }

        public virtual float V()
        {
            float value = (float)Math.PI * R2 * R2 * L;
            return value;
        }
    }
}
