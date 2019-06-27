using Microsoft.AspNetCore.Mvc;
using MovieDatabase.Services.Contracts;

namespace MovieDatabase.Web.Controllers
{
    public class TVShowsController : Controller
    {
        private readonly ITVShowService tvShowService;

        public TVShowsController(ITVShowService tvShowService)
        {
            this.tvShowService = tvShowService;
        }

        public IActionResult Details(string id)
        {
            var tvShowDetailsViewModel = this.tvShowService.GetTVShowAndDetailsById(id);

            return View(tvShowDetailsViewModel);
        }

        public IActionResult All(string orderBy)
        {
            var allTVShowsViewModel = this.tvShowService.GetAllTVShowsAndOrder(orderBy);

            return View(allTVShowsViewModel);
        }
    }
}