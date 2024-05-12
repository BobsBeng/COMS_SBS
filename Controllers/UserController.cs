using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using System.Data.Entity;
using WebApplication1.ViewModels;
using PagedList.Mvc;
using PagedList;
using System.Text.RegularExpressions;
using WebApplication1.Utility;
using System.Configuration;
using System.Web.Configuration;

namespace WebApplication1.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index(string searchstring, string company, int? i)
        {
            //Check session
            string accessMode = ConfigurationManager.AppSettings.Get("accessMode");
            if (!isActiveSession() || accessMode == "External")
            {
                return RedirectToAction("Index", "Login");
            }
           
            using (Entities userModel = new Entities())
            {
                int pageList = 10;
                string CO_CD = ConfigurationManager.AppSettings.Get("COCD");
                List<userManagment> user = new List<userManagment>();

                var list = (from m in userModel.SBS_COMS_USER
                            join n in userModel.CM_BP_ALL on m.Company_CD equals n.COMPANY_CD into o
                            from n in o.DefaultIfEmpty()
                            where n.CO_CD == CO_CD
                            select new
                            {
                                m.Id,
                                m.UserName,
                                m.PasswordHash,
                                m.LockoutEnabled,
                                m.AccessFailedCount,
                                m.Company_CD,
                                m.ISACTIVE,
                                m.ROLES,
                                n.CO_NM
                            }).ToList();


                foreach (var p in list)
                {
                    userManagment addNew = new userManagment();
                    addNew.Id = p.Id;
                    addNew.UserName = p.UserName;
                    addNew.PasswordHash = p.PasswordHash;
                    addNew.Company_CD = p.Company_CD;
                    addNew.co_nm = p.CO_NM;
                    addNew.LockoutEnabled = p.LockoutEnabled;
                    addNew.ROLES = p.ROLES;
                    addNew.ISACTIVE = p.ISACTIVE;
                    addNew.AccessFailedCount = p.AccessFailedCount;

                    user.Add(addNew);
                }

                if (!string.IsNullOrEmpty(searchstring))
                {
                    user = user.Where(d => d.UserName.ToUpper().Contains(searchstring.ToUpper())).ToList();

                }
                if (!string.IsNullOrEmpty(company))
                {
                    user = user.Where(d => d.co_nm.ToUpper().Contains(company.ToUpper())).ToList();
                }


                if (user.Count() < pageList && user.Count() > 0)
                {
                    pageList = user.Count();
                }

                /*
                using (Entities dbModel = new Entities())
                {
                    var COlist = dbModel.CM_BP_ALL.Where(a => a.CO_CD.Equals(CO_CD)).ToList().Select(q => new
                    {
                        Company_CD = q.COMPANY_CD,
                        CO_NM = q.CO_NM + " (" + q.COMPANY_CD + ")"
                    });
                    ViewBag.Company = new SelectList(COlist, "Company_CD", "CO_NM");
                }
                */

                //return View(user.OrderBy(c => c.UserName).ToPagedList(i ?? 1, 10));
                string pdfDownloadEnable = ConfigurationManager.AppSettings.Get("pdfUser");
                ViewBag.pdfDownloadEnable = pdfDownloadEnable;
                return View(user);
            }

        }

        public ActionResult search()
        {
            //Check session
            if (!isActiveSession())
            {
                return RedirectToAction("Index", "Login");
            }
            return View();
        }

        //Registration method
        public ActionResult AddorEdit()
        {
            //Check session
            if (!isActiveSession())
            {
                return RedirectToAction("Index", "Login");
            }
           
            createViewBag(string.Empty);
            userRegistration useregistration = new userRegistration();
            return View(useregistration);
        }

        [HttpPost]
        public ActionResult AddorEdit(SBS_COMS_USER userModel)
        {

            //Check session
            if (!isActiveSession())
            {
                return RedirectToAction("Index", "Login");
            }
           

            //userLogin userlogin=new userLogin();
            string passwordCheck = userModel.PasswordHash;
            if (Regex.IsMatch(passwordCheck, @"^(?=.*[A-Za-z])(?=.*\d).+$"))
            {
                if (Utility.autoNumbering.HasSpecialCharacter(passwordCheck))
                {

                    try
                    {
                        using (Entities dbModel = new Entities())
                        {
                            var user = dbModel.SBS_COMS_USER.Where(x => x.UserName == userModel.UserName).FirstOrDefault();

                            if (user != null)
                            {
                                ViewBag.Message = "Username already exist";
                                createViewBag(string.Empty);
                                return View("AddOrEdit");

                            }
                            else
                            {
                                int userAging = 0;
                                string userAgingStr = ConfigurationManager.AppSettings.Get("userAging");
                                int.TryParse(userAgingStr, out userAging);
                                
                                userModel.Id = Guid.NewGuid().ToString();
                                userModel.ISACTIVE = "1";
                                userModel.LockoutEnabled = "0";
                                userModel.AccessFailedCount = 0;
                                userModel.EXPIRATION_DT = DateTime.Now.AddDays(userAging);
                                dbModel.Entry(userModel).State = EntityState.Added;
                                dbModel.SaveChanges();
                                ModelState.Clear();
                                createViewBag(string.Empty);
                                ViewBag.Message = "Registration successful.";

                            }
                        }
                    }

                    catch
                    {
                        createViewBag(string.Empty);
                        ViewBag.Message = "Registration failed.";
                        return View();
                    }
                }
                else
                {
                    ViewBag.Message = "Password should contain Special character";
                }
            }
            else
            {
                ViewBag.Message = "Password should contain alphanumeric";
            }

            createViewBag(string.Empty);
            return View("AddOrEdit");


        }

        //Get Detail Data
        public ActionResult Details(string Id)
        {
            using (Entities dbModel = new Entities())
            {
                return View(dbModel.SBS_COMS_USER.Where(x => x.Id == Id).FirstOrDefault());
            }
        }

        //Edit Customer method
        //Get: Customer /edit/5
        [HttpGet]
        public ActionResult Edit(string Id)
        {
            //Check session
            if (!isActiveSession())
            {
                return RedirectToAction("Index", "Login");
            }
           
            using (Entities dbModel = new Entities())
            {
                createViewBag(Id);
                return View(dbModel.SBS_COMS_USER.Where(x => x.Id == Id).FirstOrDefault());
            }
               
        }

        [HttpPost]
        public ActionResult Edit(string Id, SBS_COMS_USER aspnetUser)
        {
            //Check session
            if (!isActiveSession())
            {
                return RedirectToAction("Index", "Login");
            }
           

            try
            {
                using (Entities dbModel = new Entities())
                {

                    if (string.IsNullOrEmpty(aspnetUser.LockoutEnabled))
                    {
                        aspnetUser.LockoutEnabled = "0";
                    }

                    if (aspnetUser.LockoutEnabled.ToLower() == "true")
                    {
                        aspnetUser.LockoutEnabled = "1";
                    }
                    else
                    {
                        aspnetUser.LockoutEnabled = "0";
                    }

                    dbModel.SBS_COMS_USER.Add(aspnetUser);
                    dbModel.Entry(aspnetUser).State = EntityState.Modified;
                    dbModel.SaveChanges();

                }
                createViewBag(Id);
                ViewBag.Message = "Data updated successfully!";
                return View(aspnetUser);//return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Message = "Failed to update data!";
                return View();
            }

        }

        //Delete method actually is change isActive status become 0
        public ActionResult Delete(string Id)
        {
            //Check session
            if (!isActiveSession())
            {
                return RedirectToAction("Index", "Login");
            }
           
            using (Entities dbModel = new Entities())
            {
                return View(dbModel.SBS_COMS_USER.Where(x => x.Id == Id).FirstOrDefault());
            }
        }

        [HttpPost]
        public ActionResult Delete(string Id, SBS_COMS_USER aspnetUser)
        {
            //Check session
            if (!isActiveSession())
            {
                return RedirectToAction("Index", "Login");
            }
           
            try
            {
                using (var context = new Entities())
                {
                    aspnetUser.ISACTIVE = "0";
                    context.SBS_COMS_USER.Add(aspnetUser);
                    context.Entry(aspnetUser).State = EntityState.Modified;
                    context.SaveChanges();
                    ViewBag.Message = "Data deleted successfully!";
                    return View(aspnetUser);
                }
            }
            catch
            {
                return View();
            }
        }

        public ActionResult changePassword()
        {
            //Check session
            if (!isActiveSession())
            {
                return RedirectToAction("Index", "Login");
            }
            string message = ViewBag.Message;
            return View();

        }

        [HttpPost]
        public ActionResult changePassword(resetPassword resetPassword)
        {
            //Check session
            if (!isActiveSession())
            {
                return RedirectToAction("Index", "Login");
            }
           
            string userId = Convert.ToString(Session["userName"]);
            if (Utility.autoNumbering.HasSpecialCharacter(resetPassword.newPassword))
            {
                if (Regex.IsMatch(resetPassword.newPassword, @"^(?=.*[A-Za-z])(?=.*\d).+$"))
                {
                   
                    using (Entities dbModel = new Entities())
                    {
                        var user = dbModel.SBS_COMS_USER.Where(x => x.UserName == userId).FirstOrDefault();
                        if (user.PasswordHash == resetPassword.currentPassword)
                        {
                            string username = Session["userName"].ToString();
                            if (WebApplication1.Utility.Helper.checkPasswordHistory(resetPassword.newPassword, username))
                            {
                                ViewBag.Message = "Password Has Been Used";
                                return View();
                            }
                            int userAging = 0;
                            string userAgingStr = ConfigurationManager.AppSettings.Get("userAging");
                            int.TryParse(userAgingStr, out userAging);
                            user.PasswordHash = resetPassword.newPassword;
                            user.EXPIRATION_DT = DateTime.Now.AddDays(userAging);
                            dbModel.Entry(user).State = EntityState.Modified;
                            dbModel.SaveChanges();
                            ViewBag.Message = "Your Password is updated Successfully";
                        }
                        else
                        {
                            ViewBag.Message = "Invalid current password";
                        }
                    }
                }
                else
                {
                    ViewBag.Message = "password should contain alpanumeric";
                }
            }
            else
            {
                ViewBag.Message = "password should contain special character";
            }
            return View();
        }

        public ActionResult notAuthorized()
        {
            return View();
        }

        public void createViewBag(string Id)
        {
            string CO_CD = ConfigurationManager.AppSettings.Get("COCD");

            
            using (Entities dbModel = new Entities())
            {
             
                var list = dbModel.CM_TOKUI_ALL.Where(a => a.CO_CD.Equals(CO_CD)).ToList().Select(q => new
                {
                    Company_CD = q.CST_CD,
                    CO_NM = q.CST_CD + " (" + q.CST_NM + ")"
                });
                
                ViewBag.CompanyList = new SelectList(list.OrderBy(c => c.Company_CD), "Company_CD", "CO_NM", Id);

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
                return  false;
            }
            return isActive;
        }

        [HttpPost]
        public ActionResult UpdateAppSettings(bool pdfUser)
        {
            // Update the value in appSettings
            try
            {

                Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
                config.AppSettings.Settings["pdfUser"].Value = pdfUser.ToString();
                config.Save(ConfigurationSaveMode.Modified);
                return new HttpStatusCodeResult(200);
                }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(500, "An error occurred while updating AppSettings. Please try again later."); ; // Internal Server Error
            }

            
        }
    }
}