using BLL.Interface.Services;
using DAL.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Interface.Entities;
using System.Linq.Expressions;
using BLL.Infrastructure;
using DAL.Interface.Entities;
using Common.Expressions.PropertyDictionaries;
using Common.Expressions;

namespace BLL.Services
{
    public class AuctionsService : IAuctionService<int>
    {
        private IUnitOfWork Context { get; set; }

        public AuctionsService(IUnitOfWork uow)
        {
            Context = uow ?? throw new ArgumentNullException(nameof(uow));
        }

        /// <summary>
        /// Count of entities matching expression
        /// </summary>
        /// <param name="predicate">Expression</param>
        /// <returns>Count of entities</returns>
        public int Count(Expression<Func<BllLot, bool>> predicate = null)
        {
            if (predicate != null)
            {
                var param = Expression.Parameter(typeof(DalLot));
                IDictionary<string, string> mapperDictionary;
                PropertyMapperDictionaries.TryGetMapperDictionary(typeof(BllLot), out mapperDictionary);
                var result = new ExpressionConverter<BllLot, DalLot>(param, mapperDictionary).Visit(predicate.Body);
                Expression<Func<DalLot, bool>> lambda = Expression.Lambda<Func<DalLot, bool>>(result, param);
                return Context.LotsRepository.Count(lambda);
            }
            return Context.LotsRepository.Count();
        }

        /// <summary>
        /// Combining lot info, bids and comments into one object
        /// </summary>
        /// <param name="id">Id of a lot</param>
        /// <param name="take">Number of comments to take</param>
        /// <returns>Total info about auction</returns>
        public async Task<BllAuction> GetTotalAuctionInfo(int id, int take)
        {
            if (id < 0)
                throw new ArgumentOutOfRangeException(nameof(id));

            var auction = new BllAuction();
            auction.Lot = (await Context.LotsRepository.GetById(id)).ToBllLot();
            auction.Bids = (await Context.BidsRepository.FindLotBids(id)).Select(t => t.ToBllBid());
            if (take > 0)
                auction.Comments = (await Context.CommentsRepository.FindAllLotComments(id)).OrderByDescending(t => t.Date).Take(take).Select(t => t.ToBllComment());
            else
                auction.Comments = (await Context.CommentsRepository.FindAllLotComments(id)).OrderByDescending(t => t.Date).Select(t => t.ToBllComment());
            return auction;
        }

        /// <summary>
        /// Get lot info
        /// </summary>
        /// <param name="id">Id of the lot</param>
        /// <returns>BllLot if id correct</returns>
        public async Task<BllLot> GetLot(int id)
        {
            if (id < 0)
                throw new ArgumentOutOfRangeException(nameof(id));

            return (await Context.LotsRepository.GetById(id)).ToBllLot();
        }

        async Task<BllAuction> IService<BllAuction, int>.GetById(int id)
        {
            return new BllAuction()
            {
                Lot = await GetLot(id)
            };
        }

        /// <summary>
        /// Get lots matching predicate
        /// </summary>
        /// <param name="predicate">Expression to match</param>
        /// <returns></returns>
        public async Task<IEnumerable<BllLot>> GetByPredicate(Expression<Func<BllLot, bool>> predicate)
        {
            var param = Expression.Parameter(typeof(DalLot));
            IDictionary<string, string> mapperDictionary;
            PropertyMapperDictionaries.TryGetMapperDictionary(typeof(BllLot), out mapperDictionary);
            var result = new ExpressionConverter<BllLot, DalLot>(param, mapperDictionary).Visit(predicate.Body);
            Expression<Func<DalLot, bool>> lambda = Expression.Lambda<Func<DalLot, bool>>(result, param);
            IEnumerable<BllLot> l;

            try
            {
                l = (await Context.LotsRepository.GetByPredicate(lambda)).Select(t => t.ToBllLot());
            }
            catch
            {
                throw new InvalidOperationException();
            }

            return l;
        }

        Task<BllAuction> IService<BllAuction, int>.GetByPredicate(Expression<Func<BllAuction, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get all lots
        /// </summary>
        /// <returns>Collection with lots</returns>
        public async Task<IEnumerable<BllLot>> GetAll()
        {
            return (await Context.LotsRepository.GetAll()).Select(t => t.ToBllLot());
        }

        async Task<IEnumerable<BllAuction>> IService<BllAuction, int>.GetAll()
        {
            var lots = await GetAll();
            var auctions = new BllAuction[lots.Count()];
            IEnumerator<BllLot> enumerator = lots.GetEnumerator();
            for (int i = 0; i < auctions.Length; i++)
            {
                auctions[i] = new BllAuction() { Lot = enumerator.Current };
                enumerator.MoveNext();
            }
            return auctions;
        }

        /// <summary>
        /// Create lot
        /// </summary>
        /// <param name="e">Lot to create</param>
        /// <returns>Id of the created lot</returns>
        public async Task<int> Create(BllLot e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));

            return await Context.LotsRepository.Create(e.ToDalLot());
        }

        /// <summary>
        /// Create lot
        /// </summary>
        /// <param name="e">Lot to create</param>
        /// <returns>Id of the created lot</returns>
        async Task<int> IService<BllAuction, int>.Create(BllAuction e)
        {
            return await Create(e.Lot);
        }

        /// <summary>
        /// Delete all info about auction (inc. comments, bids)
        /// </summary>
        /// <param name="e">Auction to delete</param>
        public async Task Delete(BllAuction e)
        {
            if (e == null || e.Lot == null)
                throw new ArgumentNullException(nameof(e));

            if(e.Comments != null)
                await Context.CommentsRepository.DeleteRange(e.Comments.Select(t => t.ToDalCommentary()));
            if (e.Bids != null)
                await Context.BidsRepository.DeleteRange(e.Bids.Select(t => t.ToDalBid()));
            if (e.Lot != null)
                await Context.LotsRepository.Delete(e.Lot.ToDalLot());
            Context.Commit();
        }

        /// <summary>
        /// Update lot info
        /// </summary>
        /// <param name="e">Lot with new information</param>
        public async Task Update(BllLot e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));

            var dbLot = await Context.LotsRepository.GetById(e.Id);
            if (dbLot != null)
                await Context.LotsRepository.Update(e.ToDalLot());
            else
                throw new ArgumentOutOfRangeException(nameof(e));
        }

        /// <summary>
        /// Update auction info
        /// </summary>
        /// <param name="e">Auction to update</param>
        public async Task Update(BllAuction e)
        {
            if (e == null || e.Lot == null)
                throw new ArgumentNullException(nameof(e));

            await Update(e.Lot);
        }

        /// <summary>
        /// Get range of lots
        /// </summary>
        /// <param name="skip">Number of skiped lots matched the predicate</param>
        /// <param name="take">Number of lots to take</param>
        /// <param name="predicate">Expression to match</param>
        /// <returns>Enumeration with lots matching the predicate</returns>
        public async Task<IEnumerable<BllLot>> GetRange(int skip, int take = 12, Expression<Func<BllLot, bool>> predicate = null)
        {
            Expression<Func<DalLot, bool>> lambda = null;
            if (predicate != null)
            {
                var param = Expression.Parameter(typeof(DalLot));
                IDictionary<string, string> mapperDictionary;
                PropertyMapperDictionaries.TryGetMapperDictionary(typeof(BllLot), out mapperDictionary);
                var result = new ExpressionConverter<BllLot, DalLot>(param, mapperDictionary).Visit(predicate.Body);
                lambda = Expression.Lambda<Func<DalLot, bool>>(result, param);
            }
            return (await Context.LotsRepository.GetRange(skip, take, lambda)).Select(t => t.ToBllLot());
        }

        #region IDisposable Support
        private bool disposedValue = false; // Для определения избыточных вызовов

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: освободить управляемое состояние (управляемые объекты).
                }

                // TODO: освободить неуправляемые ресурсы (неуправляемые объекты) и переопределить ниже метод завершения.
                // TODO: задать большим полям значение NULL.

                disposedValue = true;
            }
        }

        // TODO: переопределить метод завершения, только если Dispose(bool disposing) выше включает код для освобождения неуправляемых ресурсов.
        // ~AuctionsService() {
        //   // Не изменяйте этот код. Разместите код очистки выше, в методе Dispose(bool disposing).
        //   Dispose(false);
        // }

        // Этот код добавлен для правильной реализации шаблона высвобождаемого класса.
        public void Dispose()
        {
            // Не изменяйте этот код. Разместите код очистки выше, в методе Dispose(bool disposing).
            Dispose(true);
            // TODO: раскомментировать следующую строку, если метод завершения переопределен выше.
            // GC.SuppressFinalize(this);
        }

        #endregion
    }
}
