using LetsPlay_app.Classes;
using MySql.Data.MySqlClient;
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
    /// 
    /// 1) get info from balance table
    /// 2)sum , min , max , BalanceNr 1 amount
    /// 3)get logged in user info UserNr and balance 
    ///
    /// 4) set fields
    /// 5) buy btn click
    /// 6) check amount from input
    /// 7) redistribute if min*4 is lower than user balance amount
    /// 8) insert or update balance table
    /// example insert:  INSERT INTO `balance` (`BalanceNr`, `UserNr`, `TransactionDate`, `SendTo`, `ReceivedFrom`, `Amount`, `TransactionTax`) VALUES ('3', '3', '', '', '', '5.0', '0');
    /// 
    /// repeat step 1 to 4

    public partial class Wallet : Page
    {

        SelectLogin select = new SelectLogin();
        List<double> allBalances;

        public Wallet()
        {
            InitializeComponent();
            
            int userId = 0;
            int FundId = 1;

            // get email of logged in user
            string userEmail = UserSession.LoggedInUserEmail;

            if (userEmail != null)
            {
                UserInfo userInfo = select.GetUserInfo(userEmail);

                if (userInfo != null)
                {
                    //set userNr to userId
                    userId = userInfo.UserNr;
                }
            }
            // get user and fund balances
            RetrieveUserBalance(FundId);
            RetrieveUserBalance(userId);

            //get all balances
            allBalances = select.GetAllBalances();
            RetrieveAllBalances();

        }


        // get user Balance from database
        private void RetrieveUserBalance(int userId)
        {
       
            UserInfo userBalance = select.GetUserBalance(userId);
     
            if (userId == 1)
            {
                lblFundAmount.Content = userBalance.Amount.ToString();
            }
            else if (userBalance != null)
            {
                lblUserBalanceAmount.Content = userBalance.Amount.ToString();
            }
            else
            {
                MessageBox.Show("balance is null");
            }
        }
     
        //get all balances from database
        private void RetrieveAllBalances()
        {
            lblSumAllBalancesAmount.Content = allBalances.Sum().ToString();
            lblMaxBalanceAmount.Content= allBalances.Max().ToString();
            lblMinBalanceAmount.Content= allBalances.Min().ToString();
        }
    }
}
