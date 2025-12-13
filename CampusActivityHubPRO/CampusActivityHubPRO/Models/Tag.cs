using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CampusActivityHubPRO.Data
{
    public class Tag
    {
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        public ICollection<Event>? EventTags { get; set; }
    }
}
