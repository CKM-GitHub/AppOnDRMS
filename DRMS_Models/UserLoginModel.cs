using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace DRMS_Models
{
    public class UserLoginModel:BaseModel
    {
        public string member_id { get; set; }
        
        public int company_id { get; set; }
       
        public string company_name { get; set; }

        public int window_Size { get; set; }
    }
}
