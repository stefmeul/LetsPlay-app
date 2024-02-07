using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LetsPlay_app.Pages;
using System.Windows.Navigation;
using MySql.Data.MySqlClient;
using System.Web.UI.WebControls.WebParts;
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

    }
}
