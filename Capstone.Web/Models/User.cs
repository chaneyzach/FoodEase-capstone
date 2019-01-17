using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Capstone.Web.Models
{
    public class User : Base
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }        
        public string Username { get; set; }
        public string Email { get; set; }
        public string Hash { get; set; }
        public string Salt { get; set; }

        public User Clone()
        {
            User user = new User();
            user.Id = Id;
            user.FirstName = FirstName;
            user.LastName = LastName;
            user.Username = Username;
            user.Email = Email;
            user.Hash = Hash;
            user.Salt = Salt;
            return user;
        }
    }
}