using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using User_BL;
using DRMS_Models;

namespace AppOnDRMS.Controllers
{
    public class UserController : Controller
    {
        
        // GET: User
        public ActionResult UserLogin() 
        {
            
            UserBL u_BL = new UserBL();
            UserLoginModel m = new UserLoginModel();
            m.member_id = "admin";
            string a = u_BL.GetUser(m);
            return View();
        }
        [HttpPost]
        public ActionResult UserLogin(UserLoginModel m_Login)
        {
            UserBL u_BL = new UserBL();
            UserLoginModel m = new UserLoginModel();
            m.member_id = "admin";
            string a = u_BL.GetUser(m);
            return View();
        }
    }
}