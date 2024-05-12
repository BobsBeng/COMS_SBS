using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models;
using WebApplication1.Controllers;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class userManagment
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public Nullable<System.DateTimeOffset> LockoutEnd { get; set; }
        public string LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public string Company_CD { get; set; }
        public string ISACTIVE { get; set; }
        public string ROLES { get; set; }
        public string co_nm { get; set; }
    }

    public class itemUnit
    {
        public string ITM_CD { get; set; }
        public string UNIT_CD { get; set; }
        public string  BASE_UNIT_CD { get; set; }
        public Nullable<System.Decimal> CONV_QTY { get; set; }
    }

    public class OrderH
    {
        public string T_SLO_NO { get; set; }
        public string CST_PO_NO { get; set; }
        public string CST_CD { get; set; }
        public string CST_CNTCT_PSN_NM { get; set; }
        public string SCST_CD { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<System.DateTime> T_SLO_DT { get; set; }
        public string SLIP_RMRKS { get; set; }
        public string CUR_CD { get; set; }
        public string INCTRMS_CD { get; set; }
        public string UKEBASHO_CD { get; set; }
        public string CNTRY { get; set; }
        public string PO_STS { get; set; }
        public string PO_STS_NM { get; set; }
        public string INTR_RMRKS { get; set; }
        public string SHIPPER { get; set; }
        public string CO_NM { get; set; }
        public string COUNTRY_NM { get; set; }
        public string PIC_NM { get; set; }
        public string INCOTERM { get; set; }
        public string CURRENCY { get; set; }
        public string DELIVERY_DEST { get; set; }
        public string REMARKS { get; set; }
        public string APPRV_STS { get; set; }

        public virtual ICollection<OrderD> OrderD { get; set; }

    }
    public class OrderD
    {
        public string T_SLO_NO { get; set; }
        public string T_SLO_D_NO { get; set; }
        public string SLO_NO { get; set; }
        public string SLO_D_NO { get; set; }
        public string ITM_CD { get; set; }
        public string ITM_NM { get; set; }
        public string INPT_UNIT_CD { get; set; }
        public decimal INPT_QTY { get; set; }
        public decimal UPRI { get; set; }
        public decimal CNV_QTY { get; set; }
        public Nullable<System.DateTime> RQST_DLV_DT { get; set; }
        public Nullable<System.DateTime> APPRV_DT { get; set; }
        public Nullable<System.DateTime> RQST_ARVD_DT { get; set; }
        public Nullable<System.DateTime> CAN_DT { get; set; }
        public decimal QTY { get; set; }
        public string UNIT_CD { get; set; }
        public string RMRKS { get; set; }
        public decimal SLS_AMT { get; set; }    
    }

    public class message
    {
        public string MID { get; set; }
        public string MSG { get; set; }
        public string VAL { get; set; }
    }

   
}