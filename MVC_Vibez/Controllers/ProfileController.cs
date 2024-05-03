using Microsoft.AspNetCore.Identity;
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

    public async Task<IActionResult> UploadProfilePicture(IFormFile profilePicture)
    {
        if (profilePicture != null && profilePicture.Length > 0)
        {
            var fileName = Path.GetFileName(profilePicture.FileName);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await profilePicture.CopyToAsync(stream);
            }

            // Update the user's profile picture URL
            var user = _dbContext.Users.FirstOrDefault(u => u.Email == User.Identity.Name);
            if (user != null)
            {
                user.ProfilePicture = "/images/" + fileName;
                _dbContext.Users.Update(user);
                await _dbContext.SaveChangesAsync();
            }
        }

        return RedirectToAction("Index");
    }
}
