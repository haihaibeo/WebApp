using DAL.Interfaces;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class CommentRepo : IRepository<Comment>, IDisposable
    {
        public void Create(Comment item)
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

        public IEnumerable<Comment> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Comment> GetById(string Id)
        {
            throw new NotImplementedException();
        }

        public Task<int> SaveChangeAsync()
        {
            throw new NotImplementedException();
        }

        public void Update(Comment item)
        {
            throw new NotImplementedException();
        }
    }
}
