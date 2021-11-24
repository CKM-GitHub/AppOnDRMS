using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DRMS_Models;
using CKM_DataLayer;

namespace Base_BL
{
    public class BaseBL
    {
        CKMDL cKMDL;
       
        public BaseBL()
        {
            cKMDL = new CKMDL();
        }
        public string GetConnectionString()
        {
            return "Data Source=163.43.113.92;Initial Catalog=AppOnDRMS;Persist Security Info=True;User ID=sa;Password=admin12345!;Connection Timeout=60;";
        }

    }
}
