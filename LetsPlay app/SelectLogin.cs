using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LetsPlay_app.Pages;
using System.Windows.Navigation;
using MySql.Data.MySqlClient;

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
    }
}
