using DAL.DataContext;
using DAL.Interfaces;
using DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class UserRepo : IUserRepository<User>, IDisposable
    {
        private RegisterContext context;
        private UserManager<User> userManager;
        public UserRepo(RegisterContext context, UserManager<User> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public Task<IdentityResult> Create(User user, string password)
        {
            return userManager.CreateAsync(user, password);
        }

        public void Delete(string Id)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            context.Dispose();
        }

        public IEnumerable<User> GetList()
        {
            throw new NotImplementedException();
        }

        public Task<User> GetUserById(string Id)
        {
            return context.Users.SingleOrDefaultAsync(u => u.Id == Id);
        }

        public Task<int> SaveChangesAsync()
        {
            return context.SaveChangesAsync();
        }

        public void Update(User item)
        {
            throw new NotImplementedException();
        }
    }
}
