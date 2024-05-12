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
using System.Dynamic;
using System.IO;
using System.Drawing;
using PagedList.Mvc;
using PagedList;
using CrystalDecisions.CrystalReports.Engine;
using System.ComponentModel;


namespace WebApplication1.Controllers
{
    public class InquiryPOController : Controller
    {
        // GET: InquiryPO
        public ActionResult Index(string T_SLO_NO, string PONumber, string CustomerList, List<string> SelectedStatusNames /*, int? i*/)
        {

            //Check session
          
            if (!isActiveSession())
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
            
            List<string> statusNames = GetStatusNamesFromDatabase();
            ViewBag.StatusNames = statusNames;
            ViewBag.SelectedStatusNames = SelectedStatusNames;
            string pdfDownloadEnable = ConfigurationManager.AppSettings.Get("pdfUser");
            ViewBag.pdfDownloadEnable = pdfDownloadEnable;
            //List<OrderH> myOrder = new List<OrderH>();
          
            using (Entities db = new Entities())
            {

                string companyCd = Session["companyCD"].ToString();
                string roles = Session["roles"].ToString();
                var list = from m in db.SBS_HT_JUCHUJ_TMP_ALL
                           join status in db.SBS_SC_PO_STATUS on m.ORD_STS equals status.STS_ID
                           where m.SRC_TYP == "0"
                           select new
                           {
                               Juchuj = m,
                               m.CST_CD,
                               m.CST_PO_NO,
                               m.ORD_STS,
                               m.T_SLO_NO,
                               m.T_SLO_DT,
                               m.SLIP_RMRKS,
                               m.CST_CNTCT_PSN_NM,
                               m.CUR_CD,
                               m.INCTRMS_CD,
                               m.SCST_CD,
                               StatusName = status.STS_NM
                           };

                if (roles != "Administrator")
                {
                    if (!string.IsNullOrEmpty(PONumber))
                    {

                        list = list.Where(d => d.CST_CD.Contains(companyCd.ToUpper())).Where(d => d.CST_PO_NO.ToUpper().Contains(PONumber.ToUpper()));
                    }
                    else
                    {
                        list = list.Where(d => d.CST_CD.Equals(companyCd.ToUpper()));
                    }
                }
                if (!string.IsNullOrEmpty(CustomerList))
                {
                    list = list.Where(d => d.CST_CD.Contains(CustomerList.ToUpper()));
                }
                if (!string.IsNullOrEmpty(PONumber))
                {
                    list = list.Where(d => d.CST_PO_NO.ToUpper().Contains(PONumber.ToUpper()));
                }
                if (!string.IsNullOrEmpty(T_SLO_NO))
                {
                    list = list.Where(d => d.T_SLO_NO.Contains(T_SLO_NO));
                }
                if (SelectedStatusNames != null && SelectedStatusNames.Any())
                {
                    list = list.Where(d => SelectedStatusNames.Contains(d.StatusName));
                }

                var resultList = (from juchuj in list
                                  join cmBp in db.CM_BP_ALL on juchuj.CST_CD equals cmBp.COMPANY_CD
                                  join cmCountry in db.CM_COUNTRY_ALL on cmBp.CNTRY_CD equals cmCountry.COUNTRY_CD
                                  join sbsComsStatusPo in db.SBS_SC_PO_STATUS on juchuj.ORD_STS equals sbsComsStatusPo.STS_ID
                                  select new inquiryPO
                                  {
                                      // Select the properties you need from both tables
                                      T_SLO_NO = juchuj.T_SLO_NO,
                                      CO_NM = cmBp.CO_NM,
                                      CST_PO_NO = juchuj.CST_PO_NO,
                                      COUNTRY_NM = cmCountry.COUNTRY_NM,
                                      PIC_NM = juchuj.CST_CNTCT_PSN_NM,
                                      INCOTERM = juchuj.INCTRMS_CD,
                                      CURRENCY = juchuj.CUR_CD,
                                      DELIVERY_DEST = juchuj.SCST_CD,
                                      T_SLO_DT = juchuj.T_SLO_DT,
                                      REMARKS = juchuj.SLIP_RMRKS,
                                      APPRV_STS = sbsComsStatusPo.STS_NM
                                  }).ToList();

               
                var distinctResultList = resultList
                .GroupBy(r => r.T_SLO_NO)
                .Select(g => g.First())
                .ToList();

                List<OrderH> myOrder = distinctResultList.Select(item => new OrderH
                {
                    T_SLO_NO = item.T_SLO_NO,
                    CO_NM = item.CO_NM,
                    CST_PO_NO = item.CST_PO_NO,
                    COUNTRY_NM = item.COUNTRY_NM,
                    PIC_NM = item.PIC_NM,
                    INCOTERM = item.INCOTERM,
                    CURRENCY = item.CURRENCY,
                    DELIVERY_DEST = item.DELIVERY_DEST,
                    T_SLO_DT = item.T_SLO_DT,
                    REMARKS = item.REMARKS,
                    APPRV_STS = item.APPRV_STS
                }).ToList();

                               

                createViewBag(companyCd);
                return View(myOrder);
               
                //return View(distinctResultList.ToList().ToPagedList(i ?? 1, 10));
                //return View(distinctResultList.ToList());
                
            }

            
          
        }

        public JsonResult GETSBSJUCHUH(string T_SLO_NO, string purpose)
        {
            using (Entities db = new Entities())
            {
                var order = db.SBS_HT_JUCHUJ_TMP_ALL.Include(e => e.SBS_HT_JUCHUH_TMP_ALL)
                            .FirstOrDefault(e => e.T_SLO_NO == T_SLO_NO);

                if (order != null)
                {
                    var orderDetailsList = (from od in order.SBS_HT_JUCHUH_TMP_ALL
                                            join tokuih in db.HM_TOKUIH_ALL on od.ITM_CD equals tokuih.ITM_CD
                                            select new
                                            {
                                                T_SLO_D_NO = od.T_SLO_D_NO,
                                                ITM_CD = tokuih.CST_ITM_CD,
                                                ITM_NM = tokuih.CST_ITM_NM,
                                                QUANTITY = od.QTY,
                                                PRICE = od.UPRI,
                                                AMOUNT = od.QTY * od.UPRI
                                            }).ToList();

                    var orderDto = new
                    {
                        T_SLO_NO = order.T_SLO_NO,
                        CST_PO_NO = order.CST_PO_NO,
                        CST_CD = order.CST_CD,
                        orderDetails = orderDetailsList
                    };

                    return Json(orderDto, JsonRequestBehavior.AllowGet);
                }

                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GeneratePdf2(string T_SLO_NO)
        {
            ReportDocument reportDocument = new ReportDocument();
            string tempFilePath = null;

            try
            {
                List<reportData> reportDataList;
                //prepare dataset
                using (Entities db = new Entities())
                {
                    var query = from juchuJ in db.SBS_HT_JUCHUJ_TMP_ALL
                                join companyInfo in db.CM_BP_ALL on juchuJ.CST_CD equals companyInfo.COMPANY_CD
                                join companyInfo2 in db.CM_BP_ALL on juchuJ.SCST_CD equals companyInfo2.COMPANY_CD
                                join nouhin in db.CM_NOUHIN_ALL on juchuJ.CST_CD equals nouhin.CST_CD
                                join tokui in db.CM_TOKUI_ALL on juchuJ.CST_CD equals tokui.CST_CD
                                join paycond in db.CM_PAYCOND_ALL on tokui.PAY_COND_CD equals paycond.PAY_COND_CD
                                join juchuH in db.SBS_HT_JUCHUH_TMP_ALL on juchuJ.T_SLO_NO equals juchuH.T_SLO_NO
                                join hinmo in db.CM_HINMO_ALL on juchuH.ITM_CD equals hinmo.ITM_CD
                                join tokuih in db.HM_TOKUIH_ALL on juchuH.ITM_CD equals tokuih.ITM_CD
                                where juchuJ.T_SLO_NO == T_SLO_NO
                                where nouhin.CO_CD == "SBS" &&  companyInfo.CO_CD=="SBS" && tokui.CO_CD=="SBS" && paycond.CO_CD=="SBS" && companyInfo2.CO_CD=="SBS" && hinmo.CO_CD=="SBS"

                                select new reportData
                                {
                                    //header information
                                    T_SLO_NO = juchuJ.T_SLO_NO,
                                    PO_DT = juchuJ.T_SLO_DT??DateTime.MinValue,
                                    CST_CD = juchuJ.CST_CD,
                                    REMARKS = juchuJ.SLIP_RMRKS,
                                    CST_NM = companyInfo.CO_NM,
                                    CST_ADDR=companyInfo.ADDR1,
                                    CST_ADDR1=companyInfo.ADDR2,
                                    CST_ADDR2 = companyInfo.ADDR3,
                                    TELEPHONE = companyInfo.TEL,
                                    PO_NO = juchuJ.CST_PO_NO,
                                    INCOTERM =tokui.INCTRMS_CD,
                                    PAYTERM=tokui.PAY_COND_CD,
                                    PAYTERM_NM=paycond.PAY_COND_NM,
                                    DLVRLOC=juchuJ.SCST_CD,
                                    DLVLOC_NM = companyInfo2.CO_NM,
                                    SCST_ADDR = companyInfo2.ADDR1,
                                    SCST_ADDR1 = companyInfo2.ADDR2,
                                    SCST_ADDR2 = companyInfo2.ADDR3,
                                    SCST_ADDR3 = companyInfo2.ADDR4,
                                    SCST_PHONE = companyInfo2.TEL,
                                    SCST_MAIL = companyInfo2.ML_ADDR,
                                    USR_NM = companyInfo2.USR_NM,
                                    CURR=juchuJ.CUR_CD,
                                    SHIPPER=juchuJ.INTR_RMRKS,

                                    // Detail info
                                    TokuihId = tokuih.CST_ITM_CD,
                                    TokuihNm = tokuih.CST_ITM_NM,
                                    MODELNM = hinmo.EXT_ITM_NM,
                                    juchuHQty = juchuH.QTY,
                                    juchuHPrice = juchuH.UPRI,
                                    ETA = juchuH.RQST_ARVD_DT??DateTime.MinValue,
                                    ETD = juchuH.RQST_DLV_DT??DateTime.MinValue,
                                    UNIT = juchuH.UNIT_CD

                                };
                  
                                    reportDataList = query.ToList();

                }
                // Load the Crystal Reports RPT file
                reportDocument.Load(Server.MapPath("~/Reports/PurchaseOrder.rpt"));
                string dynamicFilename = reportDataList.Count > 0 ? reportDataList[0].CST_CD + reportDataList[0].T_SLO_NO : "DefaultFilename";
                // Your additional Crystal Reports configuration (parameters, data source, etc.)
                reportDocument.SetDataSource(reportDataList);
                // Export the report to a temporary file
                tempFilePath = Path.GetTempFileName();
                reportDocument.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, tempFilePath);
                
                // Set response headers
                Response.Buffer = true;
                Response.Clear();
                Response.ClearHeaders();
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename="+dynamicFilename+".pdf");

                // Open the file stream for reading
                using (FileStream stream = new FileStream(tempFilePath, FileMode.Open, FileAccess.Read))
                {
                    // Write the content to the response
                    stream.CopyTo(Response.OutputStream);
                }

                // End the response
                Response.Flush();
                Response.SuppressContent = true;  // to avoid undesired additional content
                HttpContext.ApplicationInstance.CompleteRequest();

                // Return a result (this line should not be reached because of Response.End)
                return Content("Download initiated...");
            }
            catch (Exception ex)
            {
                return Content("Error generating or downloading PDF: {ex.Message}");
            }
            finally
            {
                // Clean up resources
                reportDocument.Close();
                reportDocument.Dispose();

                // Delete the temporary file
                if (!string.IsNullOrEmpty(tempFilePath) && System.IO.File.Exists(tempFilePath))
                {
                    System.IO.File.Delete(tempFilePath);
                }
            }

        }

        public void createViewBag(string cstCD)
        {
            string CO_CD = ConfigurationManager.AppSettings.Get("COCD");
            string role = Session["roles"].ToString();
            using (Entities dbModel = new Entities())
            {
                string defaultValue = null;
                var list = dbModel.CM_TOKUI_ALL.Where(a => a.CO_CD.Equals(CO_CD)).ToList().Select(q => new
                {
                    Company_CD = q.CST_CD,
                    CO_NM = q.CST_CD + " (" + q.CST_NM+ ")"
                });

                if (!string.IsNullOrEmpty(cstCD))
                {
                    defaultValue = cstCD;
                }
                if (role == "Administrator")
                {
                    defaultValue = "";
                }

                ViewBag.CustomerList = new SelectList(
                new List<SelectListItem>
                {
                    new SelectListItem { Value = "", Text = "Select an option" }, // Blank or default value
                }.Concat(list.OrderBy(c => c.CO_NM).Select(c => new SelectListItem { Value = c.Company_CD, Text = c.CO_NM })),
                "Value",
                "Text",
                defaultValue
                );
            }
        }

        private List<string> GetStatusNamesFromDatabase()
        {
            // Replace this with your actual database retrieval logic
            using (var dbContext = new Entities())
            {
                return dbContext.SBS_SC_PO_STATUS.Select(s => s.STS_NM).ToList();
            }
        }

        public bool isActiveSession()
        {
            //Check session
            bool isActive = true;
            string user = string.Empty;
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
            try
            {
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

     
    }
}