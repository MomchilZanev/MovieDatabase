using MovieDatabase.Data;
using MovieDatabase.Models.ViewModels.Movie;
using MovieDatabase.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MovieDatabase.Services
{
    public class MovieService : IMovieService
    {
        private readonly MovieDatabaseDbContext dbContext;

        public MovieService(MovieDatabaseDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        //TODO: Implement AutoMapper
        public List<MovieAllViewModel> GetAllMoviesAndOrder(string orderBy, string userId)
        {
            var allMovies = dbContext.Movies.ToList();

            var movieAllViewModel = allMovies
                .Select(m => new MovieAllViewModel
                {
                    Id = m.Id,
                    Name = m.Name,
                    CoverImageLink = m.CoverImageLink,
                    Rating = m.Rating,
                    ReleaseDate = m.ReleaseDate,
                    TotalReviews = m.Reviews.Count(),
                    Watchlisted = dbContext.MovieUsers.Any(mu => mu.MovieId == m.Id && mu.UserId == userId),
                })
                .ToList();

            if (orderBy == "release")
            {
                movieAllViewModel = movieAllViewModel
                    .Where(m => m.ReleaseDate != null)
                    .OrderByDescending(m => m.ReleaseDate)
                    .ToList();
            }
            else if (orderBy == "popularity")
            {
                movieAllViewModel = movieAllViewModel
                    .Where(m => m.ReleaseDate != null)
                    .OrderByDescending(m => m.TotalReviews)
                    .ToList();
            }
            else if (orderBy == "rating")
            {
                movieAllViewModel = movieAllViewModel
                    .Where(m => m.ReleaseDate != null)
                    .OrderByDescending(m => m.Rating)
                    .ToList();
            }
            else if (orderBy == "soon")
            {
                movieAllViewModel = movieAllViewModel
                    .Where(m => m.ReleaseDate != null && m.ReleaseDate > DateTime.UtcNow)
                    .OrderBy(m => m.ReleaseDate)
                    .ToList();
            }

            return movieAllViewModel;
        }

        public MovieDetailsViewModel GetMovieAndDetailsById(string movieId)
        {
            var movie = dbContext.Movies.Find(movieId);
            
            var movieDetailsViewModel = new MovieDetailsViewModel
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

            return movieDetailsViewModel;
        }
    }
}
