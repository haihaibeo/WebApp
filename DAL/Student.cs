using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppCore31
{
    public class Student : User
    {
        public Student()
        {
            StudentCourses = new HashSet<StudentCourse>();
        }

#nullable enable
        public string? UniYear { get; set; }
        public virtual ICollection<StudentCourse>? StudentCourses { get; set; }
    }
}
