using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppCore31
{
    public class User : IdentityUser
    {
        public User()
        {
            Comments = new HashSet<Comment>();
        }

        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
