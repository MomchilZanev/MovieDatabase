using MovieDatabase.Models.ViewModels.Watchlist;
using System.Collections.Generic;

namespace MovieDatabase.Services.Contracts
{
    public interface IWatchlistService
    {
        bool RemoveItemFromUserWatchlist(string userId, string itemId);

        List<WatchlistAllViewModel> GetItemsInUserWatchlist(string userId);
    }
}
