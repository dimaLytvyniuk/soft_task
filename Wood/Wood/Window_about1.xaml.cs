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
    /// Логика взаимодействия для Window_about1.xaml
    /// </summary>
    public partial class Window_about1 : Window
    {
        public Window_about1()
        {
            InitializeComponent();
            String path = @"Resources\forest_6.bmp";
            this.Background = new ImageBrush(new BitmapImage(new Uri(path,UriKind.Relative)));
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //this.DialogResult = false;
        }
    }
}
