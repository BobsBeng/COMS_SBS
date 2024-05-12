using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace WebApplication1.ViewModels
{
    [NotMapped]
    public partial class company
    {
        public string COMPANY_CD { get; set; }
        public string CO_CD { get; set; }
        public string CO_NM { get; set; }
        public string OFCL_NM { get; set; }
        public string KANA_NM { get; set; }
        public string AREA_CD { get; set; }
        public string ZIP_CD { get; set; }
        public string ADDR1 { get; set; }
        public string ADDR2 { get; set; }
        public string ADDR3 { get; set; }
        public string ADDR4 { get; set; }
        public string TEL { get; set; }
        public string FAX { get; set; }
        public string CNTCT_PSN { get; set; }
        public string DPT_NM { get; set; }
        public string USR_NM { get; set; }
        public string ML_ADDR { get; set; }
        public string URL { get; set; }
        public string SUNDRY_FLG { get; set; }
        public System.DateTime TRD_ST_DT { get; set; }
        public System.DateTime TRD_END_DT { get; set; }
        public string BCUR_CD { get; set; }
        public string CNTRY_CD { get; set; }
        public string CNTRY_NM { get; set; }
        public string PRNT_LOCALE_CD { get; set; }
        public string INVALID_FLG { get; set; }
        public string NOTE { get; set; }
        public string FORWARDER_FLG { get; set; }
        public Nullable<System.DateTime> INS_TS { get; set; }
        public string INS_USR_ID { get; set; }
        public int UPD_CNTR { get; set; }
        public Nullable<System.DateTime> UPD_TS { get; set; }
        public string UPD_USR_ID { get; set; }
        public string INCTRMS_CD { get; set; }
        public string UKEBASHO_CD { get; set; }
        public string SCST_CD { get; set; }
    }
}