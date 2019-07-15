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
        public IActionResult Remove(string id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var idIsValidMovieOrTVShowId = watchlistService.IsValidMovieOrTVShowId(id);
            if (idIsValidMovieOrTVShowId)
            {
                var itemType = watchlistService.IsIdMovieOrTVShowId(id);

                switch (itemType)
                {
                    case "Movie":
                        if (!watchlistService.RemoveMovieFromUserWatchlist(userId, id))
                        {
                            return Redirect("/Home/Error");
                        }

                        return Redirect("/Watchlist/Index");
                    case "TV Show":
                        if (!watchlistService.RemoveTVShowFromUserWatchlist(userId, id))
                        {
                            return Redirect("/Home/Error");
                        }

                        return Redirect("/Watchlist/Index");
                    default:
                        return Redirect("/Home/Error");
                }
            }

            return Redirect("/Home/Error");
        }

        [Authorize]
        public IActionResult AddRemove(string id, string returnQuery)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var idIsValidMovieOrTVShowId = watchlistService.IsValidMovieOrTVShowId(id);
            if (idIsValidMovieOrTVShowId)
            {
                var itemType = watchlistService.IsIdMovieOrTVShowId(id);

                switch (itemType)
                {
                    case "Movie":
                        if (watchlistService.MovieIsInUserWatchlist(userId, id))
                        {
                            if (!watchlistService.RemoveMovieFromUserWatchlist(userId, id))
                            {
                                return Redirect("/Home/Error");
                            }

                            return Redirect($"/Movies/All" + returnQuery);
                        }
                        else
                        {
                            if (!watchlistService.AddMovieToUserWatchlist(userId, id))
                            {
                                return Redirect("/Home/Error");
                            }

                            return Redirect($"/Movies/All" + returnQuery);
                        }
                    case "TV Show":
                        if (watchlistService.TVShowIsInUserWatchlist(userId, id))
                        {
                            if (!watchlistService.RemoveTVShowFromUserWatchlist(userId, id))
                            {
                                return Redirect("/Home/Error");
                            }

                            return Redirect($"/TVShows/All" + returnQuery);
                        }
                        else
                        {
                            if (!watchlistService.AddTVShowToUserWatchlist(userId, id))
                            {
                                return Redirect("/Home/Error");
                            }

                            return Redirect($"/TVShows/All" + returnQuery);
                        }
                    default:
                        return Redirect("/Home/Error");
                }
            }

            return Redirect("/Home/Error");
        }
    }
}