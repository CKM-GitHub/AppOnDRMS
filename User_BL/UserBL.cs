using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using CKM_CommonFunction;
using CKM_DataLayer;
using DRMS_Models;
using System.Globalization;

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
        
        public string GetUser(UserLoginModel userModel)
        {
            userModel.Sqlprms = new SqlParameter[1];
            userModel.Sqlprms[0] = new SqlParameter("@Member_Id", userModel.member_id);
            return cKMDL.SelectJson("User_Select", ff.GetConnectionWithDefaultPath("AppOnDRMS"), userModel.Sqlprms);
        }
        public CompanyModel GetCompanyName()
        {
            DataTable dt_companyName = cKMDL.SelectDatatable("Select_CompanyName", ff.GetConnectionWithDefaultPath("AppOnDRMS"));
            CompanyModel com_Model = new CompanyModel();
            com_Model.company_id = Convert.ToInt32(dt_companyName.Rows[0]["company_id"].ToString());
            com_Model.company_name = dt_companyName.Rows[0]["company_name"].ToString();
            return com_Model;
        }
        public string GetStaff()
        {
            return cKMDL.SelectJson("Select_Staff", ff.GetConnectionWithDefaultPath("AppOnDRMS"));
        }
        public UserLoginModel GetUserLoginModel()
        {
            CompanyModel com_Model = GetCompanyName();
            UserLoginModel login_Model = new UserLoginModel();
            login_Model.company_name = com_Model.company_name;
            login_Model.member_id = string.Empty;
            return login_Model;
        }

        public string GetPDF(string name)
        {

            var date = DateTime.Now.ToString("yyyyMMdd");
            Random r = new Random();
            int num = r.Next(1000000000,1999999999);
            Random ra = new Random();
            int num1 = ra.Next(10, 99);
            string fileName = name +"_" + date + "_" + num + num1 + ".pdf";

            return (fileName);
        }

        public string GetTextDateJapan(DateTime date)
        {
            string result = string.Empty;
            CultureInfo Japanese = new CultureInfo("ja-JP");
            Japanese.DateTimeFormat.Calendar = new JapaneseCalendar();
            result = date.ToString("ggy年M月d日", Japanese);

            return result;
        }
        public DataTable GetStaff_PDFData(DateDifferenceModel model)
        {
            
            SqlParameter[] sqlparams = new SqlParameter[3];
            sqlparams[0] = new SqlParameter("@memberID", model.Radio_Value);
            sqlparams[1] = new SqlParameter("@StartDate", model.From_Date.ToString("yyyy-MM-dd"));
            sqlparams[2] = new SqlParameter("@EndDate", model.To_Date.ToString("yyyy-MM-dd"));
            return cKMDL.SelectDatatable("GetStaff_PDFDate", ff.GetConnectionWithDefaultPath("AppOnDRMS"), sqlparams);
        }
    }
}

