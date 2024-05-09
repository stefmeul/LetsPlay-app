using System;
using System.Collections.Generic;
using System.Windows;
using MySql.Data.MySqlClient;
using LetsPlay_app.Classes;

namespace LetsPlay_app
{
    internal class SelectLogin
    {
        Connection con = new Connection();

        public string SelectLoginData(string emailInsert, string passInsert)
        {
            try
            {
                Connection.DataSource();
                con.connOpen();
                MySqlCommand command = new MySqlCommand();
                command.CommandText = "SELECT * FROM users WHERE (Email, Password) = (@email, @password);";
                command.Parameters.AddWithValue("@email", emailInsert);
                command.Parameters.AddWithValue("@password", Encrypt.HashString(passInsert));
                command.Connection = Connection.connMaster;

                MySqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    // welcome message
                    MessageBox.Show($"hi, {emailInsert}");

                    // logged in to true
                    LoginStatus.IsUserLoggedIn = true;
                }

                else
                {
                    MessageBox.Show("error");
                }

                return emailInsert + passInsert;
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }

            finally
            {
                con.connClose();
            }
        }


        // get user info from database
        public UserInfo GetUserInfo(string email)
        {
            try
            {
                Connection.DataSource();
                con.connOpen();

                MySqlCommand command = new MySqlCommand();
                command.CommandText = "SELECT * FROM users WHERE Email = @email;";
                command.Parameters.AddWithValue("@email", email);
                command.Connection = Connection.connMaster;

                MySqlDataReader reader = command.ExecuteReader();

                if (reader.Read()) 
                { 
                    UserInfo info = new UserInfo();
                    {
                        info.UserNr = Convert.ToInt32(reader["UserNr"]);
                        info.UserName = reader["UserName"].ToString();
                        info.Email = reader["Email"].ToString();
                        info.Password = reader["Password"].ToString();
                        info.ImgUrl = reader["ImgUrl"].ToString();
                        info.WebsiteUrl = reader["WebsiteUrl"].ToString();
                    };
                    return info;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally 
            {
                con.connClose(); 
            }
            return null;
        }

        // get user balance
        public UserInfo GetUserBalance(int userNr)
        {
            Connection con = new Connection();

            try
            {
                Connection.DataSource();
                con.connOpen();

                MySqlCommand command = new MySqlCommand();
                command.CommandText = "SELECT * FROM balance WHERE UserNr = @userNr;";
                command.Parameters.AddWithValue("@userNr", userNr);
                command.Connection = Connection.connMaster;

                MySqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    UserInfo info = new UserInfo();
                    {
                        info.Amount = Convert.ToDouble(reader["Amount"]);
                    //    info.Email = reader["Email"].ToString();
                    //    info.Password = reader["Password"].ToString();
                    //    info.ImgUrl = reader["ImgUrl"].ToString();
                    //    info.WebsiteUrl = reader["WebsiteUrl"].ToString();

                        Console.WriteLine(info);
                    };
                    return info;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.connClose();
            }
            return null;
        }


        // get all balances
        public List<double> GetAllBalances()
        {
            Connection con = new Connection();

            List<double> balanceAmounts = new List<double>();
            
            // clear list
            balanceAmounts.Clear();

            try
            {
                Connection.DataSource();
                con.connOpen();

                MySqlCommand command = new MySqlCommand();
                command.CommandText = "SELECT Amount FROM balance;";
                command.Connection = Connection.connMaster;

                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    double amount = Convert.ToDouble(reader["Amount"]);
                    
                    // add amount to list
                    balanceAmounts.Add(amount);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.connClose();
            }

            return balanceAmounts;
        }

    }
}
