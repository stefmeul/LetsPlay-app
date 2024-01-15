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

namespace LetsPlay_app.Pages
{
    /// <summary>
    /// Interaction logic for Wallet.xaml
    /// </summary>
    public partial class Wallet : Page
    {
        public Wallet()
        {
            InitializeComponent();
            /*
            if (!LoginStatus.IsUserLoggedIn == true)
            {
                MessageBox.Show("user not logged in, redirect to login");

                //  NavigationService?.Navigate(new Uri("/Pages/Login.xaml", UriKind.Relative));
                //  navframe.Navigate(new Uri("Pages/Login.xaml", UriKind.Relative));
                // this.NavigationService?.Navigate(new Login());
                (App.Current.MainWindow as MainWindow)?.navframe.Navigate(new Uri("/Pages/Login.xaml", UriKind.Relative));

            }
            else
            {
                MessageBox.Show("display user balance");

            }*/
        }

    }
}
