using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.ViewModels
{
    [NotMapped]
    public partial class userRegistration
    {
        public string Id { get; set; }
        
        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }
        [Display(Name = "Password")]
        [Required]
        [RegularExpression(@"^.{8,}$", ErrorMessage = "Minimum 8 characters required")]
        public string passwordHash { get; set; }
        [Required(ErrorMessage = "Please confirm the password.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("passwordHash", ErrorMessage = "The password and confirmation password do not match.")]
        public string confirmPassword { get; set; }
        [Display(Name = "Company")]
        public string Company_CD { get; set; }
        [Display(Name = "Role")]
        public string ROLES { get; set; }
    }
}