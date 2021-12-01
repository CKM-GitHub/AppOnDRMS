using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DRMS_Models;
using WorkHistory_BL;
using User_BL;

namespace AppOnDRMS.Controllers
{
    public class DailyReportAPIController : ApiController
    {
        [HttpPost]
        [ActionName("DailyReportInsert")]
        public IHttpActionResult DailyReportInsert([FromBody] WorkHistoryModel workmodel)
        {
            WorkHistoryBL workbl = new WorkHistoryBL();
            return Ok(workbl.DailyReportInsert(workmodel));
        }

        [HttpPost]
        [ActionName("GetProjectName")]
        public IHttpActionResult GetProjectName([FromBody]WorkHistoryModel workmodel)
        {
            WorkHistoryBL workbl = new WorkHistoryBL();
            //string aa = workbl.GetProjectName(workmodel);
            return Ok(workbl.GetProjectName(workmodel));
        }

        [HttpPost]
        [ActionName("Getmember")]
        public IHttpActionResult Getmember([FromBody]UserLoginModel ulmodel)
        {
            UserBL userbl = new UserBL();
            //string aa = userbl.GetUser(ulmodel);
            return Ok(userbl.GetUser(ulmodel));
        }

        [HttpPost]
        [ActionName("GetWorkHistory")]
        public IHttpActionResult GetWorkHistory([FromBody]WorkHistoryModel workmodel)
        {
            WorkHistoryBL workbl = new WorkHistoryBL();
            //string aa = workbl.GetWorkHistory(workmodel);
            return Ok(workbl.GetWorkHistory(workmodel));
            //return Ok();
        }

    }
}
