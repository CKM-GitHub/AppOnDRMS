﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppOnDRMS.Controllers
{
    public class DailyReportController : Controller
    {
        // GET: DailyReport
        public ActionResult DailyReportEntry()
        {
            return View();
        }
    }
}