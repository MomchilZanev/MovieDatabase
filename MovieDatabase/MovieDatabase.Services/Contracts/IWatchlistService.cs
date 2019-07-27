using MovieDatabase.Models.ViewModels.Watchlist;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieDatabase.Services.Contracts
{
    public interface IWatchlistService
    {
        Task<bool> IsValidMovieOrTVShowIdAsync(string id);

        Task<string> IsIdMovieOrTVShowIdAsync(string id);

        Task<bool> MovieIsInUserWatchlistAsync(string userId, string movieId);

        Task<bool> TVShowIsInUserWatchlistAsync(string userId, string tvShowId);

        Task<List<WatchlistAllViewModel>> GetItemsInUserWatchlistAsync(string userId);

        Task AddMovieToUserWatchlistAsync(string userId, string movieId);

        Task AddTVShowToUserWatchlistAsync(string userId, string tvShowId);

        Task RemoveMovieFromUserWatchlistAsync(string userId, string movieId);

        Task RemoveTVShowFromUserWatchlistAsync(string userId, string tvShowId);
    }
}
