using DAL.DataContext;
using DAL.Interfaces;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class CourseRepo : IRepository<Course>, IDisposable
    {
        private RegisterContext context;

        public CourseRepo(RegisterContext context)
        {
            this.context = context;
        }

        public void Create(Course item)
        {
            context.Courses.Add(item);
        }

        public void Delete(string Id)
        {
            var item = context.Courses.Find(Id);
            if (item != null) context.Courses.Remove(item);
        }

        public void Dispose()
        {
            context.Dispose();
        }

        public IEnumerable<Course> GetAll()
        {
            return context.Courses.ToList();
        }

        public Task<Course> GetById(string Id)
        {
            return context.Courses.SingleOrDefaultAsync(c => c.Id == Id);
        }

        public Task<int> SaveChangeAsync()
        {
            return context.SaveChangesAsync();
        }

        public void Update(Course item)
        {
            context.Courses.Update(item);
        }
    }
}
