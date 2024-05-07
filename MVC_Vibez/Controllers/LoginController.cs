using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MVC_Vibez.Core;
using MVC_Vibez.Model;
using MVC_Vibez.Models;
using MVC_Vibez.Services;

namespace MVC_Vibez.Controllers;

public class LoginController : Controller
{
    private readonly VibezDbContext _dbContext;
    private readonly EmailService _emailService;
    private readonly ILogger<LoginController> _logger;
    private readonly LoginService _loginService;

    public LoginController(ILogger<LoginController> logger, VibezDbContext dbContext, EmailService emailService,
        LoginService loginService)
    {
        _dbContext = dbContext;
        _logger = logger;
        _emailService = emailService;
        _loginService = loginService;
    }

    public IActionResult Index() => View();
    [HttpGet] public IActionResult Create() => View();

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(User user)
    {
        if (_dbContext.Users.Any(_user => _user.Email.Equals(user.Email)))
        {
            ModelState.AddModelError("alreadyExist", "User already exists!");
            return View(user);
        }

        user.ValidationToken = Guid.NewGuid().ToString();
        _loginService.Create(user);

        var validationLink = $"https://localhost:7286/Login/Validate?token={user.ValidationToken}";
        var emailBody = $"Welcome to Vibez! </br> Please click <a href='{validationLink}'>here</a> to validate your account and login. </br> If you have any questions or issues please contact : vibezteamhelp@gmail.com </br> Have fun and vibe on!";
        await _emailService.SendEmailAsync(user.Email, "Dear user,", emailBody);

        return RedirectToAction("Index", "Login");
    }

    [HttpPost]
    public async Task<IActionResult> Login(User user)
    {
        var validUser = _dbContext.Users.FirstOrDefault(u => u.Email == user.Email && u.Password == user.Password);

        if (validUser == null)
        {
            ModelState.AddModelError("", "Invalid email or password");
            return View("Index", user);
        }

        if (!validUser.IsValid)
        {
            ModelState.AddModelError("",
                "Please validate your account first by clicking on the link in the email we sent you.");
            return View("Index", user);
        }

        validUser.Loggedin = true;
        await _dbContext.SaveChangesAsync();

        var claims = new[] { new Claim(ClaimTypes.Name, validUser.Email) };
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        return RedirectToAction("Index", "Program");
    }

    public IActionResult Logout() => RedirectToAction("Index", "Login");

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [HttpGet]
    public async Task<IActionResult> Validate(string token)
    {
        try
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.ValidationToken == token);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid validation token");
                return View("Index");
            }

            user.IsValid = true;
            await _dbContext.SaveChangesAsync();

            var claims = new[] { new Claim(ClaimTypes.Name, user.Email) };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            return RedirectToAction("Index", "Program");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during validation");
            return View("Error");
        }
    }
}