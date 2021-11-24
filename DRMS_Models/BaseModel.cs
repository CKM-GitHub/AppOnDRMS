using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRMS_Models
{
    public class BaseModel
    {
        public SqlParameter[] Sqlprms { get; set; }
    }
}
