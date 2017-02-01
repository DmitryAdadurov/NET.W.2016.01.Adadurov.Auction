using DAL.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Interface.Entities;
using System.Linq.Expressions;
using System.Data.Entity;
using ORM.EF;
using DAL.Concrete.Mappers;

namespace DAL.Concrete
{
    public sealed class BidsRepositry : GenericRepository<Bid, DalBid>, IBidsRepository
    {
        public BidsRepositry(DbContext dbContext) : base(dbContext, new BidDalBidMapper())
        {
        }

        public async Task DeleteRange(IEnumerable<DalBid> bids)
        {
            if (bids == null)
                throw new ArgumentNullException(nameof(bids));

            IEnumerable<int> ids = bids.Select(t => t.Id);
            IEnumerable<Bid> dbBids = context.Set<Bid>().Where(t => ids.Contains(t.Id));
            context.Set<Bid>().RemoveRange(dbBids);
        }

        public async Task<DalBid> FindLastLotBid(int lotId)
        {
            return (await FindLotBids(lotId)).LastOrDefault();
        }

        public async Task<IEnumerable<DalBid>> FindLotBids(int lotId)
        {
            return (await context.Set<Bid>().Where(t => t.LotId == lotId).OrderBy(t => t.BidDate).ToListAsync()).ConvertAll(converter.ToDalEntity);
        }

        public DalBid GetById(int key)
        {
            return converter.ToDalEntity(context.Set<Bid>().FirstOrDefault(t => t.Id == key));
        }
    }

}
