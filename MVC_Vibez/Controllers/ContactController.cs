using Microsoft.AspNetCore.Mvc;
using MVC_Vibez.Model;
using MVC_Vibez.Models;
using MVC_Vibez.Services;

namespace MVC_Vibez.Controllers;

public class ContactController : Controller
{
    private readonly ContactService _contactService;
    private readonly ProgramService _ProgramService;

    public ContactController(ContactService contact, ProgramService programService)
    { 
        _contactService = contact;
        _ProgramService = programService;
    }

    public IActionResult Index()
    {
        var user = _ProgramService.GetUserByEmail(User.Identity.Name);
        var contactForm = new ContactFormSubmission();

        if (user == null) return NotFound();

        return View(new ProgramPage { user = user, contactForm = contactForm});
    }

    [HttpPost]
    public async Task<IActionResult> SubmitContactForm(ContactFormSubmission submission)
    {
        // Check if the model is valid
        if (!ModelState.IsValid) return View("Index");
        _contactService.Submit(submission.Message, submission.Email);
        // Redirect to a confirmation view
        return RedirectToAction("Confirmation");
    }

    public ActionResult Confirmation() => View();
}