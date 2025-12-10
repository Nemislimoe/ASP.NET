namespace Lab13.Services
{
    public class FakeEmailService : IEmailService
    {
        public string Send(string to, string subject, string body)
        {
            return "Test email sent";
        }
    }
}
