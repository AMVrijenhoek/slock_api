using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using api.db;

namespace Models
{
    public class UserQuerry
    {
        public AppDb Db { get; }

        public UserQuerry(AppDb db)
        {
            Db = db;
        }

        public async Task<User> FindOneAsync(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT email, username, first_name, last_name FROM `users` WHERE `id` = @id";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = id,
            });
            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }
        
        public async Task<User> GetUserByEmail(string email)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.Connection.Open();
            cmd.CommandText = @"SELECT id, email, username, first_name, last_name, password FROM `users` WHERE `email` = @email";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@email",
                DbType = DbType.String,
                Value = email,
            });
            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
            cmd.Connection.Close();
            return result.Count > 0 ? result[0] : null;
        }
        
        public async Task<User> CheckIfUsernameExists(string username)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.Connection.Open();
            cmd.CommandText = @"SELECT id, email, username, first_name, last_name, password FROM `users` WHERE `username` = @username";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@username",
                DbType = DbType.String,
                Value = username,
            });
            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
            cmd.Connection.Close();
            return result.Count > 0 ? result[0] : null;
        }

        /*
        public async Task<List<BlogPost>> LatestPostsAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `Id`, `Title`, `Content` FROM `BlogPost` ORDER BY `Id` DESC LIMIT 10;";
            return await ReadAllAsync(await cmd.ExecuteReaderAsync());
        }

        public async Task DeleteAllAsync()
        {
            using var txn = await Db.Connection.BeginTransactionAsync();
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `BlogPost`";
            await cmd.ExecuteNonQueryAsync();
            await txn.CommitAsync();
        }
        /**/

        private async Task<List<User>> ReadAllAsync(DbDataReader reader)
        {
            var users = new List<User>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var user = new User(Db)
                    {
                        Id = reader.GetInt32(0),
                        Email = reader.GetString(1),
                        Username = reader.GetString(2),
                        FirstName = reader.GetString(3),
                        LastName = reader.GetString(4),
                        Password = reader.GetString(5),
                    };
                    users.Add(user);
                }
            }
            return users;
        }
        
    }
}