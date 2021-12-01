using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ConstructionList_BL;
using DRMS_Models;

namespace AppOnDRMS.Controllers
{
    public class ConstructionListAPIController : ApiController
    {
        [HttpPost]
        [ActionName("GetPrjData")]
        public IHttpActionResult GetPrjData()
        {
            ConstructionListBL constructionBL = new ConstructionListBL();
            return Ok(constructionBL.GetPrjData());
        }

        [HttpPost]
        [ActionName("GetPDFData")]
        public IHttpActionResult GetPDFData(ConstructionListModel clmodel)
        {
            ConstructionListBL constructionBL = new ConstructionListBL();
            return Ok(constructionBL.GetPDFData(clmodel));
        }
    }
}
