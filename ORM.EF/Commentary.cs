namespace ORM.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Commentary : IOrmEntity<int>
    {
        public int Id { get; set; }

        public int LotId { get; set; }

        public DateTime Date { get; set; }

        [Required]
        [StringLength(256)]
        public string Text { get; set; }

        public int UserId { get; set; }

        public virtual Lot Lot { get; set; }

        public virtual User User { get; set; }
    }
}
