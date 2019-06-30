using Microsoft.AspNetCore.Mvc;
using MovieDatabase.Services.Contracts;

namespace MovieDatabase.Web.Controllers
{
    public class ArtistsController : Controller
    {
        private readonly IArtistService artistService;

        public ArtistsController(IArtistService artistService)
        {
            this.artistService = artistService;
        }

        public IActionResult All()
        {
            var allArtists = this.artistService.GetAllArtists();

            return View(allArtists);
        }

        public IActionResult Details(string id)
        {
            var artistDetailsViewModel = artistService.GetArtistAndDetailsById(id);

            return View(artistDetailsViewModel);
        }
    }
}