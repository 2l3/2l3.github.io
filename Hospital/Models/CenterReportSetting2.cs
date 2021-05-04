using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hospital.Models
{
    public class CenterReportSetting2 : BaseModel
    {
        public string DescAr { set; get; }
        public string DescEn { set; get; }
        public int Sort { set; get; }
        public int FontSize { set; get; }
        public int Above { set; get; }
    }
}