using DRMS_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using User_BL;

namespace AppOnDRMS.Controllers
{
    public class EmployeeListController : Controller
    {
        // GET: EmployeeList
        public ActionResult EmployeeList()
        {
            HttpCookie cookie = HttpContext.Request.Cookies.Get("Admin_Member_ID");
            if (cookie != null)
            {
                UserBL user_bl = new UserBL();
                CompanyModel com_Model = user_bl.GetCompanyName();
                ViewBag.CompanyName = com_Model.company_name;
                return View();
            }
                
            else
                return RedirectToAction("UserLogin", "User");
        }
      
    }
}