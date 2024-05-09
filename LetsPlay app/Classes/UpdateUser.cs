using System;
using System.Windows;
using MySql.Data.MySqlClient;



namespace LetsPlay_app.Classes
{
    internal class UpdateUser
    {
        private readonly Connection con = new Connection();

                                                                // check password validation 
        public bool UpdateUserInfo(string email, string name,/* string newPassword,*/ string imgUrl, string websiteUrl)
        {
            try
            {
                Connection.DataSource(); //

                con.connOpen();
                MySqlCommand command = new MySqlCommand();
                command.CommandText = "UPDATE users SET UserName = @name, ImgUrl = @imgUrl, WebsiteUrl = @websiteUrl WHERE Email = @email;"; // Password = @password, 
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@email", email);
                /*
                // get user password from database
                SelectLogin selectLogin = new SelectLogin();
                string userEmail = UserSession.LoggedInUserEmail;
                UserInfo userInfo = selectLogin.GetUserInfo(userEmail);

                // check if password is same, if not encrypt it
                if (userInfo.Password != newPassword)
                {
                    command.Parameters.AddWithValue("@password", Encrypt.HashString(newPassword));
                }
                else
                {*/
            //    command.Parameters.AddWithValue("@password", newPassword);
                //}


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
