using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MachineLearing.Controllers
{
    public class LogoutController : Controller
    {
        // GET: Logout
        public ActionResult Logout()
        {
            //return View();
            if(Session["UserName"] != null)
            {
                Session.Remove("UserName");
            }
            return RedirectToAction("UserLogIn", "LogIn");
        }
    }
}