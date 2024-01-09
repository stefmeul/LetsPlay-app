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

        internal string InsertData(string userInsert, string emailInsert,string passInsert, string insertDateTime)
        {
            try
            {
                Connection.DataSource();
                con.connOpen();
                MySqlCommand command = new MySqlCommand();
                // insert sql
                command.CommandText = "INSERT INTO users (UserName, Email ,Password, Registered) values (@name, @email ,@password, @datetime)";
                command.Parameters.AddWithValue("@name", userInsert);
                command.Parameters.AddWithValue("@email", emailInsert);
                command.Parameters.AddWithValue("@password", Encrypt.HashString(passInsert)); // encrypt password from Encrypt.cs class
                command.Parameters.AddWithValue("@datetime", insertDateTime);
                command.Connection = Connection.connMaster; // from Connection.cs class
                command.ExecuteNonQuery();

                MessageBox.Show($"{userInsert}, your account has been created");
                con.connClose();

                return userInsert + emailInsert + passInsert + insertDateTime;

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
