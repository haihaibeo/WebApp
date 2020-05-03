using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppCore31
{
    public class Author : User
    {
        public Author()
        {
            Courses = new HashSet<Course>();
        }

        [StringLength(20)]
        public override string PhoneNumber { get => base.PhoneNumber; set => base.PhoneNumber = value; }

        public virtual ICollection<Course> Courses { get; set; }
    }
}
