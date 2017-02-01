using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UI.Mvc.Models
{
    public class AddAuctionModel
    {
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Title length must be more than 3 characters.")]
        [Required(ErrorMessage = "Title is required.")]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [StringLength(600, MinimumLength = 10, ErrorMessage = "Description length must be between 10 and 600.")]
        [Required(ErrorMessage = "Description is required.")]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [DataType(DataType.Upload)]
        [Display(Name = "Photos")]
        public IEnumerable<HttpPostedFileBase> Photos { get; set; }

        [Range(typeof(decimal), "0", "99999999", ErrorMessage = "Wrong start price.")]
        [Required(ErrorMessage = "Start price cant'be blank.")]
        [DataType(DataType.Currency)]
        [Display(Name = "Start price")]
        public decimal StartPrice { get; set; }

        [Required(ErrorMessage = "Choose categorie.")]
        [Display(Name = "Categorie")]
        public string Categorie { get; set; }

        [Required(ErrorMessage = "Select auction end date")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Auction end date")]
        public DateTime AuctionEndDate { get; set; }
    }
}