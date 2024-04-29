using System;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MVC_Vibez.Core;
using MVC_Vibez.Models;

namespace MVC_Vibez.Controllers
{
    public class LoginController : Controller
    {
        private readonly VibezDbContext _dbContext;
        private readonly ILogger<LoginController> _logger;
        private readonly EmailService _emailService;

        // Constructor to initialize the instances
        public LoginController(ILogger<LoginController> logger, VibezDbContext dbContext, EmailService emailService)
        {
            _dbContext = dbContext;
            _logger = logger;
            _emailService = emailService;
        }

        // Action to display the login page
        public async Task<IActionResult> Index()
        {
            // Sign out any existing user
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            // Return the login view
            return View();
        }

        // Action to display the user creation form
        [HttpGet]
        public IActionResult Create()
        {
            // Return the create view
            return View();
        }

        // Action to handle user creation form submission
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(User user)
        {
            // Check if user already exists
            if (Enumerable.Any(_dbContext.Users, _user => _user.Email.Equals(user.Email)))
            {
                // If user already exists, add error and return the create view
                ModelState.AddModelError("alreadyExist", "User already exists!");
                return ViewWithUser(user);
            }

            // Add user to the database
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            // Send confirmation email
            await _emailService.SendEmailAsync(user.Email, "Dear user,", "You have successfully created a Vibez account!");

            // Redirect to home page after successful creation
            return RedirectToAction("Index", "Home");
        }

        // Action to handle user login
        [HttpPost]
        public async Task<IActionResult> Login(User user)
        {
            // Find user in the database
            var validUser = _dbContext.Users.FirstOrDefault(u => u.Email == user.Email);

            // Check if user exists and password is correct
            if (validUser != null && validUser.Password == user.Password)
            {
                // Mark user as logged in
                validUser.loggedin = true;
                _dbContext.SaveChanges();

                // Create claims for authentication
                var claims = new[] { new Claim(ClaimTypes.Name, validUser.Email) };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                // Sign in user and redirect to program index page
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                return RedirectToAction("Index", "Program");
            }

            // If user not found or password incorrect, add error and return login view
            ModelState.AddModelError("", "Invalid email or password");
            return View("Index", user);
        }

        // Action to handle user logout
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            // Sign out user and redirect to home page
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        // Action to handle errors
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            // Return error view with request ID
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // Helper method to pass users to the view
        private IActionResult ViewWithUser(User? user)
        {
            ViewBag.User = _dbContext.Users.ToList();
            return View(user);
        }
    }
}
