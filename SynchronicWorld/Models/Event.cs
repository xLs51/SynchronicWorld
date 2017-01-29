using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SynchronicWorld.Models
{
    public class Event
    {
        [HiddenInput(DisplayValue = false)]
        [Key]
        public int EventId { get; set; }

        [Required]
        public String Name { get; set; }

        [Required]
        public String Address { get; set; }

        [Required]
        public String Description { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int TypeEventId { get; set; }

        [Required]
        public String Status { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int CreatorId { get; set; }
        public virtual ICollection<UserEvent> UserEvents { get; set; }
        public virtual ICollection<Contribution> Contributions { get; set; }
        public virtual TypeEvent TypeEvent { get; set; }
    }

    public class EventContributionModel
    {
        public Event Event { get; set; }
        public List<Contribution> Contributions { get; set; } 
    }
}