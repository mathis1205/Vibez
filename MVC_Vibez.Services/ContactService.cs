using MVC_Vibez.Models;

namespace MVC_Vibez.Services;

public class ContactService
{
    //create an instance of the emailservice
    private readonly EmailService _emailService;

    //initialize the emailservice
    public ContactService(EmailService emailService)
    {
        _emailService = emailService;
    }

    public async void submit(string email, string message)
    {
        //send an email to the supportemailaddress with the variables of the contact form
        await _emailService.SendEmailAsync("vibezteamhelp@gmail.com", "Contact submission", $"{message}<br>{email}");
    }
}