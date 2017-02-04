using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using DAL.Interface;
using BLL.Infrastructure;
using System.Linq.Expressions;
using BLL.Exceptions;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using BLL.Interface.Entities;
using DAL.Interface.Identity.Entities;
using BLL.Interface.Services;
using DAL.Interface.Repository;
using System.Security.Principal;
using Common.Expressions.PropertyDictionaries;
using Common.Expressions;

namespace BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork context;
        private readonly IPasswordsHasher hasher;
        public UserService(IUnitOfWork uow, IPasswordsHasher passwordsHasher)
        {
            context = uow ?? throw new ArgumentNullException(nameof(uow));
            hasher = passwordsHasher ?? throw new ArgumentNullException(nameof(passwordsHasher));
        }

        /// <summary>
        /// Find user by login
        /// </summary>
        /// <param name="userName">User login</param>
        /// <returns>null if not found</returns>
        public async Task<BllUser> FindByName(string userName)
        {
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentNullException(nameof(userName));

            return (await context.UserStore.FindByNameAsync(userName)).ToBllUser();
        }

        /// <summary>
        /// Create IIdentity for specifyed user
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="isAuthenticated">Is authenticated</param>
        /// <returns></returns>
        public Identity CreateIdentity(BllUser user, bool isAuthenticated = false)
        {
            Identity userIdentity = new Identity(DefaultAuthenticationTypes.ApplicationCookie)
            {
                Id = user.Id,
                Name = user.Login,
                IsAuthenticated = isAuthenticated
            };
            return userIdentity;
        }

        async Task<int> IService<BllUser, int>.Create(BllUser user)
        {
            return (int)(await Register(user));
        }

        /// <summary>
        /// Delete user
        /// </summary>
        /// <param name="user">User</param>
        public async Task Delete(BllUser user)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (await context.UserStore.IsExistingUserAsync(user.Login))
            {
                await context.UserStore.DeleteAsync(user.ToDalUser());
            }
            else
            {
                throw new NotExistingUserException(nameof(user));
            }
        }

        /// <summary>
        /// Get user id
        /// </summary>
        /// <param name="userName">User login</param>
        /// <returns>Id if user login correct</returns>
        public async Task<int> GetId(string userName)
        {
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentNullException(nameof(userName));

            return (await context.UserStore.FindByNameAsync(userName)).Id;
        }

        /// <summary>
        /// Update user info
        /// </summary>
        /// <param name="user">User with updated information</param>
        public async Task Update(BllUser user)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (await context.UserStore.IsExistingUserAsync(user.Login))
            {
                await context.UserStore.UpdateAsync(user.ToDalUser());
            }
            else
            {
                throw new NotExistingUserException(nameof(user));
            }
        }

        /// <summary>
        /// Verifying user email
        /// </summary>
        /// <param name="token">Email verification token</param>
        /// <returns>true - if succeded</returns>
        public async Task<bool> VerifyEmail(string token)
        {
            if (string.IsNullOrEmpty(token))
                throw new ArgumentNullException(nameof(token));

            BllUser user = (await context.UserStore.GetByPredicate(t => t.ActivationToken == token)).ToBllUser();

            if (user == null)
                return false;

            if (user.IsEmailConfirmed)
                return false;

            user.IsEmailConfirmed = true;
            await context.UserStore.UpdateAsync(user.ToDalUser());
            await context.UserStore.AddToRoleAsync(user.ToDalUser(), "user");
            return true;
        }

        private void ThrowIfDisposed()
        {
            if (disposedValue)
                throw new ObjectDisposedException(GetType().Name);
        }

        /// <summary>
        /// Create new user
        /// </summary>
        /// <param name="user">New user</param>
        /// <returns>Id of created user</returns>
        public async Task<int> Register(BllUser user)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var dbUser = await context.UserStore.FindByNameAsync(user.Login);

            if (dbUser == null)
            {
                if (await context.UserStore.IsEmailExist(user.Email))
                    return (int)UserVerificationResult.ExistWithSameEmail;

                int userId = await context.UserStore.CreateAsync(user.ToDalUser());
                context.Commit();
                return userId;
            }
            else
            {
                return (int)UserVerificationResult.ExistWithSameLogin;
            }
        }

        /// <summary>
        /// Authenticate a user
        /// </summary>
        /// <param name="login">User login</param>
        /// <param name="password">User password</param>
        /// <returns>null if failed</returns>
        public async Task<BllUser> Authenticate(string login, string password)
        {
            if (string.IsNullOrEmpty(login))
                throw new ArgumentNullException(nameof(login));

            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException(nameof(password));

            var findedUser = (await context.UserStore.FindByNameAsync(login)).ToBllUser();

            if (findedUser == null)
                return null;

            if (hasher.VerifyHashedPassword(findedUser.Password, password) != PasswordVerificationResult.Failed)
                return findedUser;
            else
                return null;
        }
        
        /// <summary>
        /// Provide roles for the specified user
        /// </summary>
        /// <param name="user">User to authorize</param>
        /// <returns>IPrincipal for user if succeded</returns>
        public Task<Principal> Authorize(BllUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (user.Roles == null)
                throw new ArgumentNullException(nameof(user.Roles));

            var identity = CreateIdentity(user, true);
            return Task.FromResult<Principal>(new Principal(identity, user.Roles));
        }

        /// <summary>
        /// Get user by id
        /// </summary>
        /// <param name="id">Id of the user</param>
        /// <returns>null if fails</returns>
        public async Task<BllUser> GetById(int id)
        {
            if (id < 0)
                throw new ArgumentOutOfRangeException(nameof(id));

            return (await context.UserStore.FindByIdAsync(id)).ToBllUser();
        }

        /// <summary>
        /// Get user matching the predicate
        /// </summary>
        /// <param name="predicate">Expression to match</param>
        /// <returns>BllUser if succeded</returns>
        public async Task<BllUser> GetByPredicate(Expression<Func<BllUser, bool>> predicate)
        {
            var param = Expression.Parameter(typeof(BllUser));
            IDictionary<string, string> mapperDictionary;
            PropertyMapperDictionaries.TryGetMapperDictionary(typeof(BllUser), out mapperDictionary);
            var result = new ExpressionConverter<BllUser, DalUser>(param, mapperDictionary).Visit(predicate.Body);
            Expression<Func<DalUser, bool>> lambda = Expression.Lambda<Func<DalUser, bool>>(result, param);
            DalUser u;

            try
            {
                u = await context.UserStore.GetByPredicate(lambda);
            }
            catch
            {
                throw new InvalidOperationException();
            }

            return u.ToBllUser();
        }

        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns>Enumeration with all users</returns>
        public async Task<IEnumerable<BllUser>> GetAll()
        {
            return (await context.UserStore.GetAll()).Select(t => t.ToBllUser());
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
        // ~UserService() {
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
