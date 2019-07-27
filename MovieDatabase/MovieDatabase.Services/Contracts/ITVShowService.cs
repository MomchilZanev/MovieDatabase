using MovieDatabase.Models.InputModels.TVShow;
using MovieDatabase.Models.ViewModels.TVShow;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieDatabase.Services.Contracts
{
    public interface ITVShowService
    {
        Task<List<TVShowNameViewModel>> GetAllTVShowNamesAsync();

        Task<List<SeasonsAndTVShowNameViewModel>> GetAllSeasonIdsSeasonNumbersAndTVShowNamesAsync();

        Task<List<TVShowAllViewModel>> GetAllTVShowsAsync(string userId = null);

        Task<List<TVShowAllViewModel>> FilterTVShowsByGenreAsync(List<TVShowAllViewModel> tvShowsAllViewModel, string genreFilter);

        List<TVShowAllViewModel> OrderTVShows(List<TVShowAllViewModel> tvShowsAllViewModel, string orderBy);

        Task<TVShowDetailsViewModel> GetTVShowAndDetailsByIdAsync(string tvShowId);

        Task<SeasonDetailsViewModel> GetSeasonAndDetailsByIdAsync(string seasonId, string userId = null);

        Task<bool> CreateTVShowAsync(CreateTVShowInputModel input);

        Task<bool> AddSeasonToTVShowAsync(AddSeasonInputModel input);

        Task<bool> AddRoleToTVShowSeasonAsync(AddSeasonRoleInputModel input);

        Task<bool> UpdateTVShowAsync(UpdateTVShowInputModel input);

        Task<bool> UpdateSeasonAsync(UpdateSeasonInputModel input);
    }
}
