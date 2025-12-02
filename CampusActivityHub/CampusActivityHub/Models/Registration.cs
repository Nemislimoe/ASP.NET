namespace CampusActivityHub.Models;
public class Registration
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public string UserId { get; set; }
    public DateTime RegistrationDate { get; set; }
    public string? Comment { get; set; }
    public Event Event { get; set; }
    public ApplicationUser User { get; set; }
}
