using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entities
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
