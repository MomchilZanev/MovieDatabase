using Microsoft.AspNetCore.Mvc;
using MovieDatabase.Services.Contracts;
using System.Security.Claims;

namespace MovieDatabase.Web.Controllers
{
    public class TVShowsController : Controller
    {
        private readonly ITVShowService tvShowService;

        public TVShowsController(ITVShowService tvShowService, IReviewService reviewService)
        {
            this.tvShowService = tvShowService;
        }

        public IActionResult Details(string id)
        {
            var userId = "";
            if (User.Identity.IsAuthenticated)
            {
                userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            }

            var tvShowDetailsViewModel = tvShowService.GetTVShowAndDetailsById(id, userId);

            return View(tvShowDetailsViewModel);
        }

        public IActionResult All(string orderBy)
        {
            string userId = "";
            if (User.Identity.IsAuthenticated)
            {
                userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            }

            var allTVShowsViewModel = tvShowService.GetAllTVShowsAndOrder(orderBy, userId);

            return View(allTVShowsViewModel);
        }
    }
}