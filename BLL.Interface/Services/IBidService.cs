using BLL.Interface.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interface.Services
{
    public interface IBidService : IService<BllBid, int>
    {
        int Count(Expression<Func<BllBid, bool>> predicate = null);
        Task<bool> PlaceBet(int auctionId, string userName, decimal moneyAmount);
        Task<bool> PlaceBet(int auctionId, int userid, decimal moneyAmount);
        Task RemoveBet(int bidId);
        Task<IEnumerable<BllBid>> GetRange(int skip, int take = 12, Expression<Func<BllBid, bool>> predicate = null);
    }
}
