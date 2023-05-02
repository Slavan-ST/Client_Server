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
        bool AddOrNot;
        public WindowVerifi()
        {
            InitializeComponent();
        }
        public static bool IsNewUser()
        {
            WindowVerifi window = new WindowVerifi();
            window.ShowDialog();
            return window.AddOrNot;
        }

        private void Button_Click(object sender, RoutedEventArgs e)//y
        {
            this.Close();
            AddOrNot = true;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)//n
        {
            this.Close();
            AddOrNot = false;
        }
    }
}
