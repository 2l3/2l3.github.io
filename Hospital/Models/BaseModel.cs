using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Hospital.Models
{
    public class BaseModel
    {
        [Key]
        public virtual int Id { get; set; }
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
    }

}