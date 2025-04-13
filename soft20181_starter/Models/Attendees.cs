using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace soft20181_starter.Models
{
    public class Attendee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid AttendeeID { get; set; } = Guid.NewGuid();

        [Required]
        public Guid UserID { get; set; }

        [Required]
        public Guid EventID { get; set; }

        [Required]
        public bool TicketGenerated { get; set; } = false;

        [Required]
        public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        [ForeignKey("UserID")]
        public User User { get; set; }

        [ForeignKey("EventID")]
        public Event Event { get; set; }
    }
}
