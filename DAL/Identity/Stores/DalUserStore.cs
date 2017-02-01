using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using DAL.Concrete;
using ORM.EF;
using DAL.Interface;
using DAL.Interface.Identity.Entities;
using DAL.Interface.Repository;
using System.Security.Cryptography;
using System.Linq.Expressions;
using Common.Expressions.PropertyDictionaries;
using Common.Expressions;

namespace DAL.Identity.Stores
{
    public class DalUserStore : IUserRepository<DalUser, int>
    {
        private readonly DbContext context;
        private bool disposed = false;

        public DalUserStore(DbContext dbContext)
        {
            if (dbContext.IsNull())
                throw new ArgumentNullException(nameof(dbContext));

            context = dbContext;
        }

        public async Task AddToRoleAsync(DalUser user, string roleName)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (string.IsNullOrEmpty(roleName))
                throw new ArgumentNullException(nameof(roleName));

            var roleStore = new DalRoleStore(context);
            DalRole dalRole = await roleStore.FindByNameAsync(roleName);
            if (dalRole != null)
            {
                if (user.Roles == null)
                    user.Roles = new List<DalRole>();
                user.Roles.Clear();
                user.Roles.Add(dalRole);
                await UpdateAsync(user);
            }
        }

        public async Task<int> CreateAsync(DalUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var userEntity = user.ToUserEntity();
            int id = user.Roles.FirstOrDefault().Id;
            var dbRole = context.Set<Role>().FirstOrDefault(t => t.Id == id);
            userEntity.Roles.Clear();
            userEntity.Roles.Add(dbRole);
            context.Set<User>().Add(userEntity);
            await context.SaveChangesAsync();
            return userEntity.Id;
        }

        async Task IUserStore<DalUser, int>.CreateAsync(DalUser user)
        {
            await CreateAsync(user);
        }

        public Task DeleteAsync(DalUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var userEntity = user.ToUserEntity();
            context.Set<User>().Remove(userEntity);
            return Task.FromResult<object>(null);
        }

        public void Dispose()
        {
            if (!disposed)
            {
                context.SaveChanges();
                context.Dispose();
                disposed = true;
            }
        }

        public async Task<DalUser> FindByIdAsync(int userId)
        {
            return (await context.Set<User>().SingleOrDefaultAsync(u => u.Id == userId)).ToDalUser();
        }

        public async Task<DalUser> FindByNameAsync(string userName)
        {
            var user = await context.Set<User>().SingleOrDefaultAsync(u => u.UserName == userName);
            return user?.ToDalUser();
        }

        public Task<DalUser> FindAsync(string userName, string password)
        {
            User user = context.Set<User>().SingleOrDefault(u => u.UserName == userName && u.PasswordHash == password);
            if (user != null)
                return Task.FromResult<DalUser>(user.ToDalUser());
            return null;
        }

        public async Task<bool> IsExistingUserAsync(string userName)
        {
            return await context.Set<User>().AnyAsync(u => u.UserName == userName);
        }

        public async Task<bool> IsEmailExist(string email)
        {
            return await context.Set<User>().AnyAsync(u => u.Email == email);
        }

        public Task<IList<string>> GetRolesAsync(DalUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return Task.FromResult<IList<string>>(user.Roles.Select(t => t.Name).ToList());
        }

        public Task<bool> IsInRoleAsync(DalUser user, string roleName)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (string.IsNullOrEmpty(roleName))
                throw new ArgumentNullException(nameof(roleName));

            var roleStore = new DalRoleStore(context);
            DalRole dalRole = roleStore.FindByNameAsync(roleName).Result;
            if (dalRole != null)
            {
                if (user.Roles != null && user.Roles.Contains(dalRole))
                {
                    return Task.FromResult<bool>(true);
                }
                else
                {
                    return Task.FromResult<bool>(false);
                }
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(roleName));
            }
        }

        public Task RemoveFromRoleAsync(DalUser user, string roleName)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (string.IsNullOrEmpty(roleName))
                throw new ArgumentNullException(nameof(roleName));

            if (user.Roles.Count == 0)
                return Task.FromResult<object>(null);

            var roleStore = new DalRoleStore(context);
            DalRole dalRole = roleStore.FindByNameAsync(roleName).Result;
            if (dalRole != null)
            {
                if (user.Roles.Contains(dalRole))
                {
                    user.Roles.Remove(dalRole);
                    this.UpdateAsync(user);
                }
                return Task.FromResult<object>(null);
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(roleName));
            }
        }

        public async Task UpdateAsync(DalUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var dbUser = await context.Set<User>().FindAsync(user.Id);
            if (dbUser == null)
                return;
            int roleId = user.Roles.FirstOrDefault().Id;
            var dbRoles = await context.Set<Role>().FirstOrDefaultAsync(t => t.Id == roleId);
            dbUser.Roles.Clear();
            dbUser.Roles.Add(dbRoles);
            context.Entry<User>(dbUser).CurrentValues.SetValues(user.ToUserEntity());
            await context.SaveChangesAsync();
        }

        public Task<DalUser> GetByPredicate(Expression<Func<DalUser, bool>> predicate)
        {
            var param = Expression.Parameter(typeof(User));
            IDictionary<string, string> mapperDictionary;
            PropertyMapperDictionaries.TryGetMapperDictionary(typeof(DalUser), out mapperDictionary);
            var result = new ExpressionConverter<DalUser, User>(param, mapperDictionary).Visit(predicate.Body);
            Expression<Func<User, bool>> lambda = Expression.Lambda<Func<User, bool>>(result, param);
            User u;

            try
            {
                u = context.Set<User>().FirstOrDefault(lambda);
            }
            catch
            {
                throw new InvalidOperationException();
            }

            return Task.FromResult<DalUser>(u.ToDalUser());
        }

        public async Task<IEnumerable<DalUser>> GetAll()
        {
            return await context.Set<User>().Select(t => t.ToDalUser()).ToArrayAsync();
        }

        public Task SetPasswordHashAsync(DalUser user, string passwordHash)
        {
            if (user.IsNull())
                throw new ArgumentNullException(nameof(user));

            if (passwordHash.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(passwordHash));

            user.Password = passwordHash;
            return this.UpdateAsync(user);
        }

        public async Task<string> GetPasswordHashAsync(DalUser user)
        {
            if (user.IsNull())
                throw new ArgumentNullException(nameof(user));

            DalUser dalUser = await this.FindByIdAsync(user.Id);
            return dalUser.Password;
        }

        public async Task<bool> HasPasswordAsync(DalUser user)
        {
            if (user.IsNull())
                throw new ArgumentNullException(nameof(user));

            DalUser dalUser = await this.FindByIdAsync(user.Id);

            if (dalUser.Password.IsNullOrEmpty())
                return false;
            else
                return true;
        }
    }
}
