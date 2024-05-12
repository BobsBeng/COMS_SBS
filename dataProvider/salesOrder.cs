using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.dataProvider
{
    public class salesOrder
    {
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

        public List<SBS_HT_JUCHUH_TMP_ALL> SBS_HT_JUCHUH_TMP_ALL { get; set; }
    }

    public class SBS_HT_JUCHUH_TMP_ALL 
    {
        public string CO_CD { get; set; }
        public string T_SLO_NO { get; set; }
        public string T_SLO_D_NO { get; set; }
        public string SLO_NO { get; set; }
        public string SLO_D_NO { get; set; }
        public string SET_NO { get; set; }
        public string GRP_NO { get; set; }
        public string SLS_ARRG_TYP { get; set; }
        public string DSH_TYP { get; set; }
        public string ITM_CD { get; set; }
        public string ITM_NM { get; set; }
        public string EXT_ITM_NM { get; set; }
        public string INPT_UNIT_CD { get; set; }
        public decimal INPT_QTY { get; set; }
        public decimal CNV_QTY { get; set; }
        public string UNIT_CD { get; set; }
        public Nullable<System.DateTime> SHIP_RQST_DT { get; set; }
        public Nullable<System.DateTime> RQST_DLV_DT { get; set; }
        public Nullable<System.DateTime> APPRV_DT { get; set; }
        public string APPRV_STS_TYP { get; set; }
        public string APPRV_USR_CD { get; set; }
        public decimal QTY { get; set; }
        public decimal UPRI { get; set; }
        public decimal SPE_DSCNT_UPRI { get; set; }
        public string DSCNT_FLG { get; set; }
        public decimal DSCNT { get; set; }
        public decimal DSCNT_UPRI { get; set; }
        public decimal SLS_AMT { get; set; }
        public decimal XTX_SLS_AMT { get; set; }
        public string CTAX_SPEC_FLG { get; set; }
        public string CTAX_CD { get; set; }
        public decimal CTAX_RATE { get; set; }
        public string CTAX_TYP { get; set; }
        public Nullable<decimal> CTAX_AMT { get; set; }
        public Nullable<decimal> SLS_COST_UPRI { get; set; }
        public string RMRKS { get; set; }
        public string CAN_FLG { get; set; }
        public Nullable<System.DateTime> CAN_DT { get; set; }
        public string CAN_USR_CD { get; set; }
        public string CUR_CD { get; set; }
        public Nullable<decimal> XRATE { get; set; }
        public Nullable<System.DateTime> XRATE_DT { get; set; }
        public string XCNTRCT_FLG { get; set; }
        public string XCNTRCT_NO { get; set; }
        public Nullable<decimal> BCUR_XTX_SLS_AMT { get; set; }
        public string XRATE_TYP { get; set; }
        public string SNO { get; set; }
        public string SUB_SNO { get; set; }
        public Nullable<System.DateTime> INS_TS { get; set; }
        public string INS_USR_CD { get; set; }
        public Nullable<int> UPD_CNTR { get; set; }
        public Nullable<System.DateTime> UPD_TS { get; set; }
        public string UPD_USR_CD { get; set; }

    }
}