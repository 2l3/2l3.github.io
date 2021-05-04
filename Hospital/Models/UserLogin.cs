using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Hospital.Models
{
    [Table("Security.UserLogin")]
    public class UserLogin : IdentityUserLogin<int>
    {
        public int Id { set; get; }
        public ApplicationUser User { get; set; }
    }
}