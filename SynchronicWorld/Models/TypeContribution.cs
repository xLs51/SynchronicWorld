using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SynchronicWorld.Models
{
    public class TypeContribution
    {
        [HiddenInput(DisplayValue = false)]
        [Key]
        public int TypeContributionId { get; set; }

        [Required]
        [Display(Name = "Type")]
        public String Name { get; set; }

        public virtual ICollection<Contribution> Contributions { get; set; }
    }
}