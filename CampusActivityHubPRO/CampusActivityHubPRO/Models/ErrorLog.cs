namespace CampusActivityHubPRO.Data
{
    public class ErrorLog
    {
        public int Id { get; set; }
        public DateTime OccurredAt { get; set; } = DateTime.UtcNow;
        public string? Path { get; set; }
        public string? Method { get; set; }
        public string? UserId { get; set; }
        public string? Exception { get; set; }
    }
}
