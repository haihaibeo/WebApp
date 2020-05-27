using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Return the list of <typeparamref name="TEntity"/> in this table
        /// </summary>
        IEnumerable<TEntity> GetAll();

        /// <summary>
        /// Return item through a specific ID
        /// </summary>
        /// <param name="id">ID of object</param>
        Task<TEntity> GetById(string Id);

        /// <summary>
        /// Create new record of this object in table
        /// </summary>
        /// <param name="item">new object</param>
        void Create(TEntity item);

        /// <summary>
        /// Update a selected object in the table
        /// </summary>
        /// <param name="item">object that needs updated</param>
        void Update(TEntity item);

        /// <summary>
        /// Delete record of this object in the table
        /// </summary>
        /// <param name="id">ID of this object</param>
        void Delete(string Id);

        /// <summary>
        /// Save changes
        /// </summary>
        /// <returns></returns>
        Task<int> SaveChangeAsync();
    }
}
