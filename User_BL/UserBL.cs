using CKM_CommonFunction;
using CKM_DataLayer;
using System.Data.SqlClient;
using DRMS_Models;
using Base_BL;

namespace User_BL
{
    public class UserBL:BaseBL
    {
        CKMDL cKMDL;
        FileFunction ff;
        public UserBL()
        {
            cKMDL = new CKMDL();
            ff = new FileFunction();
        }
        public string GetUser(UserModel userModel)
        {
            userModel.Sqlprms = new SqlParameter[1];
            userModel.Sqlprms[0] = new SqlParameter("@Password", userModel.Password);
            return cKMDL.SelectJson("User_Select", ff.GetConnectionWithDefaultPath("AppOnDRMS"), userModel.Sqlprms);
        }
    }
}
