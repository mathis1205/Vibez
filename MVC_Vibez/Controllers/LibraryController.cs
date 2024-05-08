using Microsoft.AspNetCore.Mvc;
using MVC_Vibez.Model;
using MVC_Vibez.Services;

namespace MVC_Vibez.Controllers;

public class LibraryController : Controller
{
    private readonly ProgramService _ProgramService;
    public LibraryController(ProgramService programService) => _ProgramService = programService;
    public IActionResult Index() => View(_ProgramService.GetUserByEmail(User.Identity.Name));

    public IActionResult AddToFavorites(Spotify item)
    {
        var user = _ProgramService.GetUserByEmail(User.Identity.Name);

        if (user != null && item != null)
        {
            user.FavoriteSpotifyItems.Add(item);
            _ProgramService.UpdateUser(user);
        }

        return RedirectToAction("Index");
    }

    public IActionResult RemoveFromFavorites(string itemId)
    {
        var user = _ProgramService.GetUserByEmail(User.Identity.Name);
        var item = user?.FavoriteSpotifyItems.FirstOrDefault(i => i.ID == itemId);

        if (user != null && item != null)
        {
            user.FavoriteSpotifyItems.Remove(item);
            _ProgramService.UpdateUser(user);
        }

        return RedirectToAction("Index");
    }
}