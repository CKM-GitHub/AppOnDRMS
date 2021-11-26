using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ConstructionList_BL;

namespace AppOnDRMS.Controllers
{
    public class ConstructionListAPIController : ApiController
    {
        [HttpPost]
        [ActionName("EmployeeList")]
        public IHttpActionResult GetPrjData()
        {
            ConstructionListBL constructionBL = new ConstructionListBL();
            return Ok(constructionBL.GetPrjData());
        }
    }
}
