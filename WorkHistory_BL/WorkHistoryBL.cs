﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DRMS_Models;
using CKM_CommonFunction;
using CKM_DataLayer;
using Base_BL;
using System.Data.SqlClient;

namespace WorkHistory_BL
{
    public class WorkHistoryBL:BaseBL
    {
        CKMDL cKMDL;
        FileFunction ff;
        public WorkHistoryBL()
        {
            cKMDL = new CKMDL();
            ff = new FileFunction();
        }

        public string WorkHistoryInsert(WorkHistoryModel workmodel)
        {
            cKMDL.UseTran = true;
            workmodel.Sqlprms = new SqlParameter[4];
            workmodel.Sqlprms[0] = new SqlParameter("@work_date", workmodel.work_date);
            workmodel.Sqlprms[1] = new SqlParameter("@project_id", workmodel.project_id);
            workmodel.Sqlprms[2] = new SqlParameter("@Attendance_time", workmodel.Attendance_time);
            workmodel.Sqlprms[3] = new SqlParameter("@Leave_time", workmodel.Leave_time);

            return cKMDL.InsertUpdateDeleteData("WorkHistoryInsert", ff.GetConnectionWithDefaultPath("AppOnDRMS"), workmodel.Sqlprms);
        }
    }
}
