using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace UnseentalentsApp.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Dashboard()
        {
            return View();
        }

        public ActionResult ManageEvents()
        {
            return View();
        }

     
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("index", "admin");
        }
	}
}