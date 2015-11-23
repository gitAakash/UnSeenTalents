using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using UnseentalentsApp.Models;
using UnseentalentsApp.Models.Repository;

namespace UnseentalentsApp.Controllers
{
    public class WebAppController : Controller
    {
        //
        // GET: /WebApp/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult login()
        {
            return View();
        }
        public ActionResult registration()
        {
            return View();
        }
        public ActionResult accountactivate(string id)
        {
            if (!string.IsNullOrEmpty(id) && id != "Update")
            {
                var encryptedString = id.Replace("_", "/").Replace("-", "+");
                string Desctr = Encryption.DecryptURL(encryptedString);

                string[] objArray = Desctr.Split(',');
                string uid = objArray[0];
                string type = objArray[1];
                UnseenTalentsMethod dbMethod = new UnseenTalentsMethod();
                var rst = dbMethod.VerifyEmail(Convert.ToInt64(uid), type);
                ViewBag.done = rst.ToString().ToLower();
                ViewBag.email = id;
            }
            if (id == "Update")
            {
                ViewBag.expire = "Update";
            }

            return View();
        }

        public ActionResult welcome()
        {
            return View();
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("index", "webapp");
        }

        public ActionResult upcomingevents()
        {
            return View();
        }

        public ActionResult eventdetail()
        {
            return View();
        }

        public ActionResult Payment()
        {
            return View();
        }
    }
}