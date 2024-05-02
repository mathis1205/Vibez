using Microsoft.AspNetCore.Mvc;
using MVC_Vibez.Core;
using MVC_Vibez.Services;

namespace MVC_Vibez.Controllers;

public class ProfileController : Controller
{
    private readonly VibezDbContext _dbContext;

    public ProfileController(VibezDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var userEmail = User.Identity.Name;
        var user = _dbContext.Users.FirstOrDefault(u => u.Email == userEmail);
        if (user == null)
        {
            return NotFound();
        }
        return View(user);
    }
}
