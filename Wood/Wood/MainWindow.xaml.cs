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
using System.IO;

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
            String path = @"Resources\forest_6.bmp";
            this.Background = new ImageBrush(new BitmapImage(new Uri(path, UriKind.Relative)));
            Window_about1 win_1 = new Window_about1();

            if (win_1.ShowDialog() == true)
            {
                if (win_1.DialogResult == true)
                {
                    bool res = false;
                    while (!res)
                    {
                        Window_input win2 = new Window_input();

                        if (win2.ShowDialog() == true)
                        {
                            if (win2.DialogResult == true)
                            {
                                try
                                {
                                    count = Convert.ToInt32(win2.textBox.Text);

                                    if (count == 0)
                                    {
                                        MessageBox.Show("Количество колод должно быть больше 0", "Ошибка");
                                    }
                                    else
                                        res = true;
                                }
                                catch (Exception e)
                                {
                                    MessageBox.Show("Не коректное количество колод", "Ошибка");
                                }
                            }
                        }
                        else
                            Environment.Exit(0);
                    }
                }
            }
            else
                Environment.Exit(0);

            //count = 10;
            previous = -1;
            prom = new Cilinder();
            timber_list = new List<Cilinder>();

            for (int i = 0;i < count;i++)
            {
                NBox.Items.Add(i + 1);
                timber_list.Add(prom);
                prom = new Cilinder();   
            }

            NBox.SelectedIndex = 0;
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

        private void button_Click(object sender, RoutedEventArgs e)
        {
            string file_name = "result.txt";
            double V = 0;
            float r1 = 0,
                  r2 = 0,
                  l = 0;
            timber_type type = 0;

            previous = NBox.SelectedIndex;

            if (previous != -1)
            {
                prom = new Cilinder();

                try
                {
                    r1 = (float)Convert.ToDouble(textBoxR1.Text);
                    r2 = (float)Convert.ToDouble(textBoxR2.Text);
                    l = (float)Convert.ToDouble(textBoxL.Text);
                    type = (timber_type)comboBox_Type.SelectedIndex;

                    prom.Set(r1, r2, l, type);
                    timber_list[previous] = prom;
                }
                catch (FormatException e1)
                {
                    curr_error = true;
                    NBox.SelectedIndex = previous;
                    MessageBox.Show("Не коректный формат чисел\n Коректная запись: 1,234", "Ошибка");
                }
                catch (pifagorException e1)
                {
                    curr_error = true;
                    NBox.SelectedIndex = previous;
                    MessageBox.Show(e1.Message, "Ошибка");
                }

            }

            if (!curr_error)
            {
                using (StreamWriter writer = new StreamWriter(file_name, true))
                {
                    double prom_v;

                    DateTime date = DateTime.Now;

                    string str = String.Format("Создано {0:g}", date);
                    writer.Write(str);
                    writer.WriteLine();

                    str = String.Format("{0,4}", "№") + String.Format("{0,19}", "Радиус №1") + String.Format("{0,19}", "Радиус №2") + String.Format("{0,18}", "Длина") + String.Format("{0,12}", "Тип") + String.Format("{0,34}", "Объем") + "\n";
                    writer.Write(str);
                    writer.WriteLine();

                    for (int i = 0; i < count; i++)
                    {
                        str = "";

                        prom_v = timber_list[i].V();
                        V += prom_v;

                        str += String.Format("{0,4}", i + 1);
                        str += timber_list[i].ToString();
                        str += String.Format("{0,30:#0.0000} м^3", prom_v);
                        writer.Write(str);
                        writer.WriteLine();
                    }

                    str = "ВСЕГО" + String.Format("{0,97:#0.0000} м^3", V);
                    writer.Write(str);

                    writer.WriteLine();
                    writer.WriteLine();
                }

                MessageBox.Show("Результат записан в файл result.txt", "Complete");
            }
        }

        private void button_V_simple_Click(object sender, RoutedEventArgs e)
        {
            double V = 0;
            float r1 = 0,
                  r2 = 0,
                  l = 0;
            timber_type type = 0;

            previous = NBox.SelectedIndex;

            if (previous != -1)
            {
                prom = new Cilinder();

                try
                {
                    r1 = (float)Convert.ToDouble(textBoxR1.Text);
                    r2 = (float)Convert.ToDouble(textBoxR2.Text);
                    l = (float)Convert.ToDouble(textBoxL.Text);
                    type = (timber_type)comboBox_Type.SelectedIndex;

                    prom.Set(r1, r2, l, type);
                    timber_list[previous] = prom;
                }
                catch (FormatException e1)
                {
                    curr_error = true;
                    NBox.SelectedIndex = previous;
                    MessageBox.Show("Не коректный формат чисел\n Коректная запись: 1,234", "Ошибка");
                }
                catch (pifagorException e1)
                {
                    curr_error = true;
                    NBox.SelectedIndex = previous;
                    MessageBox.Show(e1.Message, "Ошибка");
                }

            }

            if (!curr_error)
            {
                for (int i = 0; i < count; i++)
                {
                    V += timber_list[i].V();
                }

                string message_res = "Общий объем колод: " + String.Format("{0,34:#0.0000} м^3", V);
                MessageBox.Show(message_res, "Complete");
            }
        }

        private void comboBox_Type_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBox_Type.SelectedIndex == 0)
            {
                image.Source = new BitmapImage(new Uri(@"Resources\cilinder.bmp", UriKind.Relative));
                textBoxR2.IsEnabled = false;
            }
            else
            {
                image.Source = new BitmapImage(new Uri(@"Resources\conus.bmp", UriKind.Relative));
                textBoxR2.IsEnabled = true;
            }
        }

        private void button_change_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;

            bool res = false;
            while (!res)
            {
                Window_input win2 = new Window_input();

                if (win2.ShowDialog() == true)
                {
                    if (win2.DialogResult == true)
                    {
                        try
                        {
                            count = Convert.ToInt32(win2.textBox.Text);

                            if (count == 0)
                            {
                                MessageBox.Show("Количество колод должно быть больше 0", "Ошибка");
                            }
                            else
                                res = true;

                            prom = new Cilinder();
                            timber_list = null;
                            timber_list = new List<Cilinder>();
                            curr_error = true;
                            NBox.Items.Clear();


                            for (int i = 0; i < count; i++)
                            {
                                NBox.Items.Add(i + 1);
                                timber_list.Add(prom);
                                prom = new Cilinder();
                            }

                            curr_error = false;
                            this.Visibility = Visibility.Visible;
                            NBox.SelectedIndex = 0;
                        }
                        catch (Exception e1)
                        { 
                            MessageBox.Show("Не коректное количество колод", "Ошибка");
                        }
                    }
                }
                else
                    Environment.Exit(0);
            }
        }

        private void NBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!curr_error)
            {
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
                        type = (timber_type)comboBox_Type.SelectedIndex;

                        prom.Set(r1, r2, l, type);
                        timber_list[previous] = prom;
                    }
                    catch (FormatException e1)
                    {
                        curr_error = true;
                        NBox.SelectedIndex = previous;
                        MessageBox.Show("Не коректный формат чисел\n Коректная запись: 1,234", "Ошибка");
                    }
                    catch (pifagorException e1)
                    {
                        curr_error = true;
                        NBox.SelectedIndex = previous;
                        MessageBox.Show(e1.Message, "Ошибка");
                    }
                    catch (OverflowException e2)
                    {
                        curr_error = true;
                        NBox.SelectedIndex = previous;
                        MessageBox.Show("Слишком большое число", "Ошибка");
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
                        comboBox_Type.SelectedIndex = (int)prom.Type;
                    }
                }
            }
            else
                curr_error = false;
            
        }
    }
}
