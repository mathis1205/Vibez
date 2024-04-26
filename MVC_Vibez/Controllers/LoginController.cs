using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MVC_Vibez.Core;
using MVC_Vibez.Models;

namespace MVC_Vibez.Controllers;

public class LoginController : Controller
{
    private readonly VibezDbContext _dbContext;
    private readonly ILogger<LoginController> _logger;
    private readonly EmailService _emailService;

    public LoginController(ILogger<LoginController> logger, VibezDbContext dbContext, EmailService emailService)
    {
        _dbContext = dbContext;
        _logger = logger;
        _emailService = emailService;
    }
    public async Task<IActionResult> Index()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return View();
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    private IActionResult ViewWithUser(User? user)
    {
        ViewBag.User = _dbContext.Users.ToList();
        return View(user);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(User user)
    {
        if (Enumerable.Any(_dbContext.Users, _user => _user.Email.Equals(user.Email)))
        {
            ModelState.AddModelError("alreadyExist", "User already exist!");
            return ViewWithUser(user);
        }

        _dbContext.Users.Add(user);
        _dbContext.SaveChanges();
        await _emailService.SendEmailAsync(user.Email, "Dear user,","You have succesfully created a Vibez account!");


        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public async Task<IActionResult> Login(User user)
    {
        var validUser = _dbContext.Users.FirstOrDefault(u => u.Email == user.Email);

        if (validUser != null && validUser.Password == user.Password)
        {
            var claims = new[] { new Claim(ClaimTypes.Name, validUser.Email) };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return RedirectToAction("Index", "Program");
        }

        ModelState.AddModelError("", "Invalid email or password");
        return View("Index", user);
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
