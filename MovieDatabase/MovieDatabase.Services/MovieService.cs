using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieDatabase.Common;
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
        private readonly IWatchlistService watchlistService;
        private readonly IMapper mapper;

        public MovieService(MovieDatabaseDbContext dbContext, IReviewService reviewService, IWatchlistService watchlistService, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.reviewService = reviewService;
            this.watchlistService = watchlistService;
            this.mapper = mapper;
        }

        public async Task<List<MovieAllViewModel>> GetAllMoviesAsync(string userId = null)
        {
            var allMoviesFromDb = await dbContext.Movies.ToListAsync();

            var moviesAllViewModel = mapper.Map<List<Movie>, List<MovieAllViewModel>>(allMoviesFromDb);
            foreach (var movie in moviesAllViewModel)
            {
                movie.Watchlisted = await watchlistService.MovieIsInUserWatchlistAsync(userId, movie.Id);
            }

            return moviesAllViewModel;
        }

        public async Task<List<MovieNameViewModel>> GetAllMovieNamesAsync()
        {
            var allMovies = await dbContext.Movies.ToListAsync();

            var allMovieNames = mapper.Map<List<Movie>, List<MovieNameViewModel>>(allMovies);

            return allMovieNames;
        }

        public async Task<List<MovieAllViewModel>> FilterMoviesByGenreAsync(List<MovieAllViewModel> moviesAllViewModel, string genreFilter)
        {
            if (await dbContext.Genres.AnyAsync(genre => genre.Name == genreFilter))
            {
                return moviesAllViewModel = moviesAllViewModel.Where(movie => movie.Genre == genreFilter).ToList();
            }

            return moviesAllViewModel.ToList();
        }

        public List<MovieAllViewModel> OrderMovies(List<MovieAllViewModel> moviesAllViewModel, string orderBy)
        {
            switch (orderBy)
            {
                case GlobalConstants.moviesTvShowsOrderByRelease:
                    return moviesAllViewModel.OrderByDescending(movie => movie.ReleaseDate).ToList();
                case GlobalConstants.moviesTvShowsOrderByPopularity:
                    return moviesAllViewModel.OrderByDescending(movie => movie.TotalReviews).ToList();
                case GlobalConstants.moviesTvShowsOrderByRating:
                    return moviesAllViewModel.OrderByDescending(movie => movie.Rating).ToList();
                case GlobalConstants.moviesTvShowsShowComingSoon:
                    return moviesAllViewModel.Where(movie => movie.ReleaseDate > DateTime.UtcNow).OrderBy(movie => movie.ReleaseDate).ToList();
                default:
                    return moviesAllViewModel.ToList();
            }
        }

        public async Task<MovieDetailsViewModel> GetMovieAndDetailsByIdAsync(string movieId, string userId = null)
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

            var movieDetailsViewModel = mapper.Map<Movie, MovieDetailsViewModel>(movieFromDb);
            movieDetailsViewModel.RandomReview = randomReview;
            movieDetailsViewModel.Cast = mapper.Map<List<MovieRole>, List<MovieCastViewModel>>(movieFromDb.Cast.ToList());
            movieDetailsViewModel.IsReviewedByCurrentUser = await reviewService.ReviewExistsAsync(userId, movieFromDb.Id);

            return movieDetailsViewModel;
        }

        public async Task<bool> CreateMovieAsync(CreateMovieInputModel input)
        {
            if (!await dbContext.Genres.AnyAsync(genre => genre.Name == input.Genre))
            {
                return false;
            }
            if (!await dbContext.Artists.AnyAsync(artist => artist.FullName == input.Director))
            {
                return false;
            }
            if (await dbContext.Movies.AnyAsync(movie => movie.Name == input.Name))
            {
                return false;
            }

            var genreFromDb = await dbContext.Genres.SingleOrDefaultAsync(g => g.Name == input.Genre);
            var directorFromDb = await dbContext.Artists.SingleOrDefaultAsync(a => a.FullName == input.Director);

            var movieForDb = mapper.Map<CreateMovieInputModel, Movie>(input);
            movieForDb.Genre = genreFromDb;
            movieForDb.Director = directorFromDb;

            await dbContext.Movies.AddAsync(movieForDb);
            await dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> AddRoleToMovieAsync(AddMovieRoleInputModel input)
        {
            if (!await dbContext.Movies.AnyAsync(movie => movie.Name == input.Movie))
            {
                return false;
            }
            if (!await dbContext.Artists.AnyAsync(artist => artist.FullName == input.Artist))
            {
                return false;
            }

            var movieFromDb = await dbContext.Movies.SingleOrDefaultAsync(movie => movie.Name == input.Movie);
            var artistFromDb = await dbContext.Artists.SingleOrDefaultAsync(artist => artist.FullName == input.Artist);

            if (await dbContext.MovieRoles.AnyAsync(movieRole => movieRole.ArtistId == artistFromDb.Id && movieRole.MovieId == movieFromDb.Id))
            {
                return false;
            }

            var movieRoleForDb = mapper.Map<AddMovieRoleInputModel, MovieRole>(input);
            movieRoleForDb.Movie = movieFromDb;
            movieRoleForDb.Artist = artistFromDb;

            await dbContext.MovieRoles.AddAsync(movieRoleForDb);
            await dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateMovieAsync(UpdateMovieInputModel input)
        {
            if (!await dbContext.Movies.AnyAsync(movie => movie.Id == input.Id))
            {
                return false;
            }
            if (!await dbContext.Genres.AnyAsync(genre => genre.Name == input.Genre))
            {
                return false;
            }
            if (!await dbContext.Artists.AnyAsync(artist => artist.FullName == input.Director))
            {
                return false;
            }

            var genreFromDb = await dbContext.Genres.SingleOrDefaultAsync(g => g.Name == input.Genre);
            var directorFromDb = await dbContext.Artists.SingleOrDefaultAsync(a => a.FullName == input.Director);

            var movieFromDb = await dbContext.Movies.FindAsync(input.Id);
            movieFromDb.Name = input.Name;
            movieFromDb.Genre = genreFromDb;
            movieFromDb.ReleaseDate = input.ReleaseDate;
            movieFromDb.Length = input.Length;
            movieFromDb.Description = input.Description;
            movieFromDb.CoverImageLink = input.CoverImageLink;
            movieFromDb.TrailerLink = input.TrailerLink;
            movieFromDb.Director = directorFromDb;

            dbContext.Update(movieFromDb);
            await dbContext.SaveChangesAsync();

            return true;
        }
    }
}
