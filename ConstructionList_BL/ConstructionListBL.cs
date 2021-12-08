using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using CKM_CommonFunction;
using CKM_DataLayer;
using DRMS_Models;
using System.Data;

namespace ConstructionList_BL
{
    public class ConstructionListBL
    {
        CKMDL dl = new CKMDL();
        FileFunction ff = new FileFunction();
        public string GetPrjData(ConstructionListModel clmodel)
        {
            clmodel.Sqlprms = new SqlParameter[1];
            clmodel.Sqlprms[0] = new SqlParameter("@prjCD", clmodel.prjCD);
            return dl.SelectJson("GetProjectName", ff.GetConnectionWithDefaultPath("AppOnDRMS"), clmodel.Sqlprms);
        }

        public DataTable GetPDFData(ConstructionListModel clmodel)
        {
            clmodel.Sqlprms = new SqlParameter[3];
            clmodel.Sqlprms[0] = new SqlParameter("@prjCD", clmodel.prjCD);
            clmodel.Sqlprms[1] = new SqlParameter("@startDate", clmodel.startDate);
            clmodel.Sqlprms[2] = new SqlParameter("@endDate", clmodel.endDate);
            return dl.SelectDatatable("GetProject_PDFData", ff.GetConnectionWithDefaultPath("AppOnDRMS"), clmodel.Sqlprms);
        }
    }
}
