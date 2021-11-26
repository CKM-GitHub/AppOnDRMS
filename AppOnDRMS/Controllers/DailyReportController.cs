using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DRMS_Models;
using User_BL;

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
                UserBL user_bl = new UserBL();
                ulmodel.member_id = cookie.Value;
                CompanyModel com_Model = user_bl.GetCompanyName();
                ulmodel.company_name = com_Model.company_name;
                ulmodel.company_id = com_Model.company_id;
                return View(ulmodel);
            }
            else
                return RedirectToAction("UserLogin", "User");
        }
    }
}