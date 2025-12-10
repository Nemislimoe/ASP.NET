namespace Lab13.Services
{
    public class EmailService : IEmailService
    {
        public string Send(string to, string subject, string body)
        {
            // Імітація відправки
            return $"Email sent to {to}";
        }
    }
}
