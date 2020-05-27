using DAL.Interfaces;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class StudentCourseRepo : IRepository<StudentCourse>, IDisposable
    {
        public void Create(StudentCourse item)
        {
            throw new NotImplementedException();
        }

        public void Delete(string Id)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<StudentCourse> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<StudentCourse> GetById(string Id)
        {
            throw new NotImplementedException();
        }

        public Task<int> SaveChangeAsync()
        {
            throw new NotImplementedException();
        }

        public void Update(StudentCourse item)
        {
            throw new NotImplementedException();
        }
    }
}
