namespace MVC_Vibez.Services;

public class ContactService
{
    //create an instance of the emailservice
    private readonly EmailService _emailService;

    //initialize the emailservice
    public ContactService(EmailService emailService) => _emailService = emailService;

    //send an email to the supportemailaddress with the variables of the contact form
    public async void Submit(string email, string message) => await _emailService.SendEmailAsync("vibezteamhelp@gmail.com", "Contact submission", $"{message}<br>{email}");
}