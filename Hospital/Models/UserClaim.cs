using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Hospital.Models
{
    [Table("Security.UserClaim")]
    public class UserClaim : IdentityUserClaim<int>
    {
        public ApplicationUser User { get; set; }
    }
}