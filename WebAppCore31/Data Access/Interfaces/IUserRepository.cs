using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WebAppCore31.Interfaces
{
    public interface IUserRepository<TUser> where TUser : class
    {
        /// <summary>
        /// Return the list of <typeparamref name="TUser"/> in this table
        /// </summary>
        IEnumerable<TUser> GetList();

        /// <summary>
        /// Return item through a specific ID
        /// </summary>
        /// <param name="id">ID of object</param>
        Task<TUser> GetUserByIdAsync(string Id);

        /// <summary>
        /// Create new record of this object in table
        /// </summary>
        /// <param name="item">new object</param>
        Task<IdentityResult> Create(TUser user, string password);

        /// <summary>
        /// Update a selected object in the table
        /// </summary>
        /// <param name="item">object that needs updated</param>
        void Update(TUser item);

        /// <summary>
        /// Delete record of this object in the table
        /// </summary>
        /// <param name="id">ID of this object</param>
        void Delete(string Id);

        /// <summary>
        /// Save changes
        /// </summary>
        /// <returns></returns>
        Task<int> SaveChangesAsync();
    }
}
