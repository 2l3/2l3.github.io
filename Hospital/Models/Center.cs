using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hospital.Models
{
    public class Center:BaseModel
    {
        public string NameAr { set; get; }
        public string NameEn { set; get; }
        public string Password { set; get; }
    }
}