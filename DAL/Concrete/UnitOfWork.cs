using DAL.Identity.Stores;
using DAL.Interface;
using DAL.Interface.Identity.Entities;
using DAL.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Interface.Entities;
using DAL.Concrete.Exceptions;
using Microsoft.AspNet.Identity;

namespace DAL.Concrete
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext context;
        private bool disposed = false;
        private DalUserStore userStore;
        private BidsRepositry bidsRepositry;
        private CategoriesRepositry categoriesRepository;
        private CommentsRepository commentsRepository;
        private LotRepository lotsRepository;
        private DalRoleStore roleStore;

        public UnitOfWork(DbContext dbcontext)
        {
            if (dbcontext.IsNull())
                throw new ArgumentNullException(nameof(dbcontext));

            if (!dbcontext.Database.Exists())
                throw new DatabaseErrorException(nameof(dbcontext));

            context = dbcontext;
        }

        public IRoleStore<DalRole, int> RoleStore
        {
            get
            {
                if (roleStore == null)
                    roleStore = new DalRoleStore(context);
                return roleStore;
            }
        }

        public IBidsRepository BidsRepository
        {
            get
            {
                if (bidsRepositry == null)
                    bidsRepositry = new BidsRepositry(context);
                return bidsRepositry;
            }
        }

        public IUserRepository<DalUser, int> UserStore
        {
            get
            {
                if (userStore == null)
                    userStore = new DalUserStore(context);
                return userStore;
            }
        }

        public ICategoriesRepository CategoriesRepository
        {
            get
            {
                if (categoriesRepository == null)
                    categoriesRepository = new CategoriesRepositry(context);
                return categoriesRepository;
            }
        }

        public ICommentsRepository CommentsRepository
        {
            get
            {
                if (commentsRepository == null)
                    commentsRepository = new CommentsRepository(context);
                return commentsRepository;
            }
        }

        public ILotRepository LotsRepository
        {
            get
            {
                if (lotsRepository == null)
                    lotsRepository = new LotRepository(context);
                return lotsRepository;
            }
        }

        public void Commit()
        {
            ThrowIfDisposed();
            if (context != null)
            {
                context.SaveChangesAsync();
            }
        }

        public void Dispose()
        {
            if (!disposed)
            {
                disposed = true;
                context.Dispose();
            }
        }

        public void RollBack()
        {
            throw new NotImplementedException();
        }

        #region Private Methods
        private void ThrowIfDisposed()
        {
            if (disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }
        #endregion
    }
}
