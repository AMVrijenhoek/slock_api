namespace Models{

    public class UserInfo{
        // This class exists so that we can return a user without giving secret or unimportant user info. or the need to return null values.
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