using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hospital.Models
{
    public class CenterDeviceRequest : BaseModel
    {
        public string Reason { set; get; }
        public CenterDevice Device { set; get; }
        public int? DeviceId { set; get; }
        public int RequestType { set; get; }
        public int? DeviceUnitId { set; get; }
        public DateTime? ConfirmedDate{set ;get;}
        public int ConfirmType { set; get; }
        public string User { set; get; }
        public string ToUnitAr { set; get; }
        public string ToUnitEn { set; get; }
        public string DeleteReason { set; get; }
        public string DeviceName { set; get; }
        public string ComputerNumber { set; get; }
        public bool? WithNoBarCode { set; get; }
        public string Date { get { return CreatedDate.Value.ToShortDateString(); } }
        public string Notes { set; get; }
        public int? EngineerId { set; get; }
        public string CenterNameAr { set; get; }
        public string CenterNameEn { set; get; }
        public DateTime? ReportDate { set; get; }
    }
}