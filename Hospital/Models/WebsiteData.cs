using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hospital.Models
{
    public class WebsiteData:BaseModel
    {
        public string ImgPath { set; get; }
        public string PPMStickerText { set; get; }
        public string Password { set; get; }

    }
}