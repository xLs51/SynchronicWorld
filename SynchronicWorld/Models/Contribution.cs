using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SynchronicWorld.Models
{
    public class Contribution
    {
        [HiddenInput(DisplayValue = false)]
        [Key]
        public int ContributionId { get; set; }

        [Required]
        public String Name { get; set; }

        [Required]
        public String Quantity { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int TypeContributionId { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int UserId { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int EventId { get; set; }

        public virtual TypeContribution TypeContribution { get; set; }
        public virtual User User { get; set; }
        public virtual Event Event { get; set; }
    }
}