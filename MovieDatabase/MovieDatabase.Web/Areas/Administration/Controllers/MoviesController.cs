using Microsoft.AspNetCore.Mvc;
using MovieDatabase.Common;
using MovieDatabase.Models.InputModels.Movie;
using MovieDatabase.Services.Contracts;
using System.Threading.Tasks;

namespace MovieDatabase.Web.Areas.Administration.Controllers
{
    public class MoviesController : AdministrationController
    {
        private const string redirectMoviesAllAndOrder = "/Movies/All?orderBy=" + GlobalConstants.moviesTvShowsOrderByRelease;
        private const string redirectMovieDetails = "/Movies/Details/";

        private readonly IMovieService movieService;

        public MoviesController(IMovieService movieService)
        {
            this.movieService = movieService;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]      
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

            return Redirect(redirectMoviesAllAndOrder);
        }

        public IActionResult AddRole()
        {
            return View();
        }

        [HttpPost]
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

            return Redirect(redirectMoviesAllAndOrder);
        }

        public async Task<IActionResult> Update(string id)
        {
            var movieDetailsModel = await movieService.GetMovieAndDetailsByIdAsync(id);

            var movieInputModel = new UpdateMovieInputModel
            {
                Id = movieDetailsModel.Id,
                Name = movieDetailsModel.Name,
                ReleaseDate = movieDetailsModel.ReleaseDate,
                Genre = movieDetailsModel.Genre,
                Director = movieDetailsModel.Director,
                Length = movieDetailsModel.Length,
                Description = movieDetailsModel.Description,
                CoverImageLink = movieDetailsModel.CoverImageLink,
                TrailerLink = movieDetailsModel.TrailerLink,
            };

            return View(movieInputModel);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateMovieInputModel input)
        {
            if (!ModelState.IsValid)
            {
                return View(input);
            }

            if (!await movieService.UpdateMovieAsync(input))
            {
                return View(input);
            }

            return Redirect(redirectMovieDetails + input.Id);
        }
    }
}