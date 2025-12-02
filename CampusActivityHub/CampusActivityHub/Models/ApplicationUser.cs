namespace CampusActivityHub.Models;
public class ApplicationUser
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; }
    public string Email { get; set; }
    public string Role { get; set; } // Admin, Organizer, Student
    public ICollection<Event> OrganizedEvents { get; set; } = new List<Event>();
    public ICollection<Registration> Registrations { get; set; } = new List<Registration>();
}
