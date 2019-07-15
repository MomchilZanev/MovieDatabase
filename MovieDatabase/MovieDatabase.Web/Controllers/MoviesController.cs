using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieDatabase.Models.InputModels.Movie;
using MovieDatabase.Services.Contracts;
using System.Security.Claims;

namespace MovieDatabase.Web.Controllers
{
    public class MoviesController : Controller
    {
        private readonly IMovieService movieService;
        private readonly IGenreService genreService;
        private readonly IArtistService artistService;

        public MoviesController(IMovieService movieService, IGenreService genreService, IArtistService artistService)
        {
            this.movieService = movieService;
            this.genreService = genreService;
            this.artistService = artistService;
        }

        public IActionResult Details(string id)
        {
            var userId = "";
            if (User.Identity.IsAuthenticated)
            {
                userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            }

            var movieViewModel = movieService.GetMovieAndDetailsById(id, userId);

            return View(movieViewModel);
        }

        public IActionResult All(string orderBy, string genreFilter)
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

            ViewBag.Genres = genreService.GetAllGenreNames();

            return View(moviesAllViewModel);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewBag.Genres = genreService.GetAllGenreNames();
            ViewBag.Directors = artistService.GetAllArtistNames();

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]        
        public IActionResult Create(CreateMovieInputModel input)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Genres = genreService.GetAllGenreNames();
                ViewBag.Directors = artistService.GetAllArtistNames();

                return View(input);
            }

            if (movieService.CreateMovie(input))
            {
                ViewBag.Genres = genreService.GetAllGenreNames();
                ViewBag.Directors = artistService.GetAllArtistNames();

                return View(input);
            }

            return Redirect("/Movies/All?orderBy=release");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult AddRole()
        {
            ViewBag.Movies = movieService.GetAllMovieNames();
            ViewBag.Artists = artistService.GetAllArtistNames();

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult AddRole(AddRoleInputModel input)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Movies = movieService.GetAllMovieNames();
                ViewBag.Artists = artistService.GetAllArtistNames();

                return View(input);
            }

            if (!movieService.AddRoleToMovie(input))
            {
                ViewBag.Movies = movieService.GetAllMovieNames();
                ViewBag.Artists = artistService.GetAllArtistNames();

                return View(input);
            }

            return Redirect("/Movies/All?orderBy=release");
        }
    }
}