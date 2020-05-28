using WebAppCore31.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WebAppCore31.Repositories
{
    public class CommentRepo : IRepository<Comment>, IDisposable
    {
        private RegisterContext context;

        public CommentRepo(RegisterContext context)
        {
            this.context = context;
        }

        public void Create(Comment item)
        {
            context.Comments.Add(item);
        }

        public void Delete(string Id)
        {
            var item = context.Comments.Find(Id);
            if (item != null) context.Comments.Remove(item);
        }

        public void Dispose()
        {
            context.Dispose();
        }

        public Task<List<Comment>> GetAllAsync()
        {
            return context.Comments.ToListAsync();
        }

        public Task<Comment> GetByIdAsync(string Id)
        {
            return context.Comments.SingleOrDefaultAsync(c => c.Id == Id);
        }

        public Task<int> SaveChangeAsync()
        {
            return context.SaveChangesAsync();
        }

        public void Update(Comment item)
        {
            context.Comments.Update(item);
        }
    }
}
