using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieDatabase.Common;
using MovieDatabase.Data;
using MovieDatabase.Domain;
using MovieDatabase.Models.ViewModels.Watchlist;
using MovieDatabase.Services.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieDatabase.Services
{
    public class WatchlistService : IWatchlistService
    {
        private readonly MovieDatabaseDbContext dbContext;
        private readonly IMapper mapper;

        public WatchlistService(MovieDatabaseDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<bool> IsValidMovieOrTVShowIdAsync(string id)
        {
            if (await dbContext.Movies.AnyAsync(movie => movie.Id == id) || dbContext.TVShows.Any(tvShow => tvShow.Id == id))
            {
                return true;
            }

            return false;
        }

        public async Task<string> IsIdMovieOrTVShowIdAsync(string id)
        {
            if (await dbContext.Movies.AnyAsync(movie => movie.Id == id))
            {
                return GlobalConstants.Movie;
            }
            else if (await dbContext.TVShows.AnyAsync(tvShow => tvShow.Id == id))
            {
                return GlobalConstants.TV_Show;
            }

            return GlobalConstants.Neither;
        }

        public async Task<bool> MovieIsInUserWatchlistAsync(string userId, string movieId)
        {
            if (await dbContext.MovieUsers.AnyAsync(movieUser => movieUser.MovieId == movieId && movieUser.UserId == userId))
            {
                return true;
            }

            return false;
        }

        public async Task<bool> TVShowIsInUserWatchlistAsync(string userId, string tvShowId)
        {
            if (await dbContext.TVShowUsers.AnyAsync(tvSHowuser => tvSHowuser.TVShowId == tvShowId && tvSHowuser.UserId == userId))
            {
                return true;
            }

            return false;
        }

        public async Task<List<WatchlistAllViewModel>> GetItemsInUserWatchlistAsync(string userId)
        {
            var watchlistAllViewModel = new List<WatchlistAllViewModel>();

            var movieUsersFromDb = await dbContext.MovieUsers.Where(movieUser => movieUser.UserId == userId).ToListAsync();
            var moviesWatchlistAllViewModel = mapper.Map<List<MovieUser>, List<WatchlistAllViewModel>>(movieUsersFromDb);
            watchlistAllViewModel.AddRange(moviesWatchlistAllViewModel);

            var tvShowUsersFromDb = await dbContext.TVShowUsers.Where(tvShowUser => tvShowUser.UserId == userId).ToListAsync();
            var tvShowsWatchlistAllViewModel = mapper.Map<List<TVShowUser>, List<WatchlistAllViewModel>>(tvShowUsersFromDb);
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
            var movieUserFromDb = await dbContext.MovieUsers
                    .SingleOrDefaultAsync(movieUser => movieUser.MovieId == movieId && movieUser.UserId == userId);

            dbContext.MovieUsers.Remove(movieUserFromDb);
            await dbContext.SaveChangesAsync();
        }

        public async Task RemoveTVShowFromUserWatchlistAsync(string userId, string tvShowId)
        {
            var tvShowUserFromDb = await dbContext.TVShowUsers
                    .SingleOrDefaultAsync(tvShowUser => tvShowUser.TVShowId == tvShowId && tvShowUser.UserId == userId);

            dbContext.TVShowUsers.Remove(tvShowUserFromDb);
            await dbContext.SaveChangesAsync();
        }
    }
}
