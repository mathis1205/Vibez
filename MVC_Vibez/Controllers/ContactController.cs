using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using MVC_Vibez.Models;
using System.Diagnostics;

namespace MVC_Vibez.Controllers
{
	public class ContactController : Controller
	{
        //create an instance of the emailservice
        private readonly EmailService _emailService;
        //initialize the emailservice
        public ContactController(EmailService emailService)
        {
            _emailService = emailService;
        }

        public IActionResult Index()
		{
            //returns the view of the action
            return View();
		}
        [HttpPost]
        public async Task<IActionResult> SubmitContactForm(ContactFormSubmission submission)
        {
            
            // Check if the model is valid
            if (ModelState.IsValid)
            {
                //send an email to the supportemailaddress with the variables of the contact form
                await _emailService.SendEmailAsync("vibezteamhelp@gmail.com","Contact submission" , $"{submission.Message}<br>{submission.Email}");

                // Redirect to a confirmation view
                return RedirectToAction("Confirmation");
            }
            else
            {
                // Model is not valid, return to the form view with validation errors
                return View("Index");
            }
        }
        public ActionResult Confirmation()
        {
            //returns the view of the action
            return View();
        }

    }
}
