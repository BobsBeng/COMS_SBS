//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApplication1.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class SBS_HT_JUCHUJ_TMP_ALL
    {
        public SBS_HT_JUCHUJ_TMP_ALL()
        {
            this.SBS_HT_JUCHUH_TMP_ALL = new HashSet<SBS_HT_JUCHUH_TMP_ALL>();
        }
    
        public string CO_CD { get; set; }
        public string T_SLO_NO { get; set; }
        public string SLS_TYP { get; set; }
        public string CST_PO_NO { get; set; }
        public string CST_CD { get; set; }
        public string CST_CNTCT_DPT_NM { get; set; }
        public string CST_CNTCT_PSN_NM { get; set; }
        public string SCST_CD { get; set; }
        public string SCST_ADDR_NO { get; set; }
        public string SCST_CNTCT_DPT_NM { get; set; }
        public string SCST_USR_NM { get; set; }
        public string CSTMR_CD { get; set; }
        public string CSTMR_ADDR_NO { get; set; }
        public Nullable<System.DateTime> T_SLO_DT { get; set; }
        public string SLS_RND_TYP { get; set; }
        public string CTAX_RND_TYP { get; set; }
        public string CTAX_CALC_TYP { get; set; }
        public string FARE_TYP { get; set; }
        public string INTR_RMRKS { get; set; }
        public string SLIP_RMRKS { get; set; }
        public string CAN_FLG { get; set; }
        public Nullable<System.DateTime> CAN_DT { get; set; }
        public string CAN_USR_CD { get; set; }
        public string CUR_CD { get; set; }
        public Nullable<decimal> XRATE { get; set; }
        public Nullable<System.DateTime> XRATE_DT { get; set; }
        public string XCNTRCT_FLG { get; set; }
        public string XCNTRCT_NO { get; set; }
        public string XRATE_TYP { get; set; }
        public string TRD_TYP { get; set; }
        public string INCTRMS_CD { get; set; }
        public string UKEBASHO_CD { get; set; }
        public string PAY_COND_CD { get; set; }
        public string SRC_TYP { get; set; }
        public Nullable<System.DateTime> INS_TS { get; set; }
        public string INS_USR_CD { get; set; }
        public int UPD_CNTR { get; set; }
        public Nullable<System.DateTime> UPD_TS { get; set; }
        public string UPD_USR_CD { get; set; }
        public string ORD_STS { get; set; }
        public string SEQ { get; set; }
    
        public virtual ICollection<SBS_HT_JUCHUH_TMP_ALL> SBS_HT_JUCHUH_TMP_ALL { get; set; }
    }
}