using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UI.Mvc.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public string FullName { get; set; }
        public string Country { get; set; }
        public string Role { get; set; }
    }
    public class RegisterModel
    {
        [Required(ErrorMessage = "Login is required.")]
        [Display(Name = "Login")]
        public string UserName { get; set; }

        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Incorrect email address")]
        [Required(ErrorMessage = "Email is required.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [StringLength(25, MinimumLength = 6, ErrorMessage = "Password length must be between 6 and 25.")]
        [Required(ErrorMessage = "Password is required")]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        [DataType(DataType.Password)]
        [StringLength(25, MinimumLength = 6, ErrorMessage = "Password length must be between 6 and 25.")]
        [Required(ErrorMessage = "Stay calm and confirm your password.")]
        [Display(Name = "Password confirmation")]
        public string PasswordConfirm { get; set; }

        [StringLength(200, MinimumLength = 6, ErrorMessage = "Name length must be between 6 and 200.")]
        [Required(ErrorMessage = "Full name is required.")]
        [Display(Name = "Full name")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Country field is required.")]
        [Display(Name = "Country")]
        public string Country { get; set; }
    }
}