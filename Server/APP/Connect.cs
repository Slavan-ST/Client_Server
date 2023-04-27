using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using WebApplication;
//Server
namespace APP
{
    public class Connect
    {

        public static List<User> ConnectClasses = new List<User>();
        public static string line = "";
       public static string connect()
        {
            string sqlQuery = "select * from test1;";
            var _conString = "Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=123";
            using (NpgsqlConnection npgsqlConnection = new NpgsqlConnection(_conString))
            {
                npgsqlConnection.Open();
                NpgsqlCommand command = new NpgsqlCommand();
                command.CommandText = sqlQuery;
                command.Connection = npgsqlConnection;
                using(NpgsqlDataReader reader = command.ExecuteReader()) 
                {
                    while (reader.Read())
                    {
                        User cls = new User();
                        cls.Id = reader.GetInt32(0);
                        cls.Name = reader.GetString(1);
                        ConnectClasses.Add(cls);
                    }
                }
            }
            return line;
        }
    }
}
