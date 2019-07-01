using MovieDatabase.Models.ViewModels.TVShow;
using System.Collections.Generic;

namespace MovieDatabase.Services.Contracts
{
    public interface ITVShowService
    {
        List<TVShowAllViewModel> GetAllTVShowsAndOrder(string orderBy, string userId);

        TVShowDetailsViewModel GetTVShowAndDetailsById(string tvShowId);
    }
}
