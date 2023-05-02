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
using WpfApp.Models;
using WpfApp.ViewModels;

namespace WpfApp.Views
{
    /// <summary>
    /// Логика взаимодействия для PageNewUser.xaml
    /// </summary>
    public partial class PageNewUser : Page
    {
        public PageNewUser(string Id)
        {
            DataContext = new PageNewUserViewModel(Id);
            InitializeComponent();
        }
        void PreviewTextInputInt32(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !MyValidation.Validated(e.Text);
        }
    }
}
