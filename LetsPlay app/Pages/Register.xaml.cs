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
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : Page
    {
        // declare from classes
        insertData insdata = new insertData();
        Connection con = new Connection();

        public Register()
        {
            InitializeComponent();
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {

            string dateRegister = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            string emailInput = txbEmailRegister.Text;

            if (txbNameRegister.Text.Length < 3 || txbEmailRegister.Text.Length < 3 || psbPasswordRegister.Password.Length < 3)
            {
                MessageBox.Show("invalid input", "info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (IsEmail(emailInput))
            {
                // Register user
                insdata.InsertData(txbNameRegister.Text, txbEmailRegister.Text, psbPasswordRegister.Password, dateRegister);
            }
            else
            {
                MessageBox.Show("please enter a valid email addres");
            }

        }

        // function check email address
        private bool IsEmail(string email)
        {
            string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            Regex regex = new Regex(emailPattern);

            return regex.IsMatch(email);
        }

    }
}
