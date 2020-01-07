
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


    public class LocksInfo{

        public int Id{ get; set; }
        public String Description{ get; set; }

        internal LocksInfo(){}
        internal LocksInfo(Lock locky){
            this.Id = locky.Id;
            this.Description = locky.Description;
        }
    }

}