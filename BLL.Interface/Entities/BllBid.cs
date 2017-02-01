using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interface.Entities
{
    public class BllBid
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public DateTime BidDate { get; set; }
        public int User { get; set; }
        public int Lot { get; set; }
    }
}
