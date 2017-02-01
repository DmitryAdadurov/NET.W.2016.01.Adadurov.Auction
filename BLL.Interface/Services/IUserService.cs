using BLL.Interface.Entities;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interface.Services
{
    public interface IUserService : IService<BllUser, int>
    {
        Task<int> Register(BllUser user);
        Task<BllUser> Authenticate(string login, string password);
        Task<Principal> Authorize(BllUser user);
        Task<bool> VerifyEmail(string token);
        Identity CreateIdentity(BllUser user, bool isAuthenticated = false);
        Task<int> GetId(string userName);
    }
}
