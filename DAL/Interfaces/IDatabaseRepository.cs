using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IDatabaseRepository
    {
        IUserRepository<User> Users { get; }
        IUserRepository<Student> Students { get; }
        IUserRepository<Author> Authors { get; }
        IRepository<Course> Courses { get; }
        IRepository<Comment> Comments { get; }
        IRepository<StudentCourse> StudentCourse { get; }

        Task<int> SaveChangesAsync();
    }
}
