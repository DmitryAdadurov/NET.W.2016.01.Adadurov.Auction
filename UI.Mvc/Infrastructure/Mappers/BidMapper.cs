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
    public static class BidMapper
    {
        public static Bid ToMvcBid(this BllBid bllBid, IUserService userService)
        {
            var bid = new Bid()
            {
                Id = bllBid.Id,
                BidDate = bllBid.BidDate,
                MoneyAmount = bllBid.Price,
                LotId = bllBid.Lot
            };

            Task<BllUser> task = Task.Run(() => userService.GetById(bllBid.User));
            task.Wait();
            bid.UserName = task.Result.Login;
            return bid;
        }
    }
}