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

namespace Wood
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int count = 0,
            previous = -1;
        List<Cilinder> timber_list;
        Cilinder prom;

        public MainWindow()
        {
            InitializeComponent();
            count = 10;
            prom = new Cilinder();
            timber_list = new List<Cilinder>();

            for (int i = 0;i < count;i++)
            {
                NBox.Items.Add(i + 1);
                timber_list.Add(prom);
                prom = new Cilinder();   
            }

        }

        private void textBoxR1_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (Char.IsDigit(e.Text, 0) || (e.Text == "."))
            {
                e.Handled = false;
            }
            else
                e.Handled = true;
        }

        private void NBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            float L = -1, 
                  R1 = -1 , 
                  R2 = -1;

            if (previous != -1)
            {
                if (comboBox_Type.SelectedIndex == 0)
                {
                    prom = new Cilinder();
                }
                else
                    prom = new Conus();

                try
                {
                    L = (float)Convert.ToDouble(textBoxL.Text);
                    R1 = (float)Convert.ToDouble(textBoxR1.Text);
                    R2 = (float)Convert.ToDouble(textBoxR2.Text);

                    prom.Set(L, R1, R2);

                    timber_list[previous] = prom;
                }
                catch (Exception e1)
                {
                    MessageBox.Show("Ви ввели не коректные данные", "Error");
                }
            }

            previous = NBox.SelectedIndex;

            if (previous != -1)
            {
                prom = timber_list[previous];

                textBoxL.Text = prom.L.ToString();
                textBoxR1.Text = prom.R1.ToString();
                textBoxR2.Text = prom.R2.ToString();
            }

        }
    }
}
