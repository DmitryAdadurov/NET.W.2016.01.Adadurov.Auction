using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interface.Entities
{
    public class Principal : IPrincipal
    {
        public Principal(IIdentity identity) : this(identity, null)
        {
        }

        public Principal(IIdentity identity, IEnumerable<BllRole> roles)
        {
            Identity = identity;
            Roles = roles;
        }
        public IEnumerable<BllRole> Roles { get; private set; }
        public IIdentity Identity { get; private set; }

        public bool IsInRole(string role)
        {
            return Roles?.Any(t => t.Name == role) ?? false;
        }
    }
}
