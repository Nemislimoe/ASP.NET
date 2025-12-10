namespace Lab13.Services
{
    public interface IEmailService
    {
        string Send(string to, string subject, string body);
    }
}
