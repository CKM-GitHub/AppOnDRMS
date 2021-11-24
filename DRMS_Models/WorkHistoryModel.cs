using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRMS_Models
{
    public class WorkHistoryModel:BaseModel
    {
        public string member_id { get; set; }
        public string work_date { get; set; }
        public string project_id { get; set; }
        public string Attendance_time { get; set; }
        public string Leave_time { get; set; }

    }
}
