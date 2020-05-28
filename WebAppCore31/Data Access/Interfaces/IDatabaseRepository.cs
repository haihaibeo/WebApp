using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WebAppCore31.Interfaces
{
    public interface IDatabaseRepository
    {
        IUserRepository<User> Users { get; }
        IUserRepository<Student> Students { get; }
        IUserRepository<Author> Authors { get; }
        IRepository<Course> Courses { get; }
        IRepository<Comment> Comments { get; }
        IRepository<StudentCourse> StudentCourses { get; }

        Task<int> SaveChangesAsync();
    }
}
