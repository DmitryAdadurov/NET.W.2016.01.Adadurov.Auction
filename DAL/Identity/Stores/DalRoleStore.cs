using Microsoft.AspNet.Identity;
using ORM.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Concrete;
using DAL.Interface.Identity.Entities;

namespace DAL.Identity.Stores
{
    public class DalRoleStore : IRoleStore<DalRole, int>
    {
        private readonly DbContext context;
        public DalRoleStore(DbContext dbcontext)
        {
            context = dbcontext;
        }

        /// <summary>
        /// Creates role.
        /// </summary>
        /// <param name="role">Role to add</param>
        /// <returns></returns>
        public Task CreateAsync(DalRole role)
        {
            if (role == null)
                throw new ArgumentNullException(nameof(role));

            context.Set<Role>().Add(role.ToRoleEntity());
            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Delete role
        /// </summary>
        /// <param name="role">Role to delete</param>
        /// <returns></returns>
        public Task DeleteAsync(DalRole role)
        {
            if (role == null)
                throw new ArgumentNullException(nameof(role));

            context.Set<Role>().Remove(role.ToRoleEntity());
            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Searching for role with specified id
        /// </summary>
        /// <param name="roleId">id of role</param>
        /// <returns>DalRole if searching succsess</returns>
        public async Task<DalRole> FindByIdAsync(int roleId)
        {
            if (roleId < 0)
                throw new ArgumentOutOfRangeException(nameof(roleId));

            return (await context.Set<Role>().SingleOrDefaultAsync(r => r.Id == roleId)).ToDalRole();
        }

        /// <summary>
        /// Searching for role by name
        /// </summary>
        /// <param name="roleName">Name of Role</param>
        /// <returns>DalRole if searching succsess</returns>
        public async Task<DalRole> FindByNameAsync(string roleName)
        {
            return (await context.Set<Role>().SingleOrDefaultAsync(r => r.Name == roleName)).ToDalRole();
        }

        /// <summary>
        /// Updates role
        /// </summary>
        /// <param name="role">Object with updated information</param>
        /// <returns></returns>
        public Task UpdateAsync(DalRole role)
        {
            if (role == null)
                throw new ArgumentNullException(nameof(role));

            var roleEntity = role.ToRoleEntity();
            context.Set<Role>().Attach(roleEntity);
            context.Entry<Role>(roleEntity).State = EntityState.Modified;
            return Task.FromResult<object>(null);
        }

        #region IDisposable Support
        private bool disposedValue = false; 

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    context.Dispose();
                }

                disposedValue = true;
            }
        }
        
        public void Dispose()
        {            
            Dispose(true);
        }
        #endregion
    }
}
