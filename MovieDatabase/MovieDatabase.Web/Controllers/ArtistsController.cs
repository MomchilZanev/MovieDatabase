using Microsoft.AspNetCore.Mvc;
using MovieDatabase.Models.ViewModels.Artist;
using MovieDatabase.Services.Contracts;
using System.Linq;

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
    }
}