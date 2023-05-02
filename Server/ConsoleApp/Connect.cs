using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
//Server
namespace APP
{
    public class Connect
    {

        static string _conString = "Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=123";

        public static List<User> Users = new List<User>();

        public static User ReadFromDB(string id)
        {
            User user = new User();
            string sqlQuery = $"select * from test1 where id = '{id}';";
            Console.WriteLine(id);
            using (NpgsqlConnection npgsqlConnection = new NpgsqlConnection(_conString))
            {
                npgsqlConnection.Open();
                NpgsqlCommand command = new NpgsqlCommand();
                command.CommandText = sqlQuery;
                command.Connection = npgsqlConnection;
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        user.Id = reader.GetInt32(0);
                        user.Name = reader.GetString(1);
                        user.Avatar = reader.GetValue(2) != DBNull.Value ? (byte[])reader.GetValue(2) : null;
                        user.LVL = reader.GetValue(3) != DBNull.Value ? reader.GetInt32(3) : 0;
                        user.Discount = reader.GetValue(4) != DBNull.Value ? reader.GetInt32(4) : 0;
                    }
                }
            }
            return user;
        }

        public static bool WriteInDb(User newUser)
        {
            string sqlQuery = $"insert into test1 values ('{newUser.Id}','{newUser.Name}', @avatar, '{newUser.LVL}', '{newUser.Discount}');";

            using (NpgsqlConnection npgsqlConnection = new NpgsqlConnection(_conString))
            {
                npgsqlConnection.Open();
                NpgsqlCommand command = new NpgsqlCommand();
                command.CommandText = sqlQuery;
                command.Connection = npgsqlConnection;
                command.Parameters.AddWithValue("avatar",newUser.Avatar);
                command.ExecuteNonQuery();
            }
            return true;
        }
        public static bool UpdateInDb(User newUser)
        {
            string sqlQuery = $"update test1 set avatar = @avatar where id = {newUser.Id};";

            using (NpgsqlConnection npgsqlConnection = new NpgsqlConnection(_conString))
            {
                npgsqlConnection.Open();
                NpgsqlCommand command = new NpgsqlCommand();
                command.CommandText = sqlQuery;
                command.Connection = npgsqlConnection;
                command.Parameters.AddWithValue("avatar", newUser.Avatar);
                command.ExecuteNonQuery();
            }
            return true;
        }
    }
}
