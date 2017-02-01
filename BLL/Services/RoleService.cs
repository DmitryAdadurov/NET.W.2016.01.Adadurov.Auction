using BLL.Interface.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Interface.Entities;
using System.Linq.Expressions;
using DAL.Interface.Repository;
using BLL.Infrastructure;

namespace BLL.Services
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork context;

        public RoleService(IUnitOfWork uow)
        {
            context = uow;
        }

        public async Task<int> Create(BllRole e)
        {
            if (e == null || string.IsNullOrEmpty(e.Name))
                throw new ArgumentNullException(nameof(e));

            await context.RoleStore.CreateAsync(e.ToDalRole());

            return (await context.RoleStore.FindByNameAsync(e.Name)).Id;
        }

        public async Task Delete(BllRole e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));

            await context.RoleStore.DeleteAsync(e.ToDalRole());
        }

        Task<IEnumerable<BllRole>> IService<BllRole, int>.GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<BllRole> GetById(int id)
        {
            return (await context.RoleStore.FindByIdAsync(id)).ToBllRole();
        }

        Task<BllRole> IService<BllRole, int>.GetByPredicate(Expression<Func<BllRole, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public async Task Update(BllRole e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));

            await context.RoleStore.UpdateAsync(e.ToDalRole());
        }

        public async Task<BllRole> FindByNameAsync(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            return (await context.RoleStore.FindByNameAsync(name)).ToBllRole();
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
        // ~RoleService() {
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
