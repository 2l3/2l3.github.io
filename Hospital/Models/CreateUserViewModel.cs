using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Hospital.Models
{
    public class CreateUserViewModel
    {
        [Required(ErrorMessage = "*")]
        public string Name { set; get; }
        public int Id { set; get; }
        [Required(ErrorMessage = "*")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "*")]
        public string Password { get; set; }
        [Required(ErrorMessage = "*")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "*")]
        [Phone]
        public string PhoneNumber { get; set; }
        public int? CityId { set; get; }
        public int? EngineerId { set; get; }
    }
}