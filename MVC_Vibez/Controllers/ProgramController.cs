using Microsoft.AspNetCore.Mvc;
using MVC_Vibez.Models;

namespace MVC_Vibez.Controllers
{
    public class ProgramController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost] public async Task<ActionResult> Search(string searchText)
        {
	        try
	        {
		        var result = await SearchHelper.SearchArtistOrSong(searchText);
		        if (result == null) return Json(new { success = false });
                var listArtist = result.artists.items.Select(item => new SpotifyArtist() { ID = item.id, Image = item.images.Any() ? item.images[0].url : "https://user-images.githubusercontent.com/24848110/33519396-7e56363c-d79d-11e7-969b-09782f5ccbab.png", Name = item.name }).ToList(); return Json(new { success = true, artists = listArtist });
	        }
	        catch (Exception ex) { return Json(new { success = false, error = ex.Message }); }
        }
	}
}
