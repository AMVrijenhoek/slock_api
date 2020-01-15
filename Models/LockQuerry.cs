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
            cmd.CommandText = @"SELECT id, owner_id, ratchet_key, ratchet_counter, description, product_key, bleuuid, displayname FROM `locks` WHERE `id` = @id";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = id
            });
            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        public async Task<Lock> GetLockByProductKey(string productKey)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT id, owner_id, ratchet_key, ratchet_counter, description, product_key, bleuuid, displayname FROM `locks` WHERE `product_key` = @product_key";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@product_key",
                DbType = DbType.Int32,
                Value = productKey
            });
            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        public async Task<List<Lock>> FindLocksByOwnerAsync(int ownerid)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT id, owner_id, ratchet_key, ratchet_counter, description, product_key, bleuuid, displayname FROM `locks` WHERE `owner_id` = @ownerid";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@ownerid",
                DbType = DbType.Int32,
                Value = ownerid
            });
            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
            return result;
        }
        
        public async Task<Lock> FindLocksByLockIdAsync(int lock_id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT id, owner_id, ratchet_key, ratchet_counter, description, product_key, bleuuid, displayname FROM `locks` WHERE `id` = @lockId";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@lockId",
                DbType = DbType.Int32,
                Value = lock_id
            });
            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        public async Task<List<Lock>> FindRentedLocksAsync(int userid)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT l.id, owner_id, ratchet_key, ratchet_counter, description, product_key, bleuuid, displayname 
                                FROM `locks` l
                                    JOIN `rented` r ON l.id = r.lock_id
                                WHERE r.`user_id` = @userid 
                                    AND r.`start` < NOW()
                                    AND r.`end` > NOW()";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@userid",
                DbType = DbType.Int32,
                Value = userid
            });
            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
            return result;
        }

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
                        RachetCounter = reader.IsDBNull(3) ? (int) 0 : reader.GetInt32(3),
                        Description = reader.IsDBNull(4) ? (string) null : reader.GetString(4),
                        ProductKey = reader.IsDBNull(5) ? (string) null : reader.GetString(5),
                        BleUuid = reader.IsDBNull(6) ? (string) null : reader.GetString(6),
                        DisplayName = reader.IsDBNull(7) ? (string) null : reader.GetString(7),
                    };
                    locks.Add(door);
                }
            }
            return locks;
        }
        
    }
}