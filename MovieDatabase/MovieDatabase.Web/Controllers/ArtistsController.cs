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

        public IActionResult Details(string id)
        {
            var artistDetailsViewModel = artistService.GetArtistAndDetailsById(id);

            return View(artistDetailsViewModel);
        }

        public IActionResult All(string orderBy)
        {
            var allArtists = artistService.GetAllArtistsAndOrder(orderBy);

            return View(allArtists);
        }
    }
}