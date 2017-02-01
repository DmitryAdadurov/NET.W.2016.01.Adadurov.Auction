using DAL.Interface.Identity.Entities;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interface.Repository
{
    public interface IUserRepository<TUser, TKey> : IUserStore<TUser, TKey>, IUserRoleStore<TUser, TKey>, IUserPasswordStore<TUser, TKey>
        where TUser : class, IUser<TKey>
    {
        Task<DalUser> GetByPredicate(Expression<Func<DalUser, bool>> predicate);
        Task<IEnumerable<DalUser>> GetAll();
        Task<DalUser> FindAsync(string userName, string password);
        Task<bool> IsExistingUserAsync(string userName);
        Task<bool> IsEmailExist(string email);
        Task<int> CreateAsync(DalUser user);
    }
}
