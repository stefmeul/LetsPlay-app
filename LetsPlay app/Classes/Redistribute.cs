using MySql.Data.MySqlClient;
using System;
using System.Windows;

namespace LetsPlay_app.Classes
{

    internal class Redistribute
    {

        private readonly Connection con = new Connection();

        // check password validation 
        public bool UpdateUserBalance(double newAmount, int userId)
        {

            try
            {
                Connection.DataSource(); //

                con.connOpen();
                MySqlCommand command = new MySqlCommand();
                command.CommandText = "UPDATE balance SET Amount = Amount + @newAmount WHERE UserNr = @userId;";
                command.Parameters.AddWithValue("@newAmount", newAmount);
                command.Parameters.AddWithValue("@userId", userId);
                command.Connection = Connection.connMaster;


                int rowsAffected = command.ExecuteNonQuery();


                if (rowsAffected != 0)
                {
                    // update message
                    MessageBox.Show($"hi,updated user balance:  {newAmount}");
                }

                else
                {
                    MessageBox.Show("error");
                }

                return (rowsAffected > 0); // returns true (1) if succes
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }

            finally
            {
                con.connClose();
            }
        }

        public bool UpdateAllBalances(double amountToAdd)
        {
            try
            {
                Connection.DataSource(); //

                con.connOpen();
                MySqlCommand command = new MySqlCommand();
                command.CommandText = "UPDATE balance SET Amount = Amount + @amountToAdd;";
                command.Parameters.AddWithValue("@amountToAdd", amountToAdd);
                command.Connection = Connection.connMaster;

                int rowsAffected = command.ExecuteNonQuery();


                if (rowsAffected != 0)
                {
                    // update message
                    MessageBox.Show($"hi,updated all balances");
                }

                else
                {
                    MessageBox.Show("did not update balances");
                }

                return (rowsAffected > 0); // returns true (1) if the update was succesfull
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }

            finally
            {
                con.connClose();
            }
        }
    }
}


