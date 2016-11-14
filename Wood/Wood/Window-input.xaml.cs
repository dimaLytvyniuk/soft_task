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
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
