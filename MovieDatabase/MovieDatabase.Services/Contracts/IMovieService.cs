using MovieDatabase.Models.InputModels.Movie;
using MovieDatabase.Models.ViewModels.Movie;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieDatabase.Services.Contracts
{
    public interface IMovieService
    {
        Task<List<MovieAllViewModel>> GetAllMoviesAsync(string userId = null);

        Task<List<MovieNameViewModel>> GetAllMovieNamesAsync();

        Task<List<MovieAllViewModel>> FilterMoviesByGenreAsync(List<MovieAllViewModel> moviesAllViewModel, string genreFilter);

        List<MovieAllViewModel> OrderMovies(List<MovieAllViewModel> moviesAllViewModel, string orderBy);

        Task<MovieDetailsViewModel> GetMovieAndDetailsByIdAsync(string movieId, string userId = null);

        Task<bool> CreateMovieAsync(CreateMovieInputModel input);

        Task<bool> AddRoleToMovieAsync(AddMovieRoleInputModel input);

        Task<bool> UpdateMovieAsync(UpdateMovieInputModel input);
    }
}
