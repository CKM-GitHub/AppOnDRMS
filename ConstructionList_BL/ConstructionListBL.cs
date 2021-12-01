using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using CKM_CommonFunction;
using CKM_DataLayer;
using DRMS_Models;

namespace ConstructionList_BL
{
    public class ConstructionListBL
    {
        CKMDL dl = new CKMDL();
        FileFunction ff = new FileFunction();
        public string GetPrjData()
        {
            return dl.SelectJson("GetProjectName", ff.GetConnectionWithDefaultPath("AppOnDRMS"));
        }

        public string GetPDFData(ConstructionListModel clmodel)
        {
            clmodel.Sqlprms = new SqlParameter[3];
            clmodel.Sqlprms[0] = new SqlParameter("@prjCD", clmodel.prjCD);
            clmodel.Sqlprms[1] = new SqlParameter("@startDate", clmodel.startDate);
            clmodel.Sqlprms[2] = new SqlParameter("@endDate", clmodel.endDate);
            return dl.SelectJson("GetPDFData", ff.GetConnectionWithDefaultPath("AppOnDRMS"), clmodel.Sqlprms);
        }
    }
}
