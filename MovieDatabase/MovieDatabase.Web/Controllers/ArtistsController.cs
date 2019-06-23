using Microsoft.AspNetCore.Mvc;
using MovieDatabase.Services.Contracts;
using MovieDatabase.Web.ViewModels.Artist;
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
            var allArtists = this.artistService.GetAllArtists()
                .Select(a => new ArtistAllViewModel
                {
                    FullName = a.FullName,
                    PhotoLink = a.PhotoLink
                })
                .ToList();

            return View(allArtists);
        }
    }
}