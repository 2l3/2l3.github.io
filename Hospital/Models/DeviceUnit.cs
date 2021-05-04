using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Hospital.Models
{
    public class DeviceUnit
    {
        [Key]
        public int Id { set; get; }
        public string NameAr { set; get; }
        public string NameEn { set; get; }
        public string CustodyOfficial { set; get; }
        public string CustodyOPhone { set; get; }
    }
}