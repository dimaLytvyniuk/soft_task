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
        public int Set(float r1,float r2,float l,timber_type type)
        {
            if (r1 < 0)
                throw new pifagorException("R1 должно быть больше за 0");
            if (r2 < 0)
                throw new pifagorException("R2 должно быть больше за 0");
            if (l < 0)
                throw new pifagorException("L должно быть больше за 0");

            if (r1 < r2 || type == 0)
            {
                this.r1 = r1 / 100;
                this.r2 = r2 / 100;
            }
            else
            {
                this.r1 = r2 / 100;
                this.r2 = r1 / 100;
            }

            if (type == timber_type.conus)
            {
                if (l < (this.r2 - this.r1))
                    throw new pifagorException("L должно быть больше за R2 - R1");

                h = (float)Math.Sqrt(Math.Pow(l, 2) - Math.Pow(r2 - r1, 2));
            }

            this.l = l;
            this.type = type;

            return 0;
        }


        public float V()
        {
            float value = 0;
 
            if (type == timber_type.cilinder)
                value = (float)Math.PI * r1 * r1 * l;
            else
                value = (float)(Math.PI * h * (Math.Pow(r1, 2) + Math.Pow(r2, 2) + r2 * r1)) / 3;// V conus

            return value;
        }

        public override string ToString()
        {
            string result = "";

            result = String.Format("{0,16}см", r1.ToString());

            if (type == 0)
                result += String.Format("{0,16}"," ");
            else
                result += String.Format("{0,16}см", r2.ToString());

            result += String.Format("{0,16}м", l.ToString());

            if (type == 0)
                result += String.Format("{0,10}", "Цилиндр");
            else
                result += String.Format("{0,10}", "Конус");

            return result;
        }
    }

    enum timber_type
    {
        cilinder = 0,
        conus = 1,
    }
}

