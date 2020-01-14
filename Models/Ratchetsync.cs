/*
 * Slock Backend
 *
 * This is the api doc for the Slock backend
 */

using System;
using System.Text;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Models
{ 
    [DataContract]
    public partial class Ratchetsync : IEquatable<Ratchetsync>
    { 
        /// Gets or Sets Counter
        [DataMember(Name="counter")]
        public string Counter { get; set; }

        /// Gets or Sets Token
        [DataMember(Name="token")]
        public string Token { get; set; }

        /// Returns the string presentation of the object
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class Ratchetsync {\n");
            sb.Append("  Counter: ").Append(Counter).Append("\n");
            sb.Append("  Token: ").Append(Token).Append("\n");
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
            return obj.GetType() == GetType() && Equals((Ratchetsync)obj);
        }

        /// Returns true if Ratchetsync instances are equal
        /// <param name="other">Instance of Ratchetsync to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(Ratchetsync other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return 
                (
                    Counter == other.Counter ||
                    Counter != null &&
                    Counter.Equals(other.Counter)
                ) && 
                (
                    Token == other.Token ||
                    Token != null &&
                    Token.Equals(other.Token)
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
                    if (Counter != null)
                    hashCode = hashCode * 59 + Counter.GetHashCode();
                    if (Token != null)
                    hashCode = hashCode * 59 + Token.GetHashCode();
                return hashCode;
            }
        }

        #region Operators
        #pragma warning disable 1591

        public static bool operator ==(Ratchetsync left, Ratchetsync right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Ratchetsync left, Ratchetsync right)
        {
            return !Equals(left, right);
        }

        #pragma warning restore 1591
        #endregion Operators
    }
}
