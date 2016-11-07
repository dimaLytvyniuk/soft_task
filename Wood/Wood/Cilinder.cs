using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Wood
{
    class Cilinder
    {
        protected float r1 = 0,r2 = 0,l = 0;

        public float R1
        {
            get
            {
                return r1;
            }
        }

        public float R2
        {
            get
            {
                return r2;
            }
        }

        public float L
        {
            get
            {
                return l;
            }
        }

        public Cilinder()
        {
          
        }

        /*
         * возращает коды ошибок
         * 1 - R1 < 0
         * 2 - R2 < 0
         * 3 - L < 0
        */
        public virtual int Set(float r1,float r2,float l)
        {
            if (r1 < 0)
                return 1;
            if (r2 < 0)
                return 2;
            if (l < 0)
                return 3;

            if (r1 < r2)
            {
                this.r1 = r1;
                this.r2 = r2;
            }
            else
            {
                this.r1 = r2;
                this.r2 = r1;
            }
            this.l = l;

            return 0;
        }

        public virtual float V()
        {
            float value = (float)Math.PI * r2 * r2 * l;
            return value;
        }
    }
}
