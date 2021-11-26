using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRMS_Models
{
   public class StaffModel:BaseModel
    {
        public string member_id { get; set; }
        public string member_name { get; set; }
        public string Passward { get; set; }
        public int member_view_num { get; set; }
    }
    public class StaffList
    {
        List<StaffModel> StaffModelList { get; set; }
    }
}
