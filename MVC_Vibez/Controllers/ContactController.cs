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
        var currentUser = _ProgramService.GetUserByEmail(User.Identity.Name);
        var newContactFormSubmission = new ContactFormSubmission();

        if (currentUser == null) return NotFound();

        return View(new ProgramPage { user = currentUser, contactForm = newContactFormSubmission });
    }

    [HttpPost]
    public async Task<IActionResult> SubmitContactForm(ContactFormSubmission contactForm)
    {
        // Check if the model is valid
        if (!ModelState.IsValid) return View("Index");
        _contactService.Submit(contactForm.Message, contactForm.Email);
        // Redirect to a confirmation view
        return RedirectToAction("Confirmation");
    }

    public ActionResult Confirmation() => View();
}