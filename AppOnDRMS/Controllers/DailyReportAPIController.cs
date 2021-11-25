using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DRMS_Models;
using WorkHistory_BL;

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
    }
}
