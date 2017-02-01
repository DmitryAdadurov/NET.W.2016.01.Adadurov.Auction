using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interface.Entities
{
    public class Identity : IIdentity
    {
        public Identity(string authenticationType)
        {
            AuthenticationType = authenticationType;
        }
        public string AuthenticationType { get; private set; }

        public bool IsAuthenticated { get; set; }

        public string Name { get; set; } = null;
        public int Id { get; set; }
    }
}
