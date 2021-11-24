using System.Data.SqlClient;

namespace DRMS_Models
{
    public class BaseModel
    {
        public SqlParameter[] Sqlprms { get; set; }
        public string InsertedBy { get; set; }
        public string InsertedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedDate { get; set; }
    }
}