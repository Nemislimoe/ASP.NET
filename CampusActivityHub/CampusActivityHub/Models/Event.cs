namespace CampusActivityHub.Models;
public class Event
{
    public int Id { get; set; }
    public string Title { get; set; }
    public DateTime StartAt { get; set; }
    public int Capacity { get; set; }
    public bool IsDeleted { get; set; }
    public int CategoryId { get; set; }
    public string OrganizerId { get; set; }
    public Category Category { get; set; }
    public ApplicationUser Organizer { get; set; }
    public ICollection<Registration> Registrations { get; set; } = new List<Registration>();
    public string? PosterPath { get; set; }
}
