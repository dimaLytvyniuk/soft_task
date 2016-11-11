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
        int count ,
            previous;

        List<Cilinder> timber_list;
        Cilinder prom;
        bool curr_error;

        public MainWindow()
        {
            InitializeComponent();
            count = 10;
            previous = -1;
            curr_error = false; 
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
            if (Char.IsDigit(e.Text, 0) || (e.Text == ","))
            {
                e.Handled = false;
            }
            else
                e.Handled = true;
        }

        private void NBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!curr_error)
            {
                int code_error;

                float r1 = 0,
                      r2 = 0,
                      l = 0;
                timber_type type = 0;

                if (previous != -1)
                {
                    prom = new Cilinder();

                    try
                    {
                        r1 = (float)Convert.ToDouble(textBoxR1.Text);
                        r2 = (float)Convert.ToDouble(textBoxR2.Text);
                        l = (float)Convert.ToDouble(textBoxL.Text);
                        type = (timber_type)comboBox.SelectedIndex;

                        prom.Set(r1, r2, l, type);
                        timber_list[previous] = prom;
                    }
                    catch (FormatException e1)
                    {
                        curr_error = true;
                        NBox.SelectedIndex = previous;
                        MessageBox.Show("Не коректный формат чисел\n Коректная запись: 1,234", "Ошыбка");
                    }
                    catch (pifagorException e1)
                    {
                        curr_error = true;
                        NBox.SelectedIndex = previous;
                        MessageBox.Show(e1.Message, "Ошыбка");
                    }

                }

                if (!curr_error)
                {
                    previous = NBox.SelectedIndex;

                    if (previous != -1)
            {
                        prom = timber_list[previous];
                textBoxL.Text = prom.L.ToString();
                textBoxR1.Text = prom.R1.ToString();
                textBoxR2.Text = prom.R2.ToString();
                        comboBox.SelectedIndex = (int)prom.Type;
                    }
                }
            }
            else
                curr_error = false;
            
        }
    }
}
