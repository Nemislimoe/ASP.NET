using System.ComponentModel.DataAnnotations;

namespace CampusActivityHubPRO.Data
{
    public class Registration
    {
        public int Id { get; set; }

        public int EventId { get; set; }
        public Event? Event { get; set; } 

        public string UserId { get; set; } = string.Empty;
        public ApplicationUser? User { get; set; }

        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;

        [MaxLength(500)]
        public string? Comment { get; set; }

        public bool Attended { get; set; } = false;
        public string? Feedback { get; set; }
    }
}
