using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using User_BL;

namespace AppOnDRMS.Controllers
{
    public class EmployeeListApiController : ApiController
    {
        [HttpPost]
        [ActionName("EmployeeList")]
        public IHttpActionResult EmployeeList()
        {
            UserBL user_Bl = new UserBL();
            return Ok(user_Bl.GetStaff());
        }
    }
}
