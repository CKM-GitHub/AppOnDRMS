using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRMS_Models
{
    public class CompanyModel:BaseModel
    {
        public int company_id { get; set; }
        public string company_name { get; set; }
        public string project_work_name { get; set; }
        public decimal project_work_time { get; set; }
    }
    public class RootObject
    {
        public Results Results { get; set; }
    }

    public class Results
    {
        public Dictionary<string, CompanyModel> JobCodes { get; set; }
    }
}
