using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppCore31
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
