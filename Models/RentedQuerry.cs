using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using api.db;

namespace Models
{
    public class RentedQuerry
    {
        public AppDb Db { get; }

        public RentedQuerry(AppDb db)
        {
            Db = db;
        }

        public async Task<Rented> FindOneAsync(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT id, locks_id, user_id, start end FROM `rented` WHERE `id` = @id";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = id,
            });
            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }
        
        public async Task<Rented> FindOneByLockId(int lock_id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT id, locks_id, user_id, start end FROM `rented` WHERE `lock_id` = @lock_id";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@lock_id",
                DbType = DbType.Int32,
                Value = lock_id,
            });
            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }
        
        public async Task<Rented> FindOneByUserId(int userId)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT id, locks_id, user_id, start FROM `rented` WHERE `user_id` = @user_id";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@user_id",
                DbType = DbType.Int32,
                Value = userId,
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

        private async Task<List<Rented>> ReadAllAsync(DbDataReader reader)
        {
            var rented = new List<Rented>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var rent = new Rented(Db)
                    {
                        Id = reader.GetInt32(0),
                        LockId = reader.GetInt32(1),
                        UserId = reader.GetInt32(2),
                        Start = reader.GetDateTime(3),
                        End = reader.GetDateTime(4),
                    };
                    rented.Add(rent);
                }
            }
            return rented;
        }
        
    }
}