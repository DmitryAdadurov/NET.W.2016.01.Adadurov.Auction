using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using DAL.Interface.Identity.Entities;
using DAL.Interface.Entities;

namespace DAL.Interface.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository<DalUser, int> UserStore { get;}
        IRoleStore<DalRole, int> RoleStore { get; }
        IBidsRepository BidsRepository { get; }
        ICategoriesRepository CategoriesRepository { get; }
        ICommentsRepository CommentsRepository { get; }
        ILotRepository LotsRepository { get; }
        void Commit();
        void RollBack();
    }
}
