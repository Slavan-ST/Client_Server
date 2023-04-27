using System.Windows.Forms;
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
using WpfApp;
using System.IO;

namespace WpfAppTEST
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        byte[] arr;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if(openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                image.Source = new BitmapImage(new Uri(openFileDialog.FileName));
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            arr = MyConverter.ConverterFromImage(image.Source as BitmapImage);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            BitmapImage b = new BitmapImage();
            b.BeginInit();
            b.StreamSource = new System.IO.MemoryStream(arr);
            b.EndInit();
            image.Source = b;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                image.Source = new BitmapImage(new Uri(openFileDialog.FileName));
            }
        }
    }
}
