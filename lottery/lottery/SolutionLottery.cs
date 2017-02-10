using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Controls;
using System.Windows.Data;

namespace lottery
{
    class SolutionLottery
    {

        List<List<int>> editions;
        Dictionary<int, int> countBalls;
        Dictionary<int, Interval> countIntervals;
        List<int> superBalls;

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
            countBalls = new Dictionary<int, int>();

            if (superBall)
                superBalls = new List<int>();

            for (int i = min; i <= max; i++)
            {
                countBalls.Add(i, 0);
            }
        }

        public bool OpenFile(string fileName)
        {
            using (StreamReader reader = new StreamReader(fileName))
            {
                string str = "";
                List<int> prom;
                while ((str = reader.ReadLine()) != null)
                {
                    prom = Cutting(str).ToList();

                    if (superBall)
                    {
                        editions.Add(new List<int>());

                        for (int i = 0; i < k; i++)
                        {
                            editions[countOfEditions].Add(prom[i]);
                        }
                        superBalls.Add(prom[k]);
                    }
                    else
                        editions.Add(prom);
                    
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
                    countBalls[editions[i][j]]++;
                }
            }
        }

        public void DisplaySumImov(DataGrid sumImovGrid)
        {
            var col = new DataGridTextColumn();
            col.Header = "Номер шару ";
            col.Binding = new Binding(string.Format("Key"));
            sumImovGrid.Columns.Add(col);
            col = new DataGridTextColumn();
            col.Header = "Кількість випадань";
            col.Binding = new Binding("Value");
            sumImovGrid.Columns.Add(col);

            List<KeyValuePair<int, int>> prom = countBalls.ToList<KeyValuePair<int, int>>();

             for (int i = 0; i < max; i++)
             {
                 sumImovGrid.Items.Add(prom[i]);
             }
        }

        public void DisplayEditiions(DataGrid sumImovGrid)
        {

            for (int i = 0; i < k; i++)
            {
                var col = new DataGridTextColumn();
                col.Header = "Число " + (i + 1);
                col.Binding = new Binding(string.Format("[{0}]", i));
                sumImovGrid.Columns.Add(col);
            }

            var col_1 = new DataGridTextColumn();
            col_1.Header = "Cума ";
            col_1.Binding = new Binding(string.Format("[{0}]", k));
            sumImovGrid.Columns.Add(col_1);

            for (int i = 0; i < countOfEditions; i++)
            {
                sumImovGrid.Items.Add(editions[i]);
            }
        }

        private void CountSumEditions()
        {
            int sum;

            for (int i = 0;i < countOfEditions;i++)
            {
                sum = 0;

                for (int j = 0; j < k;j++)
                {
                    sum += countBalls[editions[i][j]];
                }

                editions[i].Add(sum);
            }
        }

        private Charactiks CountCharactiks()
        {
            Charactiks charactiks = new Charactiks();
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

                for (int j = min; j <= max; j++)
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
                    if (copyBalls[editions[i][j]] < charactiks.MinInterval)
                        interval.Less++;
                    else if (copyBalls[editions[i][j]] > charactiks.MaxInterval)
                        interval.More++;
                    else
                        interval.InInterval++;

                    copyBalls[editions[i][j]]++;
                }

                countIntervals.Add(i, interval);
            }

            return charactiks;
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
            Dictionary<int, int> countLess = new Dictionary<int, int>();
            Dictionary<int, int> countMore = new Dictionary<int, int>();
            Dictionary<int, int> countIn = new Dictionary<int, int>();
            Charactiks moreChare,
                       lessChare,
                       inChare;
            List<int> lessBalls = new List<int>(),
                moreBalls = new List<int>(),
                inBalls = new List<int>();

            Charactiks charactiks = CountCharactiks();
            CountSumEditions();

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

            moreChare.MX = Math.Round(moreChare.MX,MidpointRounding.AwayFromZero);
            lessChare.MX = Math.Round(lessChare.MX, MidpointRounding.AwayFromZero);
            inChare.MX = Math.Round(inChare.MX, MidpointRounding.AwayFromZero);

            SortBalls(charactiks,ref lessBalls,ref moreBalls,ref inBalls);
        }

        private void SortBalls(Charactiks characticks, ref List<int> lessBalls,ref List<int> moreBalls, ref List<int> inBalls)
        {
            for (int i = min; i <= max; i++)
            {
                if (countBalls[i] < characticks.MinInterval)
                    lessBalls.Add(i);
                else if (countBalls[i] > characticks.MaxInterval)
                    moreBalls.Add(i);
                else
                    inBalls.Add(i);
            }
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
