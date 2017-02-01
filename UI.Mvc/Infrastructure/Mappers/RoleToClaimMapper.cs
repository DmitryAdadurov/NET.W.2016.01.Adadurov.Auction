using BLL.Interface.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace UI.Mvc.Infrastructure.Mappers
{
    public static class RoleToClaimMapper
    {
        public static Claim ToClaim(this BllRole role)
        {
            return new Claim(ClaimTypes.Role, role.Name);
        }
    }
}