using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieDatabase.Models.InputModels.Movie;
using MovieDatabase.Services.Contracts;
using System.Linq;
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

            var allMoviesOrdered = movieService.GetAllMoviesAndOrder(orderBy, genreFilter, userId);

            ViewBag.Genres = genreService.GetAllGenres();

            return View(allMoviesOrdered);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewBag.Genres = genreService.GetAllGenres();
            ViewBag.Directors = artistService.GetAllArtistsAndOrder().Select(a => a.FullName).ToList();

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]        
        public IActionResult Create(CreateMovieInputModel input)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Genres = genreService.GetAllGenres();
                ViewBag.Directors = artistService.GetAllArtistsAndOrder().Select(a => a.FullName).ToList();

                return View(input);
            }

            movieService.CreateMovie(input);

            return Redirect("/Movies/All?orderBy=release");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult AddRole()
        {
            ViewBag.Movies = movieService.GetAllMoviesAndOrder().Select(m => m.Name).ToList();//TODO: Create dedicated method
            ViewBag.Artists = artistService.GetAllArtistsAndOrder().Select(a => a.FullName).ToList();

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult AddRole(AddRoleInputModel input)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Movies = movieService.GetAllMoviesAndOrder().Select(m => m.Name).ToList();
                ViewBag.Artists = artistService.GetAllArtistsAndOrder().Select(a => a.FullName).ToList();

                return View(input);
            }

            movieService.AddRoleToMovie(input);

            return Redirect("/Movies/All?orderBy=release");
        }
    }
}