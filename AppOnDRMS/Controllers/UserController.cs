using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using User_BL;
using DRMS_Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AppOnDRMS.Controllers
{
    public class UserController : Controller
    {
        
        // GET: User
        public ActionResult UserLogin() 
        {
            string[] myCookies = Request.Cookies.AllKeys;
            foreach (string cookie in myCookies)
            {
                Response.Cookies[cookie].Expires = DateTime.Now.AddDays(-1);
            }
            UserBL user_bl = new UserBL();
            CompanyModel com_Model = user_bl.GetCompanyName();
            UserLoginModel login_Model = new UserLoginModel();
            login_Model.company_name = com_Model.company_name;
            login_Model.company_id = com_Model.company_id;
            return View(login_Model);
        }
        [HttpPost]
        public ActionResult UserLogin(UserLoginModel m_Login)
        {
            if(m_Login.member_id == "admin")
            {
                HttpCookie cookie = new HttpCookie("Admin_Member_ID", m_Login.member_id);
                Response.Cookies.Add(cookie);
                return RedirectToAction("Management", "User");
            }                     
            else
            {
                HttpCookie cookie = new HttpCookie("Other_Member_ID", m_Login.member_id);
                Response.Cookies.Add(cookie);
                return RedirectToAction("DailyReportEntry", "DailyReport");
            } 
        }
        public ActionResult Management(string id)
        {
            HttpCookie cookie = HttpContext.Request.Cookies.Get("Admin_Member_ID");
            if(cookie != null)
                return View();
            else
                return RedirectToAction("UserLogin", "User");
        }
    }
}