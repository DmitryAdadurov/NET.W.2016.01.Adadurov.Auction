using BLL.Interface.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UI.Mvc.Models;

namespace UI.Mvc.Infrastructure.Mappers
{
    public static class LotMapper
    {
        public static LotModel ToLotMvc (this BllLot lot)
        {
            return new LotModel()
            {
                Categorie = lot.Categorie.ToString(),
                CreationDate = lot.CreationDate,
                CurrentPrice = lot.CurrentPrice,
                Description = lot.Description,
                EndDate = lot.AuctionEndDate,
                Id = lot.Id,
                LastUpdatedDate = lot.LastUpdateDate,
                Photos = lot.Photos?.Split(':') ?? Enumerable.Empty<string>(),
                Title = lot.Title
            };
        }
    }
}