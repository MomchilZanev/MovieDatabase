using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieDatabase.Models.InputModels.Artist;
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

        public async Task<IActionResult> All(string orderBy)
        {
            var artistsAllViewModel = artistService.GetAllArtists();

            if (!string.IsNullOrEmpty(orderBy))
            {
                artistsAllViewModel = artistService.OrderArtists(artistsAllViewModel, orderBy);
            }

            return View(artistsAllViewModel);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateArtistInputModel input)
        {
            if (!ModelState.IsValid)
            {
                return View(input);
            }

            if (!await artistService.CreateArtistAsync(input))
            {
                return View(input);
            }

            return Redirect("/Artists/All/?orderBy=youngest");
        }
    }
}