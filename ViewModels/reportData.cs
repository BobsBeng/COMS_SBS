using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace WebApplication1.ViewModels
{
    public class reportData
    {
        [Display(Name="Internal Order No")]
        public string T_SLO_NO { get; set; }
        [Display(Name = "Customer Code")]
        public string CST_CD { get; set; }
        [Display(Name = "Customer Name")]
        public string CST_NM { get; set; }
        public string MODEL_NO { get; set; }
        [Display(Name = "PO Number")]
        public string PO_NO { get; set; }
        [Display(Name = "Item Code")]
        public string TokuihId { get; set; }
        [Display(Name = "Item Name")]
        public string TokuihNm { get; set; }
        public string MODELNM { get; set; }
        [Display(Name = "Quantity")]
        public decimal juchuHQty {get;set;}
        [Display(Name = "Price")]
        public decimal juchuHPrice { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime PO_DT { get; set; }
        //cust_identity
        public string CST_ADDR { get; set; }
        public string CST_ADDR1 { get; set; }
        public string CST_ADDR2 { get; set; }
        public string TELEPHONE { get; set; }
        public string INCOTERM { get; set; }
        public string REMARKS { get; set; }
        public string PAYTERM { get; set; }
        public string PAYTERM_NM { get; set; }     
        public DateTime ETA { get; set; }
        public DateTime ETD { get; set; }
        public string CURR { get; set; }
        public string SHIPPER { get; set; }
        //destination_identity
        public string DLVRLOC { get; set; }
        public string DLVLOC_NM { get; set; }
        public string USR_NM { get; set; }
        public string SCST_ADDR { get; set; }
        public string SCST_ADDR1 { get; set; }
        public string SCST_ADDR2 { get; set; }
        public string SCST_ADDR3 { get; set; }
        public string SCST_PHONE { get; set; }
        public string SCST_MAIL{ get; set; }
        public string UNIT { get; set; }
    }
}