using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DRMS_Models;

namespace AppOnDRMS.Controllers
{
    public class DailyReportController : Controller
    {
        // GET: DailyReport
        public ActionResult DailyReportEntry(UserLoginModel ulmodel)
        {
            HttpCookie cookie = HttpContext.Request.Cookies.Get("Other_Member_ID");
            if (cookie != null)
            {
                ulmodel.member_id = cookie.Value;
                return View(ulmodel);
            }
            else
                return RedirectToAction("UserLogin", "User");
        }
    }
}