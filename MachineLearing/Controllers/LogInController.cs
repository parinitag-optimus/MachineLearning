using MachineLearing.Models;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System.Xml.Linq;

namespace MachineLearing.Controllers
{
   public class LogInController : Controller
    {
        [HttpGet]
        public ActionResult UserLogIn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UserLogIn(UserLogIn u)
        {
            XDocument doc = XDocument.Load(Server.MapPath("~/App_Data/LogInXml.xml"));


            bool isUser = doc.Descendants("User")
              .Where(id => id.Attribute("userName").Value == u.UserName
                     && id.Attribute("passWord").Value == u.Password)
              .Any();

            if (isUser)
                return Redirect("/WelcomePage/WelcomePage");
            else
            {
                if(u.UserName !=null || u.Password != null)
                TempData["Message"] = "Invalid username or password";
                return View();
            }
                

        }

    }
}


