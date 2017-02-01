using BLL.Interface.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Infrastructure
{
    public static class IdentityMapper
    {
        public static ClaimsIdentity ToClaimsIdentity(this Identity bllIdentity)
        {
            return new ClaimsIdentity(bllIdentity);
        }

        public static Identity ToBllIdentity(this ClaimsIdentity claimsIdentity)
        {
            return new Identity(claimsIdentity.AuthenticationType)
            {
                Name = claimsIdentity.Name,
                IsAuthenticated = claimsIdentity.IsAuthenticated
            };
        }
    }
}
