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

        public MainWindow()
        {
            InitializeComponent();
            controlObject = new ControlClass(6,true,1,42);
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            controlObject.OpenFile();
            controlObject.Analiz();
            controlObject.PrintNewFile();

            MessageBox.Show("Complete", "Повідомлення");
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }

        private void button_disEditions_Click(object sender, RoutedEventArgs e)
        {
            controlObject.DisplayEditions();
        }

        private void button_countBalls_Click(object sender, RoutedEventArgs e)
        {
            controlObject.DisplaySumImov();
        }
    }
}
