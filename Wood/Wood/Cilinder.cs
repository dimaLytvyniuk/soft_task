using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Wood
{
    class Cilinder
    {
        float r1 = 0,r2 = 0,l = 0,h = 0;
        timber_type type = 0;

        public Cilinder()
        {
          
        }

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

        public timber_type Type
        {
            get
            {
                return type; 
            }
        }

        /*
         * возращает коды ошибок
         * 1 - R1 < 0
         * 2 - R2 < 0
         * 3 - L < 0
         * 4 - L < (R2 - R1) / 2
        */
        public virtual int Set(float r1,float r2,float l,timber_type type)
        {
            if (r1 < 0)
                return 1;
            if (r2 < 0)
                return 2;
            if (l < 0)
                return 3;

            if (type == timber_type.conus)
            {
                if (l < ((r2 - r1) / 2))
                    throw new pifagorException("L должно быть больше за (R2 - R1) / 2");

                h = (float)Math.Sqrt((float)(Math.Pow(l, 2) - Math.Pow((r2 - r1) / 2, 2)));
            }

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
            this.type = type;

            return 0;
        }

        public virtual float V()
        {
            float value = 0;
 
            if (type == timber_type.cilinder)
                value = (float)Math.PI * r2 * r2 * l;
            else
                value = (float)(Math.PI * h * (Math.Pow(r1, 2) + Math.Pow(r2, 2) + r2 * r1)) / 3;// V conus

            return value;
        }
    }

    enum timber_type
    {
        cilinder = 0,
        conus = 1,
    }
}

