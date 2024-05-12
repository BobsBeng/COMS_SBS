using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication1.Models;
using System.ComponentModel;

namespace WebApplication1.Models
{
    public partial class UserLogin
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "This field is required")]
        public string UserName { get; set; }       
                
        [DataType(DataType.Password) ]
        [DisplayName("Password")]
        [Required, MinLength(8)]
        //[RegularExpression("!@#$%")]
        public string PasswordHash { get; set; }

        //[DataType(DataType.Password)]
        //[DisplayName("Confirm Password")]
        //[Compare("PasswordHash")]
        //public string confirmPassword { get; set; }
        
        public Nullable<System.DateTimeOffset> LockoutEnd { get; set; }
        public short LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public string Company_CD { get; set; }
        public string ISACTIVE { get; set; }

        //public string LoginErrorMessage { get; set; }

        public UserLogin()
        {
            this.ISACTIVE = "1";
            this.Id = Guid.NewGuid().ToString(); ;
        }
    }
}