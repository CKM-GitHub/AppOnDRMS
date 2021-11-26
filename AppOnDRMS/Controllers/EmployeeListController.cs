using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppOnDRMS.Controllers
{
    public class EmployeeListController : Controller
    {
        // GET: EmployeeList
        public ActionResult EmployeeList()
        {
            HttpCookie cookie = HttpContext.Request.Cookies.Get("Admin_Member_ID");
            if (cookie != null)
                return View();
            else
                return RedirectToAction("UserLogin", "User");
        }
    }
}