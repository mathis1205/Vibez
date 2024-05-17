using Microsoft.AspNetCore.Mvc;
using MVC_Vibez.Model;

namespace MVC_Vibez.Controllers
{
    public class GeniusController : Controller
    {
        private readonly GeniusSearch geniusSearch;

        public GeniusController(GeniusSearch geniusSearch)
        {
            this.geniusSearch = geniusSearch;
        }

        public async Task<IActionResult> Authorize(string code)
        {
            var accessToken = await geniusSearch.GetAccessToken(code);
            // Bewaar het toegangstoken en gebruik het om verzoeken te maken namens de gebruiker

            return RedirectToAction("Index");
        }
    }
}
