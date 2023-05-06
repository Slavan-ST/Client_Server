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
using WpfApp.ViewModels;

namespace WpfApp.Views
{
    /// <summary>
    /// Логика взаимодействия для PageLoginSearch.xaml
    /// </summary>
    public partial class PageLoginSearch : Page
    {
        public PageLoginSearch()
        {
            InitializeComponent();
            DataContext = new PageLoginSearchViewModel();
        }

        private void Ellement_SizeChanged(object sender, SizeChangedEventArgs e)
        {

            Panel panel = sender as Panel;
            int columnCount = panel.Children.Count;

            double pwidth = panel.ActualWidth - panel.Children.Count * ((panel.Children[0] as TextBlock).Margin.Right + (panel.Children[0] as TextBlock).Margin.Left) ;

            for (int i = 0; i < columnCount; i++)
            {
                if (panel.Children[i] != null)
                {
                    if ((panel.Children[i] as TextBlock).Width != Double.NaN)
                    {
                        (panel.Children[i] as TextBlock).Width = pwidth / columnCount;
                    }
                }
            }
        }
    }
}
