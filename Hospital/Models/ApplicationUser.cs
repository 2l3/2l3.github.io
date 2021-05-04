using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Hospital.Models
{
    [Table("Security.User")]
    public class ApplicationUser : IdentityUser<int, UserLogin, UserRole, UserClaim>
    {
        [Required(ErrorMessage = "*")]
        public string Name { set; get; }
        public int? EngineerId { set; get; }
        public Engineer Engineer { set; get; }
        #region BaseModel
        public Nullable<bool> IsActive { get; set; }

        public Nullable<int> CreatedBy { get; set; }

        public Nullable<System.DateTime> CreatedDate { get; set; }

        public Nullable<int> ModifiedBy { get; set; }

        public Nullable<System.DateTime> ModifiedDate { get; set; }

        public Nullable<bool> IsDeleted { get; set; }

        public Nullable<int> DeletedBy { get; set; }

        public Nullable<System.DateTime> DeletedDate { get; set; }
        public string CreatorName { set; get; }
        public string DeleterName { set; get; }
        public string ModifierName { set; get; }
        #endregion
    }
}