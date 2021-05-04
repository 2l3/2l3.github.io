using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hospital.Models
{
    public class CenterDeviceUnit : BaseModel
    {
        public int CenterId { set; get; }
        public Center Center { set; get; }
        public string NameAr { set; get; }
        public string NameEn { set; get; }
        public string CustodyOfficial { set; get; }
        public string CustodyOPhone { set; get; }
    }
}