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
            if (dbContext.Movies.Any(m => m.Id == itemId) || dbContext.TVShows.Any(t => t.Id == itemId))
            {
                return true;
            }

            return false;
        }

        public bool Exists(string userId, string itemId)
        {            
            if (dbContext.MovieUsers.Any(mu => mu.MovieId == itemId && mu.UserId == userId) ||
                dbContext.TVShowUsers.Any(tu => tu.TVShowId == itemId && tu.UserId == userId))
            {
                return true;
            }

            return false;
        }

        public string AddItemToUserWatchlist(string userId, string itemId)
        {
            if (dbContext.Movies.Any(m => m.Id == itemId))
            {
                var movieUser = new MovieUser
                {
                    MovieId = itemId,
                    UserId = userId,
                };

                dbContext.MovieUsers.Add(movieUser);
                dbContext.SaveChanges();

                return "Movies";
            }
            else if (dbContext.TVShows.Any(m => m.Id == itemId))
            {
                var tvShowUser = new TVShowUser
                {
                    TVShowId = itemId,
                    UserId = userId,
                };

                dbContext.TVShowUsers.Add(tvShowUser);
                dbContext.SaveChanges();

                return "TVShows";
            }

            return "Error";
        }

        public string RemoveItemFromUserWatchlist(string userId, string itemId)
        {
            if (dbContext.MovieUsers.Any(mu => mu.MovieId == itemId && mu.UserId == userId))
            {
                var movieUser = dbContext.MovieUsers.SingleOrDefault(mu => mu.MovieId == itemId && mu.UserId == userId);

                dbContext.MovieUsers.Remove(movieUser);
                dbContext.SaveChanges();

                return "Movies";
            }
            else if (dbContext.TVShowUsers.Any(tu => tu.TVShowId == itemId && tu.UserId == userId))
            {
                var tvShowUser = dbContext.TVShowUsers.SingleOrDefault(tu => tu.TVShowId == itemId && tu.UserId == userId);

                dbContext.TVShowUsers.Remove(tvShowUser);
                dbContext.SaveChanges();

                return "TVShows";
            }

            return "Error";

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
                    Description = x.Movie.Description.Substring(0, Math.Min(500, x.Movie.Description.Length)) + "....",
                    CoverImageLink = x.Movie.CoverImageLink,
                    ReleaseDate = x.Movie.ReleaseDate,
                    Rating = x.Movie.Rating,
                    Category = Category.Movies
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
                    Description = x.TVShow.Description.Substring(0, Math.Min(500, x.TVShow.Description.Length)) + "....",
                    CoverImageLink = x.TVShow.CoverImageLink,
                    ReleaseDate = x.TVShow.FirstAired,
                    Rating = x.TVShow.OverallRating,
                    Category = Category.TVShows
                })
                .ToList();

            watchlistAllViewModel.AddRange(tvShows);

            return watchlistAllViewModel;
        }        
    }
}
