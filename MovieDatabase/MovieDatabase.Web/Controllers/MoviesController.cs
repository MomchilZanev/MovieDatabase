using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MovieDatabase.Services.Contracts;
using MovieDatabase.Web.ViewModels.Movie;

namespace MovieDatabase.Web.Controllers
{
    public class MoviesController : Controller
    {
        private readonly IMovieService movieService;

        public MoviesController(IMovieService movieService)
        {
            this.movieService = movieService;
        }

        public IActionResult All()
        {
            var allMovies = this.movieService.GetAllMovies()
                .Select(m => new MovieAllViewModel
                {
                    Name = m.Name,
                    Description = m.Description,
                    CoverImageLink = m.CoverImageLink,
                })
                .ToList();

            return View(allMovies);
        }
    }
}