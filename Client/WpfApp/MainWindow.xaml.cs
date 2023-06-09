﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
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
using System.Text.RegularExpressions;
using WpfApp.Views;

namespace WpfApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Page page;
        static public MainWindow mw;
        public MainWindow()
        {
            InitializeComponent();
            NavigationService.GetNavigationService(NavigateFrame);
            //NavigateFrame.Navigate(new PageLoginSearch());
            NavigateFrame.Navigate(page = new PageLogin());
            mw = this;
        }
    }
}
