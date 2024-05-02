using Microsoft.AspNetCore.Mvc;
using MVC_Vibez.Models;
using MVC_Vibez.Services;

namespace MVC_Vibez.Controllers;

public class ProgramController : Controller
{
    public readonly ProgramService _ProgramService;

    public ProgramController(ProgramService ProgramService)
    {
        _ProgramService = ProgramService;
    }

    public IActionResult Index()
    {
        //returns the view of the action
        return View();
    }

    //Create a task to give certain amount of possible options of the search
    [HttpPost]
    public async Task<ActionResult> Autocomplete(string searchText)
    {
        //errorhandeling
        try
        {
            //get the result of possible options with the given text
            var result = await SearchHelper.SearchAll(searchText);
            //check if the result is empty
            if (result == null)
                //return a json file with a failed succes code
                return Json(new { success = false });

            //create a list to save the different possible options to autocomplete
            var artists = result.artists.items.Select(item => new SpotifyArtist
            {
                //fill in all the variables with the right information
                ID = item.id,
                Image = item.images.Any()
                    ? item.images[0].url
                    : "https://user-images.githubusercontent.com/24848110/33519396-7e56363c-d79d-11e7-969b-09782f5ccbab.png",
                Name = item.name
            }).ToList();
            //if everything is succesfull then we send a json file with succes status and the list of options
            return Json(new { success = true, artists });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, error = ex.Message });
        }
    }
}