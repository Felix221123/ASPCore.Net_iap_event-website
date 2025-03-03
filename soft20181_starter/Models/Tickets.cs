using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace soft20181_starter.Models
{
    public class Ticket
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TicketID { get; set; }

        [Required]
        public Guid UserID { get; set; }

        [Required]
        public int EventID { get; set; }

        [Required]
        [MaxLength(255)]
        public string TicketCode { get; set; } = Guid.NewGuid().ToString(); // Unique Ticket Code

        [Required]
        public DateTime IssuedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        [ForeignKey("UserID")]
        public User User { get; set; }

        [ForeignKey("EventID")]
        public Event Event { get; set; }
    }
}
