using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.ViewModels;
using System.Data.Entity;


namespace WebApplication1.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            string userAgent = Request.UserAgent;

            if (userAgent.Contains("MSIE") || userAgent.Contains("Trident"))
            {
                // Redirect to a page indicating that Internet Explorer is not supported
                return RedirectToAction("UnsupportedBrowser");
            }
            return View();
        }

        public ActionResult UnsupportedBrowser()
        {
            ViewBag.Message = "Browser not supported. Please using Chrome, Firefox or Edge";
            return View("index");
        }
        [HttpPost]
        public ActionResult Authorize(WebApplication1.ViewModels.userLogin userModel)
        {
            string userAgent = Request.UserAgent;

            if (userAgent.Contains("MSIE") || userAgent.Contains("Trident"))
            {
                // Redirect to a page indicating that Internet Explorer is not supported
                return RedirectToAction("UnsupportedBrowser");
            }
            using (Entities db= new Entities())
            { 
                var userNameCheck = db.SBS_COMS_USER.Where(x=>x.UserName==userModel.UserName).FirstOrDefault();

                if (userNameCheck == null)
                {
                    ViewBag.Message = "Wrong username or Password!";
                    return View("Index");
                }
                else if (userNameCheck.AccessFailedCount==5 || userNameCheck.LockoutEnabled=="1" )
                {
                    ViewBag.Message = "your account has been locked, please contact administrator!";
                    return View("Index");
                   
                }

                else
                {
                    if(userModel.PasswordHash !=userNameCheck.PasswordHash)
                    {
                        userNameCheck.AccessFailedCount = userNameCheck.AccessFailedCount + 1;
                        
                        db.SaveChanges();
                        ViewBag.Message = "Wrong username or Password!";
                        return View("Index");
                    }
                }         
                  
                    var userDetails = db.SBS_COMS_USER.Where ( x => x.UserName == userModel.UserName && x.PasswordHash == userModel.PasswordHash ).FirstOrDefault();
                    if( userDetails==null)
                    {
                     
                        ViewBag.Message = "Wrong username or Password!";
                        return View("Index", userModel);
                    }
                    else
                    {
                        //Check account aging
                        
                        //if login success,account has no expiration reset lockCount =0

                        userNameCheck.AccessFailedCount = 0;
                        userNameCheck.LAST_LOGIN = DateTime.Now;
                        db.SaveChanges();
                        db.Entry(userNameCheck).State = EntityState.Detached;
                        //this session initiate will handle in other class in future

                        Session["userName"] = userDetails.UserName;
                        Session["roles"] = userDetails.ROLES;
                        Session["companyCD"] = userDetails.Company_CD;
                        
                        Session["ID"] = userDetails.Id;
                        string coCD = "SBS";
                        string companyCD = userDetails.Company_CD;
                        

                        var definedValue = db.CM_BP_ALL.Where(d => d.COMPANY_CD == companyCD).Where(d => d.CO_CD == coCD).FirstOrDefault();
                        if (definedValue != null)
                        {
                            Session["curr"] = definedValue.BCUR_CD;
                            Session["CO_NM"] = definedValue.CO_NM;
                            Session["CNTRY_CD"] = definedValue.CNTRY_CD;
                            Session["USR_NM"] = definedValue.USR_NM;

                            string CNTRY_CD = definedValue.CNTRY_CD;
                            //get country name
                            var countryName = db.CM_COUNTRY_ALL.Where(d => d.COUNTRY_CD == d.COUNTRY_CD).Where(d => d.CO_CD == coCD).FirstOrDefault();
                            Session["COUNTRY_NM"] = countryName.COUNTRY_NM;
                        }
                        //check account aging
                        string username = Session["userName"].ToString();
                        if(WebApplication1.Utility.Helper.IsAccountExpired(username))
                        {
                            return RedirectToAction("changePassword", "User");
                        }
                      
                        return RedirectToAction("Index", "Home");
                    }
                 }
            }

    
          protected bool checkBrowser()
        {
            return false;
        }

          public ActionResult LogOut(WebApplication1.ViewModels.userLogin userModel)
        {
            using (Entities db = new Entities())
            {
                string userID = (string)Session["ID"];
                var userNameCheck = db.SBS_COMS_USER.Where(x => x.Id == userID).FirstOrDefault();
                userNameCheck.LAST_LOGOUT = DateTime.Now;
                db.SaveChanges();
                db.Entry(userNameCheck).State = EntityState.Detached;
                
                Session.Abandon();
                Session.Clear();
                TempData.Clear();
                return RedirectToAction("Index", "Login");
            }
        }
    }
}