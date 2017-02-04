using BLL.Infrastructure;
using BLL.Interface.Entities;
using BLL.Interface.Services;
using DAL.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using DAL.Interface.Entities;
using Common.Expressions.PropertyDictionaries;
using Common.Expressions;

namespace BLL.Services
{
    public class BidService : IBidService
    {
        private readonly IUnitOfWork context;

        public BidService(IUnitOfWork uow)
        {
            context = uow ?? throw new ArgumentNullException(nameof(uow));
        }

        /// <summary>
        /// Count of entities matching expression
        /// </summary>
        /// <param name="predicate">Expression</param>
        /// <returns>Count of entities</returns>
        public int Count(Expression<Func<BllBid, bool>> predicate = null)
        {
            if (predicate != null)
            {
                var param = Expression.Parameter(typeof(DalBid));
                IDictionary<string, string> mapperDictionary;
                PropertyMapperDictionaries.TryGetMapperDictionary(typeof(BllBid), out mapperDictionary);
                var result = new ExpressionConverter<BllBid, DalBid>(param, mapperDictionary).Visit(predicate.Body);
                Expression<Func<DalBid, bool>> lambda = Expression.Lambda<Func<DalBid, bool>>(result, param);
                return context.BidsRepository.Count(lambda);
            }
            return context.BidsRepository.Count();
        }

        /// <summary>
        /// Get range of bids
        /// </summary>
        /// <param name="skip">Number of skiped bids matched the predicate</param>
        /// <param name="take">Number of bids to take</param>
        /// <param name="predicate">Expression to match</param>
        /// <returns>Enumeration with bids matching the predicate</returns>
        public async Task<IEnumerable<BllBid>> GetRange(int skip, int take = 12, Expression<Func<BllBid, bool>> predicate = null)
        {
            Expression<Func<DalBid, bool>> lambda = null;
            if (predicate != null)
            {
                var param = Expression.Parameter(typeof(DalBid));
                IDictionary<string, string> mapperDictionary;
                PropertyMapperDictionaries.TryGetMapperDictionary(typeof(BllBid), out mapperDictionary);
                var result = new ExpressionConverter<BllBid, DalBid>(param, mapperDictionary).Visit(predicate.Body);
                lambda = Expression.Lambda<Func<DalBid, bool>>(result, param);
            }
            return (await context.BidsRepository.GetRange(skip, take, lambda)).Select(t => t.ToBllBid());
        }

        /// <summary>
        /// Place a bet
        /// </summary>
        /// <param name="auctionId">Id of the auction</param>
        /// <param name="userId">Id the user</param>
        /// <param name="moneyAmount">Count of money</param>
        /// <returns>true - if succeded</returns>
        public async Task<bool> PlaceBet(int auctionId, int userId, decimal moneyAmount)
        {
            if (userId < 0)
                throw new ArgumentOutOfRangeException(nameof(userId));

            var user = (await context.UserStore.FindByIdAsync(userId)).ToBllUser();

            if (user == null)
                throw new ArgumentOutOfRangeException(nameof(userId));

            return await PlaceBet(auctionId, user.Login, moneyAmount);
        }

        /// <summary>
        /// Place a bet
        /// </summary>
        /// <param name="auctionId">Id of the auction</param>
        /// <param name="userName">User login</param>
        /// <param name="moneyAmount">Count of money</param>
        /// <returns>true - if succeded</returns>
        public async Task<bool> PlaceBet(int auctionId, string userName, decimal moneyAmount)
        {
            if (auctionId < 0)
                throw new ArgumentOutOfRangeException(nameof(auctionId));

            if (string.IsNullOrEmpty(userName))
                throw new ArgumentNullException(nameof(userName));

            if (moneyAmount < 0)
                throw new ArgumentOutOfRangeException(nameof(moneyAmount));

            if (await context.LotsRepository.IsExist(auctionId) && await context.UserStore.IsExistingUserAsync(userName))
            {
                BllBid bid = (await context.BidsRepository.FindLastLotBid(auctionId)).ToBllBid();

                // if bid equals null that mean that current bid is the first bid
                if (bid == null)
                {
                    await context.BidsRepository.Create(new DalBid()
                    {
                        DateOfBid = DateTime.Now,
                        Price = moneyAmount,
                        User = (await context.UserStore.FindByNameAsync(userName)).Id,
                        Lot = auctionId
                    });
                }
                else
                {

                    if (bid.Price < moneyAmount)
                    {
                        await context.BidsRepository.Create(new DalBid()
                        {
                            DateOfBid = DateTime.Now,
                            Price = moneyAmount,
                            User = (await context.UserStore.FindByNameAsync(userName)).Id,
                            Lot = auctionId
                        });
                    }
                    else
                    {
                        return false;
                    }
                }

                context.Commit();
                var lot = await context.LotsRepository.GetById(auctionId);
                lot.Price = moneyAmount;
                await context.LotsRepository.Update(lot);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Removes bet with specified id
        /// </summary>
        /// <param name="bidId">Id of the bid</param>
        /// <returns></returns>
        public async Task RemoveBet(int bidId)
        {
            if (bidId < 0)
                throw new ArgumentOutOfRangeException(nameof(bidId));

            if (!await context.BidsRepository.IsExist(bidId))
                throw new ArgumentOutOfRangeException(nameof(bidId));

            var dbBid = (await context.BidsRepository.GetByPredicate(t => t.Id == bidId)).FirstOrDefault();

            await context.BidsRepository.Delete(dbBid);
        }

        async Task<int> IService<BllBid, int>.Create(BllBid e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));

            await PlaceBet(e.Lot, e.User, e.Price);
            return -1;
        }

        async Task IService<BllBid, int>.Delete(BllBid e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));

            await RemoveBet(e.Id);
        }

        /// <summary>
        /// Get all bids
        /// </summary>
        /// <returns>Enumeration with all bids</returns>
        public async Task<IEnumerable<BllBid>> GetAll()
        {
            return (await context.BidsRepository.GetAll()).Select(t => t.ToBllBid());
        }

        /// <summary>
        /// Get bet by id
        /// </summary>
        /// <param name="id">Id of the bet</param>
        /// <returns>BllBid if id is correct</returns>
        public async Task<BllBid> GetById(int id)
        {
            if (id < 0)
                throw new ArgumentOutOfRangeException(nameof(id));

            if (!await context.BidsRepository.IsExist(id))
                throw new ArgumentOutOfRangeException(nameof(id));
            else
                return (await context.BidsRepository.GetByPredicate(t => t.Id == id)).FirstOrDefault().ToBllBid();
        }

        public Task<BllBid> GetByPredicate(Expression<Func<BllBid, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Update the bid
        /// </summary>
        /// <param name="e">Bid with new info</param>
        public async Task Update(BllBid e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));

            if (e.Id < 0)
                throw new ArgumentOutOfRangeException(nameof(e.Id));

            if (!await context.BidsRepository.IsExist(e.Id))
                throw new ArgumentOutOfRangeException(nameof(e));

            await context.BidsRepository.Update(e.ToDalBid());
        }

        #region IDisposable Support
        private bool disposedValue = false; 

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    context.Dispose();
                }                

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion


    }
}
