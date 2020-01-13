using System;

namespace Models{
    // This class exists so we can return a stripped down version of the lock class without a bunch of null values.
    public class LocksInfo{

        public int Id{ get; set; }
        public String Description{ get; set; }
        public String DisplayName{ get; set; }
        public String BleUuid{ get; set; }

        internal LocksInfo(){}
        
        // Build based on existing lock
        internal LocksInfo(Lock locky){
            this.Id = locky.Id;
            this.Description = locky.Description;
            this.DisplayName = locky.DisplayName;
            this.BleUuid = locky.BleUuid;
        }
    }
}