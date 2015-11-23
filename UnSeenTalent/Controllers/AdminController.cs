using System.Web.Mvc;
using System.Web.Security;
using UnseentalentsApp.Models.Repository;

namespace UnseentalentsApp.Controllers
{
    public class AdminController : Controller
    {
        public ActionResult Login()
        {
            var cUser = new CurrentUser();
            if (cUser.userid != 0)
            {
                return Redirect("Dashboard");
            }
            return View("Index");
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
            return RedirectToAction("Login", "admin");
        }
    }
}