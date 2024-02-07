using LetsPlay_app.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        // declare from classes
        Connection con = new Connection();
        SelectLogin slctLogin = new SelectLogin();

        public Login()
        {
            InitializeComponent();
        }

        // function check email address
        private bool IsEmail (string email)
        {
            string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            Regex regex = new Regex(emailPattern);

            return regex.IsMatch(email);
        }

      
        // login input validation 
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {

            string emailInput = txbEmailLogin.Text;

            if (txbEmailLogin.Text.Length < 3 || psbPasswordLogin.Password.Length < 3)
            {
                MessageBox.Show("invalid input", "info", MessageBoxButton.OK, MessageBoxImage.Information);

            }
            else if (IsEmail(emailInput))
            {
               // MessageBox.Show($"Email: {txbEmailLogin.Text} is valid. ");
                
                // login succesful
                slctLogin.SelectLoginData(txbEmailLogin.Text, psbPasswordLogin.Password);

                // store login email
                UserSession.LoggedInUserEmail = emailInput;

                if (LoginStatus.IsUserLoggedIn == true)
                {
                    //navigate to wallet
                    NavigationService?.Navigate(new Wallet());
                }
            }
            else
            {
                MessageBox.Show("please enter a valid email addres");
            }

        }


        // navigate to register page

        private void btnGoToRegister_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new Register());
        }


        // show hide password 

        private void psbPasswordLogin_PasswordChanged(object sender, RoutedEventArgs e)
        {
            txbShowPasswordLogin.Text = psbPasswordLogin.Password;
        }

        private void btnShowPasswordLogin_Click(object sender, RoutedEventArgs e)
        {
            if (psbPasswordLogin.PasswordChar == '●')
            {
                psbPasswordLogin.PasswordChar = 'a';
                txbShowPasswordLogin.Visibility = Visibility.Visible;
                btnShowPasswordLogin.Content = "hide";
            }
            else
            {
                psbPasswordLogin.PasswordChar = '●';
                txbShowPasswordLogin.Visibility = Visibility.Collapsed;
                btnShowPasswordLogin.Content = "show";

            }
        }
    }
}
