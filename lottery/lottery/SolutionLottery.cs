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

        List<List<int>> editions,//зберігає старі тиражі
                        newEdition;//зберігає згенеровані тиражі

        Dictionary<int, int> countBalls,//зберігає кількість випадань кожного шару
                             countBallsGen,
                             countBallsLess,
                             countBallsMore,
                             countBallsIn;

        Dictionary<int, Interval> countIntervals;//зберігає інтервали кожного тиражу
        List<int> superBalls;//зберігає супер кульки
        Charactiks sumCharact;

        int k,//кількість шарів у тиражі
            minV,//мінімальний номер кульки
            maxV,//максимальний номер кульки
            countOfEditions,//к-сть тиражів
            basicNum,//частина тиражів, які беруться за базові 
            startN,
            endN;

        bool superBall;//чи використовується суперкулька

        /*
         * SolutionLottery - конструктор класу
         */
        public SolutionLottery(int k, bool superBall, int minV, int maxV,int startN,int endN)
        {
            this.k = k;
            this.superBall = superBall;
            this.minV = minV;
            this.maxV = maxV;
            this.startN = startN;
            this.endN = endN;

            editions = new List<List<int>>();
            countBalls = new Dictionary<int, int>();

            if (superBall)
                superBalls = new List<int>();

            for (int i = minV; i <= maxV; i++)
            {
                countBalls.Add(i, 0);
            }
        }

        /*
         * OpenFile - считує дані з файлу
         * fileName - ім'я файлу
         */
        public bool OpenFile(string fileName)
        {
            List<List<int>> fileEdition = new List<List<int>>();

            using (StreamReader reader = new StreamReader(fileName))
            {
                string str = "";
                List<int> prom;

                //циклу ситування рядок за рядком
                while ((str = reader.ReadLine()) != null)
                {
                    prom = Cutting(str).ToList();//виклик ф-ції Cutting та перетворення повернутого массиву у список

                    if (superBall)//виконується якщо використовується суперкулька
                    {
                        fileEdition.Add(new List<int>());//створення списку у editions

                        //k - кульок додається у editions, суперкулька до supeBalls
                        for (int i = 0; i < k; i++)
                        {
                            fileEdition[countOfEditions].Add(prom[i]);
                        }
                        superBalls.Add(prom[k]);
                    }
                    else
                        fileEdition.Add(prom);

                    countOfEditions++;
                }
            }

            if (startN < countOfEditions && startN > 0 && endN > 1 && endN <= countOfEditions)
            {
                for (int i = startN - 1; i < endN - 1;i++)
                {
                    editions.Add(fileEdition[i]);
                }

                countOfEditions = editions.Count;
            }
            else
                return false;

            basicNum = countOfEditions / 8;//за базові береться 1/8 від кульок
            CountOfHit();//підрахунок подань кожної кульки
            CountSumEditions();

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
                stringSeparators = new string[] { ",", " + мегакулька: " };
            }
            else
                stringSeparators = new string[] { "," };

            string[] words = str.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < size; i++)
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
            for (int i = 0; i < countOfEditions; i++)
            {
                for (int j = 0; j < k; j++)
                {
                    countBalls[editions[i][j]]++;
                }
            }
        }

        public void DisplaySumImov(DataGrid sumImovGrid)
        {
            sumImovGrid.ItemsSource = null;

            var col = new DataGridTextColumn();
            col.Header = "Номер шару ";
            col.Binding = new Binding(string.Format("Key"));
            sumImovGrid.Columns.Add(col);
            col = new DataGridTextColumn();
            col.Header = "Кількість випадань";
            col.Binding = new Binding("Value");
            sumImovGrid.Columns.Add(col);

            List<KeyValuePair<int, int>> prom = countBalls.ToList<KeyValuePair<int, int>>();

            for (int i = 0; i < maxV; i++)
            {
                sumImovGrid.Items.Add(prom[i]);
            }

            using (StreamWriter writer = new StreamWriter("sort_balls.txt", false))
            {
                foreach (KeyValuePair<int, int> promball in countBalls)
                {
                    writer.WriteLine(String.Format("{0,5}{1,12}", promball.Key, promball.Value));
                }
            }

        }

        public bool DisplaySumImovGen(DataGrid sumImovGrid)
        {
            if (countBallsGen != null)
            {
                sumImovGrid.ItemsSource = null;

                var col = new DataGridTextColumn();
                col.Header = "Номер шару ";
                col.Binding = new Binding(string.Format("Key"));
                sumImovGrid.Columns.Add(col);
                col = new DataGridTextColumn();
                col.Header = "Кількість випадань";
                col.Binding = new Binding("Value");
                sumImovGrid.Columns.Add(col);

                List<KeyValuePair<int, int>> prom = countBallsGen.ToList<KeyValuePair<int, int>>();

                for (int i = 0; i < maxV; i++)
                {
                    sumImovGrid.Items.Add(prom[i]);
                }

                return true;
            }
            else
                return false;
        }

        public void DisplayEditiions(DataGrid sumImovGrid)
        {
            sumImovGrid.ItemsSource = null;

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

            for (int i = 0,size = editions.Count; i < size; i++)
            {
                sumImovGrid.Items.Add(editions[i]);
            }
        }

        public bool DisplayEditionsGen(DataGrid sumImovGrid)
        {
            if (newEdition != null)
            {
                sumImovGrid.ItemsSource = null;

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

                for (int i = 0, size = newEdition.Count; i < size; i++)
                {
                    sumImovGrid.Items.Add(newEdition[i]);
                }

                return true;
            }
            else
                return false;
        }

        public void PrintToFileStart()
        {
            using (StreamWriter writer = new StreamWriter("sort_balls_start.txt", false))
            {
                foreach (KeyValuePair<int, int> promball in countBalls)
                {
                    writer.WriteLine(String.Format("{0,5}{1,12}", promball.Key, promball.Value));
                }
            }

            using (StreamWriter writer = new StreamWriter("editions_start.txt", false))
            {
                string str = "";

                foreach (List<int> promball in editions)
                {
                    for (int i = 0; i < k; i++)
                        str += String.Format("{0,4}", promball[i]);

                    str += String.Format("{0,9}", promball[k]);

                    writer.WriteLine(str);
                    str = "";
                }
            }
        }

        private void CountSumEditions()
        {
            int sum;

            for (int i = 0; i < countOfEditions; i++)
            {
                sum = 0;

                for (int j = 0; j < k; j++)
                {
                    sum += countBalls[editions[i][j]];
                }

                editions[i].Add(sum);
            }

            sumCharact = new Charactiks(0, 0);

            for (int i = 0; i < countOfEditions; i++)
            {
                sumCharact.MX += editions[i][k];
            }

            sumCharact.MX /= countOfEditions;

            for (int i = 0; i < countOfEditions;i++)
            {
                sumCharact.SigmaX += Math.Pow(editions[i][k] - sumCharact.MX, 2);
            }

            sumCharact.SigmaX /= countOfEditions;

            sumCharact.SigmaX = Math.Sqrt(sumCharact.SigmaX);
        }

        private Charactiks CountCharactiks()
        {
            Charactiks charactiks = new Charactiks();
            Interval interval;

            countIntervals = new Dictionary<int, Interval>();
            Dictionary<int, int> copyBalls = new Dictionary<int, int>();
            Dictionary<int, int> countHit = new Dictionary<int, int>();

            for (int i = minV; i <= maxV; i++)
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

            for (int i = basicNum; i < countOfEditions; i++)
            {
                interval = new Interval(0, 0, 0);
                countHit = new Dictionary<int, int>();

                for (int j = minV; j <= maxV; j++)
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

        private Charactiks PromCharacticks(Dictionary<int, int> countHit)
        {
            int n = 0;

            Charactiks res = new Charactiks(0, 0);

            foreach (KeyValuePair<int, int> data in countHit)
            {
                n += data.Value;
                res.MX += data.Key * data.Value;
            }

            res.MX /= n;

            foreach (KeyValuePair<int, int> data in countHit)
            {
                res.SigmaX += Math.Pow((data.Key - res.MX), 2) * data.Value;
            }

            res.SigmaX = Math.Sqrt(res.SigmaX / (n - 1));

            return res;
        }

        public void Analiz()
        {
            Dictionary<int, int> countLess = new Dictionary<int, int>();
            Dictionary<int, int> countMore = new Dictionary<int, int>();
            Dictionary<int, int> countIn = new Dictionary<int, int>(),
                countSumInterval = new Dictionary<int, int>();

            List<List<int>> generBalls = new List<List<int>>();
            List<int> sumig = new List<int>();

            Charactiks moreChare,
                       lessChare,
                       inChare;

            Charactiks charactiks = CountCharactiks();

            for (int i = basicNum; i < countOfEditions; i++)
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

            moreChare.MX = Math.Round(moreChare.MX, MidpointRounding.AwayFromZero);
            lessChare.MX = Math.Round(lessChare.MX, MidpointRounding.AwayFromZero);
            inChare.MX = Math.Round(inChare.MX, MidpointRounding.AwayFromZero);

            SortBalls(charactiks, ref generBalls, (int)moreChare.MX, (int)lessChare.MX, (int)inChare.MX);
            /*
            for (int i = 0; i < countOfEditions; i++)
            {
                if (countSumInterval.ContainsKey(editions[i][k]))
                {
                    countSumInterval[editions[i][k]]++;
                }
                else
                {
                    countSumInterval.Add(editions[i][k], 1);
                }
            }

            sumChare = PromCharacticks(countSumInterval);

    */

            GenerateNewEdition(generBalls, new Interval((int)moreChare.MX, (int)lessChare.MX, (int)inChare.MX));

            FrequenceNew((int)lessChare.MX, (int)moreChare.MX, (int)inChare.MX);
        }

        private void SortBalls(Charactiks characticks, ref List<List<int>> resBalls, int moreChare, int lessChare, int inChare)
        {
            for (int i = 0; i < k; i++)
                resBalls.Add(new List<int>());

            for (int i = minV; i <= maxV; i++)
            {
                if (countBalls[i] < characticks.MinInterval)
                {
                    for (int j = 0; j < lessChare && j < lessChare; j++)
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

            for (int i = 0; i < lessChare - 1; i++)
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

        public void GenerateNewEdition(List<List<int>> generBalls, Interval intervals)
        {
            int varLess = FindC(intervals.Less, generBalls[intervals.Less - 1].Count),
                varIn = FindC(intervals.InInterval, generBalls[intervals.InInterval + intervals.Less - 1].Count),
                varMore = FindC(intervals.More, generBalls[intervals.InInterval + intervals.Less + intervals.More - 1].Count),
                n = varLess * varIn * varMore;

            newEdition = new List<List<int>>();
            List<List<int>> promEdition = new List<List<int>>();

            for (int i = 0; i < n; i++)
                promEdition.Add(new List<int>());

            if (intervals.Less > 1)
            {
                for (int q = 0; q < n / varLess; q++)
                {
                    for (int i = 0, countIter = 0; i < generBalls[0].Count; i++)
                    {
                        for (int j = 0; j < FindC(intervals.Less - 1, generBalls[intervals.Less - 1].Count - i - 1); j++, countIter++)
                        {
                            promEdition[countIter].Add(generBalls[0][i]);
                        }
                    }
                }

                for (int i = 1; i < intervals.Less; i++)
                {
                    for (int j = 0; j < generBalls[i].Count; j++)
                    {
                        for (int l = 0, prom = 0; l < n; l++)
                        {
                            if ((generBalls[i][j] > promEdition[i][l - 1]) && (promEdition[i].Count == i))
                            {
                                prom = FindC(intervals.Less - i - 1, generBalls[intervals.Less - 1].Count - j - 1);

                                for (int t = 0; t < prom; t++)
                                {
                                    promEdition[l + t].Add(generBalls[i][j]);
                                }

                                l += prom - 1; 

                                while (generBalls[i][j] > promEdition[i][l - 1])
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
                        promEdition[j + i * count].Add(generBalls[0][i]);
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
                            promEdition[countIter].Add(generBalls[intervals.Less][i]);
                            countIter++;
                        }
                    }
                }

                for (int i = intervals.Less + 1, countI = intervals.Less + intervals.InInterval - 1, countIter = 0, current = 0; i < countI; i++)
                {
                    countIter = 0;

                    while (countIter < n)
                    {
                        current = generBalls[i - intervals.Less].IndexOf(promEdition[countIter][i - 1]);

                        for (int l = current + 1, countL = generBalls[i].Count; l < countL; l++)
                        {
                            for (int q = 0, countQ = FindC(intervals.InInterval - i + intervals.Less - 1, generBalls[intervals.InInterval + intervals.Less - 1].Count - l - 1); q < countQ && countIter < n; q++)
                            {
                                promEdition[countIter].Add(generBalls[i][l]);
                                countIter++;
                            }
                        }
                    }
                }

                for (int i = 0, num = 0; i < n; i++)
                {
                    num = promEdition[i][intervals.Less + intervals.InInterval - 2];
                    i--;

                    for (int j = generBalls[intervals.Less + intervals.InInterval - 1].IndexOf(num) + 1; j < generBalls[intervals.Less + intervals.InInterval - 1].Count; j++)
                    {
                        i++;
                        promEdition[i].Add(generBalls[intervals.Less + intervals.InInterval - 1][j]);
                    }
                }

            }
            else
            {
                for (int i = 0; i < varIn; i++)
                {
                    for (int j = 0, count = n / varIn; j < count; j++)
                    {
                        promEdition[j + i * count].Add(generBalls[intervals.Less][i]);
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
                    for (int l = 0,pos = i * varIn; l < varLess; l++)
                    {
                        for (int j = 0; j < varIn; j++)
                        {
                            promEdition[j + pos].Add(generBalls[k - 1][i]);
                        }

                        pos += varMore * varIn; 
                    }
                }
            }

            
            for (int i = 0, sum = 0,size = promEdition.Count; i < size; i++)
            {
                for (int j = 0; j < k; j++)
                    sum += countBalls[promEdition[i][j]];

                if (sum < sumCharact.MaxInterval && sum > sumCharact.MinInterval)
                    newEdition.Add(promEdition[i]);

                sum = 0;
            }
            
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

        private int PartFact(int start, int end)
        {
            int res = 1;

            for (int i = start; i <= end; i++)
                res *= i;

            return res;
        }

        private void FrequenceNew(int lessChare, int moreChare, int inChare)
        {
            countBallsIn = new Dictionary<int, int>();
            countBallsLess = new Dictionary<int, int>();
            countBallsMore = new Dictionary<int, int>();
            countBallsGen = new Dictionary<int, int>();
            List<int> freeBalls = new List<int>();
            List<int> sortLess = new List<int>(),
                      sortMore = new List<int>(),
                      sortIn = new List<int>();

            for (int i = 0, n = newEdition.Count; i < n; i++)
            {
                for (int j = 0; j < lessChare; j++)
                {
                    if (countBallsLess.ContainsKey(newEdition[i][j]))
                        countBallsLess[newEdition[i][j]]++;
                    else
                        countBallsLess.Add(newEdition[i][j], 1);
                }

                for (int j = lessChare; j < inChare + lessChare; j++)
                {
                    if (countBallsIn.ContainsKey(newEdition[i][j]))
                        countBallsIn[newEdition[i][j]]++;
                    else
                        countBallsIn.Add(newEdition[i][j], 1);
                }

                for (int j = inChare + lessChare; j < k; j++)
                {
                    if (countBallsMore.ContainsKey(newEdition[i][j]))
                        countBallsMore[newEdition[i][j]]++;
                    else
                        countBallsMore.Add(newEdition[i][j], 1);
                }
            }

            sortLess = SortByValue(countBallsLess);

            for (int i = sortLess.Count - 1; i > sortLess.Count - 1 - lessChare; i--)
                freeBalls.Add(sortLess[i]);

            sortIn = SortByValue(countBallsIn);

            for (int i = sortIn.Count - 1; i > sortIn.Count - 1 - inChare; i--)
                freeBalls.Add(sortIn[i]);

            sortMore = SortByValue(countBallsMore);

            for (int i = sortMore.Count - 1; i > sortMore.Count - 1 - moreChare; i--)
                freeBalls.Add(sortMore[i]);


            using (StreamWriter writer = new StreamWriter("sort_balls_after.txt", false))
            {
                writer.WriteLine("Less :");
                string str = "";

                for (int i = 0; i < countBallsLess.Count; i++)
                {
                    str = String.Format("{0,5}{1,12}", sortLess[i], countBallsLess[sortLess[i]]);
                    countBallsGen.Add(sortLess[i], countBallsLess[sortLess[i]]);
                    writer.WriteLine(str);
                }

                writer.WriteLine();
                writer.WriteLine("In :");

                for (int i = 0; i < countBallsIn.Count; i++)
                {
                    str = String.Format("{0,5}{1,12}", sortIn[i], countBallsIn[sortIn[i]]);
                    countBallsGen.Add(sortIn[i], countBallsIn[sortIn[i]]);
                    writer.WriteLine(str);
                }

                writer.WriteLine();
                writer.WriteLine("More :");

                for (int i = 0; i < countBallsMore.Count; i++)
                {
                    str = String.Format("{0,5}{1,12}", sortMore[i], countBallsMore[sortMore[i]]);
                    countBallsGen.Add(sortMore[i], countBallsMore[sortMore[i]]);
                    writer.WriteLine(str);
                }
            }
            
            for (int i = 0, sum = 0, size = newEdition.Count; i < size; i++)
            {
                for (int j = 0; j < lessChare; j++)
                    sum += countBallsLess[newEdition[i][j]];

                for (int j = lessChare; j < inChare + lessChare; j++)
                    sum += countBallsIn[newEdition[i][j]];

                for (int j = inChare + lessChare; j < k; j++)
                    sum += countBallsMore[newEdition[i][j]];

                newEdition[i].Add(sum);

                sum = 0;
            }
            
            MessageBox.Show("Кількість випадань кульок у згенерованих тиражах записано у файл sort_balls_after.txt","Complete");
        }

        private List<int> SortByValue(Dictionary<int, int> input)
        {
            List<int> res = new List<int>();
            List<int> sort = new List<int>();

            foreach (KeyValuePair<int, int> prom in input)
            {
                sort.Add(prom.Value);
            }

            for (int i = 0, size = sort.Count - 1; i < size; i++)
            {
                bool swap = false;

                for (int j = 0; j < size - i; j++)
                {
                    if (sort[j] > sort[j + 1])
                    {
                        int prom = sort[j];
                        sort[j] = sort[j + 1];
                        sort[j + 1] = prom;
                        swap = true;
                    }
                }

                if (!swap)
                    break;
            }

            for (int i = 0; i < sort.Count; i++)
            {
                foreach (KeyValuePair<int, int> prom in input)
                {
                    if (prom.Value == sort[i] && (!res.Contains(prom.Key)))
                    {
                        res.Add(prom.Key);
                        break;
                    }
                }
            }

            return res;
        }

        public void PrintToNewBalls(string name)
        {
            //QSortNew(0, newEdition.Count - 1);

            using (StreamWriter writer = new StreamWriter(name, false))
            {
                for (int i = 0, size = newEdition.Count; i < size; i++)
                {
                    string str = "";

                    for (int j = 0; j < k; j++)
                    {
                        str += String.Format("{0,4},", newEdition[i][j]);
                    }

                    str += String.Format("{0,9}", newEdition[i][k]);

                    writer.WriteLine(str);
                }
            }
        }

        private void QSortNew(int start, int finish)
        {
            List<int> prom;

            try
            {
                if (finish - start > 20000)
                {

                    int wall = start,
                        opor = finish;

                    for (int i = start; i < finish; i++)
                    {
                        if (newEdition[i][k] <= newEdition[opor][k])
                        {
                            prom = newEdition[wall];
                            newEdition[wall] = newEdition[i];
                            newEdition[i] = prom;
                            wall++;
                        }
                    }

                    prom = newEdition[wall];
                    newEdition[wall] = newEdition[opor];
                    newEdition[opor] = prom;

                    if (wall != 0 && (wall - 1 != 0))
                        QSortNew(start, wall - 1);

                    if (wall != finish && finish != 0)
                        QSortNew(wall + 1, finish);
                }
                else if (finish - start != 0)
                {
                    for (int i = 0; i < (finish - start); i++)
                    {
                        bool swap = false;

                        for (int j = start; j < finish - i; j++)
                        {
                            if (newEdition[j][k] > newEdition[j + 1][k])
                            {
                                prom = newEdition[j];
                                newEdition[j] = newEdition[j + 1];
                                newEdition[j + 1] = prom;
                                swap = true;
                            }
                        }

                        if (!swap)
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                int y = 0;
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
            sigmax,
            minInterval,
            maxInterval;

        public Charactiks(double mx,double sigmax)
        {
            this.mx = mx;
            this.sigmax = sigmax;
            this.maxInterval = 0;
            this.minInterval = 0;
        }

        public double MX
        {
            get { return mx; }
            set {  mx = value;
                minInterval = mx - sigmax;
                maxInterval = mx + sigmax;
            }
        }

        public double SigmaX
        {
            get { return sigmax; }
            set { sigmax = value;
                minInterval = mx - sigmax;
                maxInterval = mx + sigmax;
            }
        }

        public double MinInterval
        {
            get { return minInterval; }
            set { minInterval = value; }
        }


        public double MaxInterval
        {
            get { return maxInterval; }
            set { maxInterval = value; }
        }
    }
}
