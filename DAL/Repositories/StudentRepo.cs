using DAL.DataContext;
using DAL.Interfaces;
using DAL.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class StudentRepo : IUserRepository<Student>, IDisposable
    {
        private RegisterContext context;
        private UserManager<User> userManager;
        public StudentRepo(RegisterContext context, UserManager<User> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public Task<IdentityResult> Create(Student user, string password)
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

        public IEnumerable<Student> GetList()
        {
            throw new NotImplementedException();
        }

        public Task<Student> GetUserById(string Id)
        {
            throw new NotImplementedException();
        }

        public Task<int> SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        public void Update(Student item)
        {
            throw new NotImplementedException();
        }
    }
}
