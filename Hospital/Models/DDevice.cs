using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hospital.Models
{
    public class DDevice : BaseModel
    {
        public string NameEn { set; get; }
        public string NameAr { set; get; }
        public string Com { set; get; }
        public string Serial { set; get; }
        public string Model { set; get; }
        public string ComputerNumber { set; get; }
        public DeviceUnit DeviceUnit { set; get; }
        public int DeviceUnitId { set; get; }
        public string Desc { set; get; }
        public string ImgPath { set; get; }
        public int? DeviceTypeId { set; get; }
        public DeviceType DeviceType { set; get; }
        public int DeviceCategoryId { set; get; }
        public bool ShowLastModification { set; get; }
        public bool ShowDaman { set; get; }
        public DateTime? DamanExpireDate { set; get; }
        public DateTime? LastModificationDate { set; get; }
        public string CompanyEmail { set; get; }
        public string CompanyPhone { set; get; }
        public string CompanyName { set; get; }
    }
}