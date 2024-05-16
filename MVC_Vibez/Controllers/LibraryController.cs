using Microsoft.AspNetCore.Mvc;
using MVC_Vibez.Model;
using MVC_Vibez.Services;

namespace MVC_Vibez.Controllers;

public class LibraryController : Controller
{
    private readonly ProgramService _ProgramService;

    public LibraryController(ProgramService ProgramService)
    {
        _ProgramService = ProgramService;
    }

    public async Task<IActionResult> Index()
    {
        var user = _ProgramService.GetUserByEmail(User.Identity.Name);
        var playlists = await SearchHelper.GetRandomPlaylistsAsync(36);
        var favoriteSongs = user.FavoriteSpotifyItems;
        return View(new ProgramPage { user = user, playlists = playlists, favoriteSongs = favoriteSongs });
    }

    public IActionResult AddToFavorites(Spotify item)
    {
        var user = _ProgramService.GetUserByEmail(User.Identity.Name);

        if (user == null || item == null) return RedirectToAction("Index");
        user.FavoriteSpotifyItems.Add(item);
        _ProgramService.UpdateUser(user);

        return RedirectToAction("Index");
    }

    public IActionResult RemoveFromFavorites(string itemId)
    {
        var user = _ProgramService.GetUserByEmail(User.Identity.Name);
        var item = user?.FavoriteSpotifyItems.FirstOrDefault(i => i.ID == itemId);

        if (user == null || item == null) return RedirectToAction("Index");
        user.FavoriteSpotifyItems.Remove(item);
        _ProgramService.UpdateUser(user);

        return RedirectToAction("Index");
    }
}