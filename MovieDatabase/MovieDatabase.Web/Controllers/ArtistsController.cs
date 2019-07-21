using Microsoft.AspNetCore.Mvc;
using MovieDatabase.Services.Contracts;
using System.Threading.Tasks;

namespace MovieDatabase.Web.Controllers
{
    public class ArtistsController : Controller
    {
        private readonly IArtistService artistService;

        public ArtistsController(IArtistService artistService)
        {
            this.artistService = artistService;
        }        

        public async Task<IActionResult> Details(string id)
        {
            var artistDetailsViewModel = await artistService.GetArtistAndDetailsByIdAsync(id);

            return View(artistDetailsViewModel);
        }

        public async Task<IActionResult> FullBio(string id)
        {
            var artistDetailsViewModel = await artistService.GetArtistFullBioByIdAsync(id);
            
            return View(artistDetailsViewModel);
        }

        public IActionResult All(string orderBy)
        {
            var artistsAllViewModel = artistService.GetAllArtists();

            if (!string.IsNullOrEmpty(orderBy))
            {
                artistsAllViewModel = artistService.OrderArtists(artistsAllViewModel, orderBy);
            }

            return View(artistsAllViewModel);
        }
    }
}