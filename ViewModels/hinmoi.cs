using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.ViewModels
{
    [NotMapped]
    public partial class hinmoi
    {
        public string itm_Cd { get; set; }
        public string unit_Cd { get; set; }
        public decimal min { get; set; }
        public decimal max { get; set; }
        public int lt { get; set; }
        public int standardPacking { get; set; }
    }
}