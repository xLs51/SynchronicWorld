using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SynchronicWorld.Models
{
    public class TypeEvent
    {
        [HiddenInput(DisplayValue = false)]
        [Key]
        public int TypeEventId { get; set; }

        [Required]
        [Display(Name = "Type")]
        public String Name { get; set; }

        public virtual ICollection<Event> Events { get; set; }
    }
}