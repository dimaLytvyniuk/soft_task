using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Windows.Controls;
using System.Windows;

namespace lottery
{
    class ControlClass
    {
        SolutionLottery solution;
        int k,
            minV,
            maxV,
            startN,
            endN;

        bool superBall,
            init;

        public ControlClass(String k, bool superBall,string minV,string maxV,string startN,string endN)
        {
            this.superBall = superBall;
            init = true;

            try
            {
                this.k = Convert.ToInt32(k);
            }
            catch(Exception e)
            {
                MessageBox.Show("Некоректне k", "Помилка");
                init = false;
            }

            try
            {
                this.minV = Convert.ToInt32(minV);
            }
            catch (Exception e)
            {
                MessageBox.Show("Некоректне мінімальне значення кульки", "Помилка");
                init = false;
            }

            try
            {
                this.maxV = Convert.ToInt32(maxV);
            }
            catch (Exception e)
            {
                MessageBox.Show("Некоректне максимальне значення кульки", "Помилка");
                init = false;
            }

            try
            {
                this.startN = Convert.ToInt32(startN);
            }
            catch (Exception e)
            {
                MessageBox.Show("Некоректний номер початкового тиражу", "Помилка");
                init = false;
            }

            try
            {
                this.endN = Convert.ToInt32(endN);
            }
            catch (Exception e)
            {
                MessageBox.Show("Некоректний номер кінцевого тиражу", "Помилка");
                init = false;
            }
        }

        public bool OpenFile()
        {
            if (init)
            {
                solution = new SolutionLottery(k, superBall, minV, maxV,startN,endN);

                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.Filter = "Text file (*.txt)|*.txt";
                openFileDialog1.FileName = "deafault.txt";

                if (true == openFileDialog1.ShowDialog())
                {
                    if (solution.OpenFile(openFileDialog1.FileName))
                    {
                        MessageBox.Show("Читання виконано", "Complete");
                        return true;
                    }
                    else
                    {
                        solution = null;
                        init = false;
                        MessageBox.Show("Некоректні вхідні дані", "Complete");
                    }
                }
                else
                    return false;

                return false;
            }
            else
            {
                MessageBox.Show("Некоректні вхідні дані", "Помилка");
                return false;
            }
        }

        public void DisplaySumImov()
        {
            Window_table_sumImov win_1 = new Window_table_sumImov();

            if (solution != null)
            {
                solution.DisplaySumImov(win_1.sumImovGrid);

                win_1.Show();
            }
            else
                MessageBox.Show("Немає даних", "Помилка");

        }

        public void DisplayEditions()
        {
            Window_Editions win_1 = new Window_Editions();

            if (solution != null)
            {
                solution.DisplayEditiions(win_1.editionsGrid);

                win_1.Show();
            }
            else
                MessageBox.Show("Немає даних", "Помилка");

        }

        public void DisplayEditionsGen()
        {
            Window_Editions win_1 = new Window_Editions();

            if (solution != null)
            {
                if (solution.DisplayEditionsGen(win_1.editionsGrid))
                    win_1.Show();
                else
                    MessageBox.Show("Нові вибірки не згенеровані", "Помилка");

            }
            else
                MessageBox.Show("Немає даних", "Помилка");
        }

        public void DisplaySunImovGen()
        {
            Window_table_sumImov win_1 = new Window_table_sumImov();

            if (solution != null)
            {
                if (solution.DisplaySumImovGen(win_1.sumImovGrid))
                    win_1.Show();
                else
                    MessageBox.Show("Нові вибірки не згенеровані", "Помилка");
            }
            else
                MessageBox.Show("Немає даних", "Помилка");
        }

        public void Analiz()
        {
            if (solution != null)
            {
                solution.Analiz();

                MessageBox.Show("Генерацію закінчено","Complete");
            }
            else
                MessageBox.Show("Немає даних", "Помилка");
        }

        public void PrintNewFile()
        {
            if (solution != null)
            {
                solution.PrintToNewBalls("new_editions.txt");
                MessageBox.Show("Згенеровані тиражі записані у файл new_editions.txt","Complete");
            }
            else
                MessageBox.Show("Немає даних", "Помилка");
        }

        public void PrintToFileStart()
        {
            if (solution != null)
            {
                solution.PrintToFileStart();
            }
            else
                MessageBox.Show("Немає даних", "Помилка");
        }
    }
}
