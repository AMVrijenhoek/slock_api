
using System;
using System.Data;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using api.db;
using MySql.Data.MySqlClient;
using System.Threading.Tasks;

namespace Models{


    public class UserInfo{

        public int Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Verified { get; set; }

        internal UserInfo(){}
        internal UserInfo(User user){
            Id = user.Id;
            Username = user.Username;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;
            Verified = user.Verified;
            
        }
    }

}