using DAL.Interface.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interface.Repository
{
    public interface IBidsRepository : IRepository<DalBid>
    {
        Task DeleteRange(IEnumerable<DalBid> bids);
        Task<IEnumerable<DalBid>> FindLotBids(int lotId);
        Task<DalBid> FindLastLotBid(int lotId);
    }
}
