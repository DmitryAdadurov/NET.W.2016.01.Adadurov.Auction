using DAL.Interface;
using DAL.Interface.Entities;
using ORM.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Concrete.Mappers
{
    public class BidDalBidMapper : IMapper<Bid, DalBid>
    {
        //public static DalBid ToDalBid(Bid bid)
        //{
        //    return new DalBid()
        //    {
        //        Id = bid.Id,
        //        DateOfBid = bid.BidDate,
        //        Lot = bid.Lot.ToDalLot(),
        //        Price = bid.Price,
        //        User = bid.User.ToDalUser()
        //    };
        //}

        //public static Bid ToEFBid(DalBid dalBid)
        //{
        //    return new Bid()
        //    {
        //        Id = dalBid.Id,
        //        BidDate = dalBid.DateOfBid,
        //        Lot = dalBid.Lot.ToEFLot(),
        //        Price = dalBid.Price,
        //        User = dalBid.User.ToUserEntity()
        //    };
        //}

        //public static ICollection<DalBid> ToDalBids(ICollection<Bid> bids)
        //{
        //    return bids.Select(t => t.ToDalBid()).ToList();
        //}

        //public static ICollection<Bid> ToEFBids(ICollection<DalBid> bids)
        //{
        //    return bids.Select(t => t.ToEFBid()).ToList();
        //}

        public Bid ToEFEntity(DalBid dalEntity)
        {
            return new Bid()
            {
                Id = dalEntity.Id,
                BidDate = dalEntity.DateOfBid,
                LotId = dalEntity.Lot,
                Price = dalEntity.Price,
                UserId = dalEntity.User
            };
        }

        public DalBid ToDalEntity(Bid entity)
        {
            return new DalBid()
            {
                Id = entity.Id,
                DateOfBid = entity.BidDate,
                Lot = entity.LotId,
                Price = entity.Price,
                User = entity.UserId
            };
        }
    }
}
