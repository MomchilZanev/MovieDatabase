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

        public IActionResult All(string orderBy)
        {
            var allMovies = this.movieService.GetAllMovies()
                .Select(m => new MovieAllViewModel
                {
                    Name = m.Name,
                    Description = m.Description,
                    CoverImageLink = m.CoverImageLink,
                    Rating = m.Rating,
                    ReleaseDate = m.ReleaseDate,
                    TotalReviews = m.Reviews.Count()
                })
                .ToList();

            //TODO: move this logic in Service Layer
            if (orderBy == "release")
            {
                allMovies = allMovies
                    .Where(m => m.ReleaseDate != null)
                    .OrderByDescending(m => m.ReleaseDate)
                    .ToList();
            }
            else if (orderBy == "popularity")
            {
                allMovies = allMovies
                    .Where(m => m.ReleaseDate != null)
                    .OrderByDescending(m => m.TotalReviews)
                    .ToList();
            }
            else if (orderBy == "rating")
            {
                allMovies = allMovies
                    .Where(m => m.ReleaseDate != null)
                    .OrderByDescending(m => m.Rating)
                    .ToList();
            }
            else if (orderBy == "soon")
            {
                allMovies = allMovies
                    .Where(m => m.ReleaseDate != null && m.ReleaseDate > DateTime.UtcNow)
                    .OrderBy(m => m.ReleaseDate)
                    .ToList();
            }

            return View(allMovies);
        }
    }
}