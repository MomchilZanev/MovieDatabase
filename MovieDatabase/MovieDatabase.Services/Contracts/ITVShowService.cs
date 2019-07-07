using MovieDatabase.Models.InputModels.TVShow;
using MovieDatabase.Models.ViewModels.TVShow;
using System.Collections.Generic;

namespace MovieDatabase.Services.Contracts
{
    public interface ITVShowService
    {
        List<TVShowAllViewModel> GetAllTVShowsAndOrder(string orderBy = null, string genreFilter = null, string userId = null);

        TVShowDetailsViewModel GetTVShowAndDetailsById(string tvShowId, string userId);

        bool CreateTVShow(CreateTVShowInputModel input);

        bool AddSeasonToTVShow(AddSeasonInputModel input);

        bool AddRoleToTVShowSeason(AddRoleInputModel input);

        List<SeasonsAndTVShowNameViewModel> GetAllSeasonsAndTVShowNames();
    }
}
