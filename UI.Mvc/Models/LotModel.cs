using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI.Mvc.Models
{
    public class LotModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IEnumerable<string> Photos { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public decimal CurrentPrice { get; set; }
        public string Categorie { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class LotDetaliedModel : LotModel
    {
        public UserModel Seller { get; set; }
        public IEnumerable<BidModel> Bids { get; set; }
        public IEnumerable<Comment> Comments { get; set; }
    }
}