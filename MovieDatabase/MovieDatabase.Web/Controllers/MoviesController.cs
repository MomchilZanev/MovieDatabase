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

        public IActionResult Details(string id)
        {
            var movie = this.movieService.GetMovieById(id);

            //TODO: Implement AutoMapper
            var movieViewModel = new MovieDetailsViewModel
            {
                Id = movie.Id,
                Name = movie.Name,
                Director = movie.Director.FullName,
                CoverImageLink = movie.CoverImageLink,
                Description = movie.Description,
                Genre = movie.Genre.Name,
                Length = movie.Length,
                Rating = movie.Rating,
                ReleaseDate = movie.ReleaseDate,
                Cast = movie.Cast.Select(x => new MovieCastViewModel
                {
                    Actor = x.Artist.FullName,
                    MovieCharacter = x.CharacterPlayed,
                }).ToList(),
                Reviews = movie.Reviews.Select(x => new MovieReviewViewModel
                {
                    Movie = movie.Name,
                    User = x.User.UserName,
                    Content = x.Content,
                    Rating = x.Rating,
                    Date = x.Date,
                }).ToList(),
            };

            return View(movieViewModel);
        }

        public IActionResult All(string orderBy)
        {
            //TODO: Implement AutoMapper
            var allMovies = this.movieService.GetAllMovies()
                .Select(m => new MovieAllViewModel
                {
                    Id = m.Id,
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