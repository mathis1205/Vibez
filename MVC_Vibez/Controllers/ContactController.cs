using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using MVC_Vibez.Models;
using System.Diagnostics;

namespace MVC_Vibez.Controllers
{
	public class ContactController : Controller
	{

		public IActionResult Index()
		{
			return View();
		}
        [HttpPost]
        public ActionResult SubmitContactForm(ContactFormSubmission model)
        {
            // Check if the model is valid
            if (ModelState.IsValid)
            {
                // Save the form submission to a local file
                SaveSubmissionToFile(model);

                // Redirect to a confirmation view
                return RedirectToAction("Confirmation");
            }
            else
            {
                // Model is not valid, return to the form view with validation errors
                return View("Index", model);
            }
        }

        private void SaveSubmissionToFile(ContactFormSubmission submission)
        {
            // Append the submission data to a text file
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = Path.Combine(baseDirectory, "ContactFormSubMission.txt");
            string submissionText = $"Email: {submission.Email}, Message: {submission.Message}, Submitted At: {DateTime.Now}\n";

            System.IO.File.AppendAllText(filePath, submissionText);
        }

        public ActionResult Confirmation()
        {
            return View();
        }

    }
}
