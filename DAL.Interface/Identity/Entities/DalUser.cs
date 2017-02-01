using DAL.Interface;
using DAL.Interface.Entities;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interface.Identity.Entities
{
    public class DalUser : IUser<int>, IEntity
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public string ActivationToken { get; set; }
        public ICollection<DalRole> Roles { get; set; }
    }
}
