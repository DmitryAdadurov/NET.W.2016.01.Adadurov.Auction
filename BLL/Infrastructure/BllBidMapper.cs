using BLL.Interface.Entities;
using DAL.Interface.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Infrastructure
{
    public static class BllBidMapper
    {
        public static BllBid ToBllBid(this DalBid bid)
        {
            if (bid == null)
                return null;
            return new BllBid()
            {
                Id = bid.Id,
                BidDate = bid.DateOfBid,
                Lot = bid.Lot,
                Price = bid.Price,
                User = bid.User,
            };
        }

        public static DalBid ToDalBid(this BllBid bid)
        {
            return new DalBid()
            {
                Id = bid.Id,
                DateOfBid = bid.BidDate,
                Lot = bid.Lot,
                Price = bid.Price,
                User = bid.User
            };
        }
    }
}
