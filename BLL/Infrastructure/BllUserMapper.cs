using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Infrastructure;
using DAL.Interface.Identity.Entities;
using BLL.Interface.Entities;

namespace BLL.Infrastructure
{
    public static class BllUserMapper
    {
        public static DalUser ToDalUser(this BllUser bllUser)
        {
            if (bllUser == null)
                return null;

            return new DalUser()
            {
                Email = bllUser.Email,
                Password = bllUser.Password,
                IsEmailConfirmed = bllUser.IsEmailConfirmed,
                UserName = bllUser.Login,
                Id = bllUser.Id,
                ActivationToken = bllUser.ActivationToken,
                Roles = bllUser.Roles.ToDalRoles()
            };
        }

        public static BllUser ToBllUser(this DalUser dalUser)
        {
            if (dalUser == null)
                return null;

            return new BllUser()
            {
                Id = dalUser.Id,
                Email = dalUser.Email,
                IsEmailConfirmed = dalUser.IsEmailConfirmed,
                Login = dalUser.UserName,
                Password = dalUser.Password,
                ActivationToken = dalUser.ActivationToken,
                Roles = dalUser.Roles.ToBllRoles()
            };
        }
    }
}
