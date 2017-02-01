using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ORM.EF;
using DAL.Interface.Identity.Entities;

namespace DAL.Concrete
{
    public static class UserDalUserMappers
    {
        public static DalUser ToDalUser(this User userEntity)
        {
            var dalUser = new DalUser()
            {
                Id = userEntity.Id,
                Email = userEntity.Email,
                Password = userEntity.PasswordHash,
                UserName = userEntity.UserName,
                IsEmailConfirmed = userEntity.EmailConfirmed,
                ActivationToken = userEntity.ActivationToken
            };
            foreach (var item in userEntity.Roles)
            {
                if (dalUser.Roles == null)
                    dalUser.Roles = new List<DalRole>();
                dalUser.Roles.Add(item.ToDalRole());
            }
            return dalUser;
        }

        public static User ToUserEntity(this DalUser dalUser)
        {
            var user = new User()
            {
                Id = dalUser.Id,
                UserName = dalUser.UserName,
                Email = dalUser.Email,
                EmailConfirmed = dalUser.IsEmailConfirmed,
                PasswordHash = dalUser.Password,
                ActivationToken = dalUser.ActivationToken
            };

            foreach (var item in dalUser.Roles)
            {
                user.Roles.Add(item.ToRoleEntity());
            }
            return user;
        }

        public static Role ToRoleEntity(this DalRole dalRole)
        {
            return new Role()
            {
                Id = dalRole.Id,
                Name = dalRole.Name
            };
        }

        public static DalRole ToDalRole(this Role roleEntity)
        {
            return new DalRole()
            {
                Id = roleEntity.Id,
                Name = roleEntity.Name
            };
        }
    }
}
