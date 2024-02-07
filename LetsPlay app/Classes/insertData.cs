using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySql.Data.MySqlClient;

namespace LetsPlay_app.Pages
{
    internal class insertData
    {
        Connection con = new Connection();
        Encrypt encrypt = new Encrypt();

        // create a new user or edit user settings
        internal string InsertData(string insertDateTime, string userInsert, string emailInsert, string passInsert, string imgUrlInsert, string websiteUrlInsert)
        {
            try
            {
                Connection.DataSource();
                con.connOpen();
                MySqlCommand command = new MySqlCommand();

                command.CommandText = "INSERT INTO users (RegisteredDate, UserName, Email ,Password, ImgUrl, WebsiteUrl) values (@datetime, @name, @email ,@password, @imgUrl, @websiteUrl)";
                command.Parameters.AddWithValue("@datetime", insertDateTime);
                command.Parameters.AddWithValue("@name", userInsert);
                command.Parameters.AddWithValue("@email", emailInsert);
                command.Parameters.AddWithValue("@password", Encrypt.HashString(passInsert)); // encrypt password from Encrypt.cs class
                if (imgUrlInsert != null || websiteUrlInsert != null)
                {
                    command.Parameters.AddWithValue("@ImgUrl", imgUrlInsert);
                    command.Parameters.AddWithValue("@WebsiteUrl", websiteUrlInsert);
                }
                else
                {
                    imgUrlInsert = null;
                    websiteUrlInsert = null;

                    command.Parameters.AddWithValue("@ImgUrl", imgUrlInsert);
                    command.Parameters.AddWithValue("@WebsiteUrl", websiteUrlInsert);

                }

                command.Connection = Connection.connMaster; // from Connection.cs class
                command.ExecuteNonQuery();

                MessageBox.Show($"{userInsert}, your account has been created");

                con.connClose();

                return insertDateTime + userInsert + emailInsert + passInsert + imgUrlInsert + websiteUrlInsert;

            }
            catch(Exception ex)  
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
