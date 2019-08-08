using Microsoft.AspNetCore.Mvc;
using MovieDatabase.Services.Contracts;
using System.Threading.Tasks;

namespace MovieDatabase.Web.Controllers
{
    public class MoviesController : Controller
    {
        private readonly IMovieService movieService;
        private readonly IUserService userService;

        public MoviesController(IMovieService movieService, IUserService userService)
        {
            this.movieService = movieService;
            this.userService = userService;
        }

        public async Task<IActionResult> Details(string id)
        {
            string userId = null;
            if (User.Identity.IsAuthenticated)
            {
                userId = await userService.GetUserIdFromUserNameAsync(User.Identity.Name);
            }

            var movieViewModel = await movieService.GetMovieAndDetailsByIdAsync(id, userId);

            return View(movieViewModel);
        }

        public async Task<IActionResult> All(string orderBy, string genreFilter)
        {
            string userId = null;
            if (User.Identity.IsAuthenticated)
            {
                userId = await userService.GetUserIdFromUserNameAsync(User.Identity.Name);
            }

            var moviesAllViewModel = await movieService.GetAllMoviesAsync(userId);

            moviesAllViewModel = await movieService.FilterMoviesByGenreAsync(moviesAllViewModel, genreFilter);

            moviesAllViewModel = movieService.OrderMovies(moviesAllViewModel, orderBy);

            return View(moviesAllViewModel);
        }
    }
}