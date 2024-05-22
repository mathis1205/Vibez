using Microsoft.AspNetCore.Mvc;
using MVC_Vibez.Model;
using MVC_Vibez.Services;

namespace MVC_Vibez.Controllers;

public class GeniusController : Controller
{
    private readonly GeniusSearch _geniusSearch;
    private readonly ProgramService _programService;

    public GeniusController(GeniusSearch geniusSearch, ProgramService programService)
    {
        _geniusSearch = geniusSearch;
        _programService = programService;
    }

    public IActionResult Index()
    {
        var user = _programService.GetUserByEmail(User.Identity.Name);
        return View(new ProgramPage { user = user });
    }

    public async Task<IActionResult> Search(string searchTerm)
    {
        var user = _programService.GetUserByEmail(User.Identity.Name);
        var hits = await _geniusSearch.SearchSongs(user.AccessToken, searchTerm);
        return View(new ProgramPage { user = user, Hits = hits });
    }

    public async Task<IActionResult> Authorize(string code)
    {
        // Haal het access token op
        var accessToken = await _geniusSearch.GetAccessToken(code);

        // Haal de huidige gebruiker op
        var user = _programService.GetUserByEmail(User.Identity.Name);

        // Controleer of de gebruiker bestaat
        if (user == null)
            // Behandel de fout, bijvoorbeeld door een foutmelding te tonen
            return View("Error");

        // Wijs het access token toe aan de gebruiker
        user.AccessToken = accessToken;

        // Update de gebruiker in de database
        _programService.UpdateUser(user);

        // Ga verder met de rest van de actie
        return RedirectToAction("Index");
    }
}