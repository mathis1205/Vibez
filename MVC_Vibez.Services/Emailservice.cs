using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace MVC_Vibez.Services;

public class EmailService
{
    //Create readonly variable to save the settings
    private readonly IOptions<EmailSettings> _emailSettings;

    //Constructor to initialize the emailsettings 
    public EmailService(IOptions<EmailSettings> emailSettings)
    {
        _emailSettings = emailSettings;
    }

    //Create a task that is used for sending an email to a specific emailadress , a subject and a message
    public async Task SendEmailAsync(string email, string subject, string message)
    {
        //create local variable of the message
        var emailMessage = new MimeMessage();

        //fill in the different variables in the local variable 
        emailMessage.From.Add(new MailboxAddress(_emailSettings.Value.SenderName, _emailSettings.Value.SenderEmail));
        emailMessage.To.Add(new MailboxAddress("", email));
        emailMessage.Subject = subject;
        emailMessage.Body = new TextPart(TextFormat.Html) { Text = message };

        //Initialize using the smtp server to send the email
        using var client = new SmtpClient();
        await client.ConnectAsync(_emailSettings.Value.MailServer, _emailSettings.Value.MailPort,
            SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(_emailSettings.Value.SenderEmail, _emailSettings.Value.Password);
        await client.SendAsync(emailMessage);

        await client.DisconnectAsync(true);
    }

    //create a new class with the right variables
    public class EmailSettings
    {
        public string MailServer { get; set; }
        public int MailPort { get; set; }
        public string SenderName { get; set; }
        public string SenderEmail { get; set; }
        public string Password { get; set; }
    }
}