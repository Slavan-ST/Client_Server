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

namespace WpfApp
{
    /// <summary>
    /// Логика взаимодействия для WindowVerifi.xaml
    /// </summary>
    public partial class WindowVerifi : Window
    {
        MainWindow mainWindow = null;   
        public WindowVerifi(MainWindow window)
        {
            InitializeComponent();
            this.mainWindow = window;
        }

        private void Button_Click(object sender, RoutedEventArgs e)//y
        {
           mainWindow.AddOrNot(true);
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)//n
        {
           mainWindow.AddOrNot(false);
            this.Close();
        }
    }
}
