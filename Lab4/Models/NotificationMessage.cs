namespace Lab4.Models
{
    public enum NotificationType { Success, Error, Warning }

    public class NotificationMessage
    {
        public NotificationType Type { get; set; }
        public string Text { get; set; } = "";
    }
}
