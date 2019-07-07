using MovieDatabase.Models.InputModels.Movie;
using MovieDatabase.Models.ViewModels.Movie;
using System.Collections.Generic;

namespace MovieDatabase.Services.Contracts
{
    public interface IMovieService
    {
        List<MovieAllViewModel> GetAllMoviesAndOrder(string orderBy = null, string filterByGenre = null, string userId = null);

        MovieDetailsViewModel GetMovieAndDetailsById(string movieId, string userId = null);

        bool CreateMovie(CreateMovieInputModel input);

        bool AddRoleToMovie(AddRoleInputModel input);
    }
}
