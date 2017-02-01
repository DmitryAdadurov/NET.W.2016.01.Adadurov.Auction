namespace ORM.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Bid : IOrmEntity<int>
    {
        public int Id { get; set; }

        public int LotId { get; set; }

        public decimal Price { get; set; }

        public int UserId { get; set; }

        public DateTime BidDate { get; set; }

        public virtual Lot Lot { get; set; }

        public virtual User User { get; set; }
    }
}
