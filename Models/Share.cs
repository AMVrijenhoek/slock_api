/*
 * Slock Backend
 *
 * This is the api doc for the Slock backend
 */

using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Models
{ 
    [DataContract]
    public partial class Share : IEquatable<Share>
    { 
        /// user to share the lock with
        /// <value>user to share the lock with</value>
        [DataMember(Name="username")]
        public string Username { get; set; }

        /// Gets or Sets StartDate
        [DataMember(Name="startDate")]
        public DateTime StartDate { get; set; }

        /// Gets or Sets EndDate
        [DataMember(Name="endDate")]
        public DateTime EndDate { get; set; }

        // [DataMember(Name="iso")]
        // public DateTime Iso{get;set;}

        /// Returns the string presentation of the object
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class Share {\n");
            sb.Append("  Username: ").Append(Username).Append("\n");
            sb.Append("  StartDate: ").Append(StartDate).Append("\n");
            sb.Append("  EndDate: ").Append(EndDate).Append("\n");
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
            return obj.GetType() == GetType() && Equals((Share)obj);
        }

        /// Returns true if Share instances are equal
        /// <param name="other">Instance of Share to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(Share other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return 
                (
                    Username == other.Username ||
                    Username != null &&
                    Username.Equals(other.Username)
                ) && 
                (
                    StartDate == other.StartDate ||
                    StartDate != null &&
                    StartDate.Equals(other.StartDate)
                ) && 
                (
                    EndDate == other.EndDate ||
                    EndDate != null &&
                    EndDate.Equals(other.EndDate)
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
                    if (Username != null)
                    hashCode = hashCode * 59 + Username.GetHashCode();
                    if (StartDate != null)
                    hashCode = hashCode * 59 + StartDate.GetHashCode();
                    if (EndDate != null)
                    hashCode = hashCode * 59 + EndDate.GetHashCode();
                return hashCode;
            }
        }

        #region Operators
        #pragma warning disable 1591

        public static bool operator ==(Share left, Share right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Share left, Share right)
        {
            return !Equals(left, right);
        }

        #pragma warning restore 1591
        #endregion Operators
    }
}
