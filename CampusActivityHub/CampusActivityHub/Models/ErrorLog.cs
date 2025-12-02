namespace CampusActivityHub.Models;
public class ErrorLog
{
    public int Id { get; set; }
    public DateTime OccurredAt { get; set; }
    public string Url { get; set; }
    public string Message { get; set; }
    public string? StackTrace { get; set; }
    public string? UserId { get; set; }
}
