using MovieDatabase.Models.InputModels.TVShow;
using MovieDatabase.Models.ViewModels.TVShow;
using System.Collections.Generic;

namespace MovieDatabase.Services.Contracts
{
    public interface ITVShowService
    {
        List<string> GetAllTVShowNames();

        List<TVShowAllViewModel> GetAllTVShows(string userId);

        List<TVShowAllViewModel> FilterTVShowsByGenre(List<TVShowAllViewModel> tvShowsAllViewModel, string genreFilter);

        List<TVShowAllViewModel> OrderTVShows(List<TVShowAllViewModel> tvShowsAllViewModel, string orderBy);

        TVShowDetailsViewModel GetTVShowAndDetailsById(string tvShowId, string userId);

        SeasonDetailsViewModel GetSeasonAndDetailsById(string seasonId, string userId);

        bool CreateTVShow(CreateTVShowInputModel input);

        bool AddSeasonToTVShow(AddSeasonInputModel input);

        bool AddRoleToTVShowSeason(AddRoleInputModel input);

        List<SeasonsAndTVShowNameViewModel> GetAllSeasonIdsSeasonNumbersAndTVShowNames();
    }
}
