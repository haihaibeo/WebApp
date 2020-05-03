using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppCore31
{
    public interface IRepository<Type> : IDisposable where Type : class
    {
        IEnumerable<Type> GetAll();
        Type GetById(int id);
        void Create(Type item);
        void Update(Type item);
        void Delete(int id);
        void Save();
    }
}
