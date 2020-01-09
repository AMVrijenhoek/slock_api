using System;
using MySql.Data.MySqlClient;

namespace api.db
{
    public class AppDb
    {
        public MySqlConnection Connection { get; }

        public AppDb(string connectionString)
        {
            Connection = new MySqlConnection(connectionString);
        }

        public void Dispose()
        {
        Connection.Close();
        }
    }
}