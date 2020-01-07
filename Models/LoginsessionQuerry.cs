using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using api.db;

namespace Models
{
    public class LoginsessionQuerry
    {
        public AppDb Db { get; }

        public LoginsessionQuerry(AppDb db)
        {
            Db = db;
        }

        public async Task<Loginsession> FindOneAsync(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT id, user_id, creation_date, auth_token FROM `login_session` WHERE `id` = @id";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = id,
            });
            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }
        
        public async Task<Loginsession> FindOneByUserId(int userId)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT id, user_id, creation_date, auth_token FROM `login_session` WHERE `user_id` = @user_id";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@user_id",
                DbType = DbType.Int32,
                Value = userId,
            });
            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        public bool InsertLoginTable(int id, string token)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO login_session (user_id, creation_date, auth_token) VALUES (@id, CURRENT_TIMESTAMP, @token)";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = id,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@token",
                DbType = DbType.String,
                Value = token,
            });
            cmd.ExecuteNonQueryAsync();
            return true;
        }

        public async Task<Loginsession> GetUserIdByToken(string token)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText =
                @"SELECT id, user_id, creation_date, auth_token FROM login_session WHERE auth_token = @token";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@token",
                DbType = DbType.String,
                Value = token,
            });
            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
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

        private async Task<List<Loginsession>> ReadAllAsync(DbDataReader reader)
        {
            var sessions = new List<Loginsession>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var session = new Loginsession(Db)
                    {
                        Id = reader.GetInt32(0),
                        user_id  = reader.GetInt32(1),
                        creation_date = reader.GetDateTime(2).ToString(),
                        auth_token = reader.GetString(3),
                    };
                    sessions.Add(session);
                }
            }
            return sessions;
        }
        
    }
}