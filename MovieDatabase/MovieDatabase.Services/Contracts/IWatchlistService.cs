using MovieDatabase.Models.ViewModels.Watchlist;
using System.Collections.Generic;

namespace MovieDatabase.Services.Contracts
{
    public interface IWatchlistService
    {
        bool IsValidId(string itemId);

        bool Exists(string userId, string itemId);

        string AddItemToUserWatchlist(string userId, string itemId);

        string RemoveItemFromUserWatchlist(string userId, string itemId);

        List<WatchlistAllViewModel> GetItemsInUserWatchlist(string userId);
    }
}
