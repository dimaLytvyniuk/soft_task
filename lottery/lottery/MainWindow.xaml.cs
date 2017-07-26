using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace lottery
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ControlClass controlObject;
        Charactiks sumChar;

        public MainWindow()
        {
            InitializeComponent();
            comboBox.SelectedIndex = 1;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            controlObject = new ControlClass(0.ToString(), true, 0.ToString(), 0.ToString(), textBox_startN.Text,textBox_endN.Text);

            if (controlObject.OpenFile())
            {
                sumChar = controlObject.SumChare;
                textBox_minSum.Text = String.Format("{0,4:#00.00}", sumChar.MinInterval);
                textBox_maxSum.Text = String.Format("{0,4:#00.00}", sumChar.MaxInterval);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }

        private void button_disEditions_Click(object sender, RoutedEventArgs e)
        {
            if (controlObject != null)
                controlObject.DisplayEditions();
        }

        private void button_countBalls_Click(object sender, RoutedEventArgs e)
        {
            if (controlObject != null)
                controlObject.DisplaySumImov();
        }

        private void button_countBalls_Gen_Click(object sender, RoutedEventArgs e)
        {
            if (controlObject != null)
                controlObject.DisplaySunImovGen();
        }

        private void button_disEditions_Gen_Click(object sender, RoutedEventArgs e)
        {
            if (controlObject != null)
                controlObject.DisplayEditionsGen();
        }

        private void button_printFileEdi_Click(object sender, RoutedEventArgs e)
        {
            if (controlObject != null)
                controlObject.PrintToFileStart();
        }

        private void button_Gen_Click(object sender, RoutedEventArgs e)
        {
            /*
            if (controlObject != null)
            {
                
                try
                {
                    sumChar.MinInterval = Convert.ToDouble(textBox_minSum.Text);
                    sumChar.MaxInterval = Convert.ToDouble(textBox_maxSum.Text);
                    controlObject.SumChare = sumChar;
                    controlObject.Analiz(comboBox.SelectedIndex);
                    controlObject.PrintNewFile();
                }
                catch (Exception e1)
                {
                    MessageBox.Show("Некоpектні вхідні дані", "Помилка");
                }
            }
            */
            sumChar.MinInterval = Convert.ToDouble(textBox_minSum.Text);
            sumChar.MaxInterval = Convert.ToDouble(textBox_maxSum.Text);
            controlObject.SumChare = sumChar;
            controlObject.Analiz(comboBox.SelectedIndex);
            controlObject.PrintNewFile();
        }

        private void textBox_A_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (Char.IsDigit(e.Text, 0))
            {
                e.Handled = false;
            }
            else
                e.Handled = true;
        }

        private void button_Update_Click(object sender, RoutedEventArgs e)
        {
            if (controlObject.ChangeInputV(textBox_startN.Text,textBox_endN.Text))
            {
                sumChar = controlObject.SumChare;
                textBox_minSum.Text = String.Format("{0,4:#00.00}", sumChar.MinInterval);
                textBox_maxSum.Text = String.Format("{0,4:#00.00}", sumChar.MaxInterval);
            }
        }
    }
}
