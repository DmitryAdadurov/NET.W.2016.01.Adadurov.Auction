using BLL.Interface.Entities;
using DAL.Interface.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Infrastructure
{
    public static class BllLotMapper
    {
        public static BllLot ToBllLot(this DalLot lot)
        {
            return new BllLot()
            {
                Id = lot.Id,
                AuctionEndDate = lot.Expired,
                Categorie = lot.Categorie,
                CreationDate = lot.DateOfCreation,
                CurrentPrice = lot.Price,
                Description = lot.Description,
                LastUpdateDate = lot.LastUpdated,
                Photos = lot.Photos,
                Seller = lot.Seller,
                Title = lot.Title
            };
        }

        public static DalLot ToDalLot(this BllLot lot)
        {
            return new DalLot()
            {
                Id = lot.Id,
                Categorie = lot.Categorie,
                DateOfCreation = lot.CreationDate,
                Description = lot.Description,
                Expired = lot.AuctionEndDate,
                LastUpdated = lot.LastUpdateDate,
                Photos = lot.Photos,
                Price = lot.CurrentPrice,
                Seller = lot.Seller,
                Title = lot.Title
            };
        }
    }
}
