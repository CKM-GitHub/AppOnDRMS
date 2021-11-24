using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppOnDRMS.Controllers
{
    public class UserApiController : Controller
    {
        // GET: UserApi
        [HttpPost]
        [ActionName("GetUser")]
        public ActionResult GetUser()
        {
            return View();
        }
    }
}