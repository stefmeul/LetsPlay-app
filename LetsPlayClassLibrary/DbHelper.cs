using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace LetsPlayClassLibrary
{
    internal class DbHelper
    {
        public static List<T> LoadRecords<T>(string query, Func<MySqlDataReader, T> mapFunction, params MySqlParameter[] parameters)
        {
            List<T> objects = new List<T>();
            string connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddRange(parameters);
                connection.Open();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        T obj = mapFunction(reader);
                        objects.Add(obj);
                    }
                }
                connection.Close();
            }
            return objects;
        }

        public static T? LoadSingleRecord<T>(string query, Func<MySqlDataReader, T> mapFunction, params MySqlParameter[] parameters)
        {
            T? obj = default(T);
            string connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddRange(parameters);
                connection.Open();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        obj = mapFunction(reader);
                    }
                }
                connection.Close();
            }
            return obj;
        }

        public static int Execute(string query, params MySqlParameter[] parameters)
        {
            int retVal = 0;
            string connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddRange(parameters);
                connection.Open();
                cmd.ExecuteNonQuery();
                retVal = Convert.ToInt32(cmd.LastInsertedId);
                connection.Close();
            }
            return retVal;
        }
    }
}
