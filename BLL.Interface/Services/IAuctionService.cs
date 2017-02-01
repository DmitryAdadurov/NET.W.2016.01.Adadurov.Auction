using BLL.Interface.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interface.Services
{
    public interface IAuctionService<TKey> : IService<BllAuction, TKey>
    {
        int Count(Expression<Func<BllLot, bool>> predicate = null);
        Task<BllAuction> GetTotalAuctionInfo(TKey id, int take);
        Task<BllLot> GetLot(TKey id);
        Task<int> Create(BllLot e);
        Task<IEnumerable<BllLot>> GetRange(int skip, int take = 12, Expression<Func<BllLot, bool>> predicate = null);
        Task<IEnumerable<BllLot>> GetByPredicate(Expression<Func<BllLot, bool>> predicate);
    }
}
