using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppTEST
{
    public class Connect
    {

        static string _conString = "Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=123";

        public static byte[] avatar;
        public static byte[] connect()
        {
            string sqlQuery = "select * from test1;";
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
                        avatar = reader.GetValue(2) != DBNull.Value ? (byte[])reader.GetValue(2) : null;
                    }
                }
            }
            return avatar;
        }


        public static bool WriteInDb(byte[] bytes)
        {
            string sqlQuery = $"insert into test1 values ('{1}','{1}', @avatar, '{1}', '{1}');";

            using (NpgsqlConnection npgsqlConnection = new NpgsqlConnection(_conString))
            {
                npgsqlConnection.Open();
                NpgsqlCommand command = new NpgsqlCommand();
                command.CommandText = sqlQuery;
                command.Connection = npgsqlConnection;
                command.Parameters.AddWithValue("@avatar", bytes);
                command.ExecuteNonQuery();
            }
            return true;
        }
    }
}
