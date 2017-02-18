using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows;

namespace lottery
{
    class SolutionLottery
    {

        List<List<int>> editions,
                        newEdition;

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
            Dictionary<int, int> countIn = new Dictionary<int, int>(),
                countSumIn = new Dictionary<int, int>();

            List<List<int>> generBalls = new List<List<int>>();
            List<int> sumig = new List<int>();

            Charactiks moreChare,
                       lessChare,
                       inChare,
                       sumChare;

            CountSumEditions();
            Charactiks charactiks = CountCharactiks();

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

            SortBalls(charactiks,ref generBalls,(int)moreChare.MX,(int)lessChare.MX,(int)inChare.MX);

            for (int i = 0; i < countOfEditions;i++)
            {
                if (countSumIn.ContainsKey(editions[i][k]))
                {
                    countSumIn[editions[i][k]]++;
                }
                else
                {
                    countSumIn.Add(editions[i][k], 1);
                }
            }

            sumChare = PromCharacticks(countSumIn);

            /*
            for (int i = 0; i < editions.Count; i++)
            {
                bool fl = false;
                fl = editions[i].Contains(15);                
               
            }
            */

            if (lessChare.MX > 1)
            {
                for (int i = 1; i < lessChare.MX; i++)
                    sumig.Add(i);
            }

            if (inChare.MX > 1)
            {
                for (int i = (int)lessChare.MX + 1; i < inChare.MX + lessChare.MX; i++)
                    sumig.Add(i);
            }

            if (moreChare.MX > 1)
            {
                for (int i = (int)lessChare.MX + (int)inChare.MX + 1; i < k; i++)
                    sumig.Add(i);
            }

            GenerateNewEdition(generBalls,sumChare, new Interval((int)moreChare.MX,(int)lessChare.MX,(int)inChare.MX),sumig);

        }

        private void SortBalls(Charactiks characticks, ref List<List<int>> resBalls,int moreChare,int lessChare,int inChare)
        {
            for (int i = 0; i < k;i++)
                resBalls.Add(new List<int>());

            for (int i = min; i <= max; i++)
            {
                if (countBalls[i] < characticks.MinInterval)
                {
                    for (int j = 0; j < lessChare && j < lessChare;j++)
                    {
                        resBalls[j].Add(i);
                    } 
                }
                else if (countBalls[i] > characticks.MaxInterval)
                {
                    for (int j = inChare + lessChare; j < k; j++)
                    {
                        resBalls[j].Add(i);
                    }
                }
                else
                {
                    for (int j = lessChare; j < inChare + lessChare; j++)
                    {
                        resBalls[j].Add(i);
                    }
                }
            }

            for (int i = 0; i < lessChare - 1;i++)
            {
                for (int size = resBalls[i].Count, j = size - 1; (j > size - lessChare + i); j--)
                {
                    resBalls[i].RemoveAt(j);
                }
                        
            }

            for (int i = inChare + lessChare; i < k - 1; i++)
            {
                for (int size = resBalls[i].Count, j = size - 1; (j > size - moreChare + i); j--)
                {
                    resBalls[i].RemoveAt(j);
                }
            }

            for (int i = lessChare; i < inChare + lessChare; i++)
            {
                for (int size = resBalls[i].Count, j = size - 1; (j > size - inChare + i - lessChare); j--)
                {
                    resBalls[i].RemoveAt(j);
                }
            }
        }
        /*
        public void GenerateNewEdition(List<List<int>> generBalls,Charactiks sumCharact)
        {
            int n = 1,
                countIter;

            newEdition = new List<List<int>>();

            for (int i = 0; i < k;i++)
                n *= generBalls[i].Count;

            for (int i = 0; i < n; i++)
                newEdition.Add(new List<int>());

            for (int i = 0; i < k;i++)
            {
                countIter = n / generBalls[i].Count;
                
                for (int j = 0, size = generBalls[i].Count; j < size;j++)
                {
                    for (int l = 0; l < countIter;l++)
                    {
                        if (newEdition[l + countIter * j].Contains(generBalls[i][j]) == false)
                            newEdition[l + countIter * j].Add(generBalls[i][j]);
                    }
                }
            }

            
            for (int i = 0, size = newEdition.Count; i < size;i++)
            {
                if (newEdition[i].Count < k)
                {
                     newEdition.RemoveAt(i);
                     i--;
                     size--;
                }
            }
            

            MessageBox.Show("Count = " + newEdition.Count, "Помилка");
        }
        */

        public void GenerateNewEdition(List<List<int>> generBalls, Charactiks sumCharact, Interval intervals, List<int> sumig)
        {
            int varLess = FindC(intervals.Less, generBalls[intervals.Less - 1].Count),
                varIn = FindC(intervals.InInterval, generBalls[intervals.InInterval + intervals.Less - 1].Count),
                varMore = FindC(intervals.More, generBalls[intervals.InInterval + intervals.Less + intervals.More - 1].Count),
                n = varLess * varIn * varMore;

            newEdition = new List<List<int>>();

            for (int i = 0; i < n; i++)
                newEdition.Add(new List<int>());

            if (intervals.Less > 1)
            {
                for (int q = 0; q < n / varLess; q++)
                {
                    for (int i = 0, countIter = 0; i < generBalls[0].Count; i++)
                    {
                        for (int j = 0; j < FindC(intervals.Less - 1, generBalls[intervals.Less - 1].Count - i - 1); j++, countIter++)
                        {
                            newEdition[countIter].Add(generBalls[0][i]);
                        }
                    }
                }

                for (int i = 1; i < intervals.Less; i++)
                {
                    for (int j = 0; j < generBalls[i].Count; j++)
                    {
                        for (int l = 0, prom = 0; l < n; l++)
                        {
                            if ((generBalls[i][j] > newEdition[i][l - 1]) && (newEdition[i].Count == i))
                            {
                                prom = FindC(intervals.Less - i - 1, generBalls[intervals.Less - 1].Count - j - 1);

                                for (int t = 0; t < prom; t++)
                                {
                                    newEdition[l + t].Add(generBalls[i][j]);
                                }

                                l += prom - 1;//hzzzzzz 

                                while (generBalls[i][j] > newEdition[i][l - 1])
                                    l++;
                            }
                        }
                    }
                }

            }
            else
            {
                for (int i = 0; i < varLess; i++)
                {
                    for (int j = 0, count = n / varLess; j < count; j++)
                    {
                        newEdition[j + i * count].Add(generBalls[0][i]);
                    }
                }
            }

            if (intervals.InInterval > 1)
            {
                for (int q = 0, countIter = 0; q < n / varIn; q++)
                {
                    for (int i = 0; i < generBalls[intervals.Less].Count; i++)
                    {
                        for (int j = 0; j < FindC(intervals.InInterval - 1, generBalls[intervals.InInterval - 1].Count - i); j++)
                        {
                            newEdition[countIter].Add(generBalls[intervals.Less][i]);
                            countIter++;
                        }
                    }
                }

                for (int i = intervals.Less + 1; i < intervals.Less + intervals.InInterval - 1; i++)
                {
                    for (int j = 0; j < generBalls[i].Count; j++)
                    {
                        for (int l = 0, prom = 0, last = 0; l < n; l++)
                        {
                            if (newEdition[l].Count == i)
                            {
                                if ((generBalls[i][j] > newEdition[l][i - 1]))
                                {
                                    prom = FindC(intervals.InInterval - i - 1 + intervals.Less, generBalls[intervals.InInterval - 1].Count - j - 1);
                                    last = newEdition[l][i - 1];

                                    if (prom == 0)
                                        prom = 1;

                                    l--;

                                    for (int t = 0; t < prom; t++)
                                    {
                                        l++;
                                        newEdition[l].Add(generBalls[i][j]);
                                    }

                                    while ((l < n) && (newEdition[l].Count == i) && last == newEdition[l][i - 1])
                                        l++;

                                }
                            }
                        }
                    }
                }

                for (int i = 0, num = 0; i < n; i++)
                {
                    num = newEdition[i][intervals.Less + intervals.InInterval - 2];
                    i--;
                    for (int j = generBalls[intervals.Less + intervals.InInterval - 1].IndexOf(num) + 1; j < generBalls[intervals.Less + intervals.InInterval - 1].Count; j++)
                    {
                        i++;
                        newEdition[i].Add(generBalls[intervals.Less + intervals.InInterval - 1][j]);
                    }
                }

            }
            else
            {
                for (int i = 0; i < varIn; i++)
                {
                    for (int j = 0, count = n / varIn; j < count; j++)
                    {
                        newEdition[j + i * count].Add(generBalls[intervals.Less][i]);
                    }
                }
            }

            if (intervals.More > 1)
            {

            }
            else
            {
                for (int i = 0; i < varMore; i++)
                {
                    for (int j = 0, count = n / varMore; j < count; j++)
                    {
                        newEdition[j + i * count].Add(generBalls[k - 1][i]);
                    }
                }
            }

            for (int i = 0, sum = 0; i < newEdition.Count; i++)
            {
                for (int j = 0; j < k; j++)
                {
                    sum += countBalls[newEdition[i][j]];
                }

                if (sum > sumCharact.MaxInterval || sum < sumCharact.MinInterval)
                {
                    newEdition.RemoveAt(i);
                    i--;
                }

                sum = 0;
            }

            int y = 0;
        }

        private int FindC(int up, int down)
        {
            int res = 0;

            if (down == 0 || up == 0 || up > down)
                res = 0;
            else if ((down - up) > up)
                res = PartFact(down - up + 1, down) / Fact(up);
            else
                res = PartFact(up + 1, down) / Fact(down - up);

            return res;
        }

        private int Fact(int a)
        {
            if (a > 1)
            {
                return a * Fact(a - 1);
            }
            else
                return 1;
        }

        private int PartFact(int start,int end)
        {
            int res = 1;

            for (int i = start; i <= end; i++)
                res *= i;

            return res;
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
