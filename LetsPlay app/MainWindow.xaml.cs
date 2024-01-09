﻿using System;
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

namespace LetsPlay_app
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();


        }

        private void sidebar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           NavButton selected = (NavButton)sidebar.SelectedItem;

            if (selected != null)
            {

                if (selected.Navlink == new Uri("Pages/Wallet.xaml", UriKind.Relative) && !LoginStatus.IsUserLoggedIn)
                {
                    MessageBox.Show("you need to be logged in to see this page");

                    navframe.Navigate(new Uri("Pages/Login.xaml", UriKind.Relative));
                }
                else
                {
                    navframe.Navigate(selected.Navlink);
                }
            }

        }
    }
}
