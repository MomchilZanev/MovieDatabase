using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MovieDatabase.Common;
using MovieDatabase.Models.InputModels.Artist;
using MovieDatabase.Models.ViewModels.Artist;
using MovieDatabase.Services.Contracts;
using System.Threading.Tasks;

namespace MovieDatabase.Web.Areas.Administration.Controllers
{
    public class ArtistsController : AdministrationController
    {
        private const string redirectArtistsAllAndOrder = "/Artists/All/?orderBy=" + GlobalConstants.artistsOrderByMostPopular;
        private const string redirectArtistsDetails = "/Artists/Details/";

        private readonly IArtistService artistService;
        private readonly IMapper mapper;

        public ArtistsController(IArtistService artistService, IMapper mapper)
        {
            this.artistService = artistService;
            this.mapper = mapper;
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

        public async Task<IActionResult> Update(string id)
        {
            var artistDetailsModel = await artistService.GetArtistAndDetailsByIdAsync(id);

            var artistInputModel = mapper.Map<ArtistDetailsViewModel, UpdateArtistInputModel>(artistDetailsModel);

            return View(artistInputModel);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateArtistInputModel input)
        {
            if (!ModelState.IsValid)
            {
                return View(input);
            }

            if (!await artistService.UpdateArtistAsync(input))
            {
                return View(input);
            }

            return Redirect(redirectArtistsDetails + input.Id);
        }
    }
}