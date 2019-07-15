using MovieDatabase.Models.ViewModels.Watchlist;
using System.Collections.Generic;

namespace MovieDatabase.Services.Contracts
{
    public interface IWatchlistService
    {
        bool IsValidMovieOrTVShowId(string id);

        string IsIdMovieOrTVShowId(string id);

        bool MovieIsInUserWatchlist(string userId, string movieId);

        bool TVShowIsInUserWatchlist(string userId, string tvShowId);

        List<WatchlistAllViewModel> GetItemsInUserWatchlist(string userId);

        bool AddMovieToUserWatchlist(string userId, string movieId);

        bool AddTVShowToUserWatchlist(string userId, string tvShowId);

        bool RemoveMovieFromUserWatchlist(string userId, string movieId);

        bool RemoveTVShowFromUserWatchlist(string userId, string tvShowId);
    }
}
