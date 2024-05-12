using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using System.Data.Entity;
using WebApplication1.ViewModels;
using WebApplication1.Utility;
using System.Configuration;
using System.Data.Entity.Validation;


namespace WebApplication1.Controllers
{
    public class PurchaseOrderController : Controller
    {

        // GET: PurchaseOrder
        public ActionResult Index(string id)
        {
            //Check session
            string accessMode = ConfigurationManager.AppSettings.Get("accessMode");
            if (!isActiveSession(id) )
            {
                return RedirectToAction("Index", "Login");
            }
            //check account aging
            string username = Session["userName"].ToString();
            if (WebApplication1.Utility.Helper.IsAccountExpired(username))
            {
                ViewBag.Message = "Your account password has expired. Please update it.";
                return RedirectToAction("changePassword", "User");

            }

            string companyCD = string.Empty;
            if (Session["userName"].ToString() != "Administrator")
            {
                companyCD = Session["companyCD"].ToString();
            }


            //Load item if available
            string CO_CD = ConfigurationManager.AppSettings.Get("COCD");
            List<SBS_HT_JUCHUJ_TMP_ALL> CustOrder = new List<SBS_HT_JUCHUJ_TMP_ALL>();
            List<OrderD> OrderD = new List<OrderD>();
            OrderH myOrder = new OrderH();
            //set default value for order date
            myOrder.T_SLO_DT = DateTime.Now;

            if (!IsPostBack())
            {
                using (Entities db = new Entities())
                {
                    CustOrder = db.SBS_HT_JUCHUJ_TMP_ALL.Where(a => a.T_SLO_NO.Equals(id) && a.CO_CD.Equals(CO_CD)).ToList();

                    foreach (var m in CustOrder)
                    {
                        //Loop for detail

                        var list = (from o in m.SBS_HT_JUCHUH_TMP_ALL
                                    join p in db.HM_TOKUIH_ALL on o.ITM_CD equals p.ITM_CD into q
                                    from p in q.DefaultIfEmpty()
                                    where p.CO_CD == CO_CD && p.CST_CD == m.CST_CD
                                    select new
                                    {
                                        o.T_SLO_NO,
                                        o.T_SLO_D_NO,
                                        o.SLO_NO,
                                        o.SLO_D_NO,
                                        p.CST_ITM_CD,
                                        p.CST_ITM_NM,
                                        o.INPT_UNIT_CD,
                                        o.INPT_QTY,
                                        o.UPRI,
                                        o.CNV_QTY,
                                        o.RQST_DLV_DT,
                                        o.RQST_ARVD_DT,
                                        o.APPRV_DT,
                                        o.RMRKS,
                                        o.QTY,
                                        o.UNIT_CD,
                                        o.CAN_DT,
                                        o.SLS_AMT,
                                        o.APPRV_STS_TYP
                                    }).ToList();

                        foreach (var n in list)
                        {

                            OrderD OrderD2 = new OrderD();
                            OrderD2.T_SLO_NO = n.T_SLO_NO;
                            OrderD2.T_SLO_D_NO = n.T_SLO_D_NO;
                            OrderD2.SLO_NO = n.SLO_NO;
                            OrderD2.SLO_D_NO = n.T_SLO_D_NO;
                            OrderD2.ITM_CD = n.CST_ITM_CD;
                            OrderD2.ITM_NM = n.CST_ITM_NM;
                            OrderD2.INPT_UNIT_CD = n.INPT_UNIT_CD;
                            OrderD2.INPT_QTY = n.INPT_QTY;
                            OrderD2.UPRI = n.UPRI;
                            OrderD2.CNV_QTY = n.CNV_QTY;
                            OrderD2.RQST_DLV_DT = n.RQST_DLV_DT;
                            OrderD2.RQST_ARVD_DT = n.RQST_ARVD_DT;
                            OrderD2.APPRV_DT = n.APPRV_DT;
                            OrderD2.QTY = n.QTY;
                            OrderD2.UNIT_CD = n.UNIT_CD;
                            OrderD2.SLS_AMT = n.SLS_AMT;
                            OrderD2.RMRKS = n.RMRKS;
                            OrderD2.CAN_DT = n.CAN_DT;
                            OrderD.Add(OrderD2);
                        }
                       
                        myOrder.T_SLO_NO = m.T_SLO_NO;
                        myOrder.CST_PO_NO = m.CST_PO_NO;
                        myOrder.CST_CD = m.CST_CD;
                        myOrder.CST_CNTCT_PSN_NM = m.CST_CNTCT_PSN_NM;
                        myOrder.SCST_CD = m.SCST_CD;
                        myOrder.T_SLO_DT = m.T_SLO_DT;
                        myOrder.SLIP_RMRKS = m.SLIP_RMRKS;
                        myOrder.CUR_CD = m.CUR_CD;
                        myOrder.INCTRMS_CD = m.INCTRMS_CD;
                        myOrder.UKEBASHO_CD = m.UKEBASHO_CD;
                        myOrder.OrderD = OrderD;
                        myOrder.PO_STS = m.ORD_STS;
                        myOrder.INTR_RMRKS = m.INTR_RMRKS;                      
                        myOrder.PO_STS_NM = db.SBS_SC_PO_STATUS.Where(a => a.STS_ID.Equals(m.ORD_STS) && a.CO_CD.Equals(CO_CD)).FirstOrDefault().STS_NM;
                        myOrder.SHIPPER = db.SBS_HT_JUCHUH_TMP_ALL.Where(a => a.T_SLO_NO.Equals(id) && a.CO_CD.Equals(CO_CD)).FirstOrDefault().RQST_ARVD_DT.ToString();
                        companyCD = m.CST_CD;
                        
                    }

                }

            }
          
            
            if (myOrder.T_SLO_NO != null )
            {
                ViewBag.uTate = "1";
                if (myOrder.PO_STS == "4")
                {
                    ViewBag.uTate = "0";
                }
            }
            
            else
            {
                ViewBag.uTate = "0";

            }

            //Create viewbag for company list,shipper
            if (string.IsNullOrEmpty(myOrder.SHIPPER))
            {
                ViewBag.shipper = "ETD";
            }
            else
            {
                ViewBag.shipper = "ETA";
            }
            createViewBag(companyCD);
            return View(myOrder);
        }

        public JsonResult getProducts(string CustCD)
        {
            string company = Session["companyCD"].ToString();
            string CO_CD = ConfigurationManager.AppSettings.Get("COCD");

            List<HM_TOKUIH_ALL> products = new List<HM_TOKUIH_ALL>();
            using (Entities db = new Entities())
            {
                products = db.HM_TOKUIH_ALL.Where(a => a.CST_CD.Equals(CustCD)).Where(a => a.CO_CD.Equals(CO_CD)).OrderBy(a => a.CST_ITM_CD).ToList();
                createViewBag(company);
            }

            return new JsonResult { Data = products, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult getProductsInfo(string itm_cd)
        {
            string company = Session["companyCD"].ToString();
            string CO_CD = ConfigurationManager.AppSettings.Get("COCD");
            string isAdmin = Session["roles"].ToString();

            List<HM_TOKUIH_ALL> products = new List<HM_TOKUIH_ALL>();
            using (Entities db = new Entities())
            {
                if (isAdmin == "Administrator")
                {
                    products = db.HM_TOKUIH_ALL.Where(a => a.CST_ITM_CD.Equals(itm_cd)).Where(a => a.CO_CD.Equals(CO_CD)).OrderBy(a => a.CST_ITM_CD).ToList();
                }

                else
                {
                    products = db.HM_TOKUIH_ALL.Where(a => a.CST_CD.Equals(company)).Where(a => a.CST_ITM_CD.Equals(itm_cd)).Where(a => a.CO_CD.Equals(CO_CD)).OrderBy(a => a.CST_ITM_CD).ToList();
                }
            }
            return new JsonResult { Data = products, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult getUnitCode(string itm_cd)
        {

            string CO_CD = ConfigurationManager.AppSettings.Get("COCD");
            using (Entities db = new Entities())
            {
                string query = @"SELECT T3.CST_ITM_CD, 
                                        T1.UNIT_CD, 
                                        T2.PCKG_UNIT_CD BASE_UNIT_CD, 
                                        T1.UNIT_CONV_QTY CONV_QTY  
                                FROM CM_ITM_UNIT_ALL T1, CM_HINMO_ALL T2, HM_TOKUIH_ALL T3
                                WHERE T1.ITM_CD = T2.ITM_CD
                                AND T2.ITM_CD = T3.ITM_CD
                                AND T1.CO_CD=T2.CO_CD 
                                AND T2.CO_CD=T3.CO_CD 
                                AND T1.CO_CD='" + CO_CD + "' AND T3.CST_ITM_CD = '" + itm_cd + "'";
                var UnitCD = db.Database.SqlQuery<itemUnit>(query).ToList();
                return new JsonResult { Data = UnitCD, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

        }

        public JsonResult getIncoterm()
        {

            string CO_CD = ConfigurationManager.AppSettings.Get("COCD");

            List<CM_INCOTERMS_ALL> products = new List<CM_INCOTERMS_ALL>();
            using (Entities db = new Entities())
            {
                products = db.CM_INCOTERMS_ALL.Where(a => a.CO_CD.Equals(CO_CD)).ToList();
            }
            return new JsonResult { Data = products, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult getCompanyCode(string COMPANY_CD, string isDDL)
        {
            string CO_CD = ConfigurationManager.AppSettings.Get("COCD");
            string isAdmin = Session["roles"].ToString();
            List<company> company = new List<company>();
            using (Entities db = new Entities())
            {
                if (isAdmin == "Administrator" && isDDL != "1")// 
                {

                    try
                    {
                        company = db.Database.SqlQuery<company>(@"SELECT DISTINCT A.*,
                                                                        B.COUNTRY_NM CNTRY_NM,
                                                                        C.INCTRMS_CD, C.UKEBASHO_CD,  D.SCST_CD
                                                                   FROM CM_BP_ALL A, CM_COUNTRY_ALL B, CM_TOKUI_ALL C, CM_NOUHIN_ALL D
                                                                   WHERE A.CNTRY_CD = B.COUNTRY_CD 
                                                                   AND A.COMPANY_CD = C.CST_CD 
                                                                   AND C.CST_CD = D.CST_CD(+)
                                                                   AND A.CO_CD = B.CO_CD 
                                                                   AND A.CO_CD = C.CO_CD 
                                                                   AND A.CO_CD = D.CO_CD(+) 
                                                                   AND A.CO_CD='" + CO_CD + "' AND ROWNUM = 1 ").ToList();
                    }
                    catch (EntitySqlException ex)
                    {
                        createViewBag(COMPANY_CD);
                        return new JsonResult { Data = new { status = ex.Message } };
                    }
                }
                else
                {
                    company = db.Database.SqlQuery<company>(@"SELECT DISTINCT A.*,
                                                                        B.COUNTRY_NM CNTRY_NM,
                                                                        C.INCTRMS_CD, C.UKEBASHO_CD,  D.SCST_CD
                                                                   FROM CM_BP_ALL A, CM_COUNTRY_ALL B, CM_TOKUI_ALL C, CM_NOUHIN_ALL D
                                                                   WHERE A.CNTRY_CD = B.COUNTRY_CD 
                                                                   AND A.COMPANY_CD = C.CST_CD
                                                                   AND C.CST_CD = D.CST_CD(+)
                                                                   AND A.CO_CD = D.CO_CD(+) 
                                                                   AND A.CO_CD = B.CO_CD 
                                                                   AND A.CO_CD = C.CO_CD 
                                                                   AND A.CO_CD='" + CO_CD + "' AND A.COMPANY_CD='" + COMPANY_CD + "' AND ROWNUM = 1").ToList();
                }

            }
            createViewBag(COMPANY_CD);
            return new JsonResult { Data = company, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult getMinMaxItemCd(string itm_Cd, string unit_Cd)
        {
            string CO_CD = ConfigurationManager.AppSettings.Get("COCD");
            List<hinmoi> hinmoi = new List<hinmoi>();
            int pckg;
            using (Entities db = new Entities())
            {
                try
                {
                    hinmoi = db.Database.SqlQuery<hinmoi>("SELECT B.CST_ITM_CD,NVL(A.SUB_NUM9,0) MIN ,NVL(A.SUB_NUM10,0) MAX , NVL(A.SUB_NUM5,0) LT , '" + unit_Cd + "' UNIT_CD FROM CM_HINMOI_ALL A, HM_TOKUIH_ALL B WHERE A.ITM_CD = B.ITM_CD AND A.CO_CD='" + CO_CD + "' AND B.CST_ITM_CD='" + itm_Cd + "'").ToList();
                    var conversion = db.Database.SqlQuery<decimal>("SELECT NVL(T1.UNIT_CONV_QTY,1) UNIT_CONV_QTY FROM CM_ITM_UNIT_ALL T1, HM_TOKUIH_ALL T2 WHERE T1.ITM_CD = T2.ITM_CD AND T2.CST_ITM_CD = '" + itm_Cd + "' AND T1.UNIT_CD ='" + unit_Cd + "' AND T1.CO_CD ='" + CO_CD + "'").SingleOrDefault();
                    var packingUnitCd = db.Database.SqlQuery<string>("SELECT PCKG_UNIT_CD FROM CM_HINMO_ALL T1, HM_TOKUIH_ALL T2 WHERE T1.ITM_CD = T2.ITM_CD AND T2.CST_ITM_CD = '" + itm_Cd + "'").SingleOrDefault();
                    if (unit_Cd == packingUnitCd)
                    {
                        //std packing 
                        pckg = 1;                       
                    }
                    else
                    {
                        var packingUnitqty = db.Database.SqlQuery<int>("SELECT PCKG_UNIT_QTY FROM CM_HINMO_ALL T1, HM_TOKUIH_ALL T2 WHERE T1.ITM_CD = T2.ITM_CD AND T2.CST_ITM_CD = '" + itm_Cd + "'").SingleOrDefault();
                        pckg = packingUnitqty;
                    }
                    var list = hinmoi.Select(m => new
                    {
                        lt = m.lt,
                        itm_Cd = m.itm_Cd,
                        unit_Cd = m.unit_Cd,
                        min = Math.Round(m.min / conversion, 2),
                        max = Math.Round(m.max / conversion, 2),
                        standardPacking = pckg
                    }).ToList();
                    return new JsonResult { Data = list, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

                }
                catch (Exception ex)
                {

                }
                return new JsonResult { Data = hinmoi, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

        }

        [HttpPost]
        public JsonResult generateOrder(OrderH order, string actionID, string state)
        {

            try
            {
                string TRD_TYP = "1";
                decimal xrate = 1;
                Entities db = new Entities();
                message msg = new message();
                SBS_HT_JUCHUJ_TMP_ALL Header = new SBS_HT_JUCHUJ_TMP_ALL();
                string insUserCd = Session["userName"].ToString();
                string CO_CD = ConfigurationManager.AppSettings.Get("COCD");
                DateTime period = Convert.ToDateTime(order.T_SLO_DT);
                int lastNumb = Utility.autoNumbering.getLastNumber("SOTH", period);
                int lastNumbD = Utility.autoNumbering.getLastNumber("SOTD", period);
                var ckCount = db.SBS_HT_JUCHUJ_TMP_ALL.Where(a => a.CO_CD.Equals(CO_CD) && a.T_SLO_NO.Equals(order.T_SLO_NO)).ToList();

                //Handling process based on action (return to cancel, return to abort, acknowledge)
                if (actionID == "3" || actionID == "4" || actionID == "5")
                {
                    order.T_SLO_NO = ckCount[0].T_SLO_NO;
                    db.Database.ExecuteSqlCommand("Update SBS_HT_JUCHUJ_TMP_ALL set ORD_STS = '" + actionID + "' where T_SLO_NO = '" + order.T_SLO_NO + "' ");
                }
               
                else
                {
                 
                    if (ckCount.Count > 0 && state == "0" && order.PO_STS !="6")
                    {
                        msg.MID = "warning";
                        msg.MSG = "PO number already exists";
                        return Json(msg, JsonRequestBehavior.AllowGet);
                    }

                    if (state == "1")
                    {
                        order.T_SLO_NO = ckCount[0].T_SLO_NO;
                    }
                    else
                    {
                        order.T_SLO_NO = Convert.ToDateTime(period).ToString("yyMM") + lastNumb.ToString("000000");
                    }

                    var customerInfo = db.CM_TOKUI_ALL.Where(a => a.CO_CD.Equals(CO_CD) && a.CST_CD.Equals(order.CST_CD)).FirstOrDefault();
                    var taxInfo = db.CM_CTAX_ALL.Where(a => a.CO_CD.Equals(CO_CD) && a.CTAX_CD.Equals(customerInfo.SLS_CTAX_CD)).FirstOrDefault();
                    if (ConfigurationManager.AppSettings.Get("CNTRY") != (db.CM_BP_ALL.Where(a => a.CO_CD.Equals(CO_CD) && a.COMPANY_CD.Equals(order.CST_CD)).FirstOrDefault().CNTRY_CD))
                    {
                        TRD_TYP = "2";
                    }

                    if (order.CUR_CD != "USD")
                    {
                        string query = @"SELECT BASE_RATE FROM CM_RATE_ALL
                                    WHERE 0=0
                                    AND CURRENCY_CD = '" + order.CUR_CD + "' AND ( TO_DATE('" + period.ToString("DD/MM/YYYY") + "','DD/MM/YYYY') BETWEEN START_TS AND END_TS) AND CO_CD='" + CO_CD + "'  AND ROWNUM=1";

                        xrate = db.Database.SqlQuery<decimal>(query).FirstOrDefault();
                    }

                    //Processing header
                    Header.CO_CD = CO_CD;
                    Header.T_SLO_NO = order.T_SLO_NO;
                    Header.SLS_TYP = "1";
                    if (Header.CST_PO_NO != null)
                    {
                        Header.CST_PO_NO = Header.CST_PO_NO.ToUpper();
                    }
                    else
                    Header.CST_PO_NO = order.CST_PO_NO;
                    Header.CST_CD = order.CST_CD;
                    Header.CST_CNTCT_DPT_NM = null;
                    Header.CST_CNTCT_PSN_NM = order.CST_CNTCT_PSN_NM;
                    Header.SCST_CD = order.SCST_CD;
                    Header.SCST_ADDR_NO = null;
                    Header.SCST_CNTCT_DPT_NM = null;
                    Header.SCST_USR_NM = null;
                    Header.CSTMR_CD = null;
                    Header.CSTMR_ADDR_NO = null;
                    if (order.PO_STS == "6")
                    {
                        Header.T_SLO_DT = DateTime.Now;
                    }
                    else
                    Header.T_SLO_DT = order.T_SLO_DT;                   
                    Header.SLS_RND_TYP = customerInfo.SLS_RND_TYP;
                    Header.CTAX_RND_TYP = customerInfo.CTAX_RND_TYP;
                    Header.CTAX_CALC_TYP = customerInfo.CTAX_CALC_TYP;
                    Header.FARE_TYP = "1";
                    Header.INTR_RMRKS = order.INTR_RMRKS;
                    Header.SLIP_RMRKS = order.SLIP_RMRKS;
                    Header.CAN_FLG = "0";
                    Header.CAN_DT = null;
                    Header.CAN_USR_CD = null;
                    Header.CUR_CD = order.CUR_CD;
                    Header.XRATE = xrate;
                    Header.XRATE_DT = order.T_SLO_DT;
                    Header.XCNTRCT_FLG = "0";
                    Header.XCNTRCT_NO = null;
                    Header.XRATE_TYP = "00"; //corporate
                    Header.TRD_TYP = TRD_TYP;
                    Header.INCTRMS_CD = order.INCTRMS_CD;
                    Header.UKEBASHO_CD = customerInfo.UKEBASHO_CD;
                    Header.PAY_COND_CD = customerInfo.PAY_COND_CD;
                    Header.SRC_TYP = "0";
                    Header.INS_TS = DateTime.Now;
                    Header.INS_USR_CD = insUserCd;
                    Header.UPD_CNTR = 0;
                    Header.UPD_TS = DateTime.Now;
                    Header.UPD_USR_CD = insUserCd;

                    if (order.PO_STS == "6")
                    {
                        Header.ORD_STS = "1";
                    }
                    else
                    Header.ORD_STS = actionID;
                    //Converting detail information

                    var result = from m in order.OrderD
                                 join n in db.HM_TOKUIH_ALL on m.ITM_CD equals n.CST_ITM_CD
                                 join o in db.CM_HINMO_ALL on n.ITM_CD equals o.ITM_CD
                                 where n.CO_CD == CO_CD && n.CST_CD == order.CST_CD
                                 select new
                                 {
                                     o.ITM_CD,
                                     o.ITM_NM,
                                     o.EXT_ITM_NM,
                                     n.CST_ITM_CD,
                                     n.CST_ITM_NM,
                                     m.INPT_QTY,
                                     m.INPT_UNIT_CD,
                                     o.UNIT_CD,
                                     m.UPRI,
                                     m.RQST_DLV_DT,
                                     m.RQST_ARVD_DT,
                                     m.APPRV_DT,
                                     m.RMRKS,
                                     o.SLS_ARRG_TYP,
                                     o.DSH_TYP

                                 };


                    foreach (var oItem in result)
                    {
                        SBS_HT_JUCHUH_TMP_ALL detail = new SBS_HT_JUCHUH_TMP_ALL();

                        //sales calculation
                        decimal salesAmt = 0;
                        if (customerInfo.SLS_RND_TYP == "1") ///round up
                        {
                            salesAmt = Convert.ToDecimal(RoundUp(Convert.ToDouble(oItem.INPT_QTY * oItem.UPRI), 2));
                        }
                        if (customerInfo.SLS_RND_TYP == "2") ///round down
                        {
                            salesAmt = RoundDown(oItem.INPT_QTY * oItem.UPRI, 2);
                        }
                        if (customerInfo.SLS_RND_TYP == "3") ///round off
                        {
                            salesAmt = Math.Round(oItem.INPT_QTY * oItem.UPRI, 2);
                        }

                        //tax calculation
                        decimal taxAmount = 0;
                        if (customerInfo.CTAX_RND_TYP == "1") ///round up
                        {
                            taxAmount = Convert.ToDecimal(RoundUp(Convert.ToDouble((taxInfo.CTAX_RATE / 100) * salesAmt), 2));
                        }
                        if (customerInfo.CTAX_RND_TYP == "2") ///round down
                        {
                            taxAmount = RoundDown((taxInfo.CTAX_RATE / 100) * salesAmt, 2);
                        }
                        if (customerInfo.CTAX_RND_TYP == "3") ///round off
                        {
                            taxAmount = Math.Round((taxInfo.CTAX_RATE / 100) * salesAmt, 2);
                        }

                        detail.CO_CD = CO_CD;
                        detail.T_SLO_NO = order.T_SLO_NO;
                        detail.T_SLO_D_NO = Convert.ToDateTime(period).ToString("yyMM") + lastNumbD.ToString("000000");
                        detail.SLO_NO = null;
                        detail.SLO_D_NO = null;
                        detail.SET_NO = null;
                        detail.GRP_NO = null;
                        detail.SLS_ARRG_TYP = oItem.SLS_ARRG_TYP;
                        detail.DSH_TYP = oItem.DSH_TYP;
                        detail.ITM_CD = oItem.ITM_CD;
                        detail.ITM_NM = oItem.ITM_NM;
                        detail.EXT_ITM_NM = oItem.EXT_ITM_NM;
                        detail.INPT_UNIT_CD = oItem.INPT_UNIT_CD;
                        detail.INPT_QTY = oItem.INPT_QTY;
                        detail.CNV_QTY = db.CM_ITM_UNIT_ALL.Where(a => a.CO_CD.Equals(CO_CD) && a.ITM_CD.Equals(oItem.ITM_CD) && a.UNIT_CD.Equals(oItem.INPT_UNIT_CD)).FirstOrDefault().UNIT_CONV_QTY;
                        detail.UNIT_CD = oItem.UNIT_CD;
                        detail.SHIP_RQST_DT = oItem.RQST_DLV_DT;
                        detail.RQST_DLV_DT = oItem.RQST_DLV_DT;
                        detail.RQST_ARVD_DT = oItem.RQST_ARVD_DT ;
                        detail.APPRV_DT = oItem.APPRV_DT;
                        detail.RMRKS = oItem.RMRKS;
                        detail.APPRV_STS_TYP = null;
                        detail.APPRV_USR_CD = null;
                        detail.QTY = Math.Round(db.CM_ITM_UNIT_ALL.Where(a => a.CO_CD.Equals(CO_CD) && a.ITM_CD.Equals(oItem.ITM_CD) && a.UNIT_CD.Equals(oItem.INPT_UNIT_CD)).FirstOrDefault().UNIT_CONV_QTY * oItem.INPT_QTY, 3);
                        detail.UPRI = oItem.UPRI;
                        detail.SPE_DSCNT_UPRI = 0;
                        detail.DSCNT_FLG = "0";
                        detail.DSCNT = 0;
                        detail.DSCNT_UPRI = oItem.UPRI;
                        detail.SLS_AMT = salesAmt + taxAmount;
                        detail.XTX_SLS_AMT = salesAmt;
                        detail.CTAX_SPEC_FLG = "0";
                        detail.CTAX_CD = customerInfo.SLS_CTAX_CD;
                        detail.CTAX_RATE = taxInfo.CTAX_RATE;
                        detail.CTAX_TYP = taxInfo.CTAX_TYP;
                        detail.CTAX_AMT = taxAmount;
                        detail.SLS_COST_UPRI = 0;
                        //detail.RMRKS = "";
                        detail.CAN_FLG = "0";
                        detail.CAN_DT = null;
                        detail.CAN_USR_CD = null;
                        detail.CUR_CD = Header.CUR_CD;
                        detail.XRATE = Header.XRATE;
                        detail.XRATE_DT = Header.XRATE_DT;
                        detail.XCNTRCT_FLG = Header.XCNTRCT_FLG;
                        detail.XCNTRCT_NO = Header.XCNTRCT_NO;
                        detail.BCUR_XTX_SLS_AMT = salesAmt * Header.XRATE;
                        detail.XRATE_TYP = Header.XRATE_TYP;
                        detail.SNO = null;
                        detail.SUB_SNO = null;
                        detail.INS_TS = DateTime.Now;
                        detail.INS_USR_CD = insUserCd;
                        detail.UPD_CNTR = 0;
                        detail.UPD_TS = DateTime.Now;
                        detail.UPD_USR_CD = insUserCd;

                        lastNumbD = lastNumbD + 1;

                        Header.SBS_HT_JUCHUH_TMP_ALL.Add(detail);
                    } //end of foreach

                    //Check if data is exist delete first before insert
                    if (ckCount.Count > 0 && order.PO_STS !="6")
                    {
                        db.Database.ExecuteSqlCommand("Delete from SBS_HT_JUCHUJ_TMP_ALL where T_SLO_NO = '" + ckCount[0].T_SLO_NO + "' ");
                        db.Database.ExecuteSqlCommand("Delete from SBS_HT_JUCHUH_TMP_ALL where T_SLO_NO = '" + ckCount[0].T_SLO_NO + "' ");
                    }

                    //Save to staging table
                    using (Entities coms = new Entities())
                    {
                        try
                        {
                            coms.SBS_HT_JUCHUJ_TMP_ALL.Add(Header);
                            coms.SaveChanges();
                            if (state != "1")
                            { Utility.autoNumbering.updateLastNumber("SOTH", period, lastNumb); }
                            Utility.autoNumbering.updateLastNumber("SOTD", period, lastNumbD - 1);
                        }
                        catch (DbEntityValidationException ex)
                        {

                            msg.MID = "error";
                            msg.MSG = ex.Message;
                            return Json(msg, JsonRequestBehavior.AllowGet);
                        }


                    }

                } // end of else
                msg.MID = "success";
                msg.MSG = "Update successfully";
                if (state == "1")
                {
                    msg.VAL = order.T_SLO_NO.ToString();
                }
                
                return Json(msg, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                message msg = new message();
                msg.MID = "error";
                msg.MSG = ex.Message;
                return Json(msg, JsonRequestBehavior.AllowGet);
            }


        }

        public void createViewBag(string cstCD)
        {
            string CO_CD = ConfigurationManager.AppSettings.Get("COCD");

            using (Entities dbModel = new Entities())
            {
                string defaultValue = null;
                var list = dbModel.CM_TOKUI_ALL.Where(a => a.CO_CD.Equals(CO_CD)).ToList().Select(q => new
                {
                    Company_CD = q.CST_CD,
                    CO_NM = q.CST_CD + " (" + q.CST_NM + ")"
                });

                if (!string.IsNullOrEmpty(cstCD))
                {
                    defaultValue = cstCD;
                }
                ViewBag.CustomerList = new SelectList(list.OrderBy(c => c.CO_NM), "Company_CD", "CO_NM", defaultValue);

            }
        }

        public bool isActiveSession(string id)
        {
            //Check session
            
            bool isActive = true;
            bool isNotSameUser = false;
            string user = string.Empty;
            string cst_CD = string.Empty;
            cst_CD =Session["companyCD"].ToString();
            string isAdmin = Session["userName"].ToString() ;
            if (id != null)
            { 
            using (Entities dbModel = new Entities())
            {
                //string defaultValue = null;
                string orderNoCust = dbModel.SBS_HT_JUCHUJ_TMP_ALL.Where(a => a.T_SLO_NO.Equals(id) && a.CO_CD.Equals("SBS")).FirstOrDefault().CST_CD;
            
                if(isAdmin !="admin")
                {
                if (orderNoCust !=cst_CD)
                    {
                        //message msg = new message();
                        //msg.MID = "error";
                        return isNotSameUser;
                    }
                }
            }
            }
            
 
            try
            {
                user = Session["userName"].ToString();
                
            }
            catch (Exception e)
            {
                return false;
            }
            return isActive;
        }

      
        protected bool IsPostBack()
        {
            bool isSameUrl = false;
            try { 
             isSameUrl = string.Compare(Request.Url.AbsolutePath,
               Request.UrlReferrer.AbsolutePath,
               StringComparison.CurrentCultureIgnoreCase) == 0;
            }
            catch (Exception e)
            {
                isSameUrl = false;
            }
            //return isSameUrl;
            return false;
        }

        public static double RoundUp(double input, int places)
        {
            double multiplier = Math.Pow(10, Convert.ToDouble(places));
            return Math.Ceiling(input * multiplier) / multiplier;
        }

        public decimal RoundDown(decimal i, double decimalPlaces)
        {
            var power = Convert.ToDecimal(Math.Pow(10, decimalPlaces));
            return Math.Floor(i * power) / power;
        }

        [HttpPost]
        public JsonResult deleteOrder(string poNumber)
        {

            try
            {
                Entities db = new Entities();
                string CO_CD = ConfigurationManager.AppSettings.Get("COCD");
                var ckCount = db.SBS_HT_JUCHUJ_TMP_ALL.Where(a => a.CO_CD.Equals(CO_CD) && a.T_SLO_NO.Equals(poNumber)).ToList();
                db.Database.ExecuteSqlCommand("delete from SBS_HT_JUCHUJ_TMP_ALL where T_SLO_NO = '" + poNumber + "' ");
                db.Database.ExecuteSqlCommand("delete from SBS_HT_JUCHUH_TMP_ALL where T_SLO_NO = '" + ckCount[0].T_SLO_NO.ToString() + "' ");

                message msg = new message();
                msg.MID = "success";
                msg.MSG = "Deleted";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                message msg = new message();
                msg.MID = "error";
                msg.MSG = ex.Message;
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
        }

    }

}


