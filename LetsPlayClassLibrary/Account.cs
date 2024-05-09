using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using System;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace LetsPlayClassLibrary
{
    public class Account
    {
        // properties
        public int AccountId { get; set; }
        public int UserId { get; set; }
        public decimal Balance { get; set; }
        public DateTime TransactionDateTime { get; set; }

        // constructor
        public Account()
        {
            AccountId = -1;
            Balance = 0;
            TransactionDateTime = DateTime.Now;
        }

        // method to map account db table to class properties
        private static Account Mapper(MySqlDataReader reader)
        {
            Account act = new Account();
            act.AccountId = reader.GetInt32("accountId");
            act.UserId = reader.GetInt32("userId");
            act.Balance = reader.GetDecimal("balance");
            act.TransactionDateTime = reader.GetDateTime("transactionDateTime");
            return act;
        }

        // method to load list of accounts
        public static List<Account> LoadAccounts()
        {
            string query = "SELECT accountId,userId,balance,transactionDateTime FROM opensilverdb.accounts ORDER BY userId ASC";
            return DbHelper.LoadRecords<Account>(query, Mapper);
        }

        // method to load and account by userId
        public static Account LoadAccountById(int userId)
        {
            string query = "SELECT accountId,userId,balance,transactionDateTime FROM opensilverdb.accounts a WHERE a.userId = @userid";
            MySqlParameter p = new MySqlParameter("@userid", userId);
            return DbHelper.LoadSingleRecord(query, Mapper, p);
        }

        // method to create an account
        public void MakeAccount()
        {
            string query = @"INSERT INTO opensilverdb.accounts (accountId,userId,balance,transactionDateTime) 
                            VALUES(@accountId,@userId,@balance,@transactionDateTime);";
            MySqlParameter[] parameters = new MySqlParameter[4];
            parameters[0] = new MySqlParameter("@accountId", AccountId);
            parameters[1] = new MySqlParameter("@userId", UserId);
            parameters[2] = new MySqlParameter("@balance", Balance);
            parameters[3] = new MySqlParameter("@transactionDateTime", TransactionDateTime);

            AccountId = DbHelper.Execute(query, parameters); // set AccountId to last inserted record
        }

        // method to update an account on userId
        public void UpdateAccount()
        {
            string query = @"UPDATE opensilverdb.accounts SET balance = @balance, transactionDateTime = @transactionDateTime WHERE userId = @userId;";
            MySqlParameter[] parameters = new MySqlParameter[3];
            parameters[0] = new MySqlParameter("@userId", UserId);
            parameters[1] = new MySqlParameter("@balance", Balance);
            parameters[2] = new MySqlParameter("@transactionDateTime", TransactionDateTime);
            DbHelper.Execute(query, parameters);
        }

        // method to delete an account
        public void DeleteAccount()
        {
            string query = @"DELETE FROM opensilverdb.accounts WHERE accountId = @accountId;";
            MySqlParameter parameter = new MySqlParameter("@accountId", AccountId);
            DbHelper.Execute(query, parameter);
        }


        // method to redistribute balances
        public static decimal Redistribute(decimal amount, int loggedId)
        {

            // log balances and userId
            Dictionary<int, decimal> balanceAndId = new Dictionary<int, decimal>();

            // add accounts balances to list Balances
            if (Model.Accounts != null)
            {
                foreach (Account a in Model.Accounts)
                {
                    balanceAndId.Add(a.UserId, a.Balance);
                }

                //add amount to userbalance
                //decimal userBalance = balanceAndId[loggedId];
                //  userBalance += amount;

                // set highest balance
                decimal highbalance = balanceAndId.Values.Max();
                //     highbalance = Math.Max(highbalance, userBalance);

                // set lowest balance
                decimal lowbalance = balanceAndId.Values.Min();

                // set number of users
                int numberOfUsers = balanceAndId.Count();


                // Limit that highest amount cannot be more than four times lowest amount
                decimal disparityLimit = highbalance - (4 * lowbalance);


                // redistribute amount above limit to all accounts equally
                decimal redistributionAmount = Math.Max(0, disparityLimit) / numberOfUsers;



                // redistribute balances
                foreach (KeyValuePair<int, decimal> kvp in balanceAndId)
                {
                    int userId = kvp.Key;
                    decimal balance = kvp.Value;


                    if (userId == loggedId)
                    {
                        balanceAndId[userId] += (amount - Math.Max(0, disparityLimit)) + redistributionAmount;
                    //    amount = balanceAndId[userId];

                    }
                    
                    else
                    {
                        balanceAndId[userId] += redistributionAmount;
                    }
                }


               // check if redistribution with amount is within limit
                decimal low = balanceAndId.Values.Min();
                decimal high = balanceAndId.Values.Max();

                if ((low * 4) < high)
                {

                    decimal redis = high - (low * 4);
                    decimal share = redis / numberOfUsers;

                    foreach (KeyValuePair<int, decimal> kvp in balanceAndId)
                    {
                        int userId = kvp.Key;
                        decimal balance = kvp.Value;

                        if (balance == high)
                        {
                            balanceAndId[userId] = (balance - redis) + share;
                        }
                        else
                        {
                        balanceAndId[userId] += share;

                        }
                    }
                }





                // update values for database
                foreach (Account a in Model.Accounts)
                {
                    a.Balance = balanceAndId[a.UserId];
                    a.UpdateAccount();
                }


            }

            return amount;

        }
    }
}