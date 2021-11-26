using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CKM_CommonFunction;
using CKM_DataLayer;

namespace ConstructionList_BL
{
    public class ConstructionListBL
    {
        CKMDL dl = new CKMDL();
        FileFunction ff;
        public string GetPrjData()
        {
            return dl.SelectJson("GetProjectName", ff.GetConnectionWithDefaultPath("AppOnDRMS"));
        }
    }
}
