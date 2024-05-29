using Microsoft.AspNetCore.Mvc;
using MVC_Vibez.Model;
using MVC_Vibez.Models;
using MVC_Vibez.Services;

namespace MVC_Vibez.Controllers;

public class ContactController : Controller
{
    private readonly ContactService _contactService;
    private readonly LoginService _LoginService;

    public ContactController(ContactService contact, LoginService programService)
    {
        _contactService = contact;
        _LoginService = programService;
    }

    public IActionResult Index()
    {
        var currentUser = _LoginService.GetUserByEmail(User.Identity.Name);
        if (currentUser == null) return NotFound();
        return View(new ProgramPage { user = currentUser, contactForm = new ContactFormSubmission() });
    }

    [HttpPost]
    public Task<IActionResult> SubmitContactForm(ContactFormSubmission contactForm)
    {
        if (!ModelState.IsValid) return Task.FromResult<IActionResult>(View("Index"));
        _contactService.Submit(contactForm.Message, contactForm.Email);
        TempData["SuccessMessage"] = "Thanks for the contacting us, we will answer as fast as possible.";
        return Task.FromResult<IActionResult>(RedirectToAction("Index"));
    }
}