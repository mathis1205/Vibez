using Microsoft.AspNetCore.Mvc;
using MVC_Vibez.Core;
using MVC_Vibez.Model;

namespace MVC_Vibez.Controllers;

public class ProfileController : Controller
{
    private readonly VibezDbContext _dbContext;

    public ProfileController(VibezDbContext dbContext) => _dbContext = dbContext;

    [HttpGet]
    public IActionResult Index()
    {
        var user = _dbContext.Users.FirstOrDefault(u => u.Email == User.Identity!.Name);
        if (user == null) return NotFound();
        return View(user);
    }

    [HttpPost]
    public async Task<IActionResult> UploadProfilePicture(IFormFile? profilePicture)
    {
        var user = _dbContext.Users.FirstOrDefault(u => u.Email == User.Identity!.Name);
        if (user == null) return RedirectToAction("Index");

        if (profilePicture != null)
        {
            var fileName = Path.GetFileName(profilePicture.FileName);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

            await using (var stream = new FileStream(filePath, FileMode.Create)) await profilePicture.CopyToAsync(stream);

            user.ProfilePicture = "/images/" + fileName;
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        user.ProfilePicture = "images/defaultuser.jpg";
        _dbContext.Users.Update(user);
        await _dbContext.SaveChangesAsync();
        ModelState.AddModelError("profilePicture", "Please select an image to upload.");
        return View("Index",user);
    }
}