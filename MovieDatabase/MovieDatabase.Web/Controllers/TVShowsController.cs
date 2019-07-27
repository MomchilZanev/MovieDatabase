using Microsoft.AspNetCore.Mvc;
using MovieDatabase.Services.Contracts;
using System.Threading.Tasks;

namespace MovieDatabase.Web.Controllers
{
    public class TVShowsController : Controller
    {
        private readonly ITVShowService tvShowService;
        private readonly IUserService userService;

        public TVShowsController(ITVShowService tvShowService, IUserService userService)
        {
            this.tvShowService = tvShowService;
            this.userService = userService;
        }

        public async Task<IActionResult> All(string orderBy, string genreFilter)
        {
            string userId = null;
            if (User.Identity.IsAuthenticated)
            {
                userId = await userService.GetUserIdFromUserNameAsync(User.Identity.Name);
            }

            var tvShowsAllViewModel = await tvShowService.GetAllTVShowsAsync(userId);

            if (!string.IsNullOrEmpty(genreFilter))
            {
                tvShowsAllViewModel = await tvShowService.FilterTVShowsByGenreAsync(tvShowsAllViewModel, genreFilter);
            }
            if (!string.IsNullOrEmpty(orderBy))
            {
                tvShowsAllViewModel = tvShowService.OrderTVShows(tvShowsAllViewModel, orderBy);
            }

            return View(tvShowsAllViewModel);
        }

        public async Task<IActionResult> Details(string id)
        {
            var tvShowDetailsViewModel = await tvShowService.GetTVShowAndDetailsByIdAsync(id);

            return View(tvShowDetailsViewModel);
        }

        public async Task<IActionResult> SeasonDetails(string id)
        {
            string userId = null;
            if (User.Identity.IsAuthenticated)
            {
                userId = await userService.GetUserIdFromUserNameAsync(User.Identity.Name);
            }

            var seasonDetailsViewModel = await tvShowService.GetSeasonAndDetailsByIdAsync(id, userId);

            return View(seasonDetailsViewModel);
        }
    }
}