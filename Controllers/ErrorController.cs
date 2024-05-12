using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index()
        {
            if (!isActiveSession())
            {
                return RedirectToAction("Index", "Login");
            }
            return View();
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