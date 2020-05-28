using WebAppCore31.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WebAppCore31.Repositories
{
    public class AuthorRepo : IUserRepository<Author>, IDisposable
    {
        private RegisterContext context;
        private UserManager<User> userManager;
        public AuthorRepo(RegisterContext context, UserManager<User> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public Task<IdentityResult> Create(Author user, string password)
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

        public IEnumerable<Author> GetList()
        {
            throw new NotImplementedException();
        }

        public Task<Author> GetUserByIdAsync(string Id)
        {
            throw new NotImplementedException();
        }

        public Task<int> SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        public void Update(Author item)
        {
            throw new NotImplementedException();
        }
    }
}
