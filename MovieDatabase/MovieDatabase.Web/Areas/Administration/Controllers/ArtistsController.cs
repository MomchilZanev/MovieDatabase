using Microsoft.AspNetCore.Mvc;
using MovieDatabase.Common;
using MovieDatabase.Models.InputModels.Artist;
using MovieDatabase.Services.Contracts;
using System.Threading.Tasks;

namespace MovieDatabase.Web.Areas.Administration.Controllers
{
    public class ArtistsController : AdministrationController
    {
        private const string redirectArtistsAllAndOrder = "/Artists/All/?orderBy=" + GlobalConstants.artistsOrderByMostPopular;

        private readonly IArtistService artistService;

        public ArtistsController(IArtistService artistService)
        {
            this.artistService = artistService;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
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

            return Redirect(redirectArtistsAllAndOrder);
        }
    }
}