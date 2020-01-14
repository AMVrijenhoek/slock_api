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
    public partial class Loginsession : IEquatable<Loginsession>
    { 
        /// Gets or Sets id
        [DataMember(Name="id")]
        public int Id { get; set; }

        /// Gets or Sets user_id
        [DataMember(Name="user_id")]
        public int user_id { get; set; }
        
        /// Gets or Sets creation_date
        [DataMember(Name="creation_date")]
        public string creation_date { get; set; }

        /// Gets or Sets auth_token
        [DataMember(Name="auth_token")]
        public string auth_token { get; set; }

        internal AppDb Db { get; set; }

        public Loginsession()
        {
        }

        internal Loginsession(AppDb db)
        {
            Db = db;
        }

        public async Task InsertAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO `login_session` (`user_id`, `creation_date`, 'auth_token') VALUES (@user_id, CURRENT_TIMESTAMP , @auth_token);";
            BindParams(cmd);
            await cmd.ExecuteNonQueryAsync();
            Id = (int) cmd.LastInsertedId;
        }

        public async Task UpdateAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"UPDATE `login_session` SET `user_id` = @user_id, `creation_date` = CURRENT_TIMESTAMP , `auth_token` = @auth_token, WHERE `id` = @id;";
            BindParams(cmd);
            BindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `login_session` WHERE `id` = @id;";
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
                ParameterName = "@auth_token",
                DbType = DbType.String,
                Value = auth_token,
            });
        }

        /// Returns the string presentation of the object
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class Loginsession {\n");
            sb.Append("  user_id: ").Append(user_id).Append("\n");
            sb.Append("  creation_date: ").Append(creation_date).Append("\n");
            sb.Append("  auth_token: ").Append(auth_token).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// Returns the JSON string presentation of the object
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
            return obj.GetType() == GetType() && Equals((User)obj);
        }

        /// <summary>
        /// Returns true if User instances are equal
        /// </summary>
        /// <param name="other">Instance of User to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(Loginsession other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return
                (
                    creation_date == other.creation_date ||
                    creation_date != null &&
                    creation_date.Equals(other.creation_date)
                ) &&
                (
                    auth_token == other.auth_token ||
                    auth_token != null &&
                    auth_token.Equals(other.auth_token)
                );
        }

        /// Gets the hash code
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                var hashCode = 41;
                // Suitable nullity checks etc, of course :)
                    if (creation_date != null)
                        hashCode = hashCode * 59 + creation_date.GetHashCode();
                    if (auth_token != null)
                        hashCode = hashCode * 59 + auth_token.GetHashCode();
                    return hashCode;
            }
        }

        #region Operators
        #pragma warning disable 1591

        public static bool operator ==(Loginsession left, Loginsession right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Loginsession left, Loginsession right)
        {
            return !Equals(left, right);
        }

        #pragma warning restore 1591
        #endregion Operators
    }
}
