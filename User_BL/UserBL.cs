using System.Collections.Generic;
using System.Data.SqlClient;
using CKM_CommonFunction;
using CKM_DataLayer;
using DRMS_Models;


namespace User_BL
{
    public class UserBL
    {
        CKMDL cKMDL;
        FileFunction ff;
        public UserBL()
        {
            cKMDL = new CKMDL();
            ff = new FileFunction();
        }
        public string GetConnectionString()
        {

            return "Data Source=163.43.113.92;Initial Catalog=AppOnDRMS;Persist Security Info=True;User ID=sa;Password=admin12345!;Connection Timeout=60;";
        }
        //public string GetUser(UserLoginModel userModel)
        //{
            
        //    userModel.Sqlprms = new SqlParameter[1];
        //    userModel.Sqlprms[0] = new SqlParameter("@Member_Id", userModel.member_id);
        //    return cKMDL.SelectJson("User_Select", GetConnectionString() , userModel.Sqlprms);
        //}

        public string GetUser(UserLoginModel userModel)
        {
            userModel.Sqlprms = new SqlParameter[1];
            userModel.Sqlprms[0] = new SqlParameter("@Member_Id", userModel.member_id);
            return cKMDL.SelectJson("User_Select", ff.GetConnectionWithDefaultPath("AppOnDRMS"), userModel.Sqlprms);
        }
    }
}
