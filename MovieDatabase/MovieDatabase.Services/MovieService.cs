using MovieDatabase.Data;
using MovieDatabase.Domain;
using MovieDatabase.Models.InputModels.Movie;
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
        private readonly IReviewService reviewService;

        public MovieService(MovieDatabaseDbContext dbContext, IReviewService reviewService)
        {
            this.dbContext = dbContext;
            this.reviewService = reviewService;
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

        public MovieDetailsViewModel GetMovieAndDetailsById(string movieId, string userId)
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
                IsReviewedByCurrentUser = reviewService.ReviewExists(userId, movie.Id),
            };

            return movieDetailsViewModel;
        }

        public bool CreateMovie(CreateMovieInputModel input)
        {
            if (!dbContext.Genres.Any(g => g.Name == input.Genre))
            {
                return false;
            }
            if (!dbContext.Artists.Any(a => a.FullName == input.Director))
            {
                return false;
            }
            if (dbContext.Movies.Any(m => m.Name == input.Name))
            {
                return false;
            }

            var genre = dbContext.Genres.SingleOrDefault(g => g.Name == input.Genre);
            var director = dbContext.Artists.SingleOrDefault(a => a.FullName == input.Director);

            var movie = new Movie
            {
                Name = input.Name,
                Genre = genre,
                ReleaseDate = input.ReleaseDate,
                Length = input.Length,
                Director = director,
                Description = input.Description,
                CoverImageLink = (input.CoverImageLink == "" || input.CoverImageLink == null) ? "/images/no_image.png" : input.CoverImageLink,
            };
            dbContext.Movies.Add(movie);
            dbContext.SaveChanges();

            return true;
        }

        public bool AddRoleToMovie(AddRoleInputModel input)
        {
            if (!dbContext.Movies.Any(m => m.Name == input.Movie))
            {
                return false;
            }
            if (!dbContext.Artists.Any(a => a.FullName == input.Artist))
            {
                return false;
            }            

            var movie = dbContext.Movies.SingleOrDefault(g => g.Name == input.Movie);
            var artist = dbContext.Artists.SingleOrDefault(a => a.FullName == input.Artist);

            if (dbContext.MovieRoles.Any(mr => mr.ArtistId == artist.Id && mr.MovieId == movie.Id))
            {
                return false;
            }

            var movieRole = new MovieRole
            {
                Movie = movie,
                Artist = artist,
                CharacterPlayed = input.CharacterPlayed,
            };
            dbContext.MovieRoles.Add(movieRole);
            dbContext.SaveChanges();

            return true;
        }
    }
}
