using MovieDatabase.Data;
using MovieDatabase.Domain;
using MovieDatabase.Models.InputModels.Movie;
using MovieDatabase.Models.ViewModels.Movie;
using MovieDatabase.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public List<MovieAllViewModel> GetAllMovies(string userId)
        {
            var allMoviesFromDb = dbContext.Movies.ToList();

            var moviesAllViewModel = allMoviesFromDb
                .Select(movie => new MovieAllViewModel
                {
                    Id = movie.Id,
                    Name = movie.Name,
                    Genre = movie.Genre.Name,
                    CoverImageLink = movie.CoverImageLink,
                    Rating = movie.Rating,
                    ReleaseDate = movie.ReleaseDate,
                    TotalReviews = movie.TotalReviews,

                    Watchlisted = dbContext.MovieUsers.Any(movieUser => movieUser.MovieId == movie.Id && movieUser.UserId == userId),//TODO
                })
                .ToList();

            return moviesAllViewModel;
        }

        public List<MovieNameViewModel> GetAllMovieNames()
        {
            var allMovieNames = dbContext.Movies.Select(movie => new MovieNameViewModel
            {
                Name = movie.Name,
            }).ToList();

            return allMovieNames;
        }

        public List<MovieAllViewModel> FilterMoviesByGenre(List<MovieAllViewModel> moviesAllViewModel, string genreFilter)
        {
            if (dbContext.Genres.Any(genre => genre.Name == genreFilter))
            {
                return moviesAllViewModel = moviesAllViewModel.Where(movie => movie.Genre == genreFilter).ToList();
            }

            return moviesAllViewModel.ToList();
        }

        public List<MovieAllViewModel> OrderMovies(List<MovieAllViewModel> moviesAllViewModel, string orderBy)
        {
            switch (orderBy)
            {
                case "release":
                    return moviesAllViewModel.OrderByDescending(movie => movie.ReleaseDate).ToList();
                case "popularity":
                    return moviesAllViewModel.OrderByDescending(movie => movie.TotalReviews).ToList();
                case "rating":
                    return moviesAllViewModel.OrderByDescending(movie => movie.Rating).ToList();
                case "soon":
                    return moviesAllViewModel.Where(movie => movie.ReleaseDate > DateTime.UtcNow).OrderBy(movie => movie.ReleaseDate).ToList();
                default:
                    return moviesAllViewModel.ToList();
            }
        }

        public async Task<MovieDetailsViewModel> GetMovieAndDetailsByIdAsync(string movieId, string userId)
        {
            var movieFromDb = await dbContext.Movies.FindAsync(movieId);

            var randomReview = new MovieReviewViewModel();

            if (movieFromDb.Reviews.Count > 0)
            {
                Random rnd = new Random();
                int reviewIndex = rnd.Next(0, movieFromDb.Reviews.Count());

                var reviewFromDb = movieFromDb.Reviews.ToList()[reviewIndex];

                randomReview.Movie = reviewFromDb.Movie.Name;
                randomReview.User = reviewFromDb.User.UserName;
                randomReview.Content = reviewFromDb.Content;
                randomReview.Rating = reviewFromDb.Rating;
                randomReview.Date = reviewFromDb.Date;
            }

            var movieDetailsViewModel = new MovieDetailsViewModel
            {
                Id = movieFromDb.Id,
                Name = movieFromDb.Name,
                Director = movieFromDb.Director.FullName,
                CoverImageLink = movieFromDb.CoverImageLink,
                TrailerLink = movieFromDb.TrailerLink,
                Description = movieFromDb.Description,
                Genre = movieFromDb.Genre.Name,
                Length = movieFromDb.Length,
                Rating = movieFromDb.Rating,
                ReleaseDate = movieFromDb.ReleaseDate,
                Cast = movieFromDb.Cast.Select(x => new MovieCastViewModel
                {
                    Actor = x.Artist.FullName,
                    MovieCharacter = x.CharacterPlayed,
                }).ToList(),
                RandomReview = randomReview,
                ReviewsCount = movieFromDb.TotalReviews,

                IsReviewedByCurrentUser = reviewService.ReviewExists(userId, movieFromDb.Id), 
            };

            return movieDetailsViewModel;
        }

        public async Task<bool> CreateMovieAsync(CreateMovieInputModel input)
        {
            if (!dbContext.Genres.Any(genre => genre.Name == input.Genre))
            {
                return false;
            }
            if (!dbContext.Artists.Any(artist => artist.FullName == input.Director))
            {
                return false;
            }
            if (dbContext.Movies.Any(movie => movie.Name == input.Name))
            {
                return false;
            }

            var genreFromDb = dbContext.Genres.SingleOrDefault(g => g.Name == input.Genre);
            var directorFromDb = dbContext.Artists.SingleOrDefault(a => a.FullName == input.Director);

            var movieForDb = new Movie
            {
                Name = input.Name,
                Genre = genreFromDb,
                ReleaseDate = input.ReleaseDate,
                Length = input.Length,
                Director = directorFromDb,
                Description = input.Description,
                CoverImageLink = (input.CoverImageLink == "" || input.CoverImageLink == null) ? "/images/no_image.png" : input.CoverImageLink,
                TrailerLink = (input.TrailerLink == "" || input.TrailerLink == null) ? "https://www.youtube.com/embed/KAOdjqyG37A" : input.TrailerLink,
            };

            await dbContext.Movies.AddAsync(movieForDb);
            await dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> AddRoleToMovieAsync(AddRoleInputModel input)
        {
            if (!dbContext.Movies.Any(movie => movie.Name == input.Movie))
            {
                return false;
            }
            if (!dbContext.Artists.Any(artist => artist.FullName == input.Artist))
            {
                return false;
            }

            var movieFromDb = dbContext.Movies.SingleOrDefault(movie => movie.Name == input.Movie);
            var artistFromDb = dbContext.Artists.SingleOrDefault(artist => artist.FullName == input.Artist);

            if (dbContext.MovieRoles.Any(movieRole => movieRole.ArtistId == artistFromDb.Id && movieRole.MovieId == movieFromDb.Id))
            {
                return false;
            }

            var movieRoleForDb = new MovieRole
            {
                Movie = movieFromDb,
                Artist = artistFromDb,
                CharacterPlayed = input.CharacterPlayed,
            };

            await dbContext.MovieRoles.AddAsync(movieRoleForDb);
            await dbContext.SaveChangesAsync();

            return true;
        }
    }
}
