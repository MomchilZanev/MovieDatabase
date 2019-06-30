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
            var movieViewModel = this.movieService.GetMovieAndDetailsById(id);

            return View(movieViewModel);
        }

        public IActionResult All(string orderBy)
        {
            var allMoviesOrdered = this.movieService.GetAllMoviesAndOrder(orderBy);

            return View(allMoviesOrdered);
        }
    }
}