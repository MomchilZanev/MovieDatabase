using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieDatabase.Services.Contracts;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MovieDatabase.Web.Controllers
{
    public class WatchlistController : Controller
    {
        private readonly IWatchlistService watchlistService;

        public WatchlistController(IWatchlistService watchlistService)
        {
            this.watchlistService = watchlistService;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var watchlistAllViewModel = watchlistService.GetItemsInUserWatchlist(userId);

            return View(watchlistAllViewModel);
        }

        [Authorize]
        public async Task<IActionResult> Add(string id, string returnAction, string returnQuery)
        {
            var idIsValidMovieOrTVShowId = watchlistService.IsValidMovieOrTVShowId(id);
            if (!idIsValidMovieOrTVShowId)
            {
                return Redirect("/Home/Error");
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var itemType = watchlistService.IsIdMovieOrTVShowId(id);
            if (itemType == "Movie")
            {
                if (watchlistService.MovieIsInUserWatchlist(userId, id))
                {
                    return Redirect("/Home/Error");
                }
                await watchlistService.AddMovieToUserWatchlistAsync(userId, id);
                return Redirect(returnAction + returnQuery);
            }
            else if (itemType == "TV Show")
            {
                if (watchlistService.TVShowIsInUserWatchlist(userId, id))
                {
                    return Redirect("/Home/Error");
                }
                await watchlistService.AddTVShowToUserWatchlistAsync(userId, id);
                return Redirect(returnAction + returnQuery);
            }
            else
            { return Redirect("/Home/Error"); }
        }

        [Authorize]
        public async Task<IActionResult> Remove(string id, string returnAction, string returnQuery)
        {
            var idIsValidMovieOrTVShowId = watchlistService.IsValidMovieOrTVShowId(id);
            if (!idIsValidMovieOrTVShowId)
            {
                return Redirect("/Home/Error");
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var itemType = watchlistService.IsIdMovieOrTVShowId(id);
            if (itemType == "Movie")
            {
                if (!watchlistService.MovieIsInUserWatchlist(userId, id))
                {
                    return Redirect("/Home/Error");
                }
                await watchlistService.RemoveMovieFromUserWatchlistAsync(userId, id);
                return Redirect(returnAction + returnQuery);
            }
            else if (itemType == "TV Show")
            {
                if (!watchlistService.TVShowIsInUserWatchlist(userId, id))
                {
                    return Redirect("/Home/Error");
                }
                await watchlistService.RemoveTVShowFromUserWatchlistAsync(userId, id);
                return Redirect(returnAction + returnQuery);
            }
            else
            { return Redirect("/Home/Error"); }
        }
    }
}