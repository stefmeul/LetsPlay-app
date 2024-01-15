using LetsPlay_app.Pages;
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
         //   LoginStatus.IsUserLoggedIn = false;
        }

        private void sidebar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           NavButton selected = (NavButton)sidebar.SelectedItem;

            // redirect to pages depending on loginstatus
            if (selected != null)
            {

                if ((selected == (NavButton)lsbWallet) && (LoginStatus.IsUserLoggedIn == false))
                {
                    MessageBox.Show("you need to be logged in to see this page");

                    navframe.Navigate(new Uri("Pages/Login.xaml", UriKind.Relative));
                }

                else
                {
                    navframe.Navigate(selected.Navlink);
                }
            }

            if (selected == (NavButton)lsbLogout)
            {
                MessageBoxResult result = MessageBox.Show("Confirm?", "Logout", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    LoginStatus.IsUserLoggedIn = false;
                    navframe.Navigate(new Uri("Pages/Login.xaml", UriKind.Relative));
                }
            }

            // show/hide login or logout in navbar
            if (LoginStatus.IsUserLoggedIn == true) 
            {
                lsbLogin.Visibility=Visibility.Collapsed;
                lsbLogout.Visibility = Visibility.Visible;
            }
            if (LoginStatus.IsUserLoggedIn == false)
            {
                lsbLogin.Visibility = Visibility.Visible;
                lsbLogout.Visibility = Visibility.Collapsed;
            }

            

         



        }
    }
}
