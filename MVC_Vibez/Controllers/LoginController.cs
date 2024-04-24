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

    public LoginController(ILogger<LoginController> logger, VibezDbContext dbContext)
    {
        _dbContext = dbContext;
        _logger = logger;
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

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(User user)
    {
        _dbContext.Users.Add(user);
        _dbContext.SaveChanges();

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
