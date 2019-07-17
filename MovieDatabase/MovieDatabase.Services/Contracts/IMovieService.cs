using MovieDatabase.Models.InputModels.Movie;
using MovieDatabase.Models.ViewModels.Movie;
using System.Collections.Generic;

namespace MovieDatabase.Services.Contracts
{
    public interface IMovieService
    {
        List<MovieAllViewModel> GetAllMovies(string userId);

        List<MovieNameViewModel> GetAllMovieNames();

        List<MovieAllViewModel> FilterMoviesByGenre(List<MovieAllViewModel> moviesAllViewModel, string genreFilter);

        List<MovieAllViewModel> OrderMovies(List<MovieAllViewModel> moviesAllViewModel, string orderBy);

        MovieDetailsViewModel GetMovieAndDetailsById(string movieId, string userId);

        bool CreateMovie(CreateMovieInputModel input);

        bool AddRoleToMovie(AddRoleInputModel input);
    }
}
