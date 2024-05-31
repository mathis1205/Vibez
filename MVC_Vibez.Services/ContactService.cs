namespace MVC_Vibez.Services;

public class ContactService
{
    private readonly EmailService _emailService;
    public ContactService(EmailService emailService) => _emailService = emailService;
    public async Task Submit(string email, string message) => await _emailService.SendEmailAsync(email, "Contact Form Submission", message);
}