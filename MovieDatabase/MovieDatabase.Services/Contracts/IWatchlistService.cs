using MovieDatabase.Models.ViewModels.Watchlist;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieDatabase.Services.Contracts
{
    public interface IWatchlistService
    {
        bool IsValidMovieOrTVShowId(string id);

        string IsIdMovieOrTVShowId(string id);

        bool MovieIsInUserWatchlist(string userId, string movieId);

        bool TVShowIsInUserWatchlist(string userId, string tvShowId);

        List<WatchlistAllViewModel> GetItemsInUserWatchlist(string userId);

        Task AddMovieToUserWatchlistAsync(string userId, string movieId);

        Task AddTVShowToUserWatchlistAsync(string userId, string tvShowId);

        Task RemoveMovieFromUserWatchlistAsync(string userId, string movieId);

        Task RemoveTVShowFromUserWatchlistAsync(string userId, string tvShowId);
    }
}
