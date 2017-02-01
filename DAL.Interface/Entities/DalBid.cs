using DAL.Interface;
using DAL.Interface.Identity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interface.Entities
{
    public class DalBid : IEntity
    {
        public int Id { get; set; }
        public int Lot { get; set; }
        public decimal Price { get; set; }
        public int User { get; set; }
        public DateTime DateOfBid { get; set; }
    }
}
