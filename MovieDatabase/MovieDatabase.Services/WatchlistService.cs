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

        public bool IsValidMovieOrTVShowId(string id)//TODO: Move to separate service
        {
            if (dbContext.Movies.Any(movie => movie.Id == id) || dbContext.TVShows.Any(tvShow => tvShow.Id == id))
            {
                return true;
            }

            return false;
        }

        public string IsIdMovieOrTVShowId(string id)//TODO: Move to separate service
        {
            if (dbContext.Movies.Any(movie => movie.Id == id))
            {
                return "Movie";
            }
            else if (dbContext.TVShows.Any(tvShow => tvShow.Id == id))
            {
                return "TV Show";
            }

            return "Neither";
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
                    Description = movieUser.Movie.Description.Substring(0, Math.Min(500, movieUser.Movie.Description.Length)) + "....",
                    CoverImageLink = movieUser.Movie.CoverImageLink,
                    ReleaseDate = movieUser.Movie.ReleaseDate,
                    Rating = movieUser.Movie.Rating,
                    Category = "Movies",
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
                    Category = "TVShows",
                })
                .ToList();

            watchlistAllViewModel.AddRange(tvShowsWatchlistAllViewModel);

            return watchlistAllViewModel;
        }

        public bool AddMovieToUserWatchlist(string userId, string movieId)
        {
            if (MovieIsInUserWatchlist(userId, movieId))
            {
                return false;
            }

            var movieUserForDb = new MovieUser
            {
                MovieId = movieId,
                UserId = userId,
            };

            dbContext.MovieUsers.Add(movieUserForDb);
            dbContext.SaveChanges();

            return true;
        }

        public bool AddTVShowToUserWatchlist(string userId, string tvShowId)
        {
            if (TVShowIsInUserWatchlist(userId, tvShowId))
            {
                return false;
            }

            var tvShowUserForDb = new TVShowUser
            {
                TVShowId = tvShowId,
                UserId = userId,
            };

            dbContext.TVShowUsers.Add(tvShowUserForDb);
            dbContext.SaveChanges();

            return true;
        }

        public bool RemoveMovieFromUserWatchlist(string userId, string movieId)
        {
            if (!MovieIsInUserWatchlist(userId, movieId))
            {
                return false;
            }

            var movieUserFromDb = dbContext.MovieUsers
                    .SingleOrDefault(mu => mu.MovieId == movieId && mu.UserId == userId);

            dbContext.MovieUsers.Remove(movieUserFromDb);
            dbContext.SaveChanges();

            return true;
        }

        public bool RemoveTVShowFromUserWatchlist(string userId, string tvShowId)
        {
            if (!TVShowIsInUserWatchlist(userId, tvShowId))
            {
                return false;
            }

            var tvShowUserFromDb = dbContext.TVShowUsers
                    .SingleOrDefault(tvShowUser => tvShowUser.TVShowId == tvShowId && tvShowUser.UserId == userId);

            dbContext.TVShowUsers.Remove(tvShowUserFromDb);
            dbContext.SaveChanges();

            return true;
        }
    }
}
