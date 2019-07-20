using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieDatabase.Common;
using MovieDatabase.Services.Contracts;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MovieDatabase.Web.Controllers
{
    public class WatchlistController : Controller
    {
        private const string redirectError = "/Home/Error";

        private readonly IWatchlistService watchlistService;

        public WatchlistController(IWatchlistService watchlistService)
        {
            this.watchlistService = watchlistService;
        }

        [Authorize]
        public IActionResult Index()
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
                return Redirect(redirectError);
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var itemType = watchlistService.IsIdMovieOrTVShowId(id);
            if (itemType == GlobalConstants.Movie)
            {
                if (watchlistService.MovieIsInUserWatchlist(userId, id))
                {
                    return Redirect(redirectError);
                }
                await watchlistService.AddMovieToUserWatchlistAsync(userId, id);
                return Redirect(returnAction + returnQuery);
            }
            else if (itemType == GlobalConstants.TV_Show)
            {
                if (watchlistService.TVShowIsInUserWatchlist(userId, id))
                {
                    return Redirect(redirectError);
                }
                await watchlistService.AddTVShowToUserWatchlistAsync(userId, id);
                return Redirect(returnAction + returnQuery);
            }
            else
            { return Redirect(redirectError); }
        }

        [Authorize]
        public async Task<IActionResult> Remove(string id, string returnAction, string returnQuery)
        {
            var idIsValidMovieOrTVShowId = watchlistService.IsValidMovieOrTVShowId(id);
            if (!idIsValidMovieOrTVShowId)
            {
                return Redirect(redirectError);
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var itemType = watchlistService.IsIdMovieOrTVShowId(id);
            if (itemType == GlobalConstants.Movie)
            {
                if (!watchlistService.MovieIsInUserWatchlist(userId, id))
                {
                    return Redirect(redirectError);
                }
                await watchlistService.RemoveMovieFromUserWatchlistAsync(userId, id);
                return Redirect(returnAction + returnQuery);
            }
            else if (itemType == GlobalConstants.TV_Show)
            {
                if (!watchlistService.TVShowIsInUserWatchlist(userId, id))
                {
                    return Redirect(redirectError);
                }
                await watchlistService.RemoveTVShowFromUserWatchlistAsync(userId, id);
                return Redirect(returnAction + returnQuery);
            }
            else
            { return Redirect(redirectError); }
        }
    }
}