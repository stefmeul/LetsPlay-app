using System.Configuration;
using MySql.Data.MySqlClient;

namespace LetsPlayClassLibrary
{
    public class User : Account
    {
        // properties
        public int UserId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime CreationDate { get; set; }

        // constructor
        public User()
        {
            UserId = -1;
            AccountId = -1;
            Email = string.Empty;
            Password = string.Empty;
            CreationDate = DateTime.Now;
        }

        // method to create a new user with an account
        public void RegisterUser()
        {
            string ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                MySqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    string userQuery = "INSERT INTO opensilverdb.users (email,password,creationDate) VALUES (@email,@password,@creationdate)";
                    MySqlCommand cmd = new MySqlCommand(userQuery, connection, transaction);
                    cmd.Parameters.AddWithValue("@email", Email);
                    cmd.Parameters.AddWithValue("@password", Password);
                    cmd.Parameters.AddWithValue("@creationDate", CreationDate);
                    cmd.ExecuteNonQuery();

                    UserId = Convert.ToInt32(cmd.LastInsertedId);

                    string accountQuery = "insert into opensilverdb.accounts (userId,balance,transactionDateTime) values (@userid,@balance,@transactiondatetime)";
                    MySqlCommand cmd2 = new MySqlCommand(accountQuery, connection, transaction);

                    cmd2.Parameters.AddWithValue("@userid", UserId);
                    cmd2.Parameters.AddWithValue("@balance", Balance);
                    cmd2.Parameters.AddWithValue("@transactiondatetime", TransactionDateTime);
                    cmd2.ExecuteNonQuery();

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    transaction.Rollback();
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        //method to login user
        public bool LoginUser()
        {
            string ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                string query = "SELECT * FROM opensilverdb.users WHERE (email, password) = (@email, @password)";
                MySqlCommand cmd = new MySqlCommand(@query, connection);
                cmd.Parameters.AddWithValue("@email", Email);
                cmd.Parameters.AddWithValue("@password", Password);
                try
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            UserId = reader.GetInt32("userId");
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;

                }
                finally
                {
                    connection.Close();
                }
            }
        }

        // method to map db table to class properties
        private static User Mapper(MySqlDataReader reader)
        {
            User usr = new User();
            usr.UserId = reader.GetInt32("userId");
            usr.Email = reader.GetString("email");
            usr.Password = reader.GetString("password");
            usr.CreationDate = reader.GetDateTime("creationDate");
            return usr;
        }

        // method to load list of users
        public static List<User> LoadUsers()
        {
            string query = "SELECT userId,email,password,creationDate FROM opensilverdb.users ORDER BY email ASC";
            return DbHelper.LoadRecords<User>(query, Mapper);
        }

        // method to load a User by userId
        public static User LoadUserById(int userId)
        {
            string query = "SELECT userId,email,password,creationDate FROM opensilverdb.users s WHERE s.userId = @userid";
            MySqlParameter p = new MySqlParameter("@userid", userId);
            return DbHelper.LoadSingleRecord<User>(query, Mapper, p);
        }

        // method to create a user (without account)
        public void SaveUser()
        {
            string query = @"INSERT INTO opensilverdb.users (userId,email,password,creationDate) 
                            VALUES(@userId,@email,@password,@creationDate);";
            MySqlParameter[] parameters = new MySqlParameter[4];
            parameters[0] = new MySqlParameter("@userId", UserId);
            parameters[1] = new MySqlParameter("@email", Email);
            parameters[2] = new MySqlParameter("@password", Password);
            parameters[3] = new MySqlParameter("@creationDate", CreationDate);
            UserId = DbHelper.Execute(query, parameters); // sets userId to last inserted record
        }

        // method to update a user
        public void Update()
        {
            string query = @"UPDATE opensilverdb.users SET 
                        email = @email,
                        password = @password,
                        WHERE userId = @userid;";
            MySqlParameter[] parameters = new MySqlParameter[3];
            parameters[0] = new MySqlParameter("@userid", UserId);
            parameters[1] = new MySqlParameter("@email", Email);
            parameters[2] = new MySqlParameter("@password", Password);
            DbHelper.Execute(query, parameters);
        }

        // method to delete a user
        public void DeleteUser()
        {
            string query = @"DELETE FROM opensilverdb.users WHERE userId = @userid;";
            MySqlParameter parameter = new MySqlParameter("@userid", UserId);
            DbHelper.Execute(query, parameter);
        }
    }
}
