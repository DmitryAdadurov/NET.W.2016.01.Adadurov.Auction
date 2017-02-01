using DAL.Interface.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interface.Repository
{
    public interface IRepository<TDalEntity> : IDisposable where TDalEntity : IEntity
    {
        int Count(Expression<Func<TDalEntity, bool>> predicate = null);
        Task<IEnumerable<TDalEntity>> GetAll();
        Task<IEnumerable<TDalEntity>> GetByPredicate(Expression<Func<TDalEntity, bool>> f);
        Task<int> Create(TDalEntity e);
        Task Delete(TDalEntity e);
        Task Update(TDalEntity e);
        Task<bool> IsExist(int id);
        Task<IEnumerable<TDalEntity>> GetRange(int skip, int take = 12, Expression<Func<TDalEntity, bool>> predicate = null);
    }
}
