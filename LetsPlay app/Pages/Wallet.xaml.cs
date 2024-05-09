using LetsPlay_app.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

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

        int userId = 0;
        string userEmail = string.Empty;

        public Wallet()
        {
            InitializeComponent();

            int FundId = 1;

            // get email of logged in user
            userEmail = UserSession.LoggedInUserEmail;

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
                lblFundAmount.Content = userBalance.Amount.ToString("0.00");
            }
            else if (userBalance != null)
            {
                lblUserBalanceAmount.Content = userBalance.Amount.ToString("0.00");
            }
            else
            {
                MessageBox.Show("balance is null");
            }
        }

        //get all balances from database
        private void RetrieveAllBalances()
        {
            lblSumAllBalancesAmount.Content = allBalances.Sum().ToString("0.00");
            lblMaxBalanceAmount.Content = allBalances.Max().ToString("0.00");
            lblMinBalanceAmount.Content = allBalances.Min().ToString("0.00");
        }

        // event button click Buy 
        private void BuyBtn_Click(object sender, RoutedEventArgs e)
        {
            // instantiate object
            Redistribute redistribution = new Redistribute();

            double amountToAdd = Convert.ToDouble(txbAmountToBuy.Text); // catch input type validation
            double newUserBalance;
            /*
            if (select.GetUserBalance(userId).Amount == Convert.ToDouble(DBNull.Value))
            {

            }
            else
            {
            }
            */
            try
            {
                newUserBalance = (select.GetUserBalance(userId).Amount) + amountToAdd;

            }

            catch (NullReferenceException ex)
            {

                MessageBox.Show(ex.Message);

                newUserBalance = amountToAdd;

            }

            // calculate limit  (4 * lowest_amount) - current_highest_amount`.
            double lowLimit = 4 * allBalances.Min();

            // compare lowestbalance times 4 with a specific number = 400,
            // and chose whichever is the highest of those
            lowLimit = Math.Max(lowLimit, 400);

            // set highLimit from highest user balance
            double highLimit = allBalances.Max();
            double limitDisparity = 0;

            // check if newuserbalance exceeds highest balance 
            if (newUserBalance > highLimit)
            {
                limitDisparity = lowLimit - newUserBalance;
            }
            else
            {
                limitDisparity = lowLimit - highLimit;
            }

            // if negative number, make positive
            if (limitDisparity < 0)
            {
                limitDisparity = limitDisparity * (-1);
            }


            // subtract redistribution amount if newUserBalance is above limitDisparity
            if (newUserBalance > limitDisparity)
            {
                // calculate amount to redistribute
                double amountAboveLimit = newUserBalance - limitDisparity;

                // find number of user wallets
                int numberOfWallets = allBalances.Count();

                // redistribute evenly to all users
                double amountToRedistribute = amountAboveLimit / numberOfWallets;


                // update user balance
                double amountUserToUpdate = amountToAdd - amountAboveLimit;
                redistribution.UpdateUserBalance(amountUserToUpdate, userId);


                // update all user balances
                redistribution.UpdateAllBalances(amountToRedistribute);


                //get new balances
                RetrieveUserBalance(userId);
                allBalances.Clear(); // clear list before getting new balances from database
                allBalances = select.GetAllBalances();
                RetrieveAllBalances();


            }
            else
            {
                // update user balance
                // update user balance
                redistribution.UpdateUserBalance(newUserBalance, userId);


                //get new balances
                RetrieveUserBalance(userId);
                allBalances.Clear(); // clear list before getting new balances from database
                allBalances = select.GetAllBalances();
                RetrieveAllBalances();
            }
        }


        // method to redistribute 
        private double Redistribute(double a)
        {
            double highestAmount = allBalances.Max();
            double lowestAmount = allBalances.Min();
            double sum = allBalances.Sum();

            // Limit that highest amount cannot be more than four times lowest amount
            double disparityLimit = (4 * lowestAmount) - highestAmount;

            // redistribute amount above limit to all equally
            double redistributionAmount = disparityLimit / allBalances.Count();

            if (redistributionAmount < 0)
            {
                redistributionAmount *= -1;
                disparityLimit *= -1;
            }

            for (int i = 0; i < allBalances.Count; i++)
            {
                if (i == allBalances.Count - 1)
                {
                    allBalances[i] -= disparityLimit;
                    allBalances[i] += redistributionAmount;
                }
                else
                {
                    allBalances[i] = allBalances[i] + redistributionAmount; // create condition on ID balance
                }
            }

            return a;
        }
    }
}
