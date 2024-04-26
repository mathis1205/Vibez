using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using MVC_Vibez.Models;
using System.Diagnostics;

namespace MVC_Vibez.Controllers
{
	public class ContactController : Controller
	{
        private readonly EmailService _emailService;
        public ContactController(EmailService emailService)
        {
            _emailService = emailService;
        }

        public IActionResult Index()
		{
			return View();
		}
        [HttpPost]
        public async Task<IActionResult> SubmitContactForm(ContactFormSubmission submission)
        {
            
            // Check if the model is valid
            if (ModelState.IsValid)
            {
                // Save the form submission to a local file
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
            return View();
        }

    }
}
