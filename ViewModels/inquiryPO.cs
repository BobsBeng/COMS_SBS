using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace WebApplication1.ViewModels
{
    public class inquiryPO
    {
        public string T_SLO_NO { get; set; }
        public string CST_PO_NO { get; set; }
        public string CO_NM { get; set; }
        public string COUNTRY_NM { get; set; }
        public string PIC_NM { get; set; }
        public string INCOTERM { get; set; }
        public string CURRENCY { get; set; }
        public string DELIVERY_DEST { get; set; }
        public Nullable<System.DateTime> T_SLO_DT { get; set; }
        public string REMARKS { get; set; }
        public string APPRV_STS { get; set; }
    }
}