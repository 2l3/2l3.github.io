using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hospital.Models
{
    public class DeviceSchedule:BaseModel
    {
        public int DeviceId { set; get; }
        public Device Device { set; get; }
        public int DayId { set; get; }
        public Day Day { set; get; }
        public int MonthId { set; get; }
        public Month Month { set; get; }
        public int LastCloseYear { set; get; }
        public int LastPrintPPMYear { set; get; }
    }
}