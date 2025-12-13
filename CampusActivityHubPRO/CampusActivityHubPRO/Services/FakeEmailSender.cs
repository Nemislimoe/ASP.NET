using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace CampusActivityHubPRO.Services
{
    public class FakeEmailSender : IEmailSender
    {
        private readonly IConfiguration _config;
        private readonly ILogger<FakeEmailSender> _logger;

        public FakeEmailSender(IConfiguration config, ILogger<FakeEmailSender> logger)
        {
            _config = config;
            _logger = logger;
        }

        public Task SendEmailAsync(string to, string subject, string body)
        {
            // For demo: log and optionally send via Mailtrap if credentials provided
            _logger.LogInformation("Sending email to {To}: {Subject}", to, subject);
            var host = _config["Email:SmtpHost"];
            var port = int.Parse(_config["Email:SmtpPort"] ?? "25");
            var user = _config["Email:User"];
            var pass = _config["Email:Pass"];
            var from = _config["Email:From"] ?? "no-reply@campushub.local";

            if (!string.IsNullOrEmpty(user) && !string.IsNullOrEmpty(pass))
            {
                try
                {
                    using var client = new SmtpClient(host, port)
                    {
                        Credentials = new NetworkCredential(user, pass),
                        EnableSsl = false
                    };
                    var mail = new MailMessage(from, to, subject, body) { IsBodyHtml = true };
                    client.Send(mail);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to send email via SMTP; logged only.");
                }
            }

            return Task.CompletedTask;
        }
    }
}
