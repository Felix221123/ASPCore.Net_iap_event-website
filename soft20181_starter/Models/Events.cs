using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace soft20181_starter.Models
{
    public class Event
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EventID { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        [MaxLength(100)]
        public string Type { get; set; } = string.Empty;

        [Required]
        public int Day { get; set; }

        [Required]
        [MaxLength(20)]
        public string Month { get; set; } = string.Empty;

        [Required]
        public int Year { get; set; }

        [Required]
        [MaxLength(50)]
        public string Time { get; set; } = string.Empty;

        [MaxLength(255)]
        public string VenueName { get; set; } = string.Empty;

        [MaxLength(500)]
        public string VenueAddress { get; set; } = string.Empty;

        [MaxLength(255)]
        public string OrganizerName { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? OrganizerContact { get; set; }

        [MaxLength(500)]
        public string? FollowLink { get; set; }

        [MaxLength(50)]
        public string TicketPrice { get; set; } = string.Empty;

        [MaxLength(10)]
        public string Currency { get; set; } = string.Empty;

        [MaxLength(500)]
        public string EventLink { get; set; } = string.Empty;

        public string? Images { get; set; } 

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
