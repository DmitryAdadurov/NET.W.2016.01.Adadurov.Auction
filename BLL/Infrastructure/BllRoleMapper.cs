using BLL.Interface.Entities;
using DAL.Interface.Identity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Infrastructure
{
    public static class BllRoleMapper
    {
        public static DalRole ToDalRole(this BllRole bllRole)
        {
            return new DalRole()
            {
                Id = bllRole.Id,
                Name = bllRole.Name
            };
        }

        public static BllRole ToBllRole(this DalRole dalRole)
        {
            return new BllRole()
            {
                Id = dalRole.Id,
                Name = dalRole.Name
            };
        }

        public static ICollection<DalRole> ToDalRoles(this ICollection<BllRole> bllRoles)
        {
            return bllRoles?.Select<BllRole, DalRole>(t => t.ToDalRole()).ToList<DalRole>();
        }

        public static ICollection<BllRole> ToBllRoles(this ICollection<DalRole> dalRoles)
        {
            return dalRoles?.Select<DalRole, BllRole>(t => t.ToBllRole()).ToList<BllRole>();
        }
    }
}
