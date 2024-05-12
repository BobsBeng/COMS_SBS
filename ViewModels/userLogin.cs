using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication1.Models;
using System.ComponentModel;

namespace WebApplication1.ViewModels
{
    [NotMapped]
    public partial class userLogin
    {
       
      
        [Required(ErrorMessage = "This field is required")]
        [DisplayName("Username")]
        public string UserName { get; set; }       
                
        [DataType(DataType.Password) ]
        [DisplayName("Password")]
        [Required(ErrorMessage = "This field is required")]
         public string PasswordHash { get; set; }

     }
}