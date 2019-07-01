using Microsoft.AspNetCore.Mvc;
using MovieDatabase.Services.Contracts;
using System.Security.Claims;

namespace MovieDatabase.Web.Controllers
{
    public class MoviesController : Controller
    {
        private readonly IMovieService movieService;

        public MoviesController(IMovieService movieService)
        {
            this.movieService = movieService;
        }

        public IActionResult Details(string id)
        {
            var movieViewModel = movieService.GetMovieAndDetailsById(id);

            return View(movieViewModel);
        }

        public IActionResult All(string orderBy)
        {
            string userId = "";

            if (User.Identity.IsAuthenticated)
            {
                userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            }

            var allMoviesOrdered = movieService.GetAllMoviesAndOrder(orderBy, userId);

            return View(allMoviesOrdered);
        }
    }
}