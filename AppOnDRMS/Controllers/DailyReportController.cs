using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppOnDRMS.Controllers
{
    public class DailyReportController : Controller
    {
        // GET: DailyReport
        public ActionResult DailyReportEntry()
        {
            HttpCookie cookie = HttpContext.Request.Cookies.Get("Other_Member_ID");
            string memeber_Id = cookie.Value;
            return View();
        }
    }
}