using System.ComponentModel.DataAnnotations;

namespace CampusActivityHubPRO.Data
{
    public class Category
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        public string? IconClass { get; set; }

        public ICollection<Event>? Events { get; set; }
    }
}
