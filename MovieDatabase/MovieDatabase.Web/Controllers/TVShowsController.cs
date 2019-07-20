using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieDatabase.Common;
using MovieDatabase.Models.InputModels.TVShow;
using MovieDatabase.Services.Contracts;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MovieDatabase.Web.Controllers
{
    public class TVShowsController : Controller
    {
        private const string redirectTVShowsAllAndOrder = "/TVShows/All?orderBy=" + GlobalConstants.moviesTvShowsOrderByRelease;

        private readonly ITVShowService tvShowService;

        public TVShowsController(ITVShowService tvShowService)
        {
            this.tvShowService = tvShowService;
        }

        public IActionResult All(string orderBy, string genreFilter)
        {
            string userId = "";
            if (User.Identity.IsAuthenticated)
            {
                userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            }

            var tvShowsAllViewModel = tvShowService.GetAllTVShows(userId);

            if (!string.IsNullOrEmpty(genreFilter))
            {
                tvShowsAllViewModel = tvShowService.FilterTVShowsByGenre(tvShowsAllViewModel, genreFilter);
            }
            if (!string.IsNullOrEmpty(orderBy))
            {
                tvShowsAllViewModel = tvShowService.OrderTVShows(tvShowsAllViewModel, orderBy);
            }

            return View(tvShowsAllViewModel);
        }

        public async Task<IActionResult> Details(string id)
        {
            var userId = "";
            if (User.Identity.IsAuthenticated)
            {
                userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            }

            var tvShowDetailsViewModel = await tvShowService.GetTVShowAndDetailsByIdAsync(id, userId);

            return View(tvShowDetailsViewModel);
        }

        public async Task<IActionResult> SeasonDetails(string id)
        {
            var userId = "";
            if (User.Identity.IsAuthenticated)
            {
                userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            }

            var seasonDetailsViewModel = await tvShowService.GetSeasonAndDetailsByIdAsync(id, userId);

            return View(seasonDetailsViewModel);
        }

        [Authorize(Roles = GlobalConstants.adminRoleName)]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = GlobalConstants.adminRoleName)]
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

        [Authorize(Roles = GlobalConstants.adminRoleName)]
        public IActionResult AddSeason()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = GlobalConstants.adminRoleName)]
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

        [Authorize(Roles = GlobalConstants.adminRoleName)]
        public IActionResult AddRole()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = GlobalConstants.adminRoleName)]
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