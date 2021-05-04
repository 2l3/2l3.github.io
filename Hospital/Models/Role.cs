using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Hospital.Models
{
    [Table("Security.Roles")]
    public class Role : IdentityRole<int, UserRole>, IRole<int>
    {
        public string TitleAr { set; get; }
        public string TitleEn { set; get; }

    }
}