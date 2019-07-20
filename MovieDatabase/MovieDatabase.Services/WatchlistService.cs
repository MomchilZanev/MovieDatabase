using MovieDatabase.Common;
using MovieDatabase.Data;
using MovieDatabase.Domain;
using MovieDatabase.Models.ViewModels.Watchlist;
using MovieDatabase.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieDatabase.Services
{
    public class WatchlistService : IWatchlistService
    {
        private readonly MovieDatabaseDbContext dbContext;

        public WatchlistService(MovieDatabaseDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public bool IsValidMovieOrTVShowId(string id)
        {
            if (dbContext.Movies.Any(movie => movie.Id == id) || dbContext.TVShows.Any(tvShow => tvShow.Id == id))
            {
                return true;
            }

            return false;
        }

        public string IsIdMovieOrTVShowId(string id)
        {
            if (dbContext.Movies.Any(movie => movie.Id == id))
            {
                return GlobalConstants.Movie;
            }
            else if (dbContext.TVShows.Any(tvShow => tvShow.Id == id))
            {
                return GlobalConstants.TV_Show;
            }

            return GlobalConstants.Neither;
        }

        public bool MovieIsInUserWatchlist(string userId, string movieId)
        {
            if (dbContext.MovieUsers.Any(movieUser => movieUser.MovieId == movieId && movieUser.UserId == userId))
            {
                return true;
            }

            return false;
        }

        public bool TVShowIsInUserWatchlist(string userId, string tvShowId)
        {
            if (dbContext.TVShowUsers.Any(tvSHowuser => tvSHowuser.TVShowId == tvShowId && tvSHowuser.UserId == userId))
            {
                return true;
            }

            return false;
        }

        public List<WatchlistAllViewModel> GetItemsInUserWatchlist(string userId)
        {
            var watchlistAllViewModel = new List<WatchlistAllViewModel>();

            var moviesFromDb = dbContext.MovieUsers.Where(movieUser => movieUser.UserId == userId).ToList();

            var moviesWatchlistAllViewModel = moviesFromDb
                .Select(movieUser => new WatchlistAllViewModel
                {
                    Id = movieUser.MovieId,
                    Name = movieUser.Movie.Name,
                    Description = movieUser.Movie.Description.Substring(0, Math.Min(GlobalConstants.movieTvShowPreviewDescriptionMaxCharLength, movieUser.Movie.Description.Length)) + "....",
                    CoverImageLink = movieUser.Movie.CoverImageLink,
                    ReleaseDate = movieUser.Movie.ReleaseDate,
                    Rating = movieUser.Movie.Rating,
                    Category = GlobalConstants.moviesCategory,
                })
                .ToList();

            watchlistAllViewModel.AddRange(moviesWatchlistAllViewModel);

            var tvShowsFromDb = dbContext.TVShowUsers.Where(tvShowUser => tvShowUser.UserId == userId).ToList();

            var tvShowsWatchlistAllViewModel = tvShowsFromDb
                .Select(tvShowUser => new WatchlistAllViewModel
                {
                    Id = tvShowUser.TVShowId,
                    Name = tvShowUser.TVShow.Name,
                    Description = tvShowUser.TVShow.Description.Substring(0, Math.Min(500, tvShowUser.TVShow.Description.Length)) + "....",
                    CoverImageLink = tvShowUser.TVShow.CoverImageLink,
                    ReleaseDate = tvShowUser.TVShow.FirstAired,
                    Rating = tvShowUser.TVShow.OverallRating,
                    Category = GlobalConstants.tvShowsCategory,
                })
                .ToList();

            watchlistAllViewModel.AddRange(tvShowsWatchlistAllViewModel);

            return watchlistAllViewModel;
        }

        public async Task AddMovieToUserWatchlistAsync(string userId, string movieId)
        {
            var movieUserForDb = new MovieUser
            {
                MovieId = movieId,
                UserId = userId,
            };

            await dbContext.MovieUsers.AddAsync(movieUserForDb);
            await dbContext.SaveChangesAsync();
        }

        public async Task AddTVShowToUserWatchlistAsync(string userId, string tvShowId)
        {
            var tvShowUserForDb = new TVShowUser
            {
                TVShowId = tvShowId,
                UserId = userId,
            };

            await dbContext.TVShowUsers.AddAsync(tvShowUserForDb);
            await dbContext.SaveChangesAsync();
        }

        public async Task RemoveMovieFromUserWatchlistAsync(string userId, string movieId)
        {
            var movieUserFromDb = dbContext.MovieUsers
                    .SingleOrDefault(movieUser => movieUser.MovieId == movieId && movieUser.UserId == userId);

            dbContext.MovieUsers.Remove(movieUserFromDb);
            await dbContext.SaveChangesAsync();
        }

        public async Task RemoveTVShowFromUserWatchlistAsync(string userId, string tvShowId)
        {
            var tvShowUserFromDb = dbContext.TVShowUsers
                    .SingleOrDefault(tvShowUser => tvShowUser.TVShowId == tvShowId && tvShowUser.UserId == userId);

            dbContext.TVShowUsers.Remove(tvShowUserFromDb);
            await dbContext.SaveChangesAsync();
        }
    }
}
