using Microsoft.AspNetCore.Mvc;
using MVC_Vibez.Models;

namespace MVC_Vibez.Controllers
{
    public class LibraryController : Controller
    {
        public async Task<IActionResult> Index()
        {
            var playlists = await SearchHelper.GetRandomPlaylistsAsync(24);
            return View(playlists);
        }
    }
}
