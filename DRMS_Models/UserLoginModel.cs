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
    }
}
