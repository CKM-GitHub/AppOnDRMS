using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DRMS_Models;
using CKM_CommonFunction;
using CKM_DataLayer;
using System.Data.SqlClient;

namespace WorkHistory_BL
{
    public class WorkHistoryBL
    {
        CKMDL cKMDL;
        FileFunction ff;
        public WorkHistoryBL()
        {
            cKMDL = new CKMDL();
            ff = new FileFunction();
        }

        public string DailyReportInsert(WorkHistoryModel workmodel)
        {
            cKMDL.UseTran = true;
            workmodel.Sqlprms = new SqlParameter[5];
            workmodel.Sqlprms[0] = new SqlParameter("@work_date", workmodel.work_date);
            workmodel.Sqlprms[1] = new SqlParameter("@project_id", workmodel.project_id);
            workmodel.Sqlprms[2] = new SqlParameter("@Attendance_time", workmodel.Attendance_time);
            workmodel.Sqlprms[3] = new SqlParameter("@Leave_time", workmodel.Leave_time);
            workmodel.Sqlprms[4] = new SqlParameter("@member_id", workmodel.member_id);

            return cKMDL.InsertUpdateDeleteData("WorkHistoryInsert", ff.GetConnectionWithDefaultPath("AppOnDRMS"), workmodel.Sqlprms);
        }

        public string GetProjectName(WorkHistoryModel workmodel)
        {
            cKMDL.UseTran = true;
            workmodel.Sqlprms = new SqlParameter[1];
            workmodel.Sqlprms[0] = new SqlParameter("@prjCD", workmodel.project_id);
            return cKMDL.SelectJson("GetProjectName", ff.GetConnectionWithDefaultPath("AppOnDRMS"), workmodel.Sqlprms);
        }

        public string GetWorkHistory(WorkHistoryModel workmodel)
        {
            cKMDL.UseTran = true;
            workmodel.Sqlprms = new SqlParameter[2];
            workmodel.Sqlprms[0] = new SqlParameter("@member_id", workmodel.member_id);
            workmodel.Sqlprms[1] = new SqlParameter("@work_date", workmodel.work_date);

            return cKMDL.SelectJson("GetWorkHistory", ff.GetConnectionWithDefaultPath("AppOnDRMS"), workmodel.Sqlprms);
        }
    }
}
