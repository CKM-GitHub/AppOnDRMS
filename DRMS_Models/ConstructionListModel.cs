using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRMS_Models
{
    public class ConstructionListModel: BaseModel
    {
        public string prjCD { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
    }
}
