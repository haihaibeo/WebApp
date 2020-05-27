
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppCore31
{
    public class UserModel
    {
        public UserModel(User user)
        {
            Id = user.Id;
            Name = user.Name;
            Email = user.Email;
            UserName = user.UserName;
        }

        public string Id { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
