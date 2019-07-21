using Microsoft.AspNetCore.Mvc;
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
    }
}