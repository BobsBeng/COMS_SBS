using System.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApplication1.Models;

namespace WebApplication1.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    
    [NotMapped]
    public partial class resetPassword
    {
        [Required]
        [Display(Name="Current Password")]
        public string currentPassword { get; set; }
        [Display(Name = "New Password")]
        [Required]
        [RegularExpression(@"^.{8,}$", ErrorMessage = "Minimum 8 characters required")]
        public string newPassword { get; set; }
        
    }
}