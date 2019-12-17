using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using api.db;

namespace Models
{
    public class LockQuerry
    {
        public AppDb Db { get; }

        public LockQuerry(AppDb db)
        {
            Db = db;
        }

        public async Task<Lock> FindOneAsync(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT id, owner_id, ratchet_key, ratchet_counter, description FROM `locks` WHERE `id` = @id";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = id
            });
            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        public async Task<List<Lock>> FindLocksByOwnerAsync(int ownerid)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT id, owner_id, ratchet_key, ratchet_counter, description FROM `locks` WHERE `owner_id` = @ownerid";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@ownerid",
                DbType = DbType.Int32,
                Value = ownerid
            });
            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
            return result;
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

        private async Task<List<Lock>> ReadAllAsync(DbDataReader reader)
        {
            var locks = new List<Lock>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    Lock door = new Lock(Db)
                    {
                        Id = reader.GetInt32(0),
                        OwnerId = reader.IsDBNull(1) ? (int?) null : reader.GetInt32(1),
                        RachetKey = reader.IsDBNull(2) ? (string) null : reader.GetString(2),
                        RatchetCounter = reader.IsDBNull(3) ? (int) 0 : reader.GetInt32(3),
                        Description = reader.IsDBNull(4) ? (string) null : reader.GetString(4),
                    };
                    locks.Add(door);
                }
            }
            return locks;
        }
        
    }
}