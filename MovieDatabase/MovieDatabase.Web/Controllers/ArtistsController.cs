using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieDatabase.Models.InputModels.Artist;
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

        public IActionResult FullBio(string id)
        {
            var artistDetailsViewModel = artistService.GetArtistFullBioById(id);
            
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

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(CreateArtistInputModel input)
        {
            if (!ModelState.IsValid)
            {
                return View(input);
            }

            if (!artistService.CreateArtist(input))
            {
                return View(input);
            }

            return Redirect("/Artists/All/?orderBy=youngest");
        }
    }
}