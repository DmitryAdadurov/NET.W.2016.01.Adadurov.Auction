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
    public class DalRole : IRole<int>, IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
