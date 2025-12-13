using System.ComponentModel.DataAnnotations;

namespace CampusActivityHubPRO.Data
{
    public class Event
    {
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public DateTime StartAt { get; set; }

        public DateTime EndAt { get; set; }

        public int Capacity { get; set; }

        public bool IsDeleted { get; set; } = false;

        public string? PosterPath { get; set; }

        // FK
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public string OrganizerId { get; set; } = string.Empty;
        public ApplicationUser? Organizer { get; set; }

        // Навігації робимо nullable
        public ICollection<Registration>? Registrations { get; set; }
        public ICollection<EventTag>? EventTags { get; set; }
    }
}
