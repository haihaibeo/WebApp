using WebAppCore31.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WebAppCore31.Repositories
{
    public class StudentCourseRepo : IRepository<StudentCourse>, IDisposable
    {
        private RegisterContext context;

        public StudentCourseRepo(RegisterContext context)
        {
            this.context = context;
        }

        public void Create(StudentCourse item)
        {
            context.StudentCourses.Add(item);
        }

        public void Delete(string Id)
        {
            var item = context.StudentCourses.Find(Id);
            if (item != null) context.StudentCourses.Remove(item);
        }

        public void Dispose()
        {
            context.Dispose();
        }

        public Task<List<StudentCourse>> GetAllAsync()
        {
            return context.StudentCourses.ToListAsync();
        }

        public Task<StudentCourse> GetByIdAsync(string Id)
        {
            return context.StudentCourses.SingleOrDefaultAsync(c => c.Id == Id);
        }

        public Task<int> SaveChangeAsync()
        {
            return context.SaveChangesAsync();
        }

        public void Update(StudentCourse item)
        {
            context.StudentCourses.Update(item);
        }
    }
}
