using BLL.Interface.Entities;
using BLL.Interface.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using UI.Mvc.Models;

namespace UI.Mvc.Infrastructure.Mappers
{
    public static class AuctionDetailMapper
    {
        public static AuctionDetail ToMvcAuctionDetail(this BllAuction auction, IUserService userService)
        {
            var returnValue = new AuctionDetail()
            {
                AuctionEndDate = auction.Lot.AuctionEndDate,
                CreationDate = auction.Lot.CreationDate,
                CurrentPrice = auction.Lot.CurrentPrice,
                Description = auction.Lot.Description,
                LastUpdateDate = auction.Lot.LastUpdateDate,
                Id = auction.Lot.Id,
                Photos = auction.Lot?.Photos?.Split(':') ?? Enumerable.Empty<string>(),
                SellerId = auction.Lot.Seller,
                Title = auction.Lot.Title,
                Bids = auction?.Bids.Select(t => t.ToMvcBid(userService)),
                Comments = auction?.Comments.Select(t => t.ToMvcComment(userService))
            };

            Task<BllUser> task = Task.Run(() => userService.GetById(auction.Lot.Seller));
            task.Wait();
            returnValue.Seller = task.Result.Login;
            return returnValue;
        }
    }
}