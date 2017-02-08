using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Controls;

namespace lottery
{
    class SolutionLottery
    {

        List<List<int>> editions;
        Dictionary<int, int> balls;
        Dictionary<int, int> countOfHits;
        Dictionary<int, Interval> countIntervals;

        int k,
            min,
            max,
            countOfEditions,
            basicNum;

        bool superBall;

        public SolutionLottery(int k, bool superBall,int min,int max)
        {
            this.k = k;
            this.superBall = superBall;
            this.min = min;
            this.max = max;

            editions = new List<List<int>>();
            balls = new Dictionary<int, int>();

            for (int i = min; i <= max; i++)
            {
                balls.Add(i, 0);
            }
        }

        public bool OpenFile(string fileName)
        {
            using (StreamReader reader = new StreamReader(fileName))
            {
                string str = "";

                while ((str = reader.ReadLine()) != null)
                {
                    editions.Add(Cutting(str).ToList());
                    countOfEditions++;
                }
            }

            basicNum = countOfEditions / 8;
            CountOfHit();

            return true;
        }

        private int[] Cutting(string str)
        {
            int size = k;

            if (superBall)
                size++;

            int[] res = new int[size];
            string[] stringSeparators;

            if (superBall)
            {
                stringSeparators = new string[] { ","," + мегакулька: "};
            }
            else
                stringSeparators = new string[] { "," };

            string[] words = str.Split(stringSeparators,StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < size;i++)
            {
                try
                {
                    res[i] = Convert.ToInt32(words[i]);
                }
                catch (Exception e)
                {

                }
            }

            return res;
        }

        private void CountOfHit()
        {
            for (int i=0; i < countOfEditions;i++)
            {
                for (int j = 0; j < k; j++)
                {
                    balls[editions[i][j]]++;
                }
            }
        }

        public void DisplaySumImov(DataGrid sumImovGrid)
        {
            sumImovGrid.ItemsSource = balls;
        }

        private int CountSumEditions()
        {
            int res = 0;

            return res;
        }

        private void CountCharactiks()
        {
            Charactiks charactiks;
            Interval interval;

            countIntervals = new Dictionary<int, Interval>();
            Dictionary<int, int> copyBalls = new Dictionary<int, int>();
            Dictionary<int, int> countHit = new Dictionary<int, int>();

            for (int i = min; i <= max; i++)
            {
                copyBalls.Add(i, 0);
            }

            for (int i = 0; i < basicNum; i++)
            {
                for (int j = 0; j < k; j++)
                {
                    copyBalls[editions[i][j]]++;
                }
            }

            for (int i = basicNum; i < countOfEditions;  i++)
            {
                interval = new Interval(0, 0, 0);
                countHit = new Dictionary<int, int>();

                for (int j = min; j < max; j++)
                {
                    if (countHit.ContainsKey(copyBalls[j]))
                    {
                        countHit[copyBalls[j]]++;
                    }
                    else
                    {
                        countHit.Add(copyBalls[j], 1);
                    }
                }

                charactiks = PromCharacticks(countHit);

                for (int j = 0; j < k; j++)
                {
                    if (editions[i][j] < charactiks.MinInterval)
                        interval.Less++;
                    else if (editions[i][j] > charactiks.MaxInterval)
                        interval.More++;
                    else
                        interval.InInterval++;

                    copyBalls[editions[i][j]]++;
                }

                countIntervals.Add(i, interval);
            }
        }

        private Charactiks PromCharacticks(Dictionary<int,int> countHit)
        {
            int n = 0;

            Charactiks res = new Charactiks(0,0);

            foreach(KeyValuePair<int,int> data in countHit)
            {
                n += data.Value;
                res.MX += data.Key * data.Value;
            }

            res.MX /= n;

            foreach (KeyValuePair<int, int> data in countHit)
            {
                res.SigmaX += Math.Pow((data.Key - res.MX),2) * data.Value;
            }

            res.SigmaX = Math.Sqrt(res.SigmaX / (n - 1));

            return res;
        }

        public void Analiz()
        {
            CountCharactiks();

            Dictionary<int, int> countLess = new Dictionary<int, int>();
            Dictionary<int, int> countMore = new Dictionary<int, int>();
            Dictionary<int, int> countIn = new Dictionary<int, int>();
            Charactiks moreChare,
                       lessChare,
                       inChare;

            for (int i = basicNum; i < countOfEditions;i++)
            {
                if (countLess.ContainsKey(countIntervals[i].Less))
                {
                    countLess[countIntervals[i].Less]++;
                }
                else
                {
                    countLess.Add(countIntervals[i].Less, 1);
                }

                if (countMore.ContainsKey(countIntervals[i].More))
                {
                    countMore[countIntervals[i].More]++;
                }
                else
                {
                    countMore.Add(countIntervals[i].More, 1);
                }

                if (countIn.ContainsKey(countIntervals[i].InInterval))
                {
                    countIn[countIntervals[i].InInterval]++;
                }
                else
                {
                    countIn.Add(countIntervals[i].InInterval, 1);
                }
            }

            moreChare = PromCharacticks(countMore);
            lessChare = PromCharacticks(countLess);
            inChare = PromCharacticks(countIn);
        }
    }

    struct Interval  
    {
        int more,
            less,
            inInterval;

        public Interval(int more,int less,int inInterval)
        {
            this.more = more;
            this.less = less;
            this.inInterval = inInterval;
        }

        public int More
        {
            get { return more; }
            set { more = value; }
        }

        public int Less
        {
            get { return less; }
            set { less = value; }
        }

        public int InInterval
        {
            get { return inInterval; }
            set { inInterval = value; }
        }
    }

    struct Charactiks
    {
        double mx,
            sigmax;

        public Charactiks(double mx,double sigmax)
        {
            this.mx = mx;
            this.sigmax = sigmax;
        }

        public double MX
        {
            get { return mx; }
            set {  mx = value; }
        }

        public double SigmaX
        {
            get { return sigmax; }
            set { sigmax = value; }
        }

        public double MinInterval
        {
            get { return mx - sigmax; }
        }


        public double MaxInterval
        {
            get { return mx + sigmax; }
        }
    }
}
