using DAL.Interface;
using DAL.Interface.Identity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interface.Entities
{
    public class DalLot : IEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Photos { get; set; }
        public DateTime DateOfCreation { get; set; }
        public DateTime LastUpdated { get; set; }
        public decimal Price { get; set; }
        public int Seller { get; set; }
        public DateTime Expired { get; set; }
        public int Categorie { get; set; }
        public ICollection<DalBid> Bids { get; set; }
    }
}
