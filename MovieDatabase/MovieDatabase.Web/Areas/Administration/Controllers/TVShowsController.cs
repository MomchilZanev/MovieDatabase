using Microsoft.AspNetCore.Mvc;
using MovieDatabase.Common;
using MovieDatabase.Models.InputModels.TVShow;
using MovieDatabase.Services.Contracts;
using System.Threading.Tasks;

namespace MovieDatabase.Web.Areas.Administration.Controllers
{
    public class TVShowsController : AdministrationController
    {
        private const string redirectTVShowsAllAndOrder = "/TVShows/All?orderBy=" + GlobalConstants.moviesTvShowsOrderByRelease;

        private readonly ITVShowService tvShowService;

        public TVShowsController(ITVShowService tvShowService)
        {
            this.tvShowService = tvShowService;
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
        public async Task<IActionResult> AddRole(AddRoleInputModel input)
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
    }
}