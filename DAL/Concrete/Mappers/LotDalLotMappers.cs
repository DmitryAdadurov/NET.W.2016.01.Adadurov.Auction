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
    public class LotDalLotMappers : IMapper<Lot, DalLot>
    {
        //public static DalLot ToDalLot(this Lot lot)
        //{
        //    return new DalLot()
        //    {
        //        Id = lot.Id,
        //        Categorie = lot.Category.ToDalCategory(),
        //        Title = lot.Title,
        //        Description = lot.Description,
        //        Photos = lot.Photos,
        //        Price = lot.CurrentPrice,
        //        Seller = lot.User.ToDalUser(),
        //        DateOfCreation = lot.CreationDate,
        //        Expired = lot.ExpirationDate,
        //        Bids = lot.Bids.ToDalBids(),
        //        LastUpdated = lot.LastUpdatedDate
        //    };
        //}

        //public static Lot ToEFLot(this DalLot dalLot)
        //{
        //    return new Lot()
        //    {
        //        Id = dalLot.Id,
        //        Category = dalLot.Categorie.ToEFCategory(),
        //        Title = dalLot.Title,
        //        Description = dalLot.Description,
        //        Photos = dalLot.Photos,
        //        CurrentPrice = dalLot.Price,
        //        User = dalLot.Seller.ToUserEntity(),
        //        CreationDate = dalLot.DateOfCreation,
        //        ExpirationDate = dalLot.Expired,
        //        Bids = dalLot.Bids.ToEFBids(),
        //        LastUpdatedDate = dalLot.LastUpdated
        //    };
        //}
        public DalLot ToDalEntity(Lot entity)
        {
            return new DalLot()
            {
                Id = entity.Id,
                Categorie = entity.CtegorieId,
                Title = entity.Title,
                Description = entity.Description,
                Photos = entity.Photos,
                Price = entity.CurrentPrice,
                Seller = entity.SellerId,
                DateOfCreation = entity.CreationDate,
                Expired = entity.ExpirationDate,
                //Bids = entity.Bids,
                LastUpdated = entity.LastUpdatedDate
            };
        }

        public Lot ToEFEntity(DalLot dalEntity)
        {
            return new Lot()
            {
                Id = dalEntity.Id,
                CtegorieId = dalEntity.Categorie,
                Title = dalEntity.Title,
                Description = dalEntity.Description,
                Photos = dalEntity.Photos,
                CurrentPrice = dalEntity.Price,
                SellerId = dalEntity.Seller,
                CreationDate = dalEntity.DateOfCreation,
                ExpirationDate = dalEntity.Expired,
                //Bids = dalEntity.Bids.ToEFBids(),
                LastUpdatedDate = dalEntity.LastUpdated
            };
        }
    }
}
