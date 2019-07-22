using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MovieDatabase.Common;
using MovieDatabase.Models.InputModels.TVShow;
using MovieDatabase.Models.ViewModels.TVShow;
using MovieDatabase.Services.Contracts;
using System.Threading.Tasks;

namespace MovieDatabase.Web.Areas.Administration.Controllers
{
    public class TVShowsController : AdministrationController
    {
        private const string redirectTVShowsAllAndOrder = "/TVShows/All?orderBy=" + GlobalConstants.moviesTvShowsOrderByRelease;
        private const string redirectTVShowDetails = "/TVShows/Details/";

        private readonly ITVShowService tvShowService;
        private readonly IMapper mapper;

        public TVShowsController(ITVShowService tvShowService, IMapper mapper)
        {
            this.tvShowService = tvShowService;
            this.mapper = mapper;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTVShowInputModel input)
        {
            if (!ModelState.IsValid)
            {
                return View(input);
            }

            if (!await tvShowService.CreateTVShowAsync(input))
            {
                return View(input);
            }

            return Redirect(redirectTVShowsAllAndOrder);
        }

        public IActionResult AddSeason()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddSeason(AddSeasonInputModel input)
        {
            if (!ModelState.IsValid)
            {
                return View(input);
            }

            if (!await tvShowService.AddSeasonToTVShowAsync(input))
            {
                return View(input);
            }

            return Redirect(redirectTVShowsAllAndOrder);
        }

        public IActionResult AddRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddRole(AddSeasonRoleInputModel input)
        {
            if (!ModelState.IsValid)
            {
                return View(input);
            }

            if (!await tvShowService.AddRoleToTVShowSeasonAsync(input))
            {
                return View(input);
            }

            return Redirect(redirectTVShowsAllAndOrder);
        }

        public async Task<IActionResult> Update(string id)
        {
            var tvShowDetailsModel = await tvShowService.GetTVShowAndDetailsByIdAsync(id);

            var tvShowInputModel = mapper.Map<TVShowDetailsViewModel, UpdateTVShowInputModel>(tvShowDetailsModel);

            return View(tvShowInputModel);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateTVShowInputModel input)
        {
            if (!ModelState.IsValid)
            {
                return View(input);
            }

            if (!await tvShowService.UpdateTVShowAsync(input))
            {
                return View(input);
            }

            return Redirect(redirectTVShowDetails + input.Id);
        }

        public async Task<IActionResult> UpdateSeason(string id)
        {
            var seasonDetailsModel = await tvShowService.GetSeasonAndDetailsByIdAsync(id);

            var seasonInputModel = mapper.Map<SeasonDetailsViewModel, UpdateSeasonInputModel>(seasonDetailsModel);

            return View(seasonInputModel);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateSeason(UpdateSeasonInputModel input)
        {
            if (!ModelState.IsValid)
            {
                return View(input);
            }

            if (!await tvShowService.UpdateSeasonAsync(input))
            {
                return View(input);
            }

            return Redirect(redirectTVShowsAllAndOrder);
        }
    }
}