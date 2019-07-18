using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieDatabase.Models.InputModels.TVShow;
using MovieDatabase.Services.Contracts;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MovieDatabase.Web.Controllers
{
    public class TVShowsController : Controller
    {
        private readonly ITVShowService tvShowService;

        public TVShowsController(ITVShowService tvShowService)
        {
            this.tvShowService = tvShowService;
        }

        public async Task<IActionResult> All(string orderBy, string genreFilter)
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

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
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

            return Redirect("/TVShows/All?orderBy=release");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddSeason()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
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

            return Redirect("/TVShows/All?orderBy=release");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddRole()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
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

            return Redirect("/TVShows/All?orderBy=release");
        }
    }
}