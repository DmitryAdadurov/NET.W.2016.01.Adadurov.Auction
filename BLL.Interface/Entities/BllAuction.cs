using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interface.Entities
{
    public class BllAuction
    {
        public BllLot Lot { get; set; }
        public IEnumerable<BllBid> Bids { get; set; }
        public IEnumerable<BllComment> Comments { get; set; }
    }
}
