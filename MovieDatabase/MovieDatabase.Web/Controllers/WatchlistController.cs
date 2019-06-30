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

            var result = watchlistService.RemoveItemFromUserWatchlist(userId, id);

            return Redirect("/Watchlist/Index");
        }
    }
}