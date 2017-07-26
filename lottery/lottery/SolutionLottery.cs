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

        string paramsfile = "";

        int k,//кількість шарів у тиражі
            minV,//мінімальний номер кульки
            maxV,//максимальний номер кульки
            countOfEditions,//к-сть тиражів
            basicNum,//частина тиражів, які беруться за базові 
            startN,
            endN;

        bool superBall;//чи використовується суперкулька
        int[] parn;
        int[,] decades;

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

                string start = reader.ReadLine();

                string[] words = start.Split(' ');

                try
                {
                    k = Convert.ToInt32(words[0]);
                    minV = Convert.ToInt32(words[1]);
                    maxV = Convert.ToInt32(words[2]);

                    for (int i = minV; i <= maxV; i++)
                    {
                        countBalls.Add(i, 0);
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("Некоректні вхідні дані","Error");
                    return false;
                }

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
                for (int i = startN - 1; i < endN;i++)
                {
                    editions.Add(fileEdition[i]);
                }

                countOfEditions = editions.Count;

                parn = new int[countOfEditions];
                decades = new int[countOfEditions,5];

            }
            else
                return false;

            basicNum = countOfEditions / 4;//за базові береться 1/8 від кульок
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
            DateTime date = DateTime.Now;
            string fileName = date.ToString("MM_HH_mm_ss") + "sort_ball" + ".txt";
            List<KeyValuePair<int, int>> prom = countBalls.ToList<KeyValuePair<int, int>>();

            for (int i = 0; i < maxV; i++)
            {
                sumImovGrid.Items.Add(prom[i]);
            }

            using (StreamWriter writer = new StreamWriter(fileName, false))
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

            col_1 = new DataGridTextColumn();
            col_1.Header = "Парні ";
            col_1.Binding = new Binding(string.Format("[{0}]", k + 1));
            sumImovGrid.Columns.Add(col_1);

            for (int i = 0; i < 5;i++)
            {
                col_1 = new DataGridTextColumn();
                col_1.Header = 10 * i;
                col_1.Binding = new Binding(string.Format("[{0}]",i + k + 2));
                sumImovGrid.Columns.Add(col_1);
            }

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

                col_1 = new DataGridTextColumn();
                col_1.Header = "Парні ";
                col_1.Binding = new Binding(string.Format("[{0}]", k + 1));
                sumImovGrid.Columns.Add(col_1);

                for (int i = 0; i < 5; i++)
                {
                    col_1 = new DataGridTextColumn();
                    col_1.Header = 10 * i;
                    col_1.Binding = new Binding(string.Format("[{0}]", i + k + 2));
                    sumImovGrid.Columns.Add(col_1);
                }

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
            DateTime date = DateTime.Now;
            using (StreamWriter writer = new StreamWriter(date.ToString("MM_HH_mm_ss") + "sort_balls_start" + ".txt", false))
            {
                foreach (KeyValuePair<int, int> promball in countBalls)
                {
                    writer.WriteLine(String.Format("{0,5}{1,12}", promball.Key, promball.Value));
                }
            }

            using (StreamWriter writer = new StreamWriter(date.ToString("MM_HH_mm_ss") + "editions_start" + ".txt", false))
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

                    if ((editions[i][j] % 2) == 0)
                        parn[i]++;

                    if (editions[i][j] / 10 == 0)
                        decades[i, 0]++;
                    else if (editions[i][j] / 20 == 0)
                        decades[i, 1]++;
                    else if (editions[i][j] / 30 == 0)
                        decades[i, 2]++;
                    else if (editions[i][j] / 40 == 0)
                        decades[i, 3]++;
                    else
                        decades[i, 4]++;
                }

                editions[i].Add(sum);
                editions[i].Add(parn[i]);

                for (int j = 0; j < 5; j++)
                    editions[i].Add(decades[i,j]);

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

        public void Analiz(int type)
        {
            Dictionary<int, int> countLess = new Dictionary<int, int>();
            Dictionary<int, int> countMore = new Dictionary<int, int>();
            Dictionary<int, int> countIn = new Dictionary<int, int>(),
                countSumInterval = new Dictionary<int, int>();

            List<List<int>> generBalls = new List<List<int>>();
            List<int> sumig = new List<int>();

            /*
            Charactiks moreChare,
                       lessChare,
                       inChare;
                       */

            int moreChare = 0,
                lessChare = 0,
                inChare = 0;

            Charactiks charactiks = CountCharactiks();
            paramsfile = "StartN = " + startN + " EndN= " + endN + " minSum = " + sumCharact.MinInterval + " maxSum = " + sumCharact.MaxInterval;
             
            /*
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
            */



            switch(type)
            {
                case 0:
                    moreChare = 2;
                    lessChare = 1;
                    inChare = 3;
                    SortBalls(charactiks, ref generBalls, moreChare, lessChare, inChare);
                    GenerateNewEdition132(generBalls, new Interval(moreChare, lessChare, inChare));
                    break;
                case 1:
                    moreChare = 1;
                    lessChare = 1;
                    inChare = 4;
                    SortBalls(charactiks, ref generBalls, moreChare, lessChare, inChare);
                    GenerateNewEdition141(generBalls, new Interval(moreChare, lessChare, inChare));
                    break;
                case 2:
                    moreChare = 2;
                    lessChare = 0;
                    inChare = 4;
                    SortBalls(charactiks, ref generBalls, moreChare, lessChare, inChare);
                    GenerateNewEdition042(generBalls, new Interval(moreChare, lessChare, inChare));
                    break;
            }

            FrequenceNew(lessChare, moreChare, inChare);
        }

        private void SortBalls(Charactiks characticks, ref List<List<int>> resBalls, int moreChare, int lessChare, int inChare)
        {
            for (int i = 0; i < k; i++)
                resBalls.Add(new List<int>());

            for (int i = minV; i <= maxV; i++)
            {
                if (countBalls[i] < characticks.MinInterval)
                {
                    for (int j = 0; j < lessChare; j++)
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
                for (int size = resBalls[i].Count, j = size - 1; (j > size - moreChare + i - inChare - lessChare); j--)
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

        
        public void GenerateNewEdition141(List<List<int>> generBalls, Interval intervals)
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

        public void GenerateNewEdition132(List<List<int>> generBalls, Interval intervals)
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
                for (int l = 0, countIter = 0; l < generBalls[0].Count; l++)
                {
                    for (int i = 0; i < generBalls[intervals.InInterval + intervals.Less].Count; i++)
                    {
                        int countProbablies = (generBalls[intervals.InInterval + intervals.Less].Count - i);

                        for (int j = i + 1; j <= countProbablies + i; j++)
                        {
                            for (int q = 0; q < varIn; q++)
                            {
                                promEdition[countIter].Add(generBalls[intervals.Less + intervals.InInterval][i]);
                                promEdition[countIter].Add(generBalls[k - 1][j]);
                                countIter++;
                            }
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < varMore; i++)
                {
                    for (int l = 0, pos = i * varIn; l < varLess; l++)
                    {
                        for (int j = 0; j < varIn; j++)
                        {
                            promEdition[j + pos].Add(generBalls[k - 1][i]);
                        }

                        pos += varMore * varIn;
                    }
                }
            }


            for (int i = 0, sum = 0, size = promEdition.Count; i < size; i++)
            {
                for (int j = 0; j < k; j++)
                    sum += countBalls[promEdition[i][j]];

                if (sum < sumCharact.MaxInterval && sum > sumCharact.MinInterval)
                    newEdition.Add(promEdition[i]);

                sum = 0;
            }

        }

        public void GenerateNewEdition042(List<List<int>> generBalls, Interval intervals)
        {
            int varIn = FindC(intervals.InInterval, generBalls[intervals.InInterval - 1].Count),
                varMore = FindC(intervals.More, generBalls[intervals.InInterval + intervals.Less + intervals.More - 1].Count),
                n = varIn * varMore;

            newEdition = new List<List<int>>();
            List<List<int>> promEdition = new List<List<int>>();

            for (int i = 0; i < n; i++)
                promEdition.Add(new List<int>());

            if (intervals.InInterval > 1)
            {
                for (int q = 0, countIter = 0; q < varMore; q++)
                {
                    for (int i = 0; i < generBalls[0].Count; i++)
                    {
                        for (int j = 0; j < FindC(intervals.InInterval - 1, generBalls[intervals.InInterval - 2].Count - i); j++)
                        {
                            promEdition[countIter].Add(generBalls[0][i]);
                            countIter++;
                        }
                    }
                }

                for (int i = 1, countI = intervals.InInterval - 1, countIter = 0, current = 0; i < countI; i++)
                {
                    countIter = 0;

                    while (countIter < n)
                    {
                        current = generBalls[i].IndexOf(promEdition[countIter][i - 1]);

                        for (int l = current + 1, countL = generBalls[i].Count; l < countL; l++)
                        {
                            for (int q = 0, countQ = FindC(intervals.InInterval - i - 1, generBalls[intervals.InInterval - 1].Count - l - 1); q < countQ && countIter < n; q++)
                            {
                                promEdition[countIter].Add(generBalls[i][l]);
                                countIter++;
                            }
                        }
                    }
                }

                for (int i = 0, num = 0; i < n; i++)
                {
                    num = promEdition[i][intervals.InInterval - 2];
                    i--;

                    for (int j = generBalls[intervals.InInterval - 1].IndexOf(num) + 1; j < generBalls[intervals.InInterval - 1].Count; j++)
                    {
                        i++;
                        promEdition[i].Add(generBalls[intervals.InInterval - 1][j]);
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
                for (int i = 0, countIter = 0; i < generBalls[intervals.InInterval].Count; i++)
                {
                    int countProbablies = (generBalls[intervals.InInterval].Count - i);

                    for (int j = 1; j <= countProbablies; j++)
                    {
                        for (int q = 0; q < varIn; q++)
                        {
                            promEdition[countIter].Add(generBalls[intervals.InInterval][i]);
                            promEdition[countIter].Add(generBalls[k - 1][j]);
                            countIter++;
                        }
                    }
                }
            }
            else
            {
            }


            for (int i = 0, sum = 0, size = promEdition.Count; i < size; i++)
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
            DateTime date = DateTime.Now;
            string fileName = date.ToString("MM_HH_mm_ss") + "sort_balls_after" + ".txt";
            for (int i = 0, size = newEdition.Count; i < size; i++)
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


            using (StreamWriter writer = new StreamWriter(fileName, false))
            {
                writer.WriteLine(paramsfile);
                writer.WriteLine("Less :");
                string str = "";

                for (int i = minV;i <= maxV;i++)
                {
                    countBallsGen.Add(i, 0);
                }

                for (int i = 0; i < countBallsLess.Count; i++)
                {
                    str = String.Format("{0,5}{1,12}", sortLess[i], countBallsLess[sortLess[i]]);
                    countBallsGen[sortLess[i]] = countBallsLess[sortLess[i]];
                    writer.WriteLine(str);
                }

                writer.WriteLine();
                writer.WriteLine("In :");

                for (int i = 0; i < countBallsIn.Count; i++)
                {
                    str = String.Format("{0,5}{1,12}", sortIn[i], countBallsIn[sortIn[i]]);
                    countBallsGen[sortIn[i]] = countBallsIn[sortIn[i]];
                    writer.WriteLine(str);
                }

                writer.WriteLine();
                writer.WriteLine("More :");

                for (int i = 0; i < countBallsMore.Count; i++)
                {
                    str = String.Format("{0,5}{1,12}", sortMore[i], countBallsMore[sortMore[i]]);
                    countBallsGen[sortMore[i]] = countBallsMore[sortMore[i]];
                    writer.WriteLine(str);
                }
            }

            for (int i = 0, size = newEdition.Count; i < size; i++)
            {
                int[] decs = new int[5];
                int paired = 0;
                int sum = 0;

                for (int j = 0; j < k; j++)
                {
                    sum += countBalls[newEdition[i][j]];

                    if ((newEdition[i][j] % 2) == 0)
                        paired++;

                    if (newEdition[i][j] / 10 == 0)
                        decs[0]++;
                    else if (newEdition[i][j] / 20 == 0)
                        decs[1]++;
                    else if (newEdition[i][j] / 30 == 0)
                        decs[2]++;
                    else if (newEdition[i][j] / 40 == 0)
                        decs[3]++;
                    else
                        decs[4]++;
                }

                newEdition[i].Add(sum);
                newEdition[i].Add(paired);

                for (int j = 0; j < 5; j++)
                    newEdition[i].Add(decs[j]);
            }

            MessageBox.Show("Кількість випадань кульок у згенерованих тиражах записано у файл " + fileName,"Complete");
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
            using (StreamWriter writer = new StreamWriter(name, false))
            {
                for (int i = 0, size = newEdition.Count; i < size; i++)
                {
                    string str = "";

                    
                    for (int j = 0; j < k - 1;j++)
                    {
                        for (int l = 0; l < k - j - 1; l++)
                        {
                            if (newEdition[i][l] > newEdition[i][l + 1])
                            {
                                int prom = newEdition[i][l];
                                newEdition[i][l] = newEdition[i][l + 1];
                                newEdition[i][l + 1] = prom;
                            }
                        }
                    }
                    
                    for (int j = 0; j < k; j++)
                    {
                        str += String.Format("{0},", newEdition[i][j]);
                    }

                    str += String.Format("{0,9}", newEdition[i][k]);

                    for (int j = k + 1, sizeJ = newEdition[i].Count; j < sizeJ; j++)
                        str += String.Format("{0},", newEdition[i][j]);

                    writer.WriteLine(str);
                }
            }
        }

        private void AddParnDecadesNewGen()
        {

        }

        public Charactiks SumCharat
        {
            get { return sumCharact; }
            set { sumCharact = value; }
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