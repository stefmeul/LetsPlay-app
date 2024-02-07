using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;



namespace LetsPlay_app.Classes
{
    internal class UpdateUser
    {
        private readonly Connection con = new Connection();

                                                                // check password validation 
        public bool UpdateUserInfo(string email, string name, string newPassword, string imgUrl, string websiteUrl)
        {
            try
            {
                Connection.DataSource(); //

                con.connOpen();
                MySqlCommand command = new MySqlCommand();
                command.CommandText = "UPDATE users SET UserName = @name, Password = @password, ImgUrl = @imgUrl, WebsiteUrl = @websiteUrl WHERE Email = @email;";
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@email", email);
                command.Parameters.AddWithValue("@password", Encrypt.HashString(newPassword));
                command.Parameters.AddWithValue("@imgUrl", imgUrl);
                command.Parameters.AddWithValue("@websiteUrl", websiteUrl);
                command.Connection = Connection.connMaster;

                int rowsAffected = command.ExecuteNonQuery();


                if (rowsAffected != 0)
                {
                    // update message
                    MessageBox.Show($"hi,update settings:  {email}");
                }

                else
                {
                    MessageBox.Show("error");
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
