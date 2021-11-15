using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DRMS_Models
{
    public class BaseModel
    {
        public string InsertedBy { get; set; }
        public string InsertedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedDate { get; set; }
    }
}