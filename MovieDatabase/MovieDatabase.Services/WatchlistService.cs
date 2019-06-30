using MovieDatabase.Data;
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

        public bool RemoveItemFromUserWatchlist(string userId, string itemId)
        {
            if (dbContext.Movies.Any(m => m.Id == itemId))
            {
                var movieUser = dbContext.MovieUsers.SingleOrDefault(mu => mu.UserId == userId && mu.MovieId == itemId);

                dbContext.MovieUsers.Remove(movieUser);
                dbContext.SaveChanges();

                return true;
            }
            else if (dbContext.TVShows.Any(m => m.Id == itemId))
            {
                var tvShowUser = dbContext.TVShowUsers.SingleOrDefault(tu => tu.UserId == userId && tu.TVShowId == itemId);

                dbContext.TVShowUsers.Remove(tvShowUser);
                dbContext.SaveChanges();

                return true;
            }

            return false;

        }

        public List<WatchlistAllViewModel> GetItemsInUserWatchlist(string userId)
        {
            var watchlistAllViewModel = new List<WatchlistAllViewModel>();

            var movies = dbContext.MovieUsers
                .Where(u => u.UserId == userId)
                .ToList()
                .Select(x => new WatchlistAllViewModel
                {
                    Id = x.MovieId,
                    Name = x.Movie.Name,
                    Description = x.Movie.Description.Substring(0, Math.Min(250, x.Movie.Description.Length)) + "....",
                    CoverImageLink = x.Movie.CoverImageLink,
                    ReleaseDate = x.Movie.ReleaseDate,
                    Rating = x.Movie.Rating,
                    Type = Class.Movie
                })
                .ToList();

            watchlistAllViewModel.AddRange(movies);

            var tvShows = dbContext.TVShowUsers
                .Where(u => u.UserId == userId)
                .ToList()
                .Select(x => new WatchlistAllViewModel
                {
                    Id = x.TVShowId,
                    Name = x.TVShow.Name,
                    Description = x.TVShow.Description.Substring(0, Math.Min(250, x.TVShow.Description.Length)) + "....",
                    CoverImageLink = x.TVShow.CoverImageLink,
                    ReleaseDate = x.TVShow.Seasons.First().ReleaseDate,
                    Rating = x.TVShow.OverallRating,
                    Type = Class.TVShow
                })
                .ToList();

            watchlistAllViewModel.AddRange(tvShows);

            return watchlistAllViewModel;
        }
    }
}
