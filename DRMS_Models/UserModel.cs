using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace DRMS_Models
{
    public class UserModel
    {
        public string Password { get; set; }

        public SqlParameter[] Sqlprms { get; set; }
    }
}
