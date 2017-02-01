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
        public Task CreateAsync(DalRole role)
        {
            if (role == null)
                throw new ArgumentNullException(nameof(role));

            context.Set<Role>().Add(role.ToRoleEntity());
            return Task.FromResult<object>(null);
        }

        public Task DeleteAsync(DalRole role)
        {
            if (role == null)
                throw new ArgumentNullException(nameof(role));

            context.Set<Role>().Remove(role.ToRoleEntity());
            return Task.FromResult<object>(null);
        }

        public async Task<DalRole> FindByIdAsync(int roleId)
        {
            if (roleId < 0)
                throw new ArgumentOutOfRangeException(nameof(roleId));

            return (await context.Set<Role>().SingleOrDefaultAsync(r => r.Id == roleId)).ToDalRole();
        }

        public async Task<DalRole> FindByNameAsync(string roleName)
        {
            return (await context.Set<Role>().SingleOrDefaultAsync(r => r.Name == roleName)).ToDalRole();
        }

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
        private bool disposedValue = false; // Для определения избыточных вызовов

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: освободить управляемое состояние (управляемые объекты).
                }

                // TODO: освободить неуправляемые ресурсы (неуправляемые объекты) и переопределить ниже метод завершения.
                // TODO: задать большим полям значение NULL.

                disposedValue = true;
            }
        }

        // TODO: переопределить метод завершения, только если Dispose(bool disposing) выше включает код для освобождения неуправляемых ресурсов.
        // ~DalRoleStore() {
        //   // Не изменяйте этот код. Разместите код очистки выше, в методе Dispose(bool disposing).
        //   Dispose(false);
        // }

        // Этот код добавлен для правильной реализации шаблона высвобождаемого класса.
        public void Dispose()
        {
            // Не изменяйте этот код. Разместите код очистки выше, в методе Dispose(bool disposing).
            Dispose(true);
            // TODO: раскомментировать следующую строку, если метод завершения переопределен выше.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
