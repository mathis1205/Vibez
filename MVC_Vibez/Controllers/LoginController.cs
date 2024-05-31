using System.Diagnostics;
using System.Security.Claims;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MVC_Vibez.Model;
using MVC_Vibez.Models;
using MVC_Vibez.Services;

namespace MVC_Vibez.Controllers;

public partial class LoginController : Controller
{
    private readonly EmailService _emailService;
    private readonly ILogger<LoginController> _logger;
    private readonly LoginService _loginService;

    public LoginController(ILogger<LoginController> logger, EmailService emailService, LoginService loginService)
    {
        _logger = logger;
        _emailService = emailService;
        _loginService = loginService;
    }

    public IActionResult Index() => View();
    [HttpGet] public IActionResult Create() => View();
    [HttpGet] public IActionResult Recovery() => View();
    [HttpPost] public IActionResult ResetPassword() => View();

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(User user)
    {
        if (_loginService.GetUsers().Any(_user => _user.Email.Equals(user.Email)))
        {
            ModelState.AddModelError("alreadyExist", "User already exists!");
            return View(user);
        }

        if (!MyRegexTekens().IsMatch(user.Password))
        {
            ModelState.AddModelError("special character", "Password must include at least one special character");
            return View(user);
        }

        if (!MyRegexLetters().IsMatch(user.Password))
        {
            ModelState.AddModelError("capitalized letter", "Password must include at least one capitalized letter");
            return View(user);
        }

        if (!MyRegexCijfers().IsMatch(user.Password))
        {
            ModelState.AddModelError("Number", "Password must include at least one Number");
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
        var storedUser = _loginService.GetUsers().FirstOrDefault(u => u.Email == user.Email);

        if (storedUser == null)
        {   
            ModelState.AddModelError("", "Invalid email or password");
            return View("Index", user);
        }

        //var hashedEnteredPassword = HashingHelper.HashPassword(user.Password);
        //if (storedUser.Password != hashedEnteredPassword)
        //{
        //    ModelState.AddModelError("", "Invalid email or password");
        //    return View("Index", user);
        //}

        if (!storedUser.IsValid)
        {
            ModelState.AddModelError("", "Please validate your account first by clicking on the link in the email we sent you.");
            return View("Index", user);
        }

        storedUser.Loggedin = true;
        _loginService.Update(storedUser);

        var claims = new[] { new Claim(ClaimTypes.Name, storedUser.Email) };
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        return RedirectToAction("Index", "Program");
    }

    public IActionResult Logout() => RedirectToAction("Index", "Home");
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)] public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

    [HttpGet]
    public async Task<IActionResult> Validate(string token)
    {
        try
        {
            var user = _loginService.GetUsers().FirstOrDefault(u => u.ValidationToken == token);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid validation token");
                return View("Index");
            }

            user.IsValid = true;
            _loginService.Update(user);

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

    [HttpPost]
    public async Task<IActionResult> Recovery(User user)
    {
        if (!string.IsNullOrEmpty(user.Email)) ViewData["MailSentTo"] = user.Email;

        var existingUser = _loginService.GetUsers().FirstOrDefault(_user => _user.Email.Equals(user.Email));
        if (existingUser == null)
        {
            ModelState.AddModelError("notFound", "Email not found!");
            return View(user);
        }

        try
        {
            existingUser.ValidationToken = Guid.NewGuid().ToString();
            _loginService.Update(existingUser);

            var recoveryLink = Url.Action("ResetPassword", "Login", new { token = existingUser.ValidationToken }, Request.Scheme);
            var emailBody = $"Dear user, </br> Please click <a href='{recoveryLink}'>here</a> to reset your password. </br> If you did not request a password reset, please ignore this email.";
            await _emailService.SendEmailAsync(existingUser.Email, "Password Recovery", emailBody);

            return View(user);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("mailError", "Failed to send email. Please check provided email.");
            return View(user);
        }
    }

    public IActionResult ResetPassword(string token) => View(new ResetPasswordModel { Token = token });

    [HttpPost]
    public IActionResult SetNewPassword(ResetPasswordModel model)
    {
        if (!ModelState.IsValid) return View("Index", model);
        var user = _loginService.GetUsers().FirstOrDefault(u => u.ValidationToken == model.Token);
        if (user != null)
        {
            if (user.Password == HashingHelper.HashPassword(model.NewPassword))
            {
                ModelState.AddModelError("SamePassword", "New password must be different from the old one.");
                return View("ResetPassword", model);
            }

            if (!MyRegexTekens().IsMatch(model.NewPassword))
            {
                ModelState.AddModelError("special character", "Password must include at least one special character");
                return View("ResetPassword", model);
            }

            if (!MyRegexLetters().IsMatch(model.NewPassword))
            {
                ModelState.AddModelError("capitalized letter", "Password must include at least one capitalized letter");
                return View("ResetPassword", model);
            }

            if (!MyRegexCijfers().IsMatch(model.NewPassword))
            {
                ModelState.AddModelError("Number", "Password must include at least one Number");
                return View("ResetPassword", model);
            }

            user.Password = HashingHelper.HashPassword(model.NewPassword);
            _loginService.Update(user);
            return RedirectToAction("Index", "Home");
        }

        ModelState.AddModelError("InvalidToken", "Invalid or expired token.");

        return View("Index", model);
    }

    [GeneratedRegex("[!@#$%^&*()_+\\-=\\[\\]{};':\"\\\\|,.<>\\/?]")] private static partial Regex MyRegexTekens();
    [GeneratedRegex("[A-Z]")] private static partial Regex MyRegexLetters();
    [GeneratedRegex("[0-9]")] private static partial Regex MyRegexCijfers();
}