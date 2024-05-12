using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            if (!isActiveSession())
            {
                return RedirectToAction("Index", "Login");
            }
            string roles = Session["roles"].ToString();
            string custCode = Session["companyCD"].ToString();
            string accessMode = ConfigurationManager.AppSettings.Get("accessMode");
            try 
            { 
                using (Entities db = new Entities())
                {
                    List<OrdStsCountResult> result = null;
                    if (roles != "Administrator")
                    {
                    result = db.SBS_HT_JUCHUJ_TMP_ALL
                    .GroupBy(item => new{item.ORD_STS, item.CST_CD})
                    .Select(group => new OrdStsCountResult
                    {
                        ord_sts = group.Key.ORD_STS,
                        CST_CD = group.Key.CST_CD,
                        ord_sts_count = group.Count()
                    })
                    .Where(item => item.CST_CD == custCode)
                    .ToList();
                         var finalResult = result.ToList(); 
                    }
                    else
                    { 
                        result = db.SBS_HT_JUCHUJ_TMP_ALL
                        .GroupBy(item => new { item.ORD_STS })
                        .Select(group => new OrdStsCountResult
                        {
                            ord_sts = group.Key.ORD_STS,
                            ord_sts_count = group.Count()
                        }).ToList();
                        
                    }
                    int countForOrdSts0 = (result.FirstOrDefault(x => x.ord_sts == "0") != null)
                    ? result.FirstOrDefault(x => x.ord_sts == "0").ord_sts_count : 0;
                    int countForOrdSts1 = (result.FirstOrDefault(x => x.ord_sts == "1") != null)
                    ? result.FirstOrDefault(x => x.ord_sts == "1").ord_sts_count : 0;
                    int countForOrdSts3 = (result.FirstOrDefault(x => x.ord_sts == "3") != null)
                    ? result.FirstOrDefault(x => x.ord_sts == "3").ord_sts_count : 0;
                    int countForOrdSts5 = (result.FirstOrDefault(x => x.ord_sts == "5") != null)
                    ? result.FirstOrDefault(x => x.ord_sts == "5").ord_sts_count : 0;
                    int countForOrdSts7 = (result.FirstOrDefault(x => x.ord_sts == "7") != null)
                    ? result.FirstOrDefault(x => x.ord_sts == "7").ord_sts_count : 0;

                   ViewBag.countForOrdSts0 = countForOrdSts0;
                   ViewBag.CountForOrdSts1 = countForOrdSts1;
                   ViewBag.CountForOrdSts3 = countForOrdSts3;
                   ViewBag.CountForOrdSts5 = countForOrdSts5;
                   ViewBag.CountForOrdSts7 = countForOrdSts7;
                   ViewBag.AccessMode = accessMode;
                   return View(result);

                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception appropriately
                ViewBag.ErrorMessage = ex.Message;
                return View("Error"); // Display an error view
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
    }
}