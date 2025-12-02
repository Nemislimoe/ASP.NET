using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace CampusActivityHub.Services;
public class SmtpEmailSender : IEmailSender
{
    private readonly IConfiguration _config;
    public SmtpEmailSender(IConfiguration config) => _config = config;

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        var host = _config["Smtp:Host"];
        var port = int.Parse(_config["Smtp:Port"] ?? "587");
        var user = _config["Smtp:User"];
        var pass = _config["Smtp:Pass"];
        var from = _config["Smtp:From"] ?? user;

        using var client = new SmtpClient(host, port)
        {
            Credentials = new NetworkCredential(user, pass),
            EnableSsl = true
        };
        var msg = new MailMessage(from, to, subject, body) { IsBodyHtml = false };
        await client.SendMailAsync(msg);
    }
}
