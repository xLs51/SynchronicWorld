using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SynchronicWorld.Models
{
    public class UserEvent
    {
        [HiddenInput(DisplayValue = false)]
        [Key]
        public int UserEventId { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int EventId { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int UserId { get; set; }

        public virtual User User { get; set; }
        public virtual Event Event { get; set; }
    }
}