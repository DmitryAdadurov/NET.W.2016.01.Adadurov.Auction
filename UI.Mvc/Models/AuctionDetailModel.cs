using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UI.Mvc.Models
{
    public class AuctionDetail
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IEnumerable<string> Photos { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public DateTime AuctionEndDate { get; set; }
        public decimal CurrentPrice { get; set; }
        public string Seller { get; set; }
        public int SellerId { get; set; }
        public string Category { get; set; }
        public IEnumerable<Bid> Bids { get; set; }
        public IEnumerable<Comment> Comments { get; set; }
    }

    public class Bid
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }
        [HiddenInput(DisplayValue = false)]
        public int LotId { get; set; }
        public string UserName { get; set; }
        public decimal MoneyAmount { get; set; }
        public DateTime BidDate { get; set; }
    }

    public class Comment
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }
        [HiddenInput(DisplayValue = false)]
        public int LotId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public DateTime PubDate { get; set; }
        public string Text { get; set; }
    }
}