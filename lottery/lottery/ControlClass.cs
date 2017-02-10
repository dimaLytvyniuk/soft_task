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
            min,
            max;

        bool superBall;

        public ControlClass(int k, bool superBall,int min,int max)
        {
            this.k = k;
            this.superBall = superBall;
            this.min = min;
            this.max = max;
        }

        public bool OpenFile()
        {
            solution = new SolutionLottery(k,superBall,min,max);

            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Text file (*.txt)|*.txt";
            openFileDialog1.FileName = "deafault.txt";

            if (true == openFileDialog1.ShowDialog())
            {
                solution.OpenFile(openFileDialog1.FileName);
            }

            return true;
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

        public void Analiz()
        {
            if (solution != null)
            {
                solution.Analiz();
            }
            else
                MessageBox.Show("Немає даних", "Помилка");
        }
    }
}
