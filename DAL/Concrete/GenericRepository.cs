using Common.Expressions;
using Common.Expressions.PropertyDictionaries;
using DAL.Concrete.Exceptions;
using DAL.Interface;
using DAL.Interface.Entities;
using DAL.Interface.Repository;
using ORM.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DAL.Concrete
{
    public abstract class GenericRepository<TEntity, TDalEntity> : IRepository<TDalEntity>
        where TEntity : class, IOrmEntity<int>
        where TDalEntity : class, IEntity
    {
        protected internal readonly DbContext context;
        protected internal readonly IMapper<TEntity, TDalEntity> converter;

        public GenericRepository(DbContext dbContext, IMapper<TEntity, TDalEntity> mapper)
        {
            if (dbContext.IsNull())
                throw new ArgumentNullException(nameof(dbContext));

            if (mapper.IsNull())
                throw new ArgumentNullException(nameof(mapper));

            context = dbContext;
            converter = mapper;

            if (!Exists())
                throw new WrongEntityException(typeof(TEntity).Name);

            Set = context.Set<TEntity>();
        }

        public int Count(Expression<Func<TDalEntity, bool>> predicate = null)
        {
            if (predicate == null)
            {
                return Set.Count();
            }

            var lambda = ConvertExpression(predicate);
            return Set.Count(lambda);
        }

        private DbSet<TEntity> Set { get; set; }

        public async Task<int> Create(TDalEntity e)
        {
            if (e.IsNull())
                throw new ArgumentNullException(nameof(e));

            TEntity efEntity = converter.ToEFEntity(e);

            Set.Add(efEntity);
            await context.SaveChangesAsync();
            return efEntity.Id;
        }

        public Task Delete(TDalEntity e)
        {
            if (e.IsNull())
                throw new ArgumentNullException(nameof(e));

            TEntity dbEntity = Set.FirstOrDefault(t => t.Id == e.Id);

            Set.Remove(dbEntity);
            return Task.FromResult<object>(null);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<TDalEntity>> GetAll()
        {
            return (await Set.ToListAsync()).ConvertAll(converter.ToDalEntity);
        }

        public async Task<IEnumerable<TDalEntity>> GetByPredicate(Expression<Func<TDalEntity, bool>> f)
        {
            var lambda = ConvertExpression(f);
            IEnumerable<TDalEntity> y;

            try
            {
                y = (await context.Set<TEntity>().Where(lambda).ToListAsync()).ConvertAll(converter.ToDalEntity);
            }
            catch
            {
                throw new BadPredicateException(nameof(f));
            }

            return y;
        }

        public async Task Update(TDalEntity e)
        {
            if (e.IsNull())
                throw new ArgumentNullException(nameof(e));

            var dbEntity = await Set.FindAsync(e.Id);
            context.Entry<TEntity>(dbEntity).CurrentValues.SetValues(converter.ToEFEntity(e));
            await context.SaveChangesAsync();
        }

        public async Task<bool> IsExist(int id)
        {
            if (id < 0)
                throw new ArgumentOutOfRangeException(nameof(id));

            var dbEntity = await GetByPredicate(t => t.Id == id);

            if (dbEntity != null)
                return true;
            else
                return false;
        }

        public async Task<IEnumerable<TDalEntity>> GetRange(int skip, int take = 12, Expression<Func<TDalEntity, bool>> predicate = null)
        {
            if (skip < 0)
                skip = 0;

            if (take < 0)
                take = 12;

            if (predicate == null)
            {
                return (await Set.OrderByDescending(t => t.Id).Skip(skip).Take(take).ToListAsync()).ConvertAll(converter.ToDalEntity);
            }
            else
            {
                var lambda = ConvertExpression(predicate);
                return (await context.Set<TEntity>().Where(lambda).OrderByDescending(t => t.Id).Skip(skip).Take(take).ToListAsync()).ConvertAll(converter.ToDalEntity);
            }
        }

        #region Private Service Methods
        private bool Exists()
        {
            string entityName = typeof(TEntity).Name;
            ObjectContext objectContext = ((IObjectContextAdapter)context).ObjectContext;
            MetadataWorkspace workspace = objectContext.MetadataWorkspace;
            return workspace.GetItems<EntityType>(DataSpace.CSpace).Any(t => t.Name == entityName);
        }

        private Expression<Func<TEntity, bool>> ConvertExpression(Expression<Func<TDalEntity, bool>> predicate)
        {
            var param = Expression.Parameter(typeof(TEntity));
            IDictionary<string, string> mapperDictionary;
            PropertyMapperDictionaries.TryGetMapperDictionary(typeof(TDalEntity), out mapperDictionary);
            var result = new ExpressionConverter<TDalEntity, TEntity>(param, mapperDictionary).Visit(predicate.Body);
            return Expression.Lambda<Func<TEntity, bool>>(result, param);
        }
#endregion
    }
}
