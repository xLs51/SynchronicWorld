using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SynchronicWorld.Models
{
    public class Role
    {
        [HiddenInput(DisplayValue = false)]
        [Key]
        public int RoleId { get; set; }

        [Required]
        [Display(Name = "Role")]
        public String Name { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}