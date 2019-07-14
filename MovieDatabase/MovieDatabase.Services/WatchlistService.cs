using MovieDatabase.Data;
using MovieDatabase.Domain;
using MovieDatabase.Models.ViewModels.Watchlist;
using MovieDatabase.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MovieDatabase.Services
{
    public class WatchlistService : IWatchlistService
    {
        private readonly MovieDatabaseDbContext dbContext;

        public WatchlistService(MovieDatabaseDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public bool IsValidId(string itemId)
        {
            if (dbContext.Movies.Any(movie => movie.Id == itemId) || dbContext.TVShows.Any(tvShow => tvShow.Id == itemId))
            {
                return true;
            }

            return false;
        }

        public bool Exists(string userId, string itemId)
        {            
            if (dbContext.MovieUsers.Any(movieUser => movieUser.MovieId == itemId && movieUser.UserId == userId) ||
                dbContext.TVShowUsers.Any(tvShowUser => tvShowUser.TVShowId == itemId && tvShowUser.UserId == userId))
            {
                return true;
            }

            return false;
        }

        public string AddItemToUserWatchlist(string userId, string itemId)
        {
            if (dbContext.Movies.Any(movie => movie.Id == itemId))
            {
                var movieUserForDb = new MovieUser
                {
                    MovieId = itemId,
                    UserId = userId,
                };

                dbContext.MovieUsers.Add(movieUserForDb);
                dbContext.SaveChanges();

                return "Movies";
            }
            else if (dbContext.TVShows.Any(tvShow => tvShow.Id == itemId))
            {
                var tvShowUserForDb = new TVShowUser
                {
                    TVShowId = itemId,
                    UserId = userId,
                };

                dbContext.TVShowUsers.Add(tvShowUserForDb);
                dbContext.SaveChanges();

                return "TVShows";
            }

            return "Error";
        }

        public string RemoveItemFromUserWatchlist(string userId, string itemId)
        {
            if (dbContext.MovieUsers.Any(movieUser => movieUser.MovieId == itemId && movieUser.UserId == userId))
            {
                var movieUserFromDb = dbContext.MovieUsers
                    .SingleOrDefault(mu => mu.MovieId == itemId && mu.UserId == userId);

                dbContext.MovieUsers.Remove(movieUserFromDb);
                dbContext.SaveChanges();

                return "Movies";
            }
            else if (dbContext.TVShowUsers.Any(tvShowUser => tvShowUser.TVShowId == itemId && tvShowUser.UserId == userId))
            {
                var tvShowUserFromDb = dbContext.TVShowUsers
                    .SingleOrDefault(tvShowUser => tvShowUser.TVShowId == itemId && tvShowUser.UserId == userId);

                dbContext.TVShowUsers.Remove(tvShowUserFromDb);
                dbContext.SaveChanges();

                return "TVShows";
            }

            return "Error";

        }

        public List<WatchlistAllViewModel> GetItemsInUserWatchlist(string userId)
        {
            var watchlistAllViewModel = new List<WatchlistAllViewModel>();

            var moviesFromDb = dbContext.MovieUsers
                .Where(movieUser => movieUser.UserId == userId)
                .ToList()
                .Select(movieUser => new WatchlistAllViewModel
                {
                    Id = movieUser.MovieId,
                    Name = movieUser.Movie.Name,
                    Description = movieUser.Movie.Description.Substring(0, Math.Min(500, movieUser.Movie.Description.Length)) + "....",
                    CoverImageLink = movieUser.Movie.CoverImageLink,
                    ReleaseDate = movieUser.Movie.ReleaseDate,
                    Rating = movieUser.Movie.Rating,
                    Category = Category.Movies
                })
                .ToList();

            watchlistAllViewModel.AddRange(moviesFromDb);

            var tvShowsFromDb = dbContext.TVShowUsers
                .Where(tvShowUser => tvShowUser.UserId == userId)
                .ToList()
                .Select(tvShowUser => new WatchlistAllViewModel
                {
                    Id = tvShowUser.TVShowId,
                    Name = tvShowUser.TVShow.Name,
                    Description = tvShowUser.TVShow.Description.Substring(0, Math.Min(500, tvShowUser.TVShow.Description.Length)) + "....",
                    CoverImageLink = tvShowUser.TVShow.CoverImageLink,
                    ReleaseDate = tvShowUser.TVShow.FirstAired,
                    Rating = tvShowUser.TVShow.OverallRating,
                    Category = Category.TVShows
                })
                .ToList();

            watchlistAllViewModel.AddRange(tvShowsFromDb);

            return watchlistAllViewModel;
        }        
    }
}
