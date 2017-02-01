using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI.Mvc.Models
{
    public class BidModel
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public DateTime Date { get; set; }
        public int LotId { get; set; }
        public int UserId { get; set; }
    }
}