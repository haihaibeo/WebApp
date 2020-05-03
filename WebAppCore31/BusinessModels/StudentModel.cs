using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppCore31
{
    public class StudentModel : UserModel
    {
        public StudentModel(Student student) : base(student)
        {
            UniYear = student.UniYear;
        }

        public string UniYear { get; set; }
    }
}
