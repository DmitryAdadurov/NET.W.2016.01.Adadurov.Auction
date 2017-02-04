using BLL.Interface.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Interface.Entities;
using System.Linq.Expressions;
using DAL.Interface.Repository;
using BLL.Infrastructure;
using DAL.Interface.Entities;
using Common.Expressions.PropertyDictionaries;
using Common.Expressions;

namespace BLL.Services
{
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork context;
        public CommentService(IUnitOfWork uow)
        {
            context = uow ?? throw new ArgumentNullException(nameof(uow));
        }

        /// <summary>
        /// Get range of comments
        /// </summary>
        /// <param name="skip">Number of skiped comments matched the predicate</param>
        /// <param name="take">Number of comments to take</param>
        /// <param name="predicate">Expression to match</param>
        /// <returns>Enumeration with comments matching the predicate</returns>
        public async Task<IEnumerable<BllComment>> GetRange(int skip, int take = 12, Expression<Func<BllComment, bool>> predicate = null)
        {
            Expression<Func<DalCommentary, bool>> lambda = null;
            if (predicate != null)
            {
                var param = Expression.Parameter(typeof(DalCommentary));
                IDictionary<string, string> mapperDictionary;
                PropertyMapperDictionaries.TryGetMapperDictionary(typeof(BllComment), out mapperDictionary);
                var result = new ExpressionConverter<BllComment, DalCommentary>(param, mapperDictionary).Visit(predicate.Body);
                lambda = Expression.Lambda<Func<DalCommentary, bool>>(result, param);
            }
            return (await context.CommentsRepository.GetRange(skip, take, lambda)).Select(t => t.ToBllComment());
        }

        /// <summary>
        /// Add comment
        /// </summary>
        /// <param name="text">Comment text</param>
        /// <param name="userName">User login</param>
        /// <param name="lotId">Id of the lot</param>
        /// <returns>Id of added comment</returns>
        public async Task<int> PostComment(string text, string userName, int lotId)
        {
            if (string.IsNullOrEmpty(text))
                throw new ArgumentNullException(nameof(text));

            if (string.IsNullOrEmpty(userName))
                throw new ArgumentNullException(nameof(userName));

            var user = (await context.UserStore.FindByNameAsync(userName)).ToBllUser();

            var comment = new BllComment()
            {
                Date = DateTime.Now,
                Lot = lotId,
                Text = text,
                User = user.Id
            };

            return await context.CommentsRepository.Create(comment.ToDalCommentary());
        }

        async Task<int> IService<BllComment, int>.Create(BllComment e)
        {
            string userName = (await context.UserStore.FindByIdAsync(e.User)).UserName;
            return await PostComment(e.Text, userName, e.Lot);
        }

        /// <summary>
        /// Delete comment
        /// </summary>
        /// <param name="commentId">Id of the comment</param>
        public async Task Delete(int commentId)
        {
            var comment = (await context.CommentsRepository.GetByPredicate(t => t.Id == commentId)).FirstOrDefault();
            await context.CommentsRepository.Delete(comment);
            context.Commit();
        }

        async Task IService<BllComment, int>.Delete(BllComment e)
        {
            await Delete(e.Id);
        }

        async Task<IEnumerable<BllComment>> IService<BllComment, int>.GetAll()
        {
            return (await context.CommentsRepository.GetAll()).Select(t => t.ToBllComment());
        }

        /// <summary>
        /// Get comment by id
        /// </summary>
        /// <param name="id">Id of the comment</param>
        /// <returns>BllComment if id is correct</returns>
        public async Task<BllComment> GetById(int id)
        {
            return (await context.CommentsRepository.GetByPredicate(t => t.Id == id)).FirstOrDefault().ToBllComment();
        }

        public Task<BllComment> GetByPredicate(Expression<Func<BllComment, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Update comment
        /// </summary>
        /// <param name="e">Updated comment entity</param>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task Update(BllComment e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));

            await context.CommentsRepository.Update(e.ToDalCommentary());
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
        // ~CommentService() {
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
