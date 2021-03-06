/*
 * Slock Backend
 *
 * This is the api doc for the Slock backend
 */

using System;
using System.Data;
using System.Text;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using api.db;
using MySql.Data.MySqlClient;
using System.Threading.Tasks;

namespace Models
{ 
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public partial class Lock : IEquatable<Lock>
    { 
        /// <summary>
        /// Gets or Sets Id
        /// </summary>
        [DataMember(Name="id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or Sets Id
        /// </summary>
        [DataMember(Name="ownerid")]
        public int? OwnerId { get; set; }

        /// <summary>
        /// Gets or Sets rachetKey
        /// </summary>
        [DataMember(Name="rachetKey")]
        public string RachetKey { get; set; }

        /// <summary>
        /// Gets or Sets ratchetCounter
        /// </summary>
        [DataMember(Name="ratchetCounter")]
        public int RachetCounter { get; set; }

        /// <summary>
        /// Gets or Sets description
        /// </summary>
        [DataMember(Name="Description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or Sets productkey
        /// </summary>
        [DataMember(Name="ProductKey")]
        public string ProductKey { get; set; }

        /// <summary>
        /// Gets or Sets productkey
        /// </summary>
        [DataMember(Name="BleUuid")]
        public string BleUuid { get; set; }
        
        /// <summary>
        /// Gets or Sets productkey
        /// </summary>
        [DataMember(Name="DisplayName")]
        public string DisplayName { get; set; }
        internal AppDb Db { get; set; }

        internal Lock()
        {}
        internal Lock(AppDb db)
        {
            Db = db;
        }
        public async Task InsertAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO `locks` (`owner_id`, `ratchet_key`, `ratchet_counter`, `description`, `product_key`, `bleuuid`, `displayname`) VALUES (@owner_id, @ratchet_key, @ratchet_counter, @description, @product_key, @bleuuid, @displayname);";
            BindParams(cmd);
            await cmd.ExecuteNonQueryAsync();
            Id = (int) cmd.LastInsertedId;
        }

        public async Task UpdateAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"UPDATE `locks` SET `owner_id` = @owner_id, `ratchet_key` = @ratchet_key, `ratchet_counter` = @ratchet_counter, `description` = @description, `bleuuid` = @bleuuid, `displayname` = @displayname WHERE `id` = @id;";
            BindParams(cmd);
            BindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task UpdateRatchetCounter(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"UPDATE `locks` SET `ratchet_counter` = ratchet_counter + 1 WHERE `id` = @id;";
            BindParams(cmd);
            BindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task SyncRatchetCounter(int id, int ratchet_counter)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"UPDATE `locks` SET `ratchet_counter` = @ratchet_counter WHERE `id` = @id;";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = id
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@ratchet_counter",
                DbType = DbType.String,
                Value = ratchet_counter,
            });
            await cmd.ExecuteReaderAsync();
        }
        
        private void BindId(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = Id,
            });
        }

        private void BindParams(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@owner_id",
                DbType = DbType.String,
                Value = OwnerId,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@ratchet_key",
                DbType = DbType.String,
                Value = RachetKey,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@ratchet_counter",
                DbType = DbType.String,
                Value = RachetCounter,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@description",
                DbType = DbType.String,
                Value = Description,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@product_key",
                DbType = DbType.String,
                Value = ProductKey,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@bleuuid",
                DbType = DbType.String,
                Value = BleUuid,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@displayname",
                DbType = DbType.String,
                Value = DisplayName,
            });
        }


        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class Lock {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  ownerid: ").Append(OwnerId).Append("\n");
            sb.Append("  RachetKey: ").Append(RachetKey).Append("\n");
            sb.Append("  RatchetCounter: ").Append(RachetCounter).Append("\n");
            sb.Append("  Description: ").Append(Description).Append("\n");
            sb.Append("  ProductKey: ").Append(ProductKey).Append("\n");
            sb.Append("  BleUuid: ").Append(BleUuid).Append("\n");
            sb.Append("  DisplayName: ").Append(DisplayName).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="obj">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Lock)obj);
        }

        /// <summary>
        /// Returns true if Lock instances are equal
        /// </summary>
        /// <param name="other">Instance of Lock to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(Lock other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return
                (
                    Id == other.Id
                ) && 
                (
                    OwnerId == other.OwnerId
                ) && 
                (
                    Description == other.Description ||
                    Description != null &&
                    Description.Equals(other.Description)
                ) && 
                (
                    RachetKey == other.RachetKey ||
                    RachetKey != null &&
                    RachetKey.Equals(other.RachetKey)
                ) && 
                (
                    RachetCounter == other.RachetCounter //||
                    // RatchetCounter != null &&
                    // RatchetCounter.Equals(other.RatchetCounter)
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                var hashCode = 41;
                // Suitable nullity checks etc, of course :)
                    if (RachetKey != null)
                    hashCode = hashCode * 59 + RachetKey.GetHashCode();
                    // if (RatchetCounter != null)
                    hashCode = hashCode * 59 + RachetCounter.GetHashCode();
                    if (Description != null)
                    hashCode = hashCode * 59 + Description.GetHashCode();
                return hashCode;
            }
        }
        

        #region Operators
        #pragma warning disable 1591

        public static bool operator ==(Lock left, Lock right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Lock left, Lock right)
        {
            return !Equals(left, right);
        }

        #pragma warning restore 1591
        #endregion Operators
    }
}
