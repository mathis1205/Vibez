using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace MVC_Vibez.Services;

public class EmailService
{
    private readonly IOptions<EmailSettings> _emailSettings;

    public EmailService(IOptions<EmailSettings> emailSettings) => _emailSettings = emailSettings;

    public async Task SendEmailAsync(string email, string subject, string message)
    {
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress(_emailSettings.Value.SenderName, _emailSettings.Value.SenderEmail));
        emailMessage.To.Add(new MailboxAddress("", email));
        emailMessage.Subject = subject;
        emailMessage.Body = new TextPart(TextFormat.Html) { Text = message };

        using var client = new SmtpClient();
        await client.ConnectAsync(_emailSettings.Value.MailServer, _emailSettings.Value.MailPort, SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(_emailSettings.Value.SenderEmail, _emailSettings.Value.Password);
        await client.SendAsync(emailMessage);
        await client.DisconnectAsync(true);
    }

    public class EmailSettings
    {
        public string MailServer { get; set; }
        public int MailPort { get; set; }
        public string SenderName { get; set; }
        public string SenderEmail { get; set; }
        public string Password { get; set; }
    }
}