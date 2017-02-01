using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interface.Entities
{
    public class BllUser
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public ICollection<BllRole> Roles { get; set; }
        public string Country { get; set; }
        public string FullName { get; set; }
        public string ActivationToken { get; set; }
    }
}
