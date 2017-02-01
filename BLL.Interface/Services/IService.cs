using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interface.Services
{
    public interface IService<TBllEntity, TKey> : IDisposable
    {
        Task<TBllEntity> GetById(TKey id);
        Task<TBllEntity> GetByPredicate(Expression<Func<TBllEntity, bool>> predicate);
        Task<IEnumerable<TBllEntity>> GetAll();
        Task<int> Create(TBllEntity e);
        Task Delete(TBllEntity e);
        Task Update(TBllEntity e);
    }
}
