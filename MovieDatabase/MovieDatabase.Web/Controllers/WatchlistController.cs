using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieDatabase.Services.Contracts;
using System.Security.Claims;

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
        public IActionResult Index()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var watchlistAllViewModel = watchlistService.GetItemsInUserWatchlist(userId);

            return View(watchlistAllViewModel);
        }

        [Authorize]
        public IActionResult Add(string id, string returnAction, string returnQuery)
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
                watchlistService.AddMovieToUserWatchlist(userId, id);
                return Redirect(returnAction + returnQuery);
            }
            else if (itemType == "TV Show")
            {
                if (watchlistService.TVShowIsInUserWatchlist(userId, id))
                {
                    return Redirect("/Home/Error");
                }
                watchlistService.AddTVShowToUserWatchlist(userId, id);
                return Redirect(returnAction + returnQuery);
            }
            else
            { return Redirect("/Home/Error"); }
        }

        [Authorize]
        public IActionResult Remove(string id, string returnAction, string returnQuery)
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
                watchlistService.RemoveMovieFromUserWatchlist(userId, id);
                return Redirect(returnAction + returnQuery);
            }
            else if (itemType == "TV Show")
            {
                if (!watchlistService.TVShowIsInUserWatchlist(userId, id))
                {
                    return Redirect("/Home/Error");
                }
                watchlistService.RemoveTVShowFromUserWatchlist(userId, id);
                return Redirect(returnAction + returnQuery);
            }
            else
            { return Redirect("/Home/Error"); }
        }
    }
}