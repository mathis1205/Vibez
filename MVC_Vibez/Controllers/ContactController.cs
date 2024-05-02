using Microsoft.AspNetCore.Mvc;
using MVC_Vibez.Models;
using MVC_Vibez.Services;

namespace MVC_Vibez.Controllers;

public class ContactController : Controller
{
    private readonly ContactService _contactService;

    public ContactController(ContactService contact)
    {
        _contactService = contact;
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
            _contactService.submit(submission.Message, submission.Email);
            // Redirect to a confirmation view
            return RedirectToAction("Confirmation");
        }

        // Model is not valid, return to the form view with validation errors
        return View("Index");
    }

    public ActionResult Confirmation()
    {
        //returns the view of the action
        return View();
    }
}