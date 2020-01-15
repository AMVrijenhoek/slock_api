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
    [DataContract]
    public partial class Rented : IEquatable<Rented>
    { 
        /// Gets or Sets Id
        [DataMember(Name="id")]
        public int Id { get; set; }

        /// Gets or Sets Id
        [DataMember(Name="userid")]
        public int UserId { get; set; }

        /// Gets or Sets lockId
        [DataMember(Name="lockid")]
        public int LockId { get; set; }

        /// Gets or Sets start
        [DataMember(Name="start")]
        public DateTime StartDate { get; set; }

        /// Gets or Sets end
        [DataMember(Name="end")]
        public DateTime EndDate { get; set; }

        internal AppDb Db { get; set; }

        // Constructors
        internal Rented()
        {}

        internal Rented(AppDb db)
        {
            Db = db;
        }
        // end constructors

        public async Task InsertAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO `rented` (`user_id`, `lock_id`, `start`, `end`) VALUES (@user_id, @lock_id, @start, @end);";
            BindParams(cmd);
            await cmd.ExecuteNonQueryAsync();
            Id = (int) cmd.LastInsertedId;
        }

        public async Task UpdateAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"UPDATE `rented` SET `user_id` = @user_id, `lock_id` = @lock_id, `start` = @start, `end` = @end WHERE `id` = @id;";
            BindParams(cmd);
            BindId(cmd);
            await cmd.ExecuteNonQueryAsync();
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
                ParameterName = "@user_id",
                DbType = DbType.String,
                Value = UserId,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@lock_id",
                DbType = DbType.String,
                Value = LockId,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@start",
                DbType = DbType.String,
                Value = StartDate,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@end",
                DbType = DbType.String,
                Value = EndDate,
            });
        }


        /// Returns the string presentation of the object
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class Rented {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  UserId: ").Append(UserId).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// Returns the JSON string presentation of the object
        /// <returns>JSON string presentation of the object</returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        /// Returns true if objects are equal
        /// <param name="obj">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Rented)obj);
        }

        /// Returns true if Rented instances are equal
        /// <param name="other">Instance of Rented to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(Rented other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return
                (
                    Id == other.Id
                ) && 
                (
                    UserId == other.UserId
                );
        }

        /// Gets the hash code
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                var hashCode = 41;
                return hashCode;
            }
        }
        

        #region Operators
        #pragma warning disable 1591

        public static bool operator ==(Rented left, Rented right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Rented left, Rented right)
        {
            return !Equals(left, right);
        }

        #pragma warning restore 1591
        #endregion Operators
    }
}
