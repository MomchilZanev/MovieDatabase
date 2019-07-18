using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieDatabase.Models.InputModels.Movie;
using MovieDatabase.Services.Contracts;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MovieDatabase.Web.Controllers
{
    public class MoviesController : Controller
    {
        private readonly IMovieService movieService;

        public MoviesController(IMovieService movieService)
        {
            this.movieService = movieService;
        }

        public async Task<IActionResult> Details(string id)
        {
            var userId = "";
            if (User.Identity.IsAuthenticated)
            {
                userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            }

            var movieViewModel = await movieService.GetMovieAndDetailsByIdAsync(id, userId);

            return View(movieViewModel);
        }

        public async Task<IActionResult> All(string orderBy, string genreFilter)
        {
            string userId = "";
            if (User.Identity.IsAuthenticated)
            {
                userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            }

            var moviesAllViewModel = movieService.GetAllMovies(userId);

            if (!string.IsNullOrEmpty(genreFilter))
            {
                moviesAllViewModel = movieService.FilterMoviesByGenre(moviesAllViewModel, genreFilter);
            }
            if (!string.IsNullOrEmpty(orderBy))
            {
                moviesAllViewModel = movieService.OrderMovies(moviesAllViewModel, orderBy);
            }

            return View(moviesAllViewModel);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]        
        public async Task<IActionResult> Create(CreateMovieInputModel input)
        {
            if (!ModelState.IsValid)
            {
                return View(input);
            }

            if (!await movieService.CreateMovieAsync(input))
            {
                return View(input);
            }

            return Redirect("/Movies/All?orderBy=release");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddRole()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddRole(AddRoleInputModel input)
        {
            if (!ModelState.IsValid)
            {
                return View(input);
            }

            if (!await movieService.AddRoleToMovieAsync(input))
            {
                return View(input);
            }

            return Redirect("/Movies/All?orderBy=release");
        }
    }
}