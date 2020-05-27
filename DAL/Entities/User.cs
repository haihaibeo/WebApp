using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entities
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
