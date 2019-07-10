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

            var isValidId = watchlistService.IsValidId(id);
            if (isValidId)
            {
                var exists = watchlistService.Exists(userId, id);

                if (exists)
                {
                    watchlistService.RemoveItemFromUserWatchlist(userId, id);
                }
            }

            return Redirect("/Watchlist/Index");
        }

        [Authorize]
        public IActionResult AddRemove(string id, string returnQuery)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var isValidId = watchlistService.IsValidId(id);
            var controller = "";
            if (isValidId)
            {
                var exists = watchlistService.Exists(userId, id);

                if (exists)
                {
                    controller = watchlistService.RemoveItemFromUserWatchlist(userId, id);
                }
                else
                {
                    controller = watchlistService.AddItemToUserWatchlist(userId, id);
                }
            }

            return Redirect($"/{controller}/All" + returnQuery);
        }
    }
}