using MovieDatabase.Models.ViewModels.Movie;
using System.Collections.Generic;

namespace MovieDatabase.Services.Contracts
{
    public interface IMovieService
    {
        List<MovieAllViewModel> GetAllMoviesAndOrder(string orderBy, string userId);

        MovieDetailsViewModel GetMovieAndDetailsById(string movieId);
    }
}
