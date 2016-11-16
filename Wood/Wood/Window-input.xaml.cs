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
using System.Windows.Shapes;

namespace Wood
{
    /// <summary>
    /// Логика взаимодействия для Window_input.xaml
    /// </summary>
    public partial class Window_input : Window
    {
        int count;

        public int Count
        {
            get
            {
                return count;
            }
        }

        public Window_input()
        {
            InitializeComponent();
            String path = "F:\\C\\DIMA\\C#\\soft_task\\Wood\\Wood\\forest_6.bmp";
            this.Background = new ImageBrush(new BitmapImage(new Uri(path)));
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void textBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (Char.IsDigit(e.Text, 0))
            {
                e.Handled = false;
            }
            else
                e.Handled = true;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }
    }
}
